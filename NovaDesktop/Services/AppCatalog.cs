using System.Diagnostics;
namespace NovaDesktop.Services;
public sealed record LauncherApp(string Name, string ShortcutPath);
public static class AppCatalog
{
    public static IReadOnlyList<LauncherApp> Discover()
    {
        var roots = new[] {
            Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)
        };
        var apps = new List<LauncherApp>();
        foreach (var root in roots.Where(Directory.Exists))
        {
            try
            {
                foreach (var path in Directory.EnumerateFiles(root, "*.lnk", SearchOption.AllDirectories))
                    apps.Add(new LauncherApp(Path.GetFileNameWithoutExtension(path), path));
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
        }
        return apps.GroupBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
                   .Select(g => g.First())
                   .OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase).ToArray();
    }
    public static void Launch(LauncherApp app)
    {
        if (!File.Exists(app.ShortcutPath) || !app.ShortcutPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            throw new FileNotFoundException("Shortcut no longer exists.");
        Process.Start(new ProcessStartInfo(app.ShortcutPath) { UseShellExecute = true });
    }
}