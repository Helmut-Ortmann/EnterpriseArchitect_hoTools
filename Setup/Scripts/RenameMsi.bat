@echo off
REM Rename and copy *.msi and Readme.md with their version of the *.msi file
REM - Par.1 *.msi file
REM - Par.2 *.readme file
REM
REM Example to call the batch inside the post build event of the setup project.
REM "$(SolutionDir)ImpactAnalysis_Addin_Setup\Scripts\renamemsi.bat" "$(SolutionDir)ImpactAnalysis_Addin_Setup\bin\release\eu-us\ImpactAnalysis.msi" "$(SolutionDir)ImpactAnalysis_Gui\Readme.md"

REM capture script output in variable
for /f "usebackq tokens=*" %%a in (`cscript "%~dp0\getmsiversion.vbs" "%~f1"`) do (set myvar=%%a)
echo %myvar%
SET MSI_NAME="%~dp1%~n1%myvar%%~x1"
SET README_NAME="%~dp1ReadMe%myvar%.md"
SET README_NAME_OLD="%~dp1ReadMe.md"
echo Copying *.msi:       %1 to %MSI_NAME%
copy /Y %1 %MSI_NAME%

REM handle readme if exists as parameter
IF [%2] == [] GOTO :eof
copy /Y %2 %README_NAME_OLD%
copy /Y %2 %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME_OLD%

