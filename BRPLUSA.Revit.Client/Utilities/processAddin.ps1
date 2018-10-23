function replace-file-content([string] $path, [string] $replace, [string] $replaceWith)
{
(Get-Content -Path $path) |
Foreach-Object {$_ -replace $replace,$replaceWith}|
Out-File $path
}

# get current directory
$root = (Get-Item ".\" -Verbose).FullName
$baseDir = [string]::Concat($root, "\Utilities")
echo "Base Path is:  $baseDir"

# find addin in the current directory
$addin = "BRPLUSA.DEBUG.addin";

# get temp variable
$value = (Get-Item Env:\TEMP).Value
echo "The temp folder is located here: $value"

# replace %temp% with actual variable path
$addinPath = [string]::Concat($baseDir, "\", $addin)
$searchItem = "%TEMP%"
echo "Replacing: $searchItem with: $value in: $addinPath"
replace-file-content $addinPath $searchItem $value
