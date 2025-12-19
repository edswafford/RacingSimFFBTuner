
## Capabilities Template

> **Purpose**  
>
> This document defines *why this component exists* and *what it must do*, independent of implementation details.  
>
> It is the primary **TDD driver document** for this component.
>
> This document should change **infrequently**. If it changes, it is usually because:
>
> - A requirement was misunderstood
> - A new legitimate capability was discovered
> - Tests revealed an invalid assumption
>
> Do **not** document implementation details here.

---

### 1. Capability

**One sentence describing why this component exists.**

- Focus on *intent*, not mechanics
- Avoid framework, technology, or class names
- Phrase it so a non-UI consumer (test, service, batch process) could invoke it

**Example:**  

> Calculates and outputs real-time force feedback commands from simulator telemetry.

---

### 2. Context

**Describe where this capability fits in the system.**

Include:

- Which layer this component belongs to
- Who/what calls it
- What it collaborates with (at a high level)

Avoid:

- Method names
- Class diagrams
- Control flow details

**Guidance questions:**

- Is this triggered by a user action, a system event, or a continuous process?
- Is this synchronous or asynchronous from the caller's perspective?

---

### 3. Inputs

**Describe the information this capability requires.**

- Use domain language, not DTO or API names
- Specify constraints and expectations
- If input is optional or conditional, say so

**Example format:**

- Telemetry data point (validated, time-ordered)
- Active force feedback profile
- Current simulator connection state

---

### 4. Outputs

**Describe what this capability produces or affects.**

Outputs may be:

- Returned values
- State changes
- Commands sent through ports

Be explicit about:

- Guarantees (e.g., always produced, best-effort, conditional)
- Observable effects

---

### 5. Behaviors

**List the externally observable behaviors that define correctness.**

Each behavior:

- Is testable
- Is phrased in business/domain terms
- Avoids algorithmic detail

Use numbered statements.

**Example:**

1. When telemetry is received, a force feedback command is produced within the same update cycle.
2. When telemetry input is invalid, no force is sent to the hardware.
3. When the system is paused, force output is zeroed.

> If a behavior cannot be tested, it does not belong here.

---

### 6. Invariants

**List conditions that must always be true, regardless of implementation.**

These guide refactoring and prevent accidental rule erosion.

**Examples:**

- Force feedback output must never exceed configured safety limits.
- Domain objects remain immutable.
- No infrastructure concern leaks into the domain.

---

### 7. Failure Modes

**Describe how this capability behaves when things go wrong.**

Include:

- Invalid input
- Missing dependencies
- External system failures

State:

- What happens
- What does *not* happen

Avoid exception-class-level detail.

---

### 8. Non-Goals

**Explicitly state what this capability does *not* do.**

This prevents scope creep and accidental coupling.

**Examples:**

- Does not manage simulator discovery
- Does not persist configuration
- Does not perform UI updates

---

### 9. Change Policy

**Define how and when this document is allowed to change.**

Typical rules:

- Adding a behavior requires a new failing test
- Removing a behavior requires explicit review
- Design changes do *not* require changes here unless behavior changes

---

### 10. Related Artifacts

**Link to relevant documents and tests.**

- Related capabilities
- High-level tests
- Architecture overview

Keep this section lightweight and navigational.

---

> **Rule of Thumb**  
>
> If you are tempted to explain *how*, you are in the wrong document.
>
> Explain *why* and *what*, then write tests.
