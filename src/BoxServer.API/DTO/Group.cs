using BoxServer.Infra.Database.Entities;

namespace BoxServer.API.DTO.Groups;

/// <summary>POST and PATCH /groups: the group's name. Nullable so a missing name is our 400, not a model error.</summary>
public record GroupRequest(string? Name);

/// <summary>A group in the shape BoxMesh reads (DeviceGroup in BoxMesh/src/types/device.ts).</summary>
public record GroupResponse(Guid Id, string Name)
{
    public static GroupResponse From(Group g) => new(g.Id, g.Name);
}
