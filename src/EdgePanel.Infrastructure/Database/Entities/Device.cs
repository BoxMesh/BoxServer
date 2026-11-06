namespace EdgePanel.Infrastructure.Database.Entities;

public class Device : IBase
{
    public string Hostname { get; set; } = string.Empty;
    public string OSName { get; set; } = string.Empty;
    public string OSVersion { get; set; } = string.Empty;
    public string OSArchitecture { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string CPU { get; set; } = string.Empty;
    public long RAM { get; set; } = 0;
    public long Disk { get; set; } = 0;
    public long Uptime { get; set; } = 0;
}