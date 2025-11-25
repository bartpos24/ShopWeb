# Generate
java -jar openapi-generator-cli.jar generate -c ./ShopWeb.Infrastructure/ApiClient/openApiConfig.json

# Organize files
$generatedBase = "./ShopWeb.Infrastructure/ApiClient/OpenApiGenerate"
$modelsSource = "$generatedBase/ShopApiClient/Models"
$modelsDest = "./ShopWeb.Domain/Models"
$clientSource = "$generatedBase/ShopApiClient/Client"
$infrastructureDest = "$generatedBase/Infrastructure"
$apiSource = "$generatedBase/ShopApiClient/Api"
$apiDest = "$generatedBase/Api"

# Function to safely write content with retry logic
function Set-ContentSafely {
    param(
        [string]$Path,
        [string]$Value,
        [int]$MaxRetries = 3
    )
    
    $retryCount = 0
    while ($retryCount -lt $MaxRetries) {
        try {
            Set-Content -Path $Path -Value $Value -ErrorAction Stop
            return $true
        }
        catch [System.IO.IOException] {
            $retryCount++
            if ($retryCount -lt $MaxRetries) {
                Write-Host "⚠ File locked: $Path. Retrying in 1 second... ($retryCount/$MaxRetries)" -ForegroundColor Yellow
                Start-Sleep -Seconds 1
            }
            else {
                Write-Host "✗ Failed to write: $Path - File is locked by another process" -ForegroundColor Red
                Write-Host "  Please close this file in Visual Studio and run the script again." -ForegroundColor Yellow
                return $false
            }
        }
    }
}

# Move models to Domain project
if (Test-Path $modelsSource) {
    New-Item -ItemType Directory -Force -Path $modelsDest | Out-Null
    $failedFiles = @()
    
    Get-ChildItem -Path $modelsSource -Filter "*.cs" | ForEach-Object {
        # Update namespace in files
        $content = Get-Content $_.FullName -Raw
        $content = $content -replace 'namespace ShopApiClient\.Models', 'namespace ShopWeb.Domain.Models'
        
        # Write to destination
        $destFile = Join-Path $modelsDest $_.Name
        if (-not (Set-ContentSafely -Path $destFile -Value $content)) {
            $failedFiles += $_.Name
        }
    }
    
    # Remove original models
    Remove-Item -Path $modelsSource -Recurse -Force
    
    if ($failedFiles.Count -eq 0) {
        Write-Host "✓ Models moved to ShopWeb.Domain/Models" -ForegroundColor Green
    }
    else {
        Write-Host "⚠ Models moved with $($failedFiles.Count) error(s)" -ForegroundColor Yellow
        Write-Host "  Failed files: $($failedFiles -join ', ')" -ForegroundColor Yellow
    }
}

# Move Client infrastructure files to Infrastructure directory
if (Test-Path $clientSource) {
    New-Item -ItemType Directory -Force -Path $infrastructureDest | Out-Null
    Get-ChildItem -Path $clientSource -Filter "*.cs" | ForEach-Object {
        # Update namespace in files
        $content = Get-Content $_.FullName -Raw
        $content = $content -replace 'namespace ShopApiClient\.Client', 'namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure'
        
        # Write to destination
        $destFile = Join-Path $infrastructureDest $_.Name
        Set-ContentSafely -Path $destFile -Value $content | Out-Null
    }
    
    # Remove original Client directory
    Remove-Item -Path $clientSource -Recurse -Force
    Write-Host "✓ Infrastructure files moved to OpenApiGenerate/Infrastructure" -ForegroundColor Green
}

# Move API files to Api directory
if (Test-Path $apiSource) {
    New-Item -ItemType Directory -Force -Path $apiDest | Out-Null
    Get-ChildItem -Path $apiSource -Filter "*.cs" | ForEach-Object {
        $content = Get-Content $_.FullName -Raw
        # Update namespace
        $content = $content -replace 'namespace ShopApiClient\.Api', 'namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api'
        # Update Client namespace references
        $content = $content -replace 'ShopApiClient\.Client\.', 'ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure.'
        $content = $content -replace 'using ShopApiClient\.Client;', 'using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;'
        # Update Models namespace references
        $content = $content -replace 'ShopApiClient\.Models\.', 'ShopWeb.Domain.Models.'
        $content = $content -replace 'using ShopApiClient\.Models;', 'using ShopWeb.Domain.Models;'
        
        # Write to destination
        $destFile = Join-Path $apiDest $_.Name
        Set-ContentSafely -Path $destFile -Value $content | Out-Null
    }
    
    # Remove original Api directory
    Remove-Item -Path $apiSource -Recurse -Force
    Write-Host "✓ API files moved to OpenApiGenerate/Api" -ForegroundColor Green
}

# Update Model files to reference new Infrastructure namespace
if (Test-Path $modelsDest) {
    Get-ChildItem -Path $modelsDest -Filter "*.cs" | ForEach-Object {
        # Skip if file is already being processed or locked
        try {
            $content = Get-Content $_.FullName -Raw -ErrorAction Stop
            # Update references to Client classes (FileParameter, OpenAPIDateConverter, ClientUtils)
            $content = $content -replace 'using FileParameter = ShopApiClient\.Client\.FileParameter;', 'using FileParameter = ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure.FileParameter;'
            $content = $content -replace 'using OpenAPIDateConverter = ShopApiClient\.Client\.OpenAPIDateConverter;', 'using OpenAPIDateConverter = ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure.OpenAPIDateConverter;'
            $content = $content -replace 'using OpenAPIClientUtils = ShopApiClient\.Client\.ClientUtils;', 'using OpenAPIClientUtils = ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure.ClientUtils;'
            
            Set-ContentSafely -Path $_.FullName -Value $content | Out-Null
        }
        catch {
            Write-Host "⚠ Skipped: $($_.Name) (file may be open in editor)" -ForegroundColor Yellow
        }
    }
}

# Clean up unnecessary files
Remove-Item -Path "$generatedBase/api/openapi.yaml" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/git_push.sh" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/README.md" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/appveyor.yml" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/.openapi-generator-ignore" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/.gitignore" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/*.sln" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/*.csproj" -ErrorAction SilentlyContinue
Remove-Item -Path "$generatedBase/ShopApiClient" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "✓ API client generation complete!" -ForegroundColor Green