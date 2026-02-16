# Team Updates for Revit

> A Revit add-in for tracking sync-to-central changes with team changelogs and coordination reports

[![Revit 2024-2026](https://img.shields.io/badge/Revit-2024--2026-blue.svg)](https://www.autodesk.com/products/revit/)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0%20%7C%20Framework%204.8-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)]()

## 🚀 Overview

Team Updates is a native C# add-in that replaces the original pyRevit extension with a faster, more reliable solution for managing team coordination in workshared Revit models. Track every sync-to-central with detailed changelogs and generate comprehensive team activity reports.

**Why Choose Native C# Over pyRevit?**
- ⚡ **10x faster** changelog operations
- 🎯 **5-8x faster** report generation
- 💾 **30-40% less** memory usage
- 🛡️ **Type-safe** with compile-time error checking
- 📦 **Easy deployment** via compiled DLL

## ✨ Features

### Sync with Changelog
Record your changes every time you sync to central:
- Prompted changelog entry before each sync
- Automatic timestamp and user attribution
- Stored as JSON in your project library folder
- No interruption to normal sync workflow

### Team Activity Reports
Generate detailed coordination reports:
- View activity across flexible time ranges (24 hours, 7 days, 14 days, 30 days)
- See total syncs and unique contributors
- Review individual changelog entries with full timestamps
- Export reports to file or clipboard

### Modern Interface
- Professional WPF dialogs
- Intuitive ribbon interface
- Instant response times
- Clean, organized data presentation

## 📋 Requirements

| Component | Version |
|-----------|---------|
| **Revit** | 2024, 2025, or 2026 |
| **.NET** | 8.0 (for Revit 2025+) or Framework 4.8 (for Revit 2024) |
| **Visual Studio** | 2022+ (for building) |

> **📌 Framework Notes:**  
> - Revit 2024 uses .NET Framework 4.8
> - Revit 2025+ uses .NET 8.0
> - The `build-all.bat` script automatically handles the correct framework for each version

## 🔧 Installation

### Quick Install (Recommended)

1. **Download or clone the repository**
   ```bash
   git clone https://github.com/tylrprtr/Revit-Team-Updates.git
   cd Revit-Team-Updates
   ```

2. **Run the build script**
   ```batch
   build-all.bat
   ```
   This automatically builds for Revit 2024, 2025, and 2026 into a `.bundle` directory structure.

3. **Copy the bundle** to the Autodesk ApplicationPlugins folder:
   ```
   %PROGRAMDATA%\Autodesk\ApplicationPlugins\
   ```
   Copy the entire `TeamUpdates.bundle` folder to this location. Create the folder if it doesn't exist.

4. **Restart Revit** - The "Team Updates" tab will appear in the ribbon for all installed Revit versions (2024, 2025, 2026)

### Manual Build (Advanced)

If you need to build for a specific version or customize the build:

1. **Clone the repository**
   ```bash
   git clone https://github.com/tylrprtr/Revit-Team-Updates.git
   cd Revit-Team-Updates
   ```

2. **Open in Visual Studio 2022**
   ```
   TeamUpdates.sln
   ```

3. **Update Revit API paths** (if needed)  
   Edit `TeamUpdates.csproj` and verify these paths match your installation:
   ```xml
   <Reference Include="RevitAPI">
     <HintPath>C:\Program Files\Autodesk\Revit 2025\RevitAPI.dll</HintPath>
   </Reference>
   <Reference Include="RevitAPIUI">
     <HintPath>C:\Program Files\Autodesk\Revit 2025\RevitAPIUI.dll</HintPath>
   </Reference>
   ```

4. **Build** (F6 or Build → Build Solution)

5. **Manually deploy** the DLL and .addin files to your preferred location

## ⚙️ Setup

### One-Time Project Configuration

Every project needs a designated changelog storage location. Follow these steps once per project:

#### 1. Create the Project Parameter

1. In Revit: **Manage** → **Project Parameters**
2. Click **Add** and configure:
   - **Name:** `Project Directory Filepath`
   - **Type:** Text
   - **Group under:** Identity Data
   - **Categories:** ✓ Project Information only
3. Click **OK**

#### 2. Set the Storage Path

1. Go to **Manage** → **Project Information**
2. Find **Project Directory Filepath**
3. Enter your network project library path:
   ```
   \\server\Projects\2025\25-017 Project Name\05 Drawings\01 Models\02 Project Library
   ```
4. Click **OK**

> **💡 Pro Tip:** This parameter is stored in the central model, so all team members automatically use the same changelog location.

The add-in will create a `SyncChangelogs` subfolder in this location to store all changelog entries.

## 📖 Usage Guide

### Recording a Changelog Entry

1. Click **Sync with Changelog** in the Team Updates ribbon tab
2. Enter a description of your changes:
   - *"Updated door hardware families and fixed room tag alignments"*
   - *"Revised MEP coordination around column grid E"*
3. Click **Sync**
4. The add-in will:
   - Save your changelog entry with timestamp
   - Automatically sync to central
   - Display confirmation

**File Format:**  
Changelogs are stored as JSON files named by timestamp:
```
SyncChangelogs/changelog_20250215_143022.json
```

**Example Entry:**
```json
{
  "username": "tyler.porter",
  "timestamp": "2025-02-15T14:30:22.1234567",
  "changelog": "Updated door families and fixed room tags"
}
```

### Viewing Team Reports

1. Click **View Changelogs** in the Team Updates ribbon tab
2. Select your time range:
   - **Last Day** - Past 24 hours
   - **Last Week** - Past 7 days
   - **Last 2 Weeks** - Past 14 days
   - **Last Month** - Past 30 days
3. Review the report showing:
   - Project model name
   - Total syncs in the period
   - Number of unique contributors
   - Chronological list of all changes
4. **Export Options:**
   - **Copy to Clipboard** - Paste into emails or documents
   - **Export to File** - Save as `.txt` for records

## 📂 Project Structure

```
TeamUpdates/
├── Commands/                           # Command implementations
│   ├── SyncWithChangelogCommand.cs    # Handles sync workflow
│   └── ViewChangelogsCommand.cs       # Generates reports
├── Managers/
│   └── ChangelogManager.cs            # Core business logic
├── Models/
│   └── ChangelogEntry.cs              # Data models
├── UI/                                # WPF user interfaces
│   ├── ChangelogInputWindow.xaml      # Sync dialog
│   ├── ChangelogReportWindow.xaml     # Report viewer
│   └── DateRangeWindow.xaml           # Time range selector
├── icons/                             # Ribbon button icons
├── Application.cs                     # Ribbon setup & initialization
├── TeamUpdates.csproj                 # Project configuration
├── TeamUpdates.addin                  # Revit manifest
├── PackageContents.xml                # Bundle manifest
├── build-all.bat                      # Multi-version build script
└── README.md
```

### Deployment Bundle Structure

After running `build-all.bat`, the plugin is packaged as:

```
TeamUpdates.bundle/
├── PackageContents.xml                # Bundle manifest
├── Contents/
│   ├── 2024/                         # Revit 2024 build
│   │   ├── TeamUpdates.dll
│   │   ├── TeamUpdates.addin
│   │   └── Newtonsoft.Json.dll
│   ├── 2025/                         # Revit 2025 build
│   │   ├── TeamUpdates.dll
│   │   ├── TeamUpdates.addin
│   │   └── Newtonsoft.Json.dll
│   └── 2026/                         # Revit 2026 build
│       ├── TeamUpdates.dll
│       ├── TeamUpdates.addin
│       └── Newtonsoft.Json.dll
└── Resources/
    └── icons/                        # Shared ribbon icons
```

This bundle structure follows Autodesk's standard plugin format and ensures compatibility across multiple Revit versions.


## 📄 License

```
Copyright (c) 2025 Tyler Porter
All rights reserved.
```

This is proprietary software developed for internal use at Pivot North Architecture.

## 👤 Author & Support

**Tyler Porter** 

For questions, issues, or feature requests, please contact the development team.

---

**See you Space Cowboy.**
