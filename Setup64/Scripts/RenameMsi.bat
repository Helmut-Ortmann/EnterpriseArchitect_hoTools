@echo off
REM Rename and copy *.msi and Readme.md with their version of the *.msi file
REM Update a zip archieve with VwToolsX64.msi in Download folder
REM !!!!!First buld x86 and then x64!!!!!
REM !!!!!7Z has to be installed and the PATH-Variable has to contain it!!!!!
REM - Par.1 *.msi file
REM - Par.2 *.readme file
REM
REM Example to call the batch inside the post build event of the setup project.
REM "$(SolutionDir)SetupX64\Scripts\renamemsi.bat" "$(SolutionDir)SetupX64\bin\release\eu-us\HoTools.msi" "$(SolutionDir)HoTools_Gui\Readme.md"

REM capture script output in variable
for /f "usebackq tokens=*" %%a in (`cscript "%~dp0\getmsiversion.vbs" "%~f1"`) do (set myvar=%%a)
echo %myvar%
SET MSI_NAME="%~dp1%~n1%myvar%%~x1"
SET README_NAME="%~dp1ReadMe%myvar%.md"
SET README_NAME_OLD="%~dp1ReadMe.md"
SET ZIP_ARCHIV="%userprofile%\Downloads\VwTools_%myvar%.zip"
echo Copying *.msi:       %1 to %MSI_NAME%
echo Copying *.readme.md: %2 to %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME_OLD%
copy /Y %1 %MSI_NAME%
REM
REM Update Zip-Archieve
cd %~dp1
7z a %ZIP_ARCHIV% VwToolsX64.msi >NUL:
echo Achieve %ZIP_ARCHIV% added VwToolsX64.msi

REM handle readme if exists as parameter
IF [%2] == [] GOTO :eof
copy /Y %2 %README_NAME_OLD%
copy /Y %2 %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME_OLD%



