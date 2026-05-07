$ErrorActionPreference = 'Stop'
$root = (Get-Item $PSScriptRoot).Parent.Parent.FullName
$agentPath = Join-Path $root '.github\hooks\agent_log.txt'
$convPath = Join-Path $root 'conversation_log.txt'
$mergedPath = Join-Path $root 'merged_agent_log.txt'

if (-Not (Test-Path $agentPath)) {
    Write-Error "agent_log.txt not found at $agentPath"
    exit 1
}
if (-Not (Test-Path $convPath)) {
    Write-Error "conversation_log.txt not found at $convPath"
    exit 1
}

$agentContent = Get-Content -Path $agentPath -Raw
$convContent = Get-Content -Path $convPath -Raw
Set-Content -Path $mergedPath -Value ($agentContent + [Environment]::NewLine + $convContent) -Encoding UTF8
Remove-Item -Path $agentPath -Force
Remove-Item -Path $convPath -Force
Rename-Item -Path $mergedPath -NewName 'agent_log.txt' -Force
