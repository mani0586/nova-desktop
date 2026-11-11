using System.Text.Json;
namespace NovaDesktop.Services;
public record DesktopSettings(string Theme = "system", string Workspace = "Home");
public sealed class SettingsStore
{
    private static readonly string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NovaDesktop");
    private static readonly string FilePath = Path.Combine(DirectoryPath, "settings.json");
    public DesktopSettings Load()
    {
        try { return File.Exists(FilePath) ? JsonSerializer.Deserialize<DesktopSettings>(File.ReadAllText(FilePath)) ?? new() : new(); }
        catch (Exception) { return new(); }
    }
    public void Save(DesktopSettings settings)
    {
        Directory.CreateDirectory(DirectoryPath);
        var temporary = FilePath + ".tmp";
        File.WriteAllText(temporary, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        File.Move(temporary, FilePath, true);
    }
}