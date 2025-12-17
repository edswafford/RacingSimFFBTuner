# Linting Rules Review

This document tracks linting rules that have been reviewed and decisions made about them.

## Rules Enabled

### **SA1005**: Single line comment should begin with a space
**Rationale:** Improves readability and consistency.

Single-line comments must begin with a space after `//` to improve readability and maintain consistent formatting across the codebase.

### **SA1009**: Closing parenthesis should not be preceded by a space
**Rationale:** Ensures consistent spacing and code formatting.

Closing parentheses should not be preceded by spaces to maintain consistent code formatting and improve readability.

### **SA1025**: Code should not contain multiple whitespace characters in a row
**Rationale:** Prevents unnecessary whitespace and maintains code clarity.

Code should not contain multiple consecutive whitespace characters to maintain clean and readable code formatting.

### **SA1111**: Closing parenthesis should be on line of last parameter
**Rationale:** Ensures consistent parenthesis placement and code structure.

Closing parentheses should be on the same line as the last parameter to maintain consistent code structure and formatting.

### **SA1208**: System using directives should be placed before other using directives
**Rationale:** Enhances code readability and maintains consistent structure.

System namespace using directives must appear before other using directives to maintain a consistent and organized code structure.

### **SA1210**: Using directives should be ordered alphabetically by namespace
**Rationale:** Enhances code readability and maintainability.

Using directives must be ordered alphabetically by namespace to provide a consistent structure and make it easier to find and manage imports.

### **SA1505**: Opening braces should not be followed by blank line
**Rationale:** Enhances code readability by maintaining consistent formatting.

Opening curly brackets should not be followed by blank lines to maintain consistent code formatting and improve readability.

### **SA1508**: Closing braces should not be preceded by blank line
**Rationale:** Ensures consistent code formatting.

Closing curly brackets should not be preceded by blank lines to maintain consistent code formatting and improve readability.

### **SA1202**: Public members should come before private members
**Rationale:** IDesign is agnostic; rule improves consistency and discoverability.

Within each element type (fields, properties, methods), public members should appear before private members. This improves code discoverability by showing the public API first. IDesign specifies element type ordering (members → properties → constructors → methods) but is agnostic about public/private ordering within each type.

### **SA1413**: Use trailing comma in multi-line initializers
**Rationale:** Improves git diff readability and makes adding new items easier.

Multi-line initializers (arrays, object initializers, collection initializers) should include a trailing comma after the last element. This produces cleaner git diffs when adding new items and maintains consistent formatting.

### **SA1518**: File is required to end with a single newline character
**Rationale:** Widely accepted standard (POSIX/Unix convention).

Files must end with exactly one newline character. This is a POSIX standard that ensures compatibility with version control systems, diff tools, and various Unix utilities.

## Rules Disabled

### **SA0001**: XML comment analysis is disabled due to project configuration
**Rationale:** Aligns with IDesign standard which states "Avoid method-level documentation" and "Use method-level comments only as tool tips for other developers."

XML comment analysis is disabled because the IDesign C# Coding Standard v3.01 discourages extensive method-level documentation, preferring self-explanatory code and external documentation for APIs. Existing XML comments can remain as tooltips without enforced analysis.

### **SA1201**: Elements should appear in the correct order
**Rationale:** Conflicts with IDesign standard element ordering.

IDesign standard (Section 1.3) specifies: members (fields), properties, constructors, methods. StyleCop SA1201 requires: fields, constructors, properties, methods. Disabled to align with IDesign standard which places properties before constructors to define the public API first, then initialization logic.

### **SA1515**: Single-line comment should be preceded by blank line
**Rationale:** Sometimes blank lines before comments reduce readability (e.g., inline attribute documentation).

Disabled because adding blank lines before inline comments in attribute documentation (like in `AssemblyInfo.cs`) would reduce readability rather than improve it.

## Build Configuration

### **NETSDK1233**: Warning suppressed
**Rationale:** Warning about .NET Standard 1.x deprecation is not applicable to this project.

The NETSDK1233 warning is suppressed in `Directory.Build.props` as this project targets .NET 10.0, not .NET Standard 1.x. The warning would only apply if targeting deprecated .NET Standard versions.

### **Visual Studio Version**: Updated to Visual Studio 2026
**Rationale:** Updated build requirements to use Visual Studio 2026.

All documentation has been updated to reference Visual Studio 2026 instead of Visual Studio 2022 to reflect current build requirements and tooling support.

