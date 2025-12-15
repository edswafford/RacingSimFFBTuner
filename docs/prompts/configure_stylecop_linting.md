# Configure StyleCop Linting Rules - Continuation Prompt

## Context
I'm working on a .NET 10.0 WPF application following Clean Architecture and TDD principles. The project should follow the **IDesign C# Coding Standard v3.01** (see `docs/IDesign C# Coding Standard.md`).

## Current State

### ✅ Completed
1. **StyleCop Analyzers Installed**: Version 1.2.0-beta.556 added to `Directory.Build.props`
2. **EditorConfig Created**: `.editorconfig` file created with IDesign-aligned rules
3. **Conflicting Rules Disabled**: 
   - SA1101 (this. prefix) - disabled (IDesign says don't use 'this.' unless necessary)
   - SA1124 (regions) - disabled (IDesign allows regions for organization)
   - IDE0008 (var usage) - set to suggestion (IDesign allows var in some cases)

### 📋 Remaining Tasks

#### 1. Fix StyleCop Warnings in Existing Code
The following files have StyleCop warnings that need to be fixed:

**`src/Domain/ValueObjects/SteeringWheelAngle.cs`**:
- SA1201: Constructor should not follow a property (line 26) - Move constructor before properties
- SA1518: File should end with newline (line 104) - Add trailing newline
- IDE0008: Use explicit type instead of 'var' (lines 54, 69) - Replace `var` with explicit types

**`tests/UnitTests/Domain/ValueObjects/SteeringWheelAngleTests.cs`**:
- SA1518: File should end with newline (line 426) - Add trailing newline
- SA1515: Single-line comment should be preceded by blank line (line 375) - Add blank line before comment
- IDE0008: Multiple instances of 'var' usage - Replace with explicit types (lines 17, 41, 54, 67, 80, 97, 110, 123, 136, 149, 160, 171, 182, etc.)

**Other files with warnings**:
- `src/Infrastructure/GlobalUsings.cs`: SA1210 - Using directives ordering
- `src/Presentation/App.xaml.cs`: SA1518 - Missing trailing newline
- `src/Presentation/AssemblyInfo.cs`: Multiple spacing/comment warnings
- `src/Presentation/MainWindow.xaml.cs`: SA1518, SA1101
- `src/Presentation/GlobalUsings.cs`: SA1208 - Using directive ordering
- `tests/IntegrationTests/UnitTest1.cs`: SA1505, SA1508 - Blank line issues

#### 2. Review and Adjust EditorConfig Rules
Review the `.editorconfig` file to ensure all StyleCop rules are properly configured according to IDesign standard. Some rules may need adjustment based on IDesign requirements.

#### 3. Enable XML Documentation Analysis (Optional)
Currently SA0001 warns that XML comment analysis is disabled. Consider enabling it if full documentation is desired, or suppress the warning if not needed.

## Key Files to Reference

1. **`Directory.Build.props`**: Contains StyleCop Analyzers package reference
2. **`.editorconfig`**: Contains all code style and StyleCop rule configurations
3. **`docs/IDesign C# Coding Standard.md`**: The coding standard to follow
4. **`src/Domain/ValueObjects/SteeringWheelAngle.cs`**: Example value object with warnings
5. **`tests/UnitTests/Domain/ValueObjects/SteeringWheelAngleTests.cs`**: Test file with warnings

## IDesign Standard Key Points

From the IDesign C# Coding Standard:
- **Naming**: Use Pascal casing for types/methods, camel casing for locals/parameters
- **Private Members**: Prefix with `m_` or `_` (not `this.`)
- **Regions**: Acceptable for code organization
- **var**: Allowed when type is apparent
- **Documentation**: XML comments required for public APIs

## Instructions for New Agent

1. **Read the current state**: Review `Directory.Build.props`, `.editorconfig`, and the files with warnings
2. **Fix warnings systematically**: Start with `SteeringWheelAngle.cs` and its test file, then move to other files
3. **Verify IDesign compliance**: Ensure fixes align with IDesign standard (see `docs/IDesign C# Coding Standard.md`)
4. **Test after fixes**: Run `dotnet build` to verify warnings are resolved
5. **Document any rule conflicts**: If StyleCop rules conflict with IDesign, document and disable appropriately

## Commands to Run

```powershell
# Build and see all warnings
dotnet build RacingSimFFB.sln

# Build with detailed output
dotnet build --verbosity detailed RacingSimFFB.sln

# Run tests after fixes
dotnet test RacingSimFFB.sln
```

## Expected Outcome

After completion:
- All StyleCop warnings should be resolved (or intentionally suppressed with justification)
- Code should comply with IDesign C# Coding Standard v3.01
- Build should complete with minimal/no warnings
- All existing tests should still pass

## Notes

- The project uses `.NET 10.0` and `xUnit` for testing
- StyleCop Analyzers runs automatically during build
- Some StyleCop rules have been disabled to align with IDesign standard
- Focus on fixing warnings, not changing functionality
