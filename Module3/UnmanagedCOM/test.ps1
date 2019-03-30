$obj = new-object -comObject UnmanagedCOM.UnmanagedManager
$obj | get-member
$ans = $obj.GetPowerInfo()
$ans