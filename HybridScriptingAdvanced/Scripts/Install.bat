@echo off

REM Install 
robocopy %1 %2 *.dll *.exe /S
exit 0