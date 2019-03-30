set pm = CreateObject("UnmanagedCOM.UnmanagedManager")
res = pm.GetPowerInfo()

WScript.Echo(res)