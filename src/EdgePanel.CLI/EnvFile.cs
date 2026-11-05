using Microsoft.Extensions.Configuration;

namespace EdgePanel.CLI;

internal static class EnvFile
{
    // Tim nguoc tu thu muc build len repo root vi cwd khac nhau giua dotnet run / F5.
    internal static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string fileName = ".env")
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, fileName)))
        {
            directory = directory.Parent;
        }
        if (directory == null)
        {
            return builder;
        }

        Dictionary<string, string?> values = [];
        foreach (string line in File.ReadAllLines(Path.Combine(directory.FullName, fileName)))
        {
            string entry = line.Trim();
            int separator = entry.IndexOf('=');
            if (entry.StartsWith('#') || separator <= 0)
            {
                continue;
            }
            // __ la dau phan cap section, giong provider env var cua ASP.NET.
            string key = entry[..separator].Trim().Replace("__", ":");
            values[key] = entry[(separator + 1)..].Trim().Trim('"');
        }
        return builder.AddInMemoryCollection(values);
    }
}
