# JetBrains Rider Setup Checklist for RacingSimFFB Project

This checklist will help you configure JetBrains Rider to work seamlessly with your Visual Studio project for TDD and refactoring practice.

## Version Compatibility

**Note**: This checklist is based on general Rider functionality that has been consistent across recent versions (Rider 2023.x, 2024.x, and 2025.x). The features and settings paths described should work with:

- Rider 2023.1 and later
- Rider 2024.x (all versions)
- Rider 2025.x (all versions)

Some UI elements, menu paths, or feature locations may vary slightly between versions. If you encounter differences:

- Check Rider's "What's New" documentation for your specific version
- Settings paths may be found under different names (e.g., "Settings" vs "Preferences" on macOS)
- Some newer features may not be available in older versions

**Recommended**: Use Rider 2024.3 or later for best compatibility with .NET 10.0 projects.

## Prerequisites

- [x] Install JetBrains Rider (latest version recommended)
- [x] Ensure .NET SDK 10.0 (or appropriate version) is installed
- [x] Verify project builds successfully in Visual Studio first

---

## 1. Initial Project Setup

### Open Solution in Rider

- [x] Open Rider and select "Open" → Navigate to `RacingSimFFB.sln`
- [x] Wait for Rider to index the solution (watch status bar)
- [x] Verify all projects are loaded in Solution Explorer
- [x] Check that Rider recognizes the solution structure (src/, tests/ folders)

### Verify Project Recognition

- [x] Confirm all 7 projects appear in Solution Explorer:
    - [x] Domain
    - [x] Application
    - [x] Infrastructure
    - [x] Presentation
    - [x] UnitTests
    - [x] IntegrationTests
- [x] Right-click solution → "Build Solution" to verify build works
- [x] Check Build Output window for any errors

---

## 2. Code Style and Formatting Configuration

### Import EditorConfig Settings

- [x] Rider should automatically detect `.editorconfig` in project root
- [x] Verify: Settings → Editor → Code Style → C# → "Use EditorConfig" is enabled
- [x] Check: Settings → Editor → Code Style → "Detect and use existing file style settings" is ON

### Align Rider Code Style with Project Standards

- [x] **Settings → Editor → Code Style → C# → Code Style**
    - [x] Verify indentation: 4 spaces (matches `.editorconfig`)
    - [x] Verify "this." qualification: OFF (matches project standard)
    - [x] Verify "var" usage: Allow when type is apparent (matches `.cursorrules`)
    - [x] Verify regions: ALLOWED (matches project standard)
- [x] **Settings → Editor → Code Style → C# → Formatting Style**
    - [x] Verify braces placement matches `.editorconfig` settings
    - [x] Verify spacing rules match project standards

### StyleCop Analyzer Integration

- [x] Verify StyleCop Analyzers are recognized:
    - [x] Open any `.cs` file in `src/Domain`
    - [x] Check that StyleCop warnings appear in Rider's inspection window
    - [x] Verify warnings match what you see in Visual Studio
- [x] **Settings → Editor → Inspections → C# → Code Analysis**
    - [x] Ensure "Run code analysis" is set to "On the fly" or "On save"
    - [x] Verify StyleCop rules are being enforced

### Disable Conflicting Inspections

- [ ] **Settings → Editor → Inspections → C#**
    - [ ] Review Rider's built-in inspections that conflict with StyleCop
    - [ ] Consider disabling duplicate inspections (e.g., if StyleCop handles naming)
    - [ ] Note: Keep both active initially to see differences, then disable duplicates

---

## 3. Test Runner Configuration

### xUnit Test Framework Setup

- [x] Verify xUnit is recognized:
    - [x] Open `tests/UnitTests/Domain/ValueObjects/EngineRPMTests.cs`
    - [x] Look for green play icons next to test methods
    - [x] Right-click test class → "Run All Tests" to verify test runner works
- [ ] **Settings → Build, Execution, Deployment → Unit Testing → xUnit**
    - [x] Verify xUnit provider is enabled
    - [x] Check "Use xUnit.net runner" is selected
    - [x] Verify test discovery works (tests appear in Unit Test Explorer)

### Test Runner Window

- [x] Open: View → Tool Windows → Unit Tests
- [x] Verify all test projects are discovered
- [x] Run a test to verify execution works
- [x] Check that test results display correctly

### TDD Workflow Shortcuts

