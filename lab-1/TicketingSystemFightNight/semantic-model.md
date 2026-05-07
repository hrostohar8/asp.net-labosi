# Semantic Model - Ticketing System Fight Night

## Arena (Dvorana)
- **Id** (PK) - Jedinstveni identifikator
- **Name** - Naziv arene
- **City** - Grad u kojem se nalazi
- **Capacity** - Kapacitet arene
- **Address** - Adresa
- **IsIndoor** - Je li unutarnja/vanjska
- **OpenedYear** - Godina otvaranja
- **Events** → ICollection<Event> (1-N) - Događaji koji se održavaju u areni

## Event (Događaj)
- **Id** (PK) - Jedinstveni identifikator
- **Name** - Naziv događaja
- **Organization** - FightOrganization enum (UFC, KSW, BELLATOR, FNC, ONE_FC)
- **City** - Grad
- **Date** - Datum događaja
- **Time** - Vrijeme početka
- **VenueId** (FK) → Arena - Referenca na arenu
- **Description** - Opis
- **BaseTicketPrice** - Bazna cijena karte
- **TicketsSold** - Broj prodanih karata
- **Matches** → ICollection<Match> (1-N) - Mečevi u sklopu događaja
- **Tickets** → ICollection<Ticket> (1-N) - Karte povezane s događajem

## Fighter (Borac)
- **Id** (PK) - Jedinstveni identifikator
- **Name** - Ime borca
- **Nickname** - Nadimak
- **WeightClass** - WeightClass enum (Flyweight, Bantamweight, Featherweight, Lightweight, Welterweight, Middleweight, LightHeavyweight, Heavyweight)
- **Organization** - FightOrganization
- **Country** - Država podrijetla
- **Wins** - Broj pobjeda
- **Losses** - Broj poraza
- **MatchesAsFighter1** → ICollection<Match> (1-N) - Mečevi gdje je prvi borac
- **MatchesAsFighter2** → ICollection<Match> (1-N) - Mečevi gdje je drugi borac

## Match (Meč)
- **Id** (PK) - Jedinstveni identifikator
- **Fighter1Id** (FK) → Fighter - Prvi borac
- **Fighter2Id** (FK) → Fighter - Drugi borac
- **EventId** (FK) → Event - Događaj kojem pripada
- **WeightClass** - Težinska klasa
- **RoundLimit** - Maksimalno broj rundi
- **Championship** - Je li titularni meč
- **Referee** - Sudac
- **Status** - Status meča (Scheduled, Title fight, itd.)

## Ticket (Karta)
- **Id** (PK) - Jedinstveni identifikator
- **EventId** (FK) → Event - Događaj za koji je karta
- **CartId** (FK, nullable) → Cart - Košara (ako je dodata)
- **Section** - Sektor
- **Row** - Red
- **Seat** - Sjedalo
- **Price** - Cijena
- **PurchaseDate** - Datum kupnje
- **IsVip** - Je li VIP karta

## User (Korisnik)
- **Id** (PK) - Jedinstveni identifikator
- **Name** - Ime
- **Email** - Email
- **Phone** - Telefonski broj
- **BirthDate** - Datum rođenja
- **LoyaltyPoints** - Lojalnosti bodovi
- **IsVip** - Je li VIP korisnik
- **MemberLevel** - Razina članstva (Gold, Silver, itd.)
- **Carts** → ICollection<Cart> (1-N) - Korisnikove košare

## Cart (Košara)
- **Id** (PK) - Jedinstveni identifikator
- **UserId** (FK) → User - Korisnik kojem pripada
- **Tickets** → ICollection<Ticket> (1-N) - Karte u košari
- **CreatedAt** - Vrijeme kreiranja
- **DiscountCode** - Kod za popust
- **DiscountPercent** - Postotak popusta
- **IsPaid** - Je li plaćeno

## Relacije - Pregled
- **Arena** (1) ↔ (N) **Event** - Jedna arena može imati više događaja
- **Event** (1) ↔ (N) **Match** - Jedan događaj može imati više mečeva
- **Event** (1) ↔ (N) **Ticket** - Jedan događaj može imati više karata
- **Fighter** (1) ↔ (N) **Match** - Jedan borac može biti u više mečeva (kao Fighter1 ili Fighter2)
- **User** (1) ↔ (N) **Cart** - Jedan korisnik može imati više košara
- **Cart** (1) ↔ (N) **Ticket** - Jedna košara može sadržavati više karata
