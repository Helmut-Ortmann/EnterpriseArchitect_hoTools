##################################################
# AddInSimpleNoAdmin Quick uninstall
#################################################
# Note: This procedure takes a long time
#
# $app = Get-WmiObject -Class Win32_Product -Filter "Name = 'AddInSimpleNoAdmin'"
# $app.Uninstall()
#
$SoftwareName = "AddInSimpleNoAdmin"
"Deinstall $SoftwareName"

# fast way to uninstall
$uninstall64 = gci "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" | foreach { gp $_.PSPath } | ? { $_ -match $SoftwareName } | select UninstallString

# uninstall 64 bit
if ($uninstall64) {
$uninstall64 = $uninstall64.UninstallString -Replace "msiexec.exe","" -Replace "/I","" -Replace "/X",""
$uninstall64 = $uninstall64.Trim()
Write "Uninstalling 64Bit..."
start-process "msiexec.exe" -arg "/X $uninstall64 /qb" -Wait}

# uninstall 32 bit
$uninstall32 = gci "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" | foreach { gp $_.PSPath } | ? { $_ -match $SoftwareName } | select UninstallString
if ($uninstall32) {
$uninstall32 = $uninstall32.UninstallString -Replace "msiexec.exe","" -Replace "/I","" -Replace "/X",""
$uninstall32 = $uninstall32.Trim()
Write "Uninstalling 32Bit..."
start-process "msiexec.exe" -arg "/X $uninstall32 /qb" -Wait}
"Deinstall $SoftwareName finished"
