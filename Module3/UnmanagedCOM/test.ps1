$bytes = [System.IO.File]::ReadAllBytes("D:\d2_d3\EPAM.NET.D2_D3.Mentoring\Module3\UnmanagedCOM\bin\Debug\UnmanagedCOM.dll")
[System.Reflection.Assembly]::Load($bytes)
$srv = New-Object -TypeName "PowerManagementCOM.PowerManagerCom"
$srv.GetLastSleepTime()