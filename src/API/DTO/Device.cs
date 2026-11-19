using System.Text.Json;

using Zin.Infra.Database.Entities;

namespace Zin.API.DTO.Devices;

public record DeviceHardware(string Cpu, string Ram, string Disk, string Uptime);

/// <summary>A process in the shape BoxMesh reads (DeviceProcess in BoxMesh/src/types/device.ts).</summary>
public record ProcessResponse(int Pid, string Name, string User, string Status, double Cpu, double MemoryMb)
{
    // Device.Processes holds the list in this same shape, so reading it back is a plain deserialize.
    public static string ToJson(IEnumerable<ProcessResponse> processes) => JsonSerializer.Serialize(processes, JsonSerializerOptions.Web);

    public static List<ProcessResponse> FromJson(string json) => JsonSerializer.Deserialize<List<ProcessResponse>>(json, JsonSerializerOptions.Web) ?? [];
}

/// <summary>PUT /devices/{id}/groups: the groups the device should be in, replacing the current ones. Null means none.</summary>
public record SetDeviceGroupsRequest(Guid[]? GroupIds);

/// <summary>PATCH /devices/{id}: the alias to set; empty clears it.</summary>
// Nullable: a non-nullable string would be [Required], which also refuses "", the way to clear the alias.
public record UpdateDeviceRequest(string? Alias);

/// <summary>A device in the shape BoxMesh reads (BoxMesh/src/types/device.ts). Id, not Host, identifies it.</summary>
public record DeviceResponse(
    Guid Id, string Host, string Alias, string Os, string OsVersion, string OsArchitecture, string OsFamily,
    string Ip, string Mac, bool Online, string AgentVersion, bool AgentOutdated, Guid[] Groups, string Tag,
    DateTime LastSeen, DateTime EnrolledAt, double Cpu, double Ram, double Disk, int LatencyMs, DeviceHardware Hardware)
{
    // shortcut: online means heard from in the last 2 minutes; tie it to the agent's ping interval once agents ping.
    private static readonly TimeSpan OnlineWindow = TimeSpan.FromMinutes(2);

    public static DeviceResponse From(Device d, Guid[] groups)
    {
        DateTime lastSeen = d.UpdatedAt ?? d.CreatedAt;
        return new DeviceResponse(
            d.Id, d.Hostname, d.Alias, d.OSName, d.OSVersion, d.OSArchitecture, OsFamilyOf(d.OSName),
            d.IPAddress, d.MacAddress, DateTime.UtcNow - lastSeen < OnlineWindow,
            // Not stored yet: tag and latency.
            d.AgentVersion, false, groups, "", lastSeen, d.CreatedAt, d.CpuUsage, d.RamUsage, d.DiskUsage, 0,
            new DeviceHardware(d.CPU, Gigabytes(d.RAM), Gigabytes(d.Disk), Uptime(d.Uptime)));
    }

    private static string OsFamilyOf(string os) =>
        os.Contains("windows", StringComparison.OrdinalIgnoreCase) ? "Windows"
        : os.Contains("darwin", StringComparison.OrdinalIgnoreCase) || os.Contains("mac", StringComparison.OrdinalIgnoreCase) ? "macOS"
        : "Linux";

    private static string Gigabytes(long bytes) => $"{bytes / 1e9:0.#} GB";

    private static string Uptime(long seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return t.TotalDays >= 1 ? $"{(int)t.TotalDays} days" : $"{t.Hours} h {t.Minutes} min";
    }
}
