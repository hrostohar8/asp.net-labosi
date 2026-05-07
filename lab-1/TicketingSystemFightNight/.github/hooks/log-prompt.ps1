#!/usr/bin/env pwsh
# Log user prompts to agent_log.txt

$ErrorActionPreference = "SilentlyContinue"

# Read the JSON input
$input_json = @"
$([System.Console]::In.ReadToEnd())
"@

$ts = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'

# Try to extract the prompt text from JSON
try {
    $data = $input_json | ConvertFrom-Json
    if ($data.userMessage -and $data.userMessage.text) {
        $text = $data.userMessage.text
    } elseif ($data.text) {
        $text = $data.text
    } else {
        $text = $input_json
    }
} catch {
    $text = $input_json
}

# Clean up the text
if ([string]::IsNullOrWhiteSpace($text)) {
    $text = "no-payload"
} else {
    $text = $text.Trim()
}

# Write to log file
$logFilePath = Join-Path $PSScriptRoot "agent_log.txt"
$logEntry = "[$ts] USER_PROMPT: $text"
Add-Content -Path $logFilePath -Value $logEntry -Encoding UTF8

# Return success
@"
{
  "continue": true
}
"@