- [x] **Settings → Keymap**
    - [x] Find "Run Test" action → Set shortcut (e.g., `Ctrl+R, T`)
    - [x] Find "Debug Test" action → Set shortcut (e.g., `Ctrl+R, D`)
    - [x] Find "Run All Tests in Context" → Set shortcut
    - [x] Find "Re-run Failed Tests" → Set shortcut

---

## 4. Refactoring Tools Configuration

### Enable Refactoring Features

- [ ] **Settings → Editor → General → Code Completion**
    - [x] Enable "Show suggestions as you type"
    - [x] Enable "Autopopup in (ms): 100"
- [ ] **Settings → Editor → Inspections → C#**
    - [ ] Enable "Suggest var keyword" (if type is apparent)
    - [ ] Enable "Suggest expression body"
    - [ ] Enable "Suggest pattern matching"

### Refactoring Shortcuts

- [x] **Settings → Keymap**
    - [ ] **Extract Method**: `Ctrl+Alt+M` (default) - Practice this for TDD
    - [ ] **Extract Variable**: `Ctrl+Alt+V` (default)
    - [ ] **Rename**: `Shift+F6` (default) - Critical for refactoring
    - [ ] **Inline Variable**: `Ctrl+Alt+N` (default)
    - [ ] **Change Signature**: `Ctrl+F6` (default)
    - [ ] **Extract Interface**: `Ctrl+Alt+I` (default)
    - [ ] **Pull Members Up**: `Ctrl+Alt+P` (default)
    - [ ] **Push Members Down**: `Ctrl+Alt+Shift+P` (default)

### Code Analysis for Refactoring

- [x] **Settings → Editor → Inspections → C# → Potential Code Quality Issues**
    - [ ] Enable "Unused member" detection
    - [ ] Enable "Unused parameter" detection
    - [ ] Enable "Unreachable code" detection
    - [ ] Enable "Redundant code" suggestions

---

## 5. Build and Compilation Settings

### Build Configuration

- [ ] **Settings → Build, Execution, Deployment → Toolset and Build**
    - [ ] Verify .NET SDK version matches project (10.0)
    - [ ] Check "Use MSBuild version" is set correctly
    - [ ] Verify build output directory structure

### Build Actions

- [ ] **Settings → Keymap**
    - [ ] Find "Build Solution" → Verify shortcut (`Ctrl+Shift+B` default)
    - [ ] Find "Rebuild Solution" → Verify shortcut
    - [ ] Find "Clean Solution" → Verify shortcut

### Build Output

- [ ] Open: View → Tool Windows → Build
- [ ] Run a build and verify output matches Visual Studio
- [ ] Check that StyleCop warnings appear in build output

---

## 6. Solution Structure and Navigation

### Solution Explorer Configuration

- [ ] **Settings → Tools → Solution Explorer**
    - [ ] Configure folder structure view (match Visual Studio if preferred)
    - [ ] Enable "Show excluded files" if needed
    - [ ] Enable "Show project files" if needed

### Navigation Features

- [ ] **Settings → Keymap**
    - [ ] **Go to Declaration**: `Ctrl+B` (default)
    - [ ] **Go to Implementation**: `Ctrl+Alt+B` (default)
    - [ ] **Go to Type**: `Ctrl+Shift+T` (default)
    - [ ] **Go to Symbol**: `Ctrl+Alt+Shift+N` (default)
    - [ ] **Recent Files**: `Ctrl+E` (default)
    - [ ] **Navigate Back/Forward**: `Ctrl+Alt+Left/Right` (default)

---

## 7. Git Integration

### Version Control Setup

- [ ] **Settings → Version Control → Git**
    - [ ] Verify Git executable path is correct
    - [ ] Enable "Update method" (merge vs rebase - your preference)
    - [ ] Configure commit message templates if desired

### Git Tool Window

- [ ] Open: View → Tool Windows → Git
- [ ] Verify repository is recognized
- [ ] Test commit, push, pull operations

---

## 8. TDD-Specific Configuration

### Live Templates for Tests

- [ ] **Settings → Editor → Live Templates → C#**
    - [ ] Create template for xUnit test method:

      - Abbreviation: `test` or `fact`
      - Template text:

        ```csharp
        [Fact]
        public void $METHOD_NAME$()
        {
            // Arrange
            $END$

            // Act

            // Assert
        }
        ```
    - [ ] Create template for test class:

      - Abbreviation: `testclass`
      - Template text:

        ```csharp
        public class $CLASS_NAME$Tests
        {
            $END$
        }
        ```

### Code Coverage

- [ ] **Settings → Build, Execution, Deployment → Unit Testing → Code Coverage**
    - [ ] Enable code coverage collection
    - [ ] Configure coverage tool (dotCover is built-in)
    - [ ] Set up coverage filters if needed

### Test First Workflow

- [ ] Practice workflow:

  1. Write test method (Red)
  2. Run test (should fail)
  3. Write minimal implementation (Green)
  4. Refactor using Rider's tools
  5. Re-run tests to verify

---

## 9. Things to Watch Out For

### ⚠️ Important Warnings

#### File Formatting Differences

- [ ] **Watch for**: Rider and Visual Studio may format code slightly differently
- [ ] **Solution**: Ensure `.editorconfig` is properly configured and both IDEs respect it
- [ ] **Action**: After opening in Rider, run "Reformat Code" (`Ctrl+Alt+L`) to align with project standards
- [ ] **Verify**: Commit formatting changes separately to see differences

#### StyleCop Analyzer Warnings

- [ ] **Watch for**: Rider may show different StyleCop warnings than Visual Studio
- [ ] **Solution**: Both should use the same `.editorconfig` and `Directory.Build.props`
- [ ] **Action**: Compare warning lists between IDEs and investigate discrepancies
- [ ] **Note**: Rider's inspection system may show additional warnings beyond StyleCop

#### Solution File Changes

- [ ] **Watch for**: Rider may modify `.sln` file (adds Rider-specific settings)
- [ ] **Solution**: These changes are usually safe and don't affect Visual Studio
- [ ] **Action**: Review `.sln` changes in Git before committing
- [ ] **Note**: Rider adds sections like `GlobalSection(Rider)` - these are harmless

#### Project File Changes

- [ ] **Watch for**: Rider might suggest adding `<LangVersion>` or other properties
- [ ] **Solution**: Review any suggested changes carefully
- [ ] **Action**: Check if changes align with project standards in `Directory.Build.props`
- [ ] **Note**: Your project uses `Directory.Build.props` for shared settings - be cautious

#### Build Output Differences

- [ ] **Watch for**: Different build order or output between IDEs
- [ ] **Solution**: Both should produce the same binaries
- [ ] **Action**: Compare build outputs if you notice differences
- [ ] **Verify**: Run `dotnet build` from command line to get baseline

#### Test Discovery Issues

- [ ] **Watch for**: Tests not appearing in Rider's test explorer
- [ ] **Solution**: 

  - Rebuild solution
  - Invalidate caches: File → Invalidate Caches → Invalidate and Restart
  - Check xUnit runner is properly configured
- [ ] **Action**: Verify test discovery works after opening solution

#### WPF Designer

- [ ] **Watch for**: Rider's WPF designer may differ from Visual Studio
- [ ] **Solution**: Use XAML preview or code-only approach
- [ ] **Note**: Rider has XAML preview but not full designer - this is usually fine for TDD

#### Performance on First Open

- [ ] **Watch for**: Slow initial indexing
- [ ] **Solution**: Let Rider finish indexing before working
- [ ] **Action**: Watch status bar for "Indexing..." to complete
- [ ] **Note**: Subsequent opens will be faster

#### Implicit Usings

- [ ] **Watch for**: Rider may handle `GlobalUsings.cs` differently
- [ ] **Solution**: Verify `GlobalUsings.cs` files are recognized
- [ ] **Action**: Check that usings appear correctly in code completion

---

## 10. Verification Checklist

### Final Verification Steps

- [ ] **Build**: Solution builds without errors in Rider
- [ ] **Tests**: All tests run and pass in Rider
- [ ] **StyleCop**: Warnings appear and match Visual Studio (approximately)
- [ ] **Refactoring**: Test a simple refactoring (Extract Method) to verify it works
- [ ] **Navigation**: Go to Declaration works for all types
- [ ] **Code Completion**: IntelliSense works correctly
- [ ] **Git**: Version control operations work
- [ ] **Formatting**: Code formatting matches project standards

### Compare with Visual Studio

- [ ] Open same solution in Visual Studio
- [ ] Compare build output (should be identical)
- [ ] Compare test results (should be identical)
- [ ] Compare StyleCop warnings (should be similar)
- [ ] Note any differences for future reference

---

## 11. Recommended Rider Plugins/Extensions

### Useful Extensions for TDD

- [ ] **.NET Core Support**: Should be built-in, verify it's enabled
- [ ] **xUnit.net Support**: Should be built-in, verify it's enabled
- [ ] **Markdown Support**: For reading project docs
- [ ] **EditorConfig Support**: Should be built-in, verify it's enabled

### Optional but Helpful

- [ ] **Sequence Diagram**: For visualizing code flow during refactoring
- [ ] **UML Support**: For architecture visualization
- [ ] **Database Tools**: If you add database features later

---

## 12. TDD Workflow in Rider

### Recommended Workflow

1. **Red Phase**

   - [ ] Write test method using live template
   - [ ] Write failing assertion
   - [ ] Run test (should fail) - `Ctrl+R, T`
   - [ ] Verify test fails for the right reason
1. **Green Phase**

   - [ ] Write minimal code to make test pass
   - [ ] Run test (should pass) - `Ctrl+R, T`
   - [ ] Verify all tests still pass
1. **Refactor Phase**

   - [ ] Use Rider's refactoring tools:

     - Extract Method (`Ctrl+Alt+M`)
     - Rename (`Shift+F6`)
     - Extract Variable (`Ctrl+Alt+V`)
     - Inline Variable (`Ctrl+Alt+N`)
   - [ ] Run tests after each refactoring
   - [ ] Use code analysis to find improvement opportunities

### Keyboard Shortcuts Cheat Sheet

Create a reference card with:

- `Ctrl+R, T` - Run test
- `Ctrl+R, D` - Debug test
- `Ctrl+Alt+M` - Extract Method
- `Shift+F6` - Rename
- `Ctrl+Alt+L` - Reformat Code
- `Ctrl+Shift+B` - Build Solution
- `Ctrl+B` - Go to Declaration

---

## 13. Troubleshooting

### If Tests Don't Run

- [ ] Rebuild solution
- [ ] Invalidate caches (File → Invalidate Caches)
- [ ] Check xUnit runner is enabled in settings
- [ ] Verify test project references are correct

### If StyleCop Warnings Don't Appear

- [ ] Verify StyleCop.Analyzers package is referenced
- [ ] Check `Directory.Build.props` is being used
- [ ] Rebuild solution
- [ ] Check Rider's inspection settings

### If Build Fails

- [ ] Compare with Visual Studio build
- [ ] Check MSBuild version in settings
- [ ] Verify .NET SDK version matches
- [ ] Check for Rider-specific build issues

### If Code Formatting Differs

- [ ] Run "Reformat Code" (`Ctrl+Alt+L`)
- [ ] Check `.editorconfig` is being respected
- [ ] Compare Rider's code style settings with `.editorconfig`
- [ ] Consider committing formatting separately to see differences

---

## 14. Best Practices for TDD in Rider

### Code Organization

- [ ] Keep test files next to source files (or in parallel structure)
- [ ] Use consistent naming: `ClassNameTests.cs`
- [ ] Organize tests with regions if needed (project allows this)

### Refactoring Safety

- [ ] Always run tests after refactoring
- [ ] Use Rider's "Safe Delete" for removing unused code
- [ ] Use "Find Usages" (`Alt+F7`) before renaming
- [ ] Use "Refactor This" (`Ctrl+Shift+Alt+T`) to see all options

### Performance

- [ ] Use Rider's "Code Vision" to see test coverage inline
- [ ] Use "Coverage" tool window to identify untested code
- [ ] Use "Analyze → Inspect Code" to find refactoring opportunities

---

## Completion

Once all items are checked, you should have:

- ✅ Rider fully configured for your project
- ✅ TDD workflow optimized
- ✅ Refactoring tools ready to use
- ✅ Understanding of potential issues and how to handle them

**Next Steps:**

1. Practice TDD workflow with a simple feature
2. Experiment with refactoring tools
3. Compare results between Rider and Visual Studio
4. Adjust settings based on your preferences

---

## Notes Section

Use this space to document any project-specific findings:

- **Date Setup**: ___
- **Rider Version**: ___
- **.NET SDK Version**: ___
- **Issues Encountered**: 
    - 
- **Custom Settings Applied**:
    - 
- **Workflow Adjustments**:
    - 

---

*Last Updated: [Current Date]*

*Project: RacingSimFFB*

*Purpose: TDD and Refactoring Practice*