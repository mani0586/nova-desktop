using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NovaDesktop.Services;

namespace NovaDesktop;
public sealed partial class MainWindow : Window
{
    private readonly SettingsStore _settings = new();
    private bool _loaded;
    public MainWindow()
    {
        InitializeComponent();
        var config = _settings.Load();
        ThemeChoice.SelectedIndex = config.Theme switch { "dark" => 1, "light" => 2, _ => 0 };
        WorkspaceChoice.SelectedIndex = config.Workspace switch { "Student" => 1, "Work" => 2, "Developer" => 3, _ => 0 };
        _loaded = true;
        ApplyTheme();
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
    private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem item) return;
        string section = item.Tag?.ToString() ?? "home";
        PageTitle.Text = section switch { "home" => "Welcome to Nova", "launcher" => "Launcher", "workspaces" => "Workspaces", "appearance" => "Appearance", _ => "About Nova" };
        PageSubtitle.Text = section switch { "launcher" => "Fast app and file search is coming in the next milestone.", "workspaces" => "Choose a preset now; workspace automation comes later.", "appearance" => "Choose a theme and keep your Windows settings intact.", "about" => "Nova Desktop 0.1 — native Windows desktop enhancement.", _ => "A calmer way to use Windows." };
        Description.Text = section switch { "launcher" => "The upcoming launcher will index approved app locations and provide keyboard-first search.", "workspaces" => "Home, Student, Work, and Developer are currently saved preferences; they do not yet rearrange windows.", "appearance" => "Nova themes currently affect Nova itself, not the Windows taskbar or third-party apps.", "about" => "Built for Windows 11 with WinUI 3. No system shell replacement, administrator privileges, or telemetry.", _ => "Nova works alongside Windows Explorer. Start with a theme and workspace preset." };
    }
}