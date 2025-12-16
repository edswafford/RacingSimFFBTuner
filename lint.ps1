# Quick script to check for linting issues across the entire solution

Write-Host "Running linter across entire solution..." -ForegroundColor Cyan

# Build the solution and capture output
$buildOutput = dotnet build RacingSimFFB.sln --no-incremental 2>&1

# Filter for StyleCop and IDE warnings/errors
$lintingIssues = $buildOutput | Select-String -Pattern "(warning|error) (SA|IDE)\d+"

if ($lintingIssues) {
    Write-Host "`nFound $($lintingIssues.Count) linting issue(s):`n" -ForegroundColor Yellow
    $lintingIssues | ForEach-Object { Write-Host $_ }
    
    # Group by type
    $warnings = ($lintingIssues | Select-String -Pattern "warning").Count
    $errors = ($lintingIssues | Select-String -Pattern "error").Count
    
    Write-Host "`nSummary: $warnings warning(s), $errors error(s)" -ForegroundColor $(if ($errors -gt 0) { "Red" } else { "Yellow" })
    
    exit 1
}
else {
    Write-Host "No linting issues found!" -ForegroundColor Green
    exit 0
}
