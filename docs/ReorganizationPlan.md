---
name: Docs Reorganization Plan
overview: Reorganize the documentation structure to follow Clean Architecture layers with a single entry point document, transform design documents into capabilities/behaviors format, and properly categorize all supporting documentation.
todos: []
---

# Documentation Reorganization Plan

## Overview

This plan reorganizes the `docs/` directory to create a clean, navigable documentation structure that follows the Clean Architecture layer pattern, with a single entry point document that links to detailed layer-specific documentation.

## Goals

- Create a single, clear entry point for understanding the application
- Follow Clean Architecture layer structure (Domain, Application, Infrastructure, Presentation)
- Transform design documents into capabilities/behaviors format
- Properly categorize supporting documentation (AI prompts, standards, tools, legacy references)
- Remove redundant and obsolete content
- Make legacy references easily removable when project is complete

---

## New Documentation Structure

```
c:\dev\SimRacingApps\RacingSimFFBTuner\
├── docs\
│   ├── README.md (NEW - main entry point)
│   ├── Domain\
│   │   ├── Entities\
│   │   │   └── Entities.md
│   │   ├── ValueObjects\
│   │   │   └── ValueObjects.md (transformed from Value Objects Analysis and Design.md)
│   │   ├── Interfaces\
│   │   │   └── Interfaces.md
│   │   ├── DomainServices\
│   │   │   └── DomainServices.md
│   │   └── ConnectionManagementService\
│   │       └── ConnectionManagementService.md
│   ├── Application\
│   │   └── UseCases\
│   │       ├── RealTimeTelemetryAndForceFeedback\
│   │       │   └── RealTimeTelemetryAndForceFeedback.md
│   │       ├── StopForceFeedback\
│   │       │   └── StopForceFeedback.md
│   │       ├── LoadForceFeedbackProfile\
│   │       │   └── LoadForceFeedbackProfile.md
│   │       ├── SaveForceFeedbackProfile\
│   │       │   └── SaveForceFeedbackProfile.md
│   │       └── AnalyzeLapTimes\
│   │           └── AnalyzeLapTimes.md
│   ├── Infrastructure\
│   │   ├── Gateways\
│   │   │   └── Gateways.md
│   │   ├── Persistence\
│   │   │   └── Persistence.md
│   │   ├── HardwareOutput\
│   │   │   └── HardwareOutput.md
│   │   └── Utilities\
│   │       └── Utilities.md
│   ├── Presentation\
│   │   ├── UserInterface\
│   │   │   └── UserInterface.md
│   │   ├── ViewModels\
│   │   │   └── ViewModels.md
│   │   └── CompositionRoot\
│   │       └── CompositionRoot.md
│   ├── AI\
│   │   ├── prompts\
│   │   │   ├── Ai coding prompt guide.md
│   │   │   ├── configure_stylecop_linting.md
│   │   │   └── marvin_herbold_context.md
│   │   └── plans\
│   │       └── (AI-generated plans)
│   ├── archive\
│   │   ├── templates\
│   │   │   └── Racing Simulator Force Feedback Architecture.md (original template)
│   │   ├── architecture\
│   │   │   ├── BuildingTradeoffs.md
│   │   │   └── HybridImplementationPlan.md
│   │   ├── implementation-plans\
│   │   │   ├── git_setup_and_project_initialization.md
│   │   │   ├── tdd_starting_layer_analysis.md
│   │   │   └── value_objects_analysis_from_legacy.md
│   │   └── detailed-design\
│   │       └── Racing Simulator Force Feedback Detail Design.md (for reference)
│   └── legacy-references\
│       ├── LegacySimRacingFFB.md
│       └── README.md (explains these are temporary references)
├── references\
│   ├── standards\
│   │   ├── IDesign C# Coding Standard.md
│   │   └── IDesign_C_Coding_Standard_3.0.pdf
│   ├── tools\
│   │   └── Visual Studio Requirements.md
│   └── external\
│       ├── other-ffb-apps\
│       │   ├── Apps for Force Feedback Control.md
│       │   ├── gamesInfo.md
│       │   └── open source FFB apps.md
│       └── Racing Simulator Force Feedback Design.md
└── docs_old\ (DELETE after migration complete)
```

---

## Phase 1: Create New Directory Structure

### 1.1 Create Main Documentation Directories

Create the following directories under `docs/`:

- `docs/Domain/Entities/`
- `docs/Domain/ValueObjects/`
- `docs/Domain/Interfaces/`
- `docs/Domain/DomainServices/`
- `docs/Domain/ConnectionManagementService/`
- `docs/Application/UseCases/RealTimeTelemetryAndForceFeedback/`
- `docs/Application/UseCases/StopForceFeedback/`
- `docs/Application/UseCases/LoadForceFeedbackProfile/`
- `docs/Application/UseCases/SaveForceFeedbackProfile/`
- `docs/Application/UseCases/AnalyzeLapTimes/`
- `docs/Infrastructure/Gateways/`
- `docs/Infrastructure/Persistence/`
- `docs/Infrastructure/HardwareOutput/`
- `docs/Infrastructure/Utilities/`
- `docs/Presentation/UserInterface/`
- `docs/Presentation/ViewModels/`
- `docs/Presentation/CompositionRoot/`

### 1.2 Create Supporting Documentation Directories

- `docs/AI/prompts/`
- `docs/AI/plans/`
- `docs/archive/templates/`
- `docs/archive/architecture/`
- `docs/archive/implementation-plans/`
- `docs/archive/detailed-design/`
- `docs/legacy-references/`

### 1.3 Create References Directories (Project Root)

- `references/standards/`
- `references/tools/`
- `references/external/other-ffb-apps/`

---

## Phase 2: Create Entry Point Document

### 2.1 Create docs/README.md

**Source:** Based on [docs/Rscing Simulator Force Feedback Architecture.md](docs/Rscing Simulator Force Feedback Architecture.md) with improvements

**Key Content:**

- Executive overview (project goals, architectural principles)
- High-level architecture overview
- Links to each layer's documentation following the directory structure
- Documentation philosophy
- Capabilities template reference

**Changes from source:**

- Fix typo in title ("Rscing" → "Racing")
- Update all document links to point to new structure
- Add navigation section for quick access
- Keep it stable and high-level

**File:** [docs/README.md](docs/README.md)

---

## Phase 3: Transform Value Objects Documentation

### 3.1 Create Domain/ValueObjects/ValueObjects.md

**Source:** [docs_old/Detailed Design/Value Objects/Value Objects Analysis and Design.md](docs_old/Detailed Design/Value Objects/Value Objects Analysis and Design.md)

**Transformation Rules:**

1. **Keep:**

   - Overview of what value objects are and why they exist
   - List of completed value objects with checkmarks
   - References to legacy GitHub code (format for easy removal)
   - High-level validation rules and business constraints
   - Testing philosophy
1. **Remove:**

   - All source code recommendations and implementation details
   - Method signatures and code examples
   - References to internal project code paths (src/Domain/...)
   - TDD test requirement checklists (belongs in tests)
   - Implementation recommendations sections
   - Overly detailed descriptions
1. **Transform to Capabilities/Behaviors:**

   - Each value object should have:
       - **Capability:** One sentence describing its purpose
       - **Context:** Where it fits in the domain
       - **Behaviors:** Observable characteristics (not implementation)
       - **Invariants:** Rules that must always hold
       - **Legacy Reference:** Links to original implementation (if applicable)

**Example transformation for SteeringWheelAngle:**

```markdown
### SteeringWheelAngle

**Status:** ✅ COMPLETED

**Capability:** Represents the current steering input angle in a simulator-agnostic manner.

**Context:** Used throughout the domain to represent steering input from telemetry and for force feedback calculations.

**Behaviors:**
1. Accepts angles within physical steering wheel rotation limits
2. Rejects invalid values (NaN, Infinity)
3. Provides conversion between radians and degrees
4. Maintains value equality semantics

**Invariants:**
- Must remain within configured physical limits
- Cannot contain invalid floating-point values

**Legacy Reference:** 
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) (line references)
```

### 3.2 Delete Obsolete Value Objects Documentation

- Delete: [docs_old/Detailed Design/Value Objects/iRacing-Specific Value Objects.md](docs_old/Detailed Design/Value Objects/iRacing-Specific Value Objects.md)

---

## Phase 4: Create Layer Documentation Files

### 4.1 Extract Content from Detail Design Template

**Source:** [docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md](docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md)

**Distribution:**

1. **Domain Layer Content → Multiple Files**

   - Entities section → `docs/Domain/Entities/Entities.md`
   - Interfaces section → `docs/Domain/Interfaces/Interfaces.md`
   - Domain Services section → `docs/Domain/DomainServices/DomainServices.md`
   - Connection Management → `docs/Domain/ConnectionManagementService/ConnectionManagementService.md`
1. **Application Layer Content**

   - Real-Time Telemetry → `docs/Application/UseCases/RealTimeTelemetryAndForceFeedback/RealTimeTelemetryAndForceFeedback.md`
   - Stop FFB → `docs/Application/UseCases/StopForceFeedback/StopForceFeedback.md`
   - Profile Management → `docs/Application/UseCases/LoadForceFeedbackProfile/` and `SaveForceFeedbackProfile/`
   - Data Logging → `docs/Application/UseCases/AnalyzeLapTimes/AnalyzeLapTimes.md`
1. **Infrastructure Layer Content**

   - Gateways section → `docs/Infrastructure/Gateways/Gateways.md`
   - Persistence section → `docs/Infrastructure/Persistence/Persistence.md`
   - Hardware Output → `docs/Infrastructure/HardwareOutput/HardwareOutput.md`
   - Utilities → `docs/Infrastructure/Utilities/Utilities.md`
1. **Presentation Layer Content**

   - UI section → `docs/Presentation/UserInterface/UserInterface.md`
   - ViewModels → `docs/Presentation/ViewModels/ViewModels.md`
   - DI Setup → `docs/Presentation/CompositionRoot/CompositionRoot.md`

**Format:** Each file should follow the capabilities template structure from the architecture document.

---

## Phase 5: Move Legacy and Reference Documentation

### 5.1 Move Legacy Code References

**Action:** Move with README explaining temporary nature

Files:

- [docs_old/LegacySimRacingFFB.md](docs_old/LegacySimRacingFFB.md) → `docs/legacy-references/LegacySimRacingFFB.md`

**Create:** `docs/legacy-references/README.md`

Content:

```markdown
# Legacy References

This directory contains references to the legacy SimRacingFFB application codebase.

**Purpose:** These documents provide implementation references during development.

**GitHub Repository:** https://github.com/edswafford/SimRacingFFB

**Note:** This directory and its contents should be reviewed for removal when the project reaches completion.
```

### 5.2 Move Coding Standards

Files:

- [docs_old/IDesign C# Coding Standard.md](docs_old/IDesign C# Coding Standard.md) → `references/standards/IDesign C# Coding Standard.md`
- [docs_old/IDesign_C_Coding_Standard_3.0.pdf](docs_old/IDesign_C_Coding_Standard_3.0.pdf) → `references/standards/IDesign_C_Coding_Standard_3.0.pdf`

### 5.3 Move Tool Documentation

Files:

- [docs_old/Visual Studio Requirements.md](docs_old/Visual Studio Requirements.md) → `references/tools/Visual Studio Requirements.md`

### 5.4 Move External References

Files:

- [docs_old/otherRacingSimFFBApps/Apps for Force Feedback Control.md](docs_old/otherRacingSimFFBApps/Apps for Force Feedback Control.md) → `references/external/other-ffb-apps/Apps for Force Feedback Control.md`
- [docs_old/otherRacingSimFFBApps/gamesInfo.md](docs_old/otherRacingSimFFBApps/gamesInfo.md) → `references/external/other-ffb-apps/gamesInfo.md`
- [docs_old/otherRacingSimFFBApps/open source FFB apps.md](docs_old/otherRacingSimFFBApps/open source FFB apps.md) → `references/external/other-ffb-apps/open source FFB apps.md`
- [docs_old/Racing Simulator Force Feedback Design.md](docs_old/Racing Simulator Force Feedback Design.md) → `references/external/Racing Simulator Force Feedback Design.md`

---

## Phase 6: Move AI and Development Documentation

### 6.1 Move AI Prompts

Files:

- [docs_old/prompts/Ai coding prompt guide.md](docs_old/prompts/Ai coding prompt guide.md) → `docs/AI/prompts/Ai coding prompt guide.md`
- [docs_old/prompts/configure_stylecop_linting.md](docs_old/prompts/configure_stylecop_linting.md) → `docs/AI/prompts/configure_stylecop_linting.md`
- [docs_old/prompts/marvin_herbold_context.md](docs_old/prompts/marvin_herbold_context.md) → `docs/AI/prompts/marvin_herbold_context.md`

**Note:** Delete [docs_old/prompts/askForMakingPrompt.txt](docs_old/prompts/askForMakingPrompt.txt) (empty/obsolete)

### 6.2 Move Development Notes

Files:

- [docs_old/myChanges/eample-unrelated-tdd-feature-implementation prompt.md](docs_old/myChanges/eample-unrelated-tdd-feature-implementation prompt.md) → `docs/AI/prompts/` (if useful) or DELETE
- [docs_old/myChanges/lintingRulesReviewed.md](docs_old/myChanges/lintingRulesReviewed.md) → `docs/archive/` or DELETE

---

## Phase 7: Archive Old Documentation

### 7.1 Archive Architecture Documents

Files:

- [docs/Rscing Simulator Force Feedback Architecture.md](docs/Rscing Simulator Force Feedback Architecture.md) → `docs/archive/templates/Racing Simulator Force Feedback Architecture.md`
- [docs_old/Architecture/BuildingTradeoffs.md](docs_old/Architecture/BuildingTradeoffs.md) → `docs/archive/architecture/BuildingTradeoffs.md`
- [docs_old/Architecture/HybridImplementationPlan.md](docs_old/Architecture/HybridImplementationPlan.md) → `docs/archive/architecture/HybridImplementationPlan.md`

### 7.2 Archive Implementation Plans

Files:

- [docs_old/plans/git_setup_and_project_initialization.md](docs_old/plans/git_setup_and_project_initialization.md) → `docs/archive/implementation-plans/git_setup_and_project_initialization.md`
- [docs_old/plans/tdd_starting_layer_analysis.md](docs_old/plans/tdd_starting_layer_analysis.md) → `docs/archive/implementation-plans/tdd_starting_layer_analysis.md`
- [docs_old/plans/value_objects_analysis_from_legacy.md](docs_old/plans/value_objects_analysis_from_legacy.md) → `docs/archive/implementation-plans/value_objects_analysis_from_legacy.md`

### 7.3 Archive Detail Design Template

File:

- [docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md](docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md) → `docs/archive/detailed-design/Racing Simulator Force Feedback Detail Design.md`

---

## Phase 8: Delete Obsolete Files

### 8.1 Delete Word Documents

- Delete: `docs_old/wordDocs/` (entire directory)

### 8.2 Delete Empty/Obsolete Files

- Delete: [docs_old/prompts/askForMakingPrompt.txt](docs_old/prompts/askForMakingPrompt.txt)
- Delete: [docs_old/Detailed Design/Value Objects/iRacing-Specific Value Objects.md](docs_old/Detailed Design/Value Objects/iRacing-Specific Value Objects.md)

### 8.3 Delete docs_old Directory

After all content has been migrated:

- Review `docs_old/` for any missed content
- Delete entire `docs_old/` directory

---

## Phase 9: Update Navigation and Links

### 9.1 Update README.md Links

Ensure all links in the main entry point document point to the correct new locations.

### 9.2 Update Cross-References

Update any cross-references between documents to reflect new structure.

### 9.3 Create Index/Navigation

Consider adding a navigation index or table of contents to each major section.

---

## Success Criteria

- [ ] Single entry point document exists at `docs/README.md`
- [ ] All documentation follows layer-based directory structure
- [ ] Value Objects documentation transformed to capabilities/behaviors format
- [ ] No implementation details in capability documents
- [ ] Legacy references clearly marked and easy to remove
- [ ] All obsolete files deleted
- [ ] All links working and up-to-date
- [ ] `docs_old/` directory deleted
- [ ] References organized at project root
- [ ] AI prompts in dedicated directory

---

## Notes

- **Backup:** Consider creating a backup of `docs_old/` before deletion
- **Review:** Have someone review the transformed documents for accuracy
- **Git:** Consider using git to track the reorganization for easy rollback if needed
- **Incremental:** This can be done in phases; commit after each major section