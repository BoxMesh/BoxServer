namespace BoxServer.Infra.Database.Entities;

public class Device : IBase
{
    public string Hostname { get; set; } = string.Empty;
    /// <summary>A name an Admin gives the device in the console; hostnames can repeat, the device ID never does.</summary>
    public string Alias { get; set; } = string.Empty;
    public string OSName { get; set; } = string.Empty;
    public string OSVersion { get; set; } = string.Empty;
    public string OSArchitecture { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string CPU { get; set; } = string.Empty;
    public long RAM { get; set; } = 0;
    public long Disk { get; set; } = 0;
    public long Uptime { get; set; } = 0;
    public string AgentVersion { get; set; } = string.Empty;
    // Latest report only, in percent and Mbps; no history is kept yet.
    public double CpuUsage { get; set; } = 0;
    public double RamUsage { get; set; } = 0;
    public double DiskUsage { get; set; } = 0;
    public double NetIn { get; set; } = 0;
    public double NetOut { get; set; } = 0;
    /// <summary>The busiest processes at the latest report, as a JSON array; replaced on every report.</summary>
    public string Processes { get; set; } = "[]";
}
