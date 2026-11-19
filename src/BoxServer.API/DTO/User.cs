using System.ComponentModel.DataAnnotations;

using Zin.Infra.Database.Entities;
using Zin.Infra.Repository;

namespace Zin.API.DTO.Users;

/// <summary>The limits on an account's fields, shared by every endpoint that sets them.</summary>
public static class AccountRules
{
    public const int MaxName = 100;
    public const int MaxEmail = 255;
    public const int MinPassword = 8;

    public static string? NameError(string name) =>
        name.Length == 0 ? "name is required" : name.Length > MaxName ? $"name must be at most {MaxName} characters" : null;

    public static string? EmailError(string email) =>
        email.Length == 0 || email.Length > MaxEmail || !new EmailAddressAttribute().IsValid(email) ? "email is not valid" : null;

    public static string? PasswordError(string? password) =>
        password == null || password.Length < MinPassword ? $"password must be at least {MinPassword} characters" : null;
}

/// <summary>POST /users. Role is optional and can only be Operator: there is one Admin.</summary>
public record CreateUserRequest(string? Name, string? Email, string? Password, string? Role);

/// <summary>What an Operator may see: every device in these groups, and these single devices. Null means none.</summary>
public record UserAccess(Guid[]? GroupIds, Guid[]? DeviceIds)
{
    public static UserAccess From(UserAccessIds? ids) => new(ids?.GroupIds ?? [], ids?.DeviceIds ?? []);
}

/// <summary>PATCH /users/{id}: only what is sent changes. A new Password replaces the old one; Access replaces the whole grant.</summary>
public record UpdateUserRequest(string? Name, bool? Disabled, string? Password, UserAccess? Access);

/// <summary>PATCH /account: an account edits itself. Changing the password needs the current one.</summary>
public record UpdateAccountRequest(string? Name, string? CurrentPassword, string? Password);

/// <summary>An account in the shape BoxMesh reads (Account in BoxMesh/src/types/admin.ts). LastSignIn is empty until the first sign-in.</summary>
public record UserResponse(Guid Id, string Email, string Name, string Role, string LastSignIn, bool Disabled, UserAccess Access)
{
    public static UserResponse From(User u, UserAccessIds? access) =>
        new(u.Id, u.Email, u.Name, u.Role, u.LastSignIn?.ToString("O") ?? "", u.Disabled, UserAccess.From(access));
}
