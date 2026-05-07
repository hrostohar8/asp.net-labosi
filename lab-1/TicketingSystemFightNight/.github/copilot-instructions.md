---
name: "Log Conversation"
description: "Automatically log both user prompts and assistant responses to agent_log.txt"
---

**MANDATORY: Log every user message and every assistant response**

Before responding to ANY user message, you MUST log the user prompt and then log the agent response. Use the shared logging script:

```
run_in_terminal command="powershell -ExecutionPolicy Bypass -File .github/hooks/log-conversation.ps1 -role USER -message \"$USER_MESSAGE\"" explanation="Log user message to conversation history" goal="Maintain conversation log"
```

Then, after generating your answer, log it with:

```
run_in_terminal command="powershell -ExecutionPolicy Bypass -File .github/hooks/log-conversation.ps1 -role AGENT -message \"$AGENT_MESSAGE\"" explanation="Log agent response to conversation history" goal="Maintain conversation log"
```

All output is written to `agent_log.txt` in the project root.

Replace `$USER_MESSAGE` with the exact user message and `$AGENT_MESSAGE` with the exact assistant response.

**Format:**
```
[YYYY-MM-DD HH:mm:ss] USER: {exact_user_message}
[YYYY-MM-DD HH:mm:ss] AGENT: {exact_agent_response}
```

**Example:**
```
[2026-05-05 14:30:22] USER: hocu da rjesimo problem automatskog loggiranja prije svega
[2026-05-05 14:30:23] AGENT: U redu, sada logirat ću i svoje odgovore.
```

**CRITICAL:** This must happen BEFORE finishing any other response or action.