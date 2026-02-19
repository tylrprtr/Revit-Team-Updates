# Team Updates - Native Revit Add-in

A native C#/.NET Revit add-in for tracking sync-to-central changes with a project changelog and generating reports for team coordination. This is a high-performance port of the original pyRevit extension.

## Features

- **Sync with Changelog**: Sync to central while recording a changelog entry
- **View Changelogs**: Generate reports of team changes over time periods
- **Multi-Version Support**: Compatible with Revit 2024, 2025, and 2026
- **High Performance**: Native compiled code for faster execution
- **Professional UI**: Modern WPF interfaces
- **Cloud Model Support**: Works with both server-based and cloud-hosted models

## Requirements

| Revit Version | .NET Framework |
|--------------|----------------|
| 2024 | .NET Framework 4.8 |
| 2025 | .NET 8.0 |
| 2026 | .NET 8.0 |

To build from source you need:
- .NET 8.0 SDK (for Revit 2025/2026)
- .NET Framework 4.8 Developer Pack (for Revit 2024)
- Visual Studio 2022 or `dotnet` CLI

No local Revit installation is required to build — Revit API references are pulled automatically via NuGet.

## Installation

### Recommended: `.bundle` Deployment

The add-in is packaged as a Revit `.bundle`, which allows Revit to automatically load the correct version-specific DLL.

1. Build the bundle (see [Building](#building) below), or obtain a pre-built `TeamUpdates.bundle` folder
2. Copy the entire `TeamUpdates.bundle` folder to:
   ```
   C:\ProgramData\Autodesk\ApplicationPlugins\
   ```
3. Restart Revit
4. The **Team Updates** tab will appear in the Revit ribbon

Revit reads `PackageContents.xml` inside the bundle and loads the correct DLL for your Revit version automatically.

### Alternative: Manual Single-Version Deployment

1. Build for your target version (e.g., `dotnet build /p:RevitVersion=2025`)
2. Copy from `TeamUpdates.bundle\Contents\2025\` to:
   ```
   %APPDATA%\Autodesk\Revit\Addins\2025\
   ```
3. Restart Revit

## Building

### Build All Versions at Once (Recommended)

```batch
build-all.bat
```

This builds for Revit 2024, 2025, and 2026 in a single step and assembles the output into a ready-to-deploy `TeamUpdates.bundle\` folder.

### Build for a Specific Version

```batch
:: Revit 2024 (.NET Framework 4.8)
dotnet build TeamUpdates.csproj -c Release /p:RevitVersion=2024

:: Revit 2025 (.NET 8.0)
dotnet build TeamUpdates.csproj -c Release /p:RevitVersion=2025

:: Revit 2026 (.NET 8.0)
dotnet build TeamUpdates.csproj -c Release /p:RevitVersion=2026
```

See [BUILD.md](BUILD.md) for full build and debugging instructions.

### Bundle Output Structure

```
TeamUpdates.bundle/
├── PackageContents.xml
└── Contents/
    ├── 2024/
    │   ├── TeamUpdates.addin
    │   ├── TeamUpdates.dll
    │   └── Newtonsoft.Json.dll
    ├── 2025/
    │   ├── TeamUpdates.addin
    │   └── TeamUpdates.dll
    └── 2026/
        ├── TeamUpdates.addin
        └── TeamUpdates.dll
```

## Project Setup

### Create Project Parameter (One-time setup per project)

1. In Revit, go to **Manage** > **Project Parameters**
2. Click **Add** to create a new parameter:
   - **Name**: `Project Directory Filepath`
   - **Type of Parameter**: Text
   - **Group parameter under**: Identity Data
   - **Categories**: Check only "Project Information"
3. Click **OK**

### Set the Project Directory Path

1. Go to **Manage** > **Project Information**
2. Find the **Project Directory Filepath** parameter
3. Enter the network path to your project library folder
   - Example: `\\server\share\Projects\2025\Project Name\Models`
4. Click **OK**

This parameter is stored in the central model, so all team members automatically use the same changelog folder.

## Usage

### Syncing with Changelog

1. Click **Sync with Changelog** in the **Sync** panel
2. Enter your changelog description
3. Click **Sync**

The add-in will:
- Save a changelog JSON entry to the network folder
- Automatically sync to central
- If automatic sync fails, open Revit's native sync dialog as a fallback

### Viewing Reports

1. Click **View Changelogs** in the **Reports** panel
2. Select a time range:
   - Last Day (24 hours)
   - Last Week (7 days)
   - Last 2 Weeks (14 days)
   - Last Month (30 days)
3. Review the report showing:
   - Central model name in the title
   - Total syncs and unique users
   - Individual changelog entries with timestamps
4. Use **Copy to Clipboard** or **Export to File** to save the report

## Project Structure

```
TeamUpdates/
├── Commands/
│   ├── SyncWithChangelogCommand.cs     # Sync command logic
│   └── ViewChangelogsCommand.cs        # View reports command logic
├── Managers/
│   └── ChangelogManager.cs             # Core changelog business logic
├── Models/
│   └── ChangelogEntry.cs               # Data model for changelog entries
├── UI/
│   ├── ChangelogInputWindow.xaml(.cs)  # Sync input dialog
│   ├── ChangelogReportWindow.xaml(.cs) # Report viewer
│   └── DateRangeWindow.xaml(.cs)       # Date range selector
├── icons/
│   ├── icon-sync.png
│   └── icon-view-changelog.png
├── Application.cs                       # Ribbon tab and button setup
├── TeamUpdates.csproj                   # Multi-version project file
├── TeamUpdates.addin                    # Revit manifest
├── PackageContents.xml                  # Bundle manifest
├── TeamUpdates.sln                      # Solution file
├── build-all.bat                        # Builds all Revit versions
├── BUILD.md                             # Detailed build instructions
└── MIGRATION.md                         # Migration guide from pyRevit
```

## Changelog Storage

Changelogs are stored as JSON files in a `SyncChangelogs` subfolder within your configured project library folder. The subfolder is created automatically on first sync.

Each file is named `changelog_YYYYMMDD_HHmmss.json` and contains:

```json
{
  "username": "tyler.porter",
  "timestamp": "2025-02-15T14:30:22.1234567",
  "changelog": "Updated door families and fixed room tags"
}
```

This format is identical to the pyRevit version — all existing changelog data is immediately accessible after migration.

## Troubleshooting

**Add-in doesn't appear in Revit:**
- Verify `TeamUpdates.bundle` is in `C:\ProgramData\Autodesk\ApplicationPlugins\`
- Check that `PackageContents.xml` exists at the bundle root
- Review Revit's journal file: `%LOCALAPPDATA%\Autodesk\Revit\Autodesk Revit 20XX\Journals`

**"Project Folder Not Configured" error:**
- Verify the `Project Directory Filepath` parameter exists in **Manage > Project Information**
- Confirm the path is a valid UNC path (not a mapped drive letter)
- Ensure you have write permissions to the folder

**Build errors:**
- Run `dotnet restore TeamUpdates.csproj` to restore NuGet packages
- Use `/p:RevitVersion=XXXX` to target only the versions you need
- See [BUILD.md](BUILD.md) for detailed troubleshooting

**Sync fails:**
- Confirm the model is workshared and you have an open local copy
- Verify you have permission to sync to central
- If automatic sync fails, the add-in will open Revit's native sync dialog as a fallback

## Migrating from pyRevit

See [MIGRATION.md](MIGRATION.md) for a full migration guide. The short version:

- No data migration needed — the same JSON files are used by both versions
- Both versions can coexist and share changelog data
- User workflow is identical

## Development

### Dependencies

| Package | Purpose |
|---------|---------|
| `Nice3point.Revit.Api.RevitAPI` | Revit API (via NuGet, no local install needed) |
| `Nice3point.Revit.Api.RevitAPIUI` | Revit UI API (via NuGet) |
| `Newtonsoft.Json` 13.0.3 | JSON serialization |
| `System.ValueTuple` 4.5.0 | Value tuple support (Revit 2024 / .NET 4.8 only) |

### Adding Features

- **New Commands**: Add classes in `Commands/` implementing `IExternalCommand`
- **New UI**: Add WPF windows in `UI/`
- **New Logic**: Extend `ChangelogManager` or add new managers in `Managers/`
- **Register Buttons**: Add `PushButtonData` entries in `Application.cs`

## Support

For issues or questions, contact Tyler Porter.

## License

Copyright (c) 2025 Pivot North Architecture
All rights reserved.
