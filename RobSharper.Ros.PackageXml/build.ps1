param(
    [string] $xsdPath
)

If ($xsdPath -eq "")
{
    Write-Host "No XSD.exe path set"
    Write-Host "Call build.ps1 with path to xsd exe"
    
    exit 1
}

If ((Test-Path $xsdPath -PathType Leaf) -eq $false)
{
    Write-Host "XSD.exe not found"
    Write-Host $xsdPath
    
    exit 2
}

# Generate code
Write-Host "Generating source code files"

Set-Variable -Name "v1Path" -Value (Join-Path -Path $PSScriptRoot "V1")
Set-Variable -Name "v2Path" -Value (Join-Path -Path $PSScriptRoot "V2")
Set-Variable -Name "v3Path" -Value (Join-Path -Path $PSScriptRoot "V3")

& $xsdPath ("""{0}""" -f (Join-Path -Path $v1Path "package_format1.xsd")) ("-outputdir:""{0}""" -f $v1Path) ("/parameters:""{0}""" -f (Join-Path -Path $v1Path "xsdOptions.xml")) /nologo
& $xsdPath ("""{0}""" -f (Join-Path -Path $v2Path "package_format2.xsd")) ("-outputdir:""{0}""" -f $v2Path) ("/parameters:""{0}""" -f (Join-Path -Path $v2Path "xsdOptions.xml")) /nologo
& $xsdPath ("""{0}""" -f (Join-Path -Path $v3Path "package_format3.xsd")) ("-outputdir:""{0}""" -f $v3Path) ("/parameters:""{0}""" -f (Join-Path -Path $v3Path "xsdOptions.xml")) /nologo

Write-Host

#Build project
& dotnet build ("""{0}""" -f (Get-ChildItem -Path $PSScriptRoot\* -Include *.csproj | Select-Object -First 1))