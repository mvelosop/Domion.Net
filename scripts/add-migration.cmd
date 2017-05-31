@echo off
cls
echo/
echo //--------------------------------------------------------------------------------------
echo // CREATE MIGRATIONS SCRIPT
echo //
echo // Required input:
echo // --------------
echo // Project name   : The project that contains the DbContext
echo // DbContext name : The DbContext for the migration
echo // Migration name : This script will add the "Migration_<dbContext>" suffix to the name
echo //--------------------------------------------------------------------------------------
echo/
set /p project="Project name   : "
set /p dbContext="DbContext name : "
set /p name="Migration name : "

set scriptsDir=%cd%
set cliProjectDir="..\samples\DFlow.CLI"

@echo cd %cliProjectDir%
cd %cliProjectDir%

@echo dotnet ef migrations add %name%Migration_%dbContext% -p ..\%project% -c %dbContext%
dotnet ef migrations add %name%Migration_%dbContext% -p ..\%project% -c %dbContext%

cd %scriptsDir%
