using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NovaDesktop.Services;

namespace NovaDesktop;
public sealed partial class MainWindow : Window
{
    private readonly SettingsStore _settings = new();
    private bool _loaded;
    private IReadOnlyList<LauncherApp> _apps = Array.Empty<LauncherApp>();
    public MainWindow()
    {
        InitializeComponent();
        var config = _settings.Load();
        ThemeChoice.SelectedIndex = config.Theme switch { "dark" => 1, "light" => 2, _ => 0 };
        WorkspaceChoice.SelectedIndex = config.Workspace switch { "Student" => 1, "Work" => 2, "Developer" => 3, _ => 0 };
        _loaded = true;
        ApplyTheme();
        _apps = AppCatalog.Discover();
        FilterApps();
    }
    private void ApplyTheme()
    {
        if (Content is FrameworkElement root)
            root.RequestedTheme = ThemeChoice.SelectedIndex switch { 1 => ElementTheme.Dark, 2 => ElementTheme.Light, _ => ElementTheme.Default };
    }
    private void ThemeChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_loaded) return;
        ApplyTheme(); Save();
    }
    private void WorkspaceChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_loaded) Save();
    }
    private void Save()
    {
        var theme = ThemeChoice.SelectedIndex switch { 1 => "dark", 2 => "light", _ => "system" };
        var workspace = (WorkspaceChoice.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Home";
        _settings.Save(new DesktopSettings(theme, workspace));
        Status.Text = $"Saved locally · {workspace} workspace";
    }
    private void FilterApps()
    {
        var query = AppSearch.Text?.Trim() ?? "";
        var matches = _apps.Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).Take(100).ToArray();
        AppResults.ItemsSource = matches;
        LauncherStatus.Text = $"{matches.Length} displayed · {_apps.Count} discovered";
    }
    private void AppSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_loaded) FilterApps();
    }
    private void AppResults_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is not LauncherApp app) return;
        try { AppCatalog.Launch(app); LauncherStatus.Text = $"Opened {app.Name}"; }
        catch (Exception ex) { LauncherStatus.Text = $"Could not open {app.Name}: {ex.Message}"; }
    }
    private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem item) return;
        string section = item.Tag?.ToString() ?? "home";
        LauncherPanel.Visibility = section == "launcher" ? Visibility.Visible : Visibility.Collapsed;
        PageTitle.Text = section switch { "home" => "Welcome to Nova", "launcher" => "Launcher", "workspaces" => "Workspaces", "appearance" => "Appearance", _ => "About Nova" };
        PageSubtitle.Text = section switch { "launcher" => "Search installed Start Menu shortcuts.", "workspaces" => "Choose a preset now; workspace automation comes later.", "appearance" => "Choose a theme and keep your Windows settings intact.", "about" => "Nova Desktop 0.1 — native Windows desktop enhancement.", _ => "A calmer way to use Windows." };
        Description.Text = section switch { "launcher" => "Search and open Start Menu applications. Packaged apps without shortcuts may not appear.", "workspaces" => "Home, Student, Work, and Developer are currently saved preferences; they do not yet rearrange windows.", "appearance" => "Nova themes currently affect Nova itself, not the Windows taskbar or third-party apps.", "about" => "Built for Windows 11 with WinUI 3. No system shell replacement, administrator privileges, or telemetry.", _ => "Nova works alongside Windows Explorer. Start with a theme and workspace preset." };
    }
}