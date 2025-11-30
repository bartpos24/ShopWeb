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

# Function to update namespaces and using statements in content
function Update-Namespaces {
    param([string]$Content)
    
    # Update namespace
    $Content = $Content -replace 'namespace ShopApiClient\.Api', 'namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api'
    # Update Client namespace references
    $Content = $Content -replace 'ShopApiClient\.Client\.', 'ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure.'
    $Content = $Content -replace 'using ShopApiClient\.Client;', 'using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;'
    # Update Models namespace references
    $Content = $Content -replace 'ShopApiClient\.Models\.', 'ShopWeb.Domain.Models.'
    $Content = $Content -replace 'using ShopApiClient\.Models;', 'using ShopWeb.Domain.Models;'
    
    return $Content
}

# Function to extract interfaces from API content and create separate interface content
function Split-ApiInterfaces {
    param(
        [string]$Content,
        [string]$FileName
    )
    
    # Extract header comment
    $headerMatch = [regex]::Match($Content, '(?s)(\/\*.*?\*\/)')
    $header = if ($headerMatch.Success) { $headerMatch.Value + "`r`n`r`n" } else { "" }
    
    # Extract using statements
    $usingPattern = '(?m)^using [^;]+;'
    $usings = [regex]::Matches($Content, $usingPattern) | ForEach-Object { $_.Value }
    $usingBlock = ($usings | Select-Object -Unique | Sort-Object) -join "`r`n"
    
    # Extract namespace
    $namespaceMatch = [regex]::Match($Content, 'namespace ([^\r\n{]+)')
    $namespace = $namespaceMatch.Groups[1].Value.Trim()
    
    # Parse line by line to find and track interfaces
    $lines = $Content -split "`r?`n"
    $interfaces = @()
    $interfaceLineRanges = @()  # Track line ranges to remove
    $inInterface = $false
    $currentInterfaceLines = @()
    $braceDepth = 0
    $interfaceStartLine = -1
    $commentStartLine = -1
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        
        # Check if we're starting an interface (looking for public interface IXxxApi)
        if (-not $inInterface -and $line -match '^\s*public interface I' + $FileName) {
            $inInterface = $true
            $interfaceStartLine = $i
            $currentInterfaceLines = @()
            $braceDepth = 0
            
            # Look backwards for XML comments and regions
            $commentLine = $i - 1
            $xmlComments = @()
            $commentStartLine = $i
            while ($commentLine -ge 0 -and ($lines[$commentLine] -match '^\s*(///|#region)' -or $lines[$commentLine] -match '^\s*$')) {
                if ($lines[$commentLine] -match '^\s*(///|#region)') {
                    $xmlComments = @($lines[$commentLine]) + $xmlComments
                    $commentStartLine = $commentLine
                }
                $commentLine--
            }
            
            # Add XML comments to interface
            $currentInterfaceLines += $xmlComments
        }
        
        if ($inInterface) {
            $currentInterfaceLines += $line
            
            # Count braces to know when interface ends
            $openBraces = ($line.ToCharArray() | Where-Object { $_ -eq '{' }).Count
            $closeBraces = ($line.ToCharArray() | Where-Object { $_ -eq '}' }).Count
            $braceDepth += ($openBraces - $closeBraces)
            
            # When braceDepth returns to 0, the interface is complete
            if ($braceDepth -eq 0 -and $line -match '\}') {
                $endLine = $i
                
                # Check for #endregion
                if (($i + 1) -lt $lines.Count -and $lines[$i + 1] -match '^\s*#endregion') {
                    $i++
                    $currentInterfaceLines += $lines[$i]
                    $endLine = $i
                }
                
                $interfaceText = ($currentInterfaceLines -join "`r`n")
                $interfaces += $interfaceText
                
                # Track the line range to remove (from comment start to end of interface)
                $interfaceLineRanges += @{
                    Start = $commentStartLine
                    End = $endLine
                }
                
                $inInterface = $false
                $currentInterfaceLines = @()
            }
        }
    }
    
    # Build interface file content if interfaces were found
    $interfaceContent = $null
    if ($interfaces.Count -gt 0) {
        $interfaceContent = $header
        $interfaceContent += $usingBlock + "`r`n`r`n"
        $interfaceContent += "namespace $namespace`r`n{`r`n"
        
        foreach ($interface in $interfaces) {
            # Indent interface content
            $interfaceLines = $interface -split "`r?`n"
            $indentedInterface = ($interfaceLines | ForEach-Object { "    $_" }) -join "`r`n"
            $interfaceContent += "`r`n$indentedInterface`r`n"
        }
        
        $interfaceContent += "}`r`n"
    }
    
    # Remove interfaces from implementation by rebuilding the lines array
    if ($interfaceLineRanges.Count -gt 0) {
        # Sort ranges by start line descending to avoid index issues
        $interfaceLineRanges = $interfaceLineRanges | Sort-Object -Property Start -Descending
        
        # Create a new lines array excluding interface ranges
        $newLines = [System.Collections.ArrayList]::new()
        $skipUntil = -1
        
        for ($i = 0; $i -lt $lines.Count; $i++) {
            $shouldSkip = $false
            
            foreach ($range in $interfaceLineRanges) {
                if ($i -ge $range.Start -and $i -le $range.End) {
                    $shouldSkip = $true
                    break
                }
            }
            
            if (-not $shouldSkip) {
                [void]$newLines.Add($lines[$i])
            }
        }
        
        $Content = $newLines -join "`r`n"
    }
    
    # Clean up the implementation content
    $Content = $Content -replace '(?m)^\s*$(\r?\n){3,}', "`r`n`r`n"
    $Content = $Content -replace '(?m)(namespace [^\r\n]+\r?\n\{)\s*(\r?\n){2,}', "`$1`r`n`r`n"
    
    return @{
        InterfaceContent = $interfaceContent
        ImplementationContent = $Content
        InterfacesFound = $interfaces.Count
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

# Update Infrastructure files to reference correct namespaces
if (Test-Path $infrastructureDest) {
    Get-ChildItem -Path $infrastructureDest -Filter "*.cs" | ForEach-Object {
        try {
            $content = Get-Content $_.FullName -Raw -ErrorAction Stop
            
            # Replace ShopApiClient.Models with ShopWeb.Domain.Models
            $content = $content -replace 'ShopApiClient\.Models\.', 'ShopWeb.Domain.Models.'
            $content = $content -replace 'using ShopApiClient\.Models;', 'using ShopWeb.Domain.Models;'
            
            # Remove ShopApiClient.Client prefix from object references
            $content = $content -replace 'ShopApiClient\.Client\.GlobalConfiguration', 'GlobalConfiguration'
            $content = $content -replace 'ShopApiClient\.Client\.Configuration', 'Configuration'
            $content = $content -replace 'ShopApiClient\.Client\.ApiClient', 'ApiClient'
            $content = $content -replace 'ShopApiClient\.Client\.ClientUtils', 'ClientUtils'
            $content = $content -replace 'ShopApiClient\.Client\.FileParameter', 'FileParameter'
            $content = $content -replace 'ShopApiClient\.Client\.OpenAPIDateConverter', 'OpenAPIDateConverter'
            $content = $content -replace 'ShopApiClient\.Client\.', ''
            
            # Remove unnecessary using statements
            $content = $content -replace 'using ShopApiClient\.Client;', ''
            
            Set-ContentSafely -Path $_.FullName -Value $content | Out-Null
        }
        catch {
            Write-Host "⚠ Skipped: $($_.Name) (file may be open in editor)" -ForegroundColor Yellow
        }
    }
    Write-Host "✓ Infrastructure namespace references updated" -ForegroundColor Green
}

# Process API files: Read, Separate, Update Namespaces, and Move
if (Test-Path $apiSource) {
    Write-Host "`nProcessing API files..." -ForegroundColor Cyan
    New-Item -ItemType Directory -Force -Path $apiDest | Out-Null
    
    $processedCount = 0
    $separatedCount = 0
    
    Get-ChildItem -Path $apiSource -Filter "*.cs" | ForEach-Object {
        Write-Host "Processing: $($_.Name)" -ForegroundColor Gray
        
        # Step 1: Take content from file
        $content = Get-Content $_.FullName -Raw
        $fileName = [System.IO.Path]::GetFileNameWithoutExtension($_.Name)
        
        # Step 2: Separate interfaces from api class
        $result = Split-ApiInterfaces -Content $content -FileName $fileName
        
        # Step 3: Change all namespace and using in both contents
        $implementationContent = Update-Namespaces -Content $result.ImplementationContent
        
        # Step 4: Move api file to $apiDest
        $destFile = Join-Path $apiDest $_.Name
        if (Set-ContentSafely -Path $destFile -Value $implementationContent) {
            Write-Host "  ✓ Moved implementation: $($_.Name)" -ForegroundColor Green
            $processedCount++
        }
        
        # If interfaces were found, update and save them
        if ($result.InterfaceContent) {
            $interfaceContent = Update-Namespaces -Content $result.InterfaceContent
            $interfaceFile = Join-Path $apiDest "I$fileName.cs"
            
            if (Set-ContentSafely -Path $interfaceFile -Value $interfaceContent) {
                Write-Host "  ✓ Created interface: I$fileName.cs ($($result.InterfacesFound) interface(s))" -ForegroundColor Cyan
                $separatedCount++
            }
        }
        else {
            Write-Host "  ⚠ No interfaces found in $($_.Name)" -ForegroundColor Yellow
        }
    }
    
    # Step 5: Remove $apiSource directory after processing all files
    Remove-Item -Path $apiSource -Recurse -Force
    
    Write-Host "✓ Processed $processedCount API file(s)" -ForegroundColor Green
    if ($separatedCount -gt 0) {
        Write-Host "✓ Separated interfaces from $separatedCount API file(s)" -ForegroundColor Green
    }
}

# Update Model files to remove FileParameter and OpenAPIDateConverter using aliases
if (Test-Path $modelsDest) {
    Write-Host "`nCleaning up Model files..." -ForegroundColor Cyan
    $cleanedModelCount = 0
    
    Get-ChildItem -Path $modelsDest -Filter "*.cs" | ForEach-Object {
        try {
            $content = Get-Content $_.FullName -Raw -ErrorAction Stop
            $originalContent = $content
            
            # Remove the FileParameter and OpenAPIDateConverter using aliases
            $content = $content -replace 'using FileParameter = ShopWeb\.Infrastructure\.ApiClient\.OpenApiGenerate\.Infrastructure\.FileParameter;\r?\n', ''
            $content = $content -replace 'using OpenAPIDateConverter = ShopWeb\.Infrastructure\.ApiClient\.OpenApiGenerate\.Infrastructure\.OpenAPIDateConverter;\r?\n', ''
            $content = $content -replace 'using OpenAPIClientUtils = ShopWeb\.Infrastructure\.ApiClient\.OpenApiGenerate\.Infrastructure\.ClientUtils;\r?\n', ''
            
            # Also remove legacy patterns if they exist
            $content = $content -replace 'using FileParameter = ShopApiClient\.Client\.FileParameter;\r?\n', ''
            $content = $content -replace 'using OpenAPIDateConverter = ShopApiClient\.Client\.OpenAPIDateConverter;\r?\n', ''
            $content = $content -replace 'using OpenAPIClientUtils = ShopApiClient\.Client\.ClientUtils;\r?\n', ''
            
            # Only write if content changed
            if ($content -ne $originalContent) {
                if (Set-ContentSafely -Path $_.FullName -Value $content) {
                    $cleanedModelCount++
                    Write-Host "  ✓ Cleaned: $($_.Name)" -ForegroundColor Cyan
                }
            }
        }
        catch {
            Write-Host "  ⚠ Skipped: $($_.Name) (file may be open in editor)" -ForegroundColor Yellow
        }
    }
    
    if ($cleanedModelCount -gt 0) {
        Write-Host "✓ Cleaned $cleanedModelCount model file(s)" -ForegroundColor Green
    }
    else {
        Write-Host "✓ No model files needed cleaning" -ForegroundColor Green
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

Write-Host "`n✓ API client generation complete!" -ForegroundColor Green