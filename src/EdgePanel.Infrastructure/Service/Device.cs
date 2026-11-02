using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace EdgePanel.Infrastructure.Services;

public class DeviceService : IService<Device>
{
    private readonly AppDbContext _context;

    public DeviceService(AppDbContext context)
    {
        _context = context;
    }

    public override async Task<Device> CreateAsync(Device entity)
    {
        _context.Devices.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public override async Task<List<Device>> GetAllAsync()
    {
        return await _context.Devices.ToListAsync();
    }

    public override async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices.FindAsync(id);
    }

    public override async Task<bool> UpdateAsync(Guid id, Device entity)
    {
        var device = await _context.Devices.FindAsync(id);

        if (device is null)
            return false;

        device.HostName = entity.HostName;
        device.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _context.Devices.FindAsync(id);

        if (device is null)
            return false;

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return true;
    }
}