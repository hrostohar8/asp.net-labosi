import json

# Učitaj JSON
with open('chat2.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# APPEND mode - 'a' umjesto 'w'
with open('.github/hooks/agent_log.txt', 'a', encoding='utf-8') as out:
    out.write("\n\n" + "=" * 80 + "\n")
    out.write("COPILOT CHAT HISTORY EXPORT\n")
    out.write("TicketingSystemFightNight Project\n")
    out.write("Student: Hrvoje Rostohar\n")
    out.write("Datum: 28.03.2026\n")
    out.write("=" * 80 + "\n\n")
    
    requests = data.get('requests', [])
    
    for idx, request in enumerate(requests, 1):
        # USER MESSAGE
        message = request.get('message', {})
        parts = message.get('parts', [])
        
        user_text = ""
        if parts and len(parts) > 0:
            user_text = parts[0].get('text', '')
        
        if user_text:
            out.write(f"\n{'='*80}\n")
            out.write(f"[CONVERSATION {idx}]\n")
            out.write(f"{'='*80}\n\n")
            out.write(f"USER:\n{user_text}\n\n")
        
        # COPILOT RESPONSE (placeholder jer export nema responses)
        out.write(f"COPILOT: [Response not saved in export format]\n")
        out.write(f"\n{'-'*80}\n")

print(f"✅ Uspješno dodano {len(requests)} razgovora na kraj agent_log.txt!")
print("Provjeri: .github/hooks/agent_log.txt")