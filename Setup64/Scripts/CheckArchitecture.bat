rem ---------------------------------------------------
rem Check architecture of Assemblies
rem ---------------------------------------------------
rem https://stackoverflow.com/questions/270531/how-can-i-determine-if-a-net-assembly-was-built-for-x86-or-x64
rem https://web.archive.org/web/20130424225355/http://theruntime.com/blogs/brianpeek/archive/2007/11/13/x64-development-with-net.aspx
rem 
REM Option   PE     32Bit
rem x86      PE32   1
rem Any CPU  PE32   0
rem X64      OE32+  0



SET CorFlags=c:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\CorFlags.exe
del *.wxs

"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoToolsX64\Utils.dll"
"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoToolsX64\hoToolsGui.dll"
"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoToolsX64\hoToolsRoot.dll"
"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoTools\Utils.dll"
"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoTools\hoToolsGui.dll"
"%CorFlags%" "c:\Users\Helmut Ortmann\AppData\Local\Apps\ho\hoTools\hoToolsRoot.dll"

