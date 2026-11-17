namespace BoxServer.Infra.Database.Entities;

/// <summary>A named set of devices. Membership is not stored yet.</summary>
public class Group : IBase
{
    public string Name { get; set; } = string.Empty;
}
