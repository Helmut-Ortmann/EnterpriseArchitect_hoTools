# Readme.md

## General

An example C# Console Application (*.exe) to run from EA Script with the following features:

- Callable from VbScript, JScript, JavaScript out of EA context
- Traverse through package structure
- LINQ to SQL to get values (here count of objects)
- LINQ to SQL to run query and show the results in EA
- Installation of all *.dll, *.exe files

## Integration in EA

Use the hoToolsHybridScripts.xml to import the following VbScripts in EA (import referenced data):

- RunCommand
- RunCommandTest

Write your VbScript and insert the function call to invoke the C# Console Application:

```vbScript
   ' Productive
	script = "c:\Temp\EaScripts\HybridScriptingAdvanced.exe"
	' Debug
	'script = "c:\hoData\Development\GitHub\EnterpriseArchitect_hoTools\HybridScriptingAdvanced\bin\Debug\HybridScriptingAdvanced.exe"

	// result contains the returned string, you may pass more parameters as string
	result = RunCommand(script, "", " ")
```

## Test

1.  Compile as Debug
2.  Make startproject
3.  Set breakpoints
4.  Start debug session

