option explicit
'[path=\Framework\ho\RunDll]
'[group=HybridScripting]


!INC Local Scripts.EAConstants-VBScript
!INC HybridScripting.RunCommand

'
' VB Example to run c# implemented as .net dll 
' The .net dll has to
' - be registered
' - developed according to COM guidelines 
sub main
	dim myObj
	Set myObj = CreateObject("HybridScriptingDll.HybridScript")
	Session.Output "Test hybrid scripting '" & Repository.LibraryVersion & "' "
	myObj.ProcessId = ProcessId("EA.exe")
	myObj.PrintModel()
	Session.Output "Test Hybrid Scripting"
end sub

main 'Run main
Session.Output "Test Hybrid Scripting finished"