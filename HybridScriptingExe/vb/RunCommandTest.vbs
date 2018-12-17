option explicit

' See:
' SPARX Webinar Hybrid Scripting
' - http://www.sparxsystems.com/resources/webinar/release/ea13/videos/hybrid-scripting.html
' SPARX Tutorial Hyper Script
' http://www.sparxsystems.com/resources/user-guides/automation/hybrid-scripting.pdf
' Geert Bellekens Tutorial: 
' https://bellekens.com/2015/12/27/how-to-use-the-enterprise-architect-vbscript-library/
'[path=\Framework\ho\run]
'[group=HybridScripting]
'-------------------------------------------------
' RunCommandTest
'
' Runs a Windows command in the script environment
' It's a convinient way to make Script by compiled languages.
' EA supports this under the name 'HyperScript'. There is a whole development environment inside EA
' EA Hyperscript supports languages like "JAVA", "C#".
'
' How it's working:
' - Develop your console application according to EA examples
' -- Deploy the *.exe and the *.dlls
' - Consider using the Windows %PATH$ variable to easily access the console application (no need to use whole path)
' - Adapt this example to your needs
' - Run
'

!INC Local Scripts.EAConstants-VBScript
!INC HybridScripting.RunCommand

sub main
    Dim result
	Dim script
	' Productive
	script = "c:\Temp\EaScripts\HybridScriptingAdvanced.exe"
	' Debug
	'script = "c:\hoData\Development\GitHub\EnterpriseArchitect_hoTools\HybridScriptingAdvanced\bin\Debug\HybridScriptingAdvanced.exe"
	result = RunCommand(script, "", " ")
	
	Session.Output vbCRLF & vbCRLF & vbCRLF 
	Session.Output "------------------------------------------------"
    Session.Output "RunCommand(..,..), Return value:" & vbCRLF & result
	Session.Output "------------------------------------------------"
	
	MsgBox "Command:'"  & vbCRLF & script & _
     	  "'" & vbCRLF & vbCRLF & _
		  "first 10000 characters of return" & _
		  vbCRLF & "'" & _
		  Mid(result,1,10000) & "'" & vbCRLF, _
    	  65, _
		  "Command run!"
end sub

'-------------------------------------------------------
' main   Runs the command
main