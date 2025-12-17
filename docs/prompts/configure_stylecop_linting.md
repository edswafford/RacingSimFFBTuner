# Fix StyleCop Linting Warning

## How to Use This Prompt

Copy this prompt and replace `[RULE_ID]` with the actual rule (e.g., SA1413):

---

**Request:** Fix linting warning `[RULE_ID]`. [Choose one: "Fix the code" OR "Disable the rule"]

---

## Workflow

### Step 1: Check IDesign Alignment

Before fixing or disabling, check if the rule aligns with or conflicts with the **IDesign C# Coding Standard** (`docs/IDesign C# Coding Standard.md`):

- **Aligns with IDesign**: Proceed with fixing the code
- **IDesign is agnostic**: Proceed with fixing the code (rule improves consistency)
- **Conflicts with IDesign**: Ask user if they want to deviate from IDesign standard before proceeding

### Step 2a: If Fixing the Code

1. Run `.\lint.ps1` to identify all occurrences
2. Fix all violations in the codebase
3. Run `.\lint.ps1` again to verify no warnings remain
4. Run `dotnet test RacingSimFFB.sln` to ensure tests pass
5. Update `docs/myChanges/lintingRulesReviewed.md` under "Rules Enabled"
6. **Update `.cursorrules`** under "Code Pattern Guidelines" so future AI-generated code follows this rule

### Step 2b: If Disabling the Rule

1. Add rule configuration to `.editorconfig`:
   ```ini
   dotnet_diagnostic.SAXXXX.severity = none
   ```
2. Run `.\lint.ps1` to verify warning is suppressed
3. Update `docs/myChanges/lintingRulesReviewed.md` under "Rules Disabled" with rationale

## Key Files

| File                                     | Purpose                          |
| ---------------------------------------- | -------------------------------- |
| `.editorconfig`                          | StyleCop rule configurations     |
| `.cursorrules`                           | AI agent coding guidelines       |
| `docs/myChanges/lintingRulesReviewed.md` | Track decisions on linting rules |
| `docs/IDesign C# Coding Standard.md`     | Coding standard reference        |

## Commands

```powershell
# Check all linting warnings (forces full rebuild)
.\lint.ps1

# Run tests after fixes
dotnet test RacingSimFFB.sln
```
