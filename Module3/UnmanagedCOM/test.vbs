set pm = CreateObject("PowerManagementCOM.PowerManagerCom")
res = pm.GetPowerInfo()

WScript.Echo(res)