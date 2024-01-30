@echo off
REM Rename and copy *.msi and Readme.md with their version of the *.msi file
REM Create a zip archieve with Readme.md, hoTools.msi, SQLs in Download folder
REM - Par.1 *.msi file
REM - Par.2 *.readme file
REM - Par.3 SQL Directory    (currently not used)
REM - Par.4 SQL SqlReadme.md (currently not used)
REM
REM Example to call the batch inside the post build event of the setup project.
REM "$(SolutionDir)HoTools_Setup\Scripts\renamemsi.bat" "$(SolutionDir)HoTools_Setup\bin\release\eu-us\HoTools.msi" "$(SolutionDir)HoTools_Gui\Readme.md" "$(SolutionDir)EA\SQL" "$(SolutionDir)EA\SQL\SqlReadme.md"

REM capture script output in variable
for /f "usebackq tokens=*" %%a in (`cscript "%~dp0\getmsiversion.vbs" "%~f1"`) do (set myvar=%%a)
echo Generated Version: %myvar%
SET MSI_NAME="%~dp1%~n1%myvar%%~x1"
SET README_NAME="%~dp1ReadMe%myvar%.md"
SET README_NAME_OLD="%~dp1ReadMe.md"
SET ZIP_ARCHIV="%userprofile%\Downloads\HoTools_%myvar%.zip"
echo Copying *.msi:       from %1 to %ZIP_ARCHIV%
echo Copying *.readme.md: from %2 to %ZIP_ARCHIV%
copy /Y %1 %MSI_NAME%
REM
REM Create Zip-Archieve
del %ZIP_ARCHIV%
REM Release folder
cd %~dp1
7z a -aoa %ZIP_ARCHIV% HoTools.msi readme.md  >NUL:

REM handle readme if exists as parameter
IF [%2] == [] GOTO :eof
copy /Y %2 %README_NAME_OLD%
copy /Y %2 %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME_OLD%

echo zip create for x86


