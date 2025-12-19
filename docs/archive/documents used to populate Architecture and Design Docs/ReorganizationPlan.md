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

## Phase 2: Create Entry Point Documents

### 2.1 Create docs/README.md

**Purpose:** This README.md is the main entry point for the `docs/` directory, providing an overview of the documentation directory and organization. This is separate from the project root README.md.

### 2.2: Create Architecture Entry Point Document

### 2.2.1 Revise docs/Architecture/Racing Simulator Force Feedback Architecture.md

Purpose: Racing Simulator Force Feedback Architecture.md is the main entry point for docs/Architecture providing an overview of the project Architecture  structure and organization.

Currently this document is incomplete. 

**Key Content:**

- Overview of the Architecture 
- Navigation to Architecture document and layer-specific documentation
- Links to supporting documentation (AI prompts, archive, legacy references)
- Brief description of documentation organization philosophy

**Structure:**

- Overview section explaining each part of Clean Architecture  
- Links to layer-specific documentation (Domain, Application, Infrastructure, Presentation)
- Links to supporting documentation directories

**File:** [docs/README.md](docs/README.md)

**Note:** The complete architectural overview with Clean Architecture details is in [Architecture/Racing Simulator Force Feedback Architecture.md](Architecture/Racing%20Simulator%20Force%20Feedback%20Architecture.md), not in the README.md.

---

## Phase 3: Transform Value Objects Documentation

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

---

## Phase 4: Create Layer Documentation Files

### 4.1 Extract Content from Detail Design Template

**Source:** [docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md](docs_old/Detailed Design/Racing Simulator Force Feedback Detail Design.md)

**Distribution:**

1. **Domain Layer Content → Multiple Files**

   - Entities section → `docs/Architecture/Domain/Entities/Entities.md`
   - Interfaces section → `docs/Architecture/Domain/Interfaces/Interfaces.md`
   - Domain Services section → `docs/Architecture/Domain/DomainServices/DomainServices.md`
   - Connection Management → `docs/Architecture/Domain/ConnectionManagementService/ConnectionManagementService.md`
1. **Application Layer Content**

   - Real-Time Telemetry → `docs/Architecture/Application/UseCases/RealTimeTelemetryAndForceFeedback/RealTimeTelemetryAndForceFeedback.md`
   - Stop FFB → `docs/Architecture/Application/UseCases/StopForceFeedback/StopForceFeedback.md`
   - Profile Management → `docs/Architecture/Application/UseCases/LoadForceFeedbackProfile/` and `SaveForceFeedbackProfile/`
   - Data Logging → `docs/Architecture/Application/UseCases/AnalyzeLapTimes/AnalyzeLapTimes.md`
1. **Infrastructure Layer Content**

   - Gateways section → `docs/Architecture/Infrastructure/Gateways/Gateways.md`
   - Persistence section → `docs/Architecture/Infrastructure/Persistence/Persistence.md`
   - Hardware Output → `docs/Architecture/Infrastructure/HardwareOutput/HardwareOutput.md`
   - Utilities → `docs/Architecture/Infrastructure/Utilities/Utilities.md`
1. **Presentation Layer Content**

   - UI section → `docs/Architecture/Presentation/UserInterface/UserInterface.md`
   - ViewModels → `docs/Architecture/Presentation/ViewModels/ViewModels.md`
   - DI Setup → `docs/Architecture/Presentation/CompositionRoot/CompositionRoot.md`

**Format:** Each file should follow the capabilities template structure from the architecture document.

---