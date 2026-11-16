namespace BoxServer.Infra.Database.Entities;

/// <summary>One usage sample from a device's agent, for the Performance charts. Kept 7 days.</summary>
// Not an IBase: a high-volume table gets a bigint key and none of the audit columns.
public class DeviceMetric
{
    public long Id { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime At { get; set; }
    // Percent; network in Mbps.
    public double Cpu { get; set; }
    public double Ram { get; set; }
    public double Disk { get; set; }
    public double NetIn { get; set; }
    public double NetOut { get; set; }
}
