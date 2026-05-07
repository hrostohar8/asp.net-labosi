param(
    [Parameter(Mandatory=$true)]
    [string]$role,

    [Parameter(Mandatory=$true)]
    [string]$message,

    [string]$path = "agent_log.txt"
)

$ErrorActionPreference = "Stop"
$repoRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName
$logFile = Join-Path $repoRoot $path
$ts = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$line = "[$ts] ${role}: $message"
Add-Content -Path $logFile -Value $line -Encoding UTF8
