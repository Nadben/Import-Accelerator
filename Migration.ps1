param (
    [string]$StagingMigration,
    [string]$LandingMigration
)

# Change this to the actual project path in our case (Accelerator.Shared.Infrastructure)
$ProjectPath = "path\to\your\project" 
Set-Location $ProjectPath

function Apply-MigrationAndUpdateDatabase {
    param (
        [string]$Context,
        [string]$Migration
    )

    if (-not $Migration) {
        Write-Host "Skipping migration for $Context (No migration provided)" -ForegroundColor Yellow
        return
    }

    Write-Host "Applying migration '$Migration' for context '$Context'..." -ForegroundColor Cyan

    try {
        $existingMigrations = dotnet ef migrations list --context $Context
        if ($existingMigrations -match $Migration) {
            Write-Host "Migration '$Migration' already exists for context '$Context', skipping 'add' step." -ForegroundColor Yellow
        } else {
            dotnet ef migrations add $Migration --context $Context
            if ($LASTEXITCODE -ne 0) { throw "Failed to add migration $Migration for $Context" }
        }

        dotnet ef database update --context $Context
        if ($LASTEXITCODE -ne 0) { throw "Failed to update database for $Context" }

        Write-Host "Migration '$Migration' applied successfully for context '$Context'!" -ForegroundColor Green
    }
    catch {
        Write-Host "Error: $_" -ForegroundColor Red
        exit 1
    }
}

if ($StagingMigration -or $LandingMigration) {
    if ($StagingMigration) {
        Apply-MigrationAndUpdateDatabase -Context "StagingImportContext" -Migration $StagingMigration
    }

    if ($LandingMigration) {
        Apply-MigrationAndUpdateDatabase -Context "LandingImportContext" -Migration $LandingMigration
    }

    Write-Host "Database migrations completed successfully!" -ForegroundColor Green
} else {
    Write-Host "No migrations provided. Exiting..." -ForegroundColor Yellow
    exit 0
}