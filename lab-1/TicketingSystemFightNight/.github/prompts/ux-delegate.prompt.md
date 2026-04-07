---
description: "Run UX Orchestrator so UX work is delegated to UX Subagent and logged as subagent activity."
name: "UX-Delegate"
agent: "UX Orchestrator"
argument-hint: "Example: Improve home page card hover states and spacing"
---
Delegate this UX task through the parent orchestration agent so UX Subagent is invoked as a subagent.

Task input:
${input}

Requirements:
- Use UX Subagent for the implementation work.
- Orchestrator must write START and STOP entries in `.github/hooks/subagent_invocation_log.txt` for each run.
- Keep current app behavior unchanged.
- Return the delegated result in concise form.