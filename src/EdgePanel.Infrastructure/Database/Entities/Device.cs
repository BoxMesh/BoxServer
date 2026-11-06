namespace EdgePanel.Infrastructure.Database.Entities;

public class Device : IBase
{
    public string Hostname { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string CPU { get; set; } = string.Empty;
    public long RAM { get; set; } = 0;
    public long Disk { get; set; } = 0;
    public long Uptime { get; set; } = 0;
}