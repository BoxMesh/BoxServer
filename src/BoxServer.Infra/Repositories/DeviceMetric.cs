using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Zin.Infra.Repository;

/// <summary>One point of a usage chart: averages over one step.</summary>
public record MetricPoint(DateTime At, double Cpu, double Ram, double Disk, double NetIn, double NetOut);

public class DeviceMetricRepository(DBContext _context)
{
    public async Task Add(DeviceMetric metric)
    {
        _context.Set<DeviceMetric>().Add(metric);
        await _context.SaveChangesAsync();
    }

    /// <summary>Averages per step from <paramref name="from"/> on, oldest first; steps with no samples are left out.</summary>
    public async Task<List<MetricPoint>> Series(Guid deviceId, DateTime from, TimeSpan step)
    {
        return await _context.Database.SqlQuery<MetricPoint>($"""
            SELECT date_bin({step}, "At", TIMESTAMPTZ '2000-01-01 00:00:00+00') AS "At",
                   round(avg("Cpu")::numeric, 2)::float8 AS "Cpu",
                   round(avg("Ram")::numeric, 2)::float8 AS "Ram",
                   round(avg("Disk")::numeric, 2)::float8 AS "Disk",
                   round(avg("NetIn")::numeric, 2)::float8 AS "NetIn",
                   round(avg("NetOut")::numeric, 2)::float8 AS "NetOut"
            FROM "DeviceMetric"
            WHERE "DeviceId" = {deviceId} AND "At" >= {from}
            GROUP BY 1
            ORDER BY 1
            """).ToListAsync();
    }

    public async Task<int> DeleteOlderThan(DateTime cutoff)
    {
        return await _context.Set<DeviceMetric>().Where(m => m.At < cutoff).ExecuteDeleteAsync();
    }
}
