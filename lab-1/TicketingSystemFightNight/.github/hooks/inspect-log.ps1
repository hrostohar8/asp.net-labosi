$path = "conversation_log.txt"
$data = [System.IO.File]::ReadAllBytes($path)
Write-Output ("Length: {0}" -f $data.Length)
$nullCount = ($data | Where-Object { $_ -eq 0 } | Measure-Object).Count
Write-Output ("Null count: {0}" -f $nullCount)
$data[0..200] | Format-Hex
