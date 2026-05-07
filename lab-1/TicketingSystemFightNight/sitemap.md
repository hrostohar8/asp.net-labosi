# Sitemap - URL Routing Map

## Routing Pregled

| URL | Controller | Akcija | View | Metoda | Opis |
|-----|-----------|--------|------|--------|------|
| `/` | Home | Index | Index.cshtml | GET | Početna stranica |
| `/pocetna` | Home | Index | Index.cshtml | GET | Početna (custom route) |
| `/borci` | Fighter | Index | Index.cshtml | GET | Lista svih boraca |
| `/borci/svi` | Fighter | Index | Index.cshtml | GET | Lista svih boraca (alternativa) |
| `/borci/detalji/{id}` | Fighter | Details | Details.cshtml | GET | Detalji borca (ID: integer) |
| `/dogadaji` | Event | Index | Index.cshtml | GET | Lista svih događaja |
| `/dogadaji/svi` | Event | Index | Index.cshtml | GET | Lista svih događaja (alternativa) |
| `/dogadaji/detalji/{id}` | Event | Details | Details.cshtml | GET | Detalji događaja (ID: integer) |
| `/match` | Match | Index | Index.cshtml | GET | Lista mečeva |
| `/match/details/{id}` | Match | Details | Details.cshtml | GET | Detalji meča |
| `/arena` | Arena | Index | Index.cshtml | GET | Lista arena |
| `/arena/details/{id}` | Arena | Details | Details.cshtml | GET | Detalji arene |
| `/ticket` | Ticket | Index | Index.cshtml | GET | Lista karata |
| `/ticket/details/{id}` | Ticket | Details | Details.cshtml | GET | Detalji karte |
| `/user` | User | Index | Index.cshtml | GET | Lista korisnika |
| `/user/details/{id}` | User | Details | Details.cshtml | GET | Detalji korisnika |
| `/cart` | Cart | Index | Index.cshtml | GET | Lista košara |
| `/cart/details/{id}` | Cart | Details | Details.cshtml | GET | Detalji košare |
| `/dashboard` | Dashboard | Index | Index.cshtml | GET | Dashboard (admin prikaz) |

## Controller Lokacije
- **HomeController** → `Controllers/HomeController.cs`
- **FighterController** → `Controllers/FighterController.cs` *(custom routing na razini kontrolera)*
- **EventController** → `Controllers/EventController.cs` *(custom routing na razini kontrolera)*
- **MatchController** → `Controllers/MatchController.cs`
- **ArenaController** → `Controllers/ArenaController.cs`
- **TicketController** → `Controllers/TicketController.cs`
- **UserController** → `Controllers/UserController.cs`
- **CartController** → `Controllers/CartController.cs`
- **DashboardController** → `Controllers/DashboardController.cs`

## View Lokacije
- `Views/Fighter/` - Index.cshtml, Details.cshtml
- `Views/Event/` - Index.cshtml, Details.cshtml
- `Views/Match/` - Index.cshtml, Details.cshtml
- `Views/Arena/` - Index.cshtml, Details.cshtml
- `Views/Ticket/` - Index.cshtml, Details.cshtml
- `Views/User/` - Index.cshtml, Details.cshtml
- `Views/Cart/` - Index.cshtml, Details.cshtml
- `Views/Home/` - Index.cshtml
- `Views/Dashboard/` - Index.cshtml
- `Views/Shared/` - _Layout.cshtml, _ViewImports.cshtml
