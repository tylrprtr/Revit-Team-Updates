# Team Updates for Revit

> A Revit add-in for tracking sync-to-central changes with team changelogs and coordination reports

[![Revit 2024-2026](https://img.shields.io/badge/Revit-2024--2026-blue.svg)](https://www.autodesk.com/products/revit/)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0%20%7C%20Framework%204.8-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)]()

## Overview

- **Sync with Changelog**: Sync to central while recording a changelog entry
- **View Changelogs**: Generate reports of team changes over time periods
- **Multi-Version Support**: Compatible with Revit 2024, 2025, and 2026
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

4. **Build** (F6 or Build → Build Solution)

5. **Manually deploy** the DLL and .addin files to your preferred location

## Setup

### One-Time Project Configuration

Every project needs a designated changelog storage location. Follow these steps once per project:

#### 1. Create the Project Parameter

1. In Revit, go to **Manage** > **Project Parameters**
2. Click **Add** to create a new parameter:
   - **Name**: `Project Directory Filepath`
   - **Type of Parameter**: Text
   - **Group parameter under**: Identity Data
   - **Categories**: Check only "Project Information"
3. Click **OK**

#### 2. Set the Storage Path

1. Go to **Manage** > **Project Information**
2. Find the **Project Directory Filepath** parameter
3. Enter the network path to your project library folder
   - Example: `\\server\share\Projects\2025\Project Name\Models`
4. Click **OK**

This parameter is stored in the central model, so all team members automatically use the same changelog folder.

The add-in will create a `SyncChangelogs` subfolder in this location to store all changelog entries.

## Usage Guide

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

**File Format:**  
Changelogs are stored as JSON files named by timestamp:
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

## License

```
Copyright (c) 2025 Tyler Porter
All rights reserved. See you, Space Cowboy...
```
