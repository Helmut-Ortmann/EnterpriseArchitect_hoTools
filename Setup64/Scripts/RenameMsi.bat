@echo off

REM capture script output in variable
for /f "usebackq tokens=*" %%a in (`cscript "%~dp0\getmsiversion.vbs" "%~f1"`) do (set myvar=%%a)

SET MSINAME="%~dp1%~n1 %myvar%%~x1"
echo Renaming to %MSINAME%
copy /Y %1 %MSINAME%

