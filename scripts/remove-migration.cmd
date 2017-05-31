@echo off
cls
echo/
echo //----------------------------------------------------------
echo // REMOVE LAST MIGRATION SCRIPT
echo //
echo // Required input:
echo // --------------
echo // Project name   : The project that contains the DbContext
echo // DbContext name : The DbContext for the migration
echo //----------------------------------------------------------
echo/
set /p project="Project name   : "
set /p dbContext="DbContext name : "

set scriptsDir=%cd%
set cliProjectDir="..\samples\DFlow.CLI"

@echo cd %cliProjectDir%
cd %cliProjectDir%

@echo dotnet ef migrations remove -p ..\%project% -c %dbContext%
dotnet ef migrations remove -p ..\%project% -c %dbContext%

cd %scriptsDir%
