rem
rem Register HybridScripting.dll as COM
rem !! Needs administration rights !!!
rem
rem make a .reg file to use it to register dll  as COM
rem Location registry file: .bin\release\HybridScriptingDll.reg
c:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase d:\hoData\Development\GitHub\EnterpriseArchitect_hoTools\HybridScriptingDll\bin\Release\HybridScriptingDll.dll /regfile HybridScripting.reg
rem Register dll  as COM
c:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase d:\hoData\Development\GitHub\EnterpriseArchitect_hoTools\HybridScriptingDll\bin\Release\HybridScriptingDll.dll