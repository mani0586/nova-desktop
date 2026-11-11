# Nova Desktop

**A cleaner, faster, more personal Windows 11 experience.**

Nova Desktop is an open-source, native Windows desktop enhancement designed for home users, students, professionals, and developers. It aims to combine an approachable interface, flexible workspaces, accessible navigation, and a privacy-conscious, lightweight architecture—without sacrificing Windows application compatibility.

> **Status: v0.1 early source preview.** This is not yet a production-ready desktop replacement or a verified installer. Windows build and runtime validation are tracked in [Issue #1](https://github.com/mani0586/nova-desktop/issues/1).

## Vision

One desktop, multiple ways to work. Nova should be simple by default, powerful when needed, and easy to disable or uninstall.

### Planned experiences

| Mode | Focus |
| --- | --- |
| Home | Simple navigation, files, media, and everyday tasks |
| Student | Study-focused workspaces and distraction reduction |
| Work | Meetings, documents, multitasking, and productivity |
| Developer | Keyboard-first launching, terminal access, and window layouts |

Modes are intended as optional presets, not separate applications.

## What's implemented in v0.1

- Native WinUI 3 application shell and navigation
- System, dark, and light appearance options for Nova's own interface
- Home, Student, Work, and Developer preference selection
- Local JSON settings persistence

**Not yet implemented:** functional app launcher, floating dock, global shortcut, workspace automation, Windows taskbar styling, window tiling, multi-monitor enhancements, installer, and update mechanism.

## Technology

- C# and .NET 8
- WinUI 3 and Windows App SDK
- Windows 11 as the primary target
- Local-first settings; no mandatory account

## Architecture direction

**Approach A (first):** run alongside Windows Explorer, using supported Windows APIs. Avoid system file patches, forced shell replacement, and unnecessary elevated permissions.

**Approach B (later):** optional immersive desktop features with compatibility testing, a safe exit path, and recovery controls.

## Build from source

1. Use Windows 11 with Visual Studio and the Windows application development workload, plus the .NET 8 SDK.
2. Clone this repository.
3. Open `NovaDesktop/NovaDesktop.csproj`.
4. Restore NuGet packages, select x64, and build/run.

The [Windows CI workflow](.github/workflows/windows-build.yml) attempts an x64 Release build. A passing build does not establish that the app has been tested on a real Windows desktop.

## Roadmap and progress

- [v0.1 — Validate Windows build and startup](../../issues/1)
- [v0.2 — Searchable application launcher](../../issues/2)
- [v0.2 — Optional floating dock](../../issues/3)
- [v0.2 — Global shortcut and settings](../../issues/4)
- [v0.3 — Workspace and window layouts](../../issues/5)
- [v0.4 — Security, accessibility and installer](../../issues/6)
- [v1.0 — Stable release criteria](../../issues/7)

Follow [GitHub Issues](../../issues) and [Actions](../../actions) for development status.

## Design principles

1. **Usable:** understandable for beginners, efficient for power users.
2. **Fast:** responsive startup, minimal background work, measurable performance.
3. **Secure:** standard-user operation, safe dependencies, and no invasive OS modifications.
4. **Accessible:** keyboard navigation, screen readers, scaling, contrast, and reduced motion.
5. **Private:** local-first preferences and no mandatory account.
6. **Reversible:** retain normal Windows workflows and an easy exit.

## Contributing and licensing

The project is in early development. Issues and feedback are welcome. **No open-source license has been added yet**; public source availability alone does not grant reuse rights. A license will be selected before accepting external code contributions.
