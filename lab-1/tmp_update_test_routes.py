from pathlib import Path

replacements = {
    '/api/arena': '/api/arenaapi',
    '/api/fighter': '/api/fighterapi',
    '/api/event': '/api/eventapi',
    '/api/match': '/api/matchapi',
    '/api/ticketshopmodel': '/api/ticketshopmodelapi',
    '/api/ticket': '/api/ticketapi',
    '/api/weightclass': '/api/weightclassapi',
    '/api/fightorganization': '/api/fightorganizationapi',
    '/api/user': '/api/userapi',
    '/api/cart': '/api/cartapi'
}

files = [
    Path('TicketingSystemFightNight.Tests') / 'Api' / 'ArenaApiTests.cs',
    Path('TicketingSystemFightNight.Tests') / 'Api' / 'ApiTests.cs'
]

for file in files:
    path = Path('TicketingSystemFightNight') / file
    if not path.exists():
        path = Path(file)
    text = path.read_text(encoding='utf-8')
    for old, new in replacements.items():
        text = text.replace(old, new)
    path.write_text(text, encoding='utf-8')
    print(f'Updated {path}')
