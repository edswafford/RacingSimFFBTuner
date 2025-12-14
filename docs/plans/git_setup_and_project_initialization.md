---
name: Git Setup and Project Initialization
overview: Set up Git repository with pre-commit hooks for linting, building, and testing; create GPL v3 LICENSE and README; provide GitHub repository setup instructions.
todos:
  - id: create_gitignore
    content: Create comprehensive .gitignore file for .NET/WPF/Visual Studio projects
    status: completed
  - id: create_license
    content: Create LICENSE file with GNU GPL v3 text and copyright placeholder
    status: completed
  - id: create_readme
    content: Create README.md with brief project overview, tech stack, and getting started info
    status: completed
  - id: init_git_repo
    content: Initialize git repository with git init
    status: completed
  - id: configure_linter
    content: Install and configure StyleCop Analyzers with EditorConfig for IDesign standards
    status: completed
  - id: create_precommit_hook
    content: Create PowerShell pre-commit hook script that runs linter, builds solution and runs tests
    status: completed
  - id: create_github_instructions
    content: Create SETUP_GITHUB.md with step-by-step instructions for creating and connecting GitHub repository
    status: completed
---

# Git Setup and Project Initialization Plan

## Overview
Initialize Git repository with pre-commit validation hooks, create open-source project files (LICENSE, README), and provide instructions for GitHub repository setup.

## Project Constraints
- **Windows-only project**: No cross-platform support required
- **Primary IDE**: Visual Studio Code (VS Code) for most development
- **Secondary IDE**: Visual Studio 2022 for building/running when needed
- **Linter**: StyleCop Analyzers (configured for IDesign C# Coding Standard v3.01)

## Files to Create

### 1. `.gitignore`
Create a comprehensive .NET/WPF `.gitignore` file that excludes:
- Visual Studio build outputs, bin/, obj/ folders
- User-specific files (*.user, *.suo)
- NuGet packages
- Test results
- Temporary files
- Cursor-specific files that shouldn't be committed

### 2. `LICENSE`
Create GNU GPL v3 license file with:
- Full GPL v3 text
- Copyright notice placeholder for the project

### 3. `README.md`
Create a brief overview README that includes:
- Project name and description
- Key features (telemetry ingestion, FFB output, tuning UI)
- Technology stack (.NET 10.0, WPF, MVVM, Clean Architecture)
- Development environment (VS Code primary, Visual Studio 2022 compatible)
- Build requirements (Visual Studio 2022, .NET 10.0, Windows only)
- License (GPL v3)
- Brief getting started section

### 4. StyleCop Analyzers Configuration
Install and configure StyleCop Analyzers to:
- Enforce IDesign C# Coding Standard v3.01
- Work seamlessly in both VS Code and Visual Studio 2022
- Integrate with pre-commit hooks for automated validation
- Provide real-time feedback during development

**Implementation:**
- Install NuGet package `StyleCop.Analyzers` in projects
- Configure via `.editorconfig` and `stylecop.json` files
- StyleCop Analyzers run as Roslyn analyzers during compilation

### 5. Pre-commit Hook Script
Create `.git/hooks/pre-commit` PowerShell script (Windows-only) that:
- Runs the configured linter/analyzer
- Builds the solution using `dotnet build` (compatible with Visual Studio solutions)
- Runs tests using `dotnet test`
- Prevents commit if any step fails

**Implementation details:**
- Use PowerShell for the pre-commit hook (Windows-native)
- Run `dotnet build` on the solution (works with Visual Studio 2022 solutions)
- Run `dotnet test` on test projects
- Check for linter/analyzer errors via build output
- Exit with non-zero error code on failure to block commit

### 6. `SETUP_GITHUB.md`
Create a step-by-step guide for:
- Creating a new GitHub repository
- Connecting local repository to remote
- Initial commit and push instructions
- Verifying the setup

## Git Configuration

### Initialize Repository
- Run `git init` in the project root
- Configure initial branch name (main/master)
- Set up basic git config if needed

### Pre-commit Hook Setup
- Create `.git/hooks/pre-commit` PowerShell script
- Make it executable (chmod +x equivalent on Windows)
- Script should:
  1. Check if solution file exists
  2. Run `dotnet build` with appropriate configuration
  3. Run `dotnet test` if test projects exist
  4. Return non-zero exit code on failure to block commit

## Implementation Details

### Pre-commit Hook Strategy
Since the project structure doesn't exist yet, the hook will:
- Check for `.sln` files in the root
- Build using `dotnet build` (works with Visual Studio solutions)
- Run tests using `dotnet test`
- Linter validation will occur during build (Roslyn analyzers run as part of compilation)
- Be designed to work once the solution is created

### StyleCop Analyzers Integration
- StyleCop Analyzers added as NuGet packages to projects
- Configuration via `.editorconfig` and `stylecop.json` files
- Pre-commit hook will catch analyzer errors during `dotnet build`
- Both VS Code and Visual Studio will show real-time analyzer feedback

### Cursor Checkpoints
The pre-commit validation ensures each commit is:
- Buildable
- Tested
- Linted (when linters are configured)
This makes it safer to undo commits since each one represents a working state.

## GitHub Repository Setup Instructions

The `SETUP_GITHUB.md` will include:
1. Creating repository on GitHub (web interface steps)
2. Connecting local repo: `git remote add origin <url>`
3. Initial commit and push workflow
4. Branch protection recommendations (optional)
5. Verifying the connection

## Notes
- **Windows-only**: All tooling and scripts are Windows-specific (PowerShell, .NET on Windows)
- **IDE Compatibility**: Configuration files (`.editorconfig`, `stylecop.json`) work in both VS Code and Visual Studio
- Pre-commit hook will be created but may need adjustment once the solution structure exists
- The hook should gracefully handle cases where solution/test projects don't exist yet
- All files will be created locally; GitHub setup is manual with provided instructions
- Linter configuration will be committed to repository for team consistency

