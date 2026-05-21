import re
import requests

url = 'http://localhost:5000/Fighter/Create'
with requests.Session() as s:
    r = s.get(url)
    r.raise_for_status()
    match = re.search(r'name="__RequestVerificationToken" value="([^"]+)"', r.text)
    if not match:
        raise RuntimeError('Token not found')
    token = match.group(1)
    payload = {
        '__RequestVerificationToken': token,
        'Name': '',
        'Nickname': 'Test',
        'WeightClassId': '1',
        'OrganizationId': '1',
        'Country': 'HR',
        'Wins': '0',
        'Losses': '0'
    }
    post = s.post(url, data=payload)
    print('POST status', post.status_code)
    print('Contains validation error?', 'Ime borca je obavezno' in post.text)
    print('Returned URL path', post.url)
