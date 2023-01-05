@echo off
REM Rename and copy *.msi and Readme.md with their version of the *.msi file
REM Create a zip archieve with Readme.md, VwTools.msi in Download folder
REM - Par.1 *.msi file
REM - Par.2 *.readme file
REM
REM Example to call the batch inside the post build event of the setup project.
REM "$(SolutionDir)Setup\Scripts\renamemsi.bat" "$(SolutionDir)Setup\bin\release\eu-us\HoTools.msi" "$(SolutionDir)HoTools_Gui\Readme.md"

REM capture script output in variable
for /f "usebackq tokens=*" %%a in (`cscript "%~dp0\getmsiversion.vbs" "%~f1"`) do (set myvar=%%a)
echo %myvar%
SET MSI_NAME="%~dp1%~n1%myvar%%~x1"
SET README_NAME="%~dp1ReadMe%myvar%.md"
SET README_NAME_OLD="%~dp1ReadMe.md"
SET ZIP_ARCHIV="%userprofile%\Downloads\VwTools_%myvar%.zip"
echo Copying *.msi:       %1 to %MSI_NAME%
eadme.md: %2 to %README_NAME%
echo Copying *.r
copy /Y %1 %MSI_NAME%
del %ZIP_ARCHIV%
cd %~dp1
tar -acf %ZIP_ARCHIV% Readme.md VwTools.msi
echo Achieve %ZIP_ARCHIV% created with Readme.md, VwTools.m

REM handle readme if exists as parameter
IF [%2] == [] GOTO :eof
copy /Y %2 %README_NAME_OLD%
copy /Y %2 %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME%
echo Copying *.readme.md: %2 to %README_NAME_OLD%

