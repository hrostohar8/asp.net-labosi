import json

with open('chat.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Pogledaj CIJELI prvi request
print("=== KOMPLETAN PRVI REQUEST ===")
print(json.dumps(data['requests'][0], indent=2))