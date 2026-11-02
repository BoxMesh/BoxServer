namespace EdgePanel.Infrastructure.Database.Entities;

public class IBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public Guid? CreatedBy { get; set; } = null;
    public Guid? UpdatedBy { get; set; } = null;
}