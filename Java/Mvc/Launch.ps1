# Starts one program of lecture 16 on the module path (JavaFX 27, pgJDBC 42.7.13
# from the Maven wrapper's dependencies), e.g.
#   .\Launch.ps1 BookMain       # example 4 (needs COURSE_DB_URL / _USER / _PASSWORD)
#   .\Launch.ps1 BookApp        # FXML variant, same DAO and variables
#   .\Launch.ps1 SlowBookApp    # FXML variant with the in-memory 5 s demo DAO (no DB)
#   .\Launch.ps1 PropertyMain   # console, no window
# `.\mvnw.cmd javafx:run` starts the POM's mainClass (demo.BookMain) the same way.
# The COURSE_DB_* variables are read from this process; the script never prints them.
param([string]$Main = 'BookMain')
$ErrorActionPreference = 'Stop'
if (-not $env:JAVA_HOME) { $env:JAVA_HOME = 'C:\Program Files\Java\jdk-27' }
Set-Location $PSScriptRoot
$list = Join-Path $PSScriptRoot 'target\modules.txt'
& .\mvnw.cmd -q -B compile dependency:build-classpath "-Dmdep.outputFile=$list"
if ($LASTEXITCODE) { exit $LASTEXITCODE }
$path = 'target\classes;' + (Get-Content $list -Raw).Trim()
& "$env:JAVA_HOME\bin\java" --enable-native-access=javafx.graphics `
    -p $path -m "demo.fx/demo.$Main"
exit $LASTEXITCODE
