---
name: "Entity Framework Skill"
description: "Guide for working with Entity Framework Core models, migrations, and CRUD operations"
---

# Entity Framework Skill

Koristi ovaj skill kada:
- Dodaješ ili mijenjаš EF model klase
- Generiraš migracije
- Radiš s DbContext-om
- Koristiš CRUD operacije nad bazom
- Trebаš Include() za eager loading
- Trebаš AsNoTracking() za read-only upite

## Model Best Practices

### Atributi koje svaka model klasa mora imati:
```csharp
[Key]
public int Id { get; set; }
```

### Foreign Key relacije:
```csharp
[ForeignKey("Entity")]
public int EntityId { get; set; }
public virtual Entity Entity { get; set; } = null!;
```

### Navigation Properties (1-N):
```csharp
public virtual ICollection<RelatedEntity> RelatedEntities { get; set; } = new List<RelatedEntity>();
```

### Svaka navigation property mora biti virtual za lazy loading:
```csharp
public virtual Arena Venue { get; set; } = null!;
```

## DbContext Konfiguracija

U `Program.cs`:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationDbContext")));
```

U `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "ApplicationDbContext": "Data Source=127.0.0.1;Initial Catalog=baza;User ID=sa;Password=pwd;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  }
}
```

## Migracije

```bash
# Nova migracija
dotnet ef migrations add NazivMigracije

# Primjena na bazu
dotnet ef database update

# SQL skripta
dotnet ef migrations script FROM TO
```

## CRUD Operacije

### Create
```csharp
var entity = new Entity { Property = value };
_context.Entities.Add(entity);
_context.SaveChanges();
```

### Read (s Include za relacije)
```csharp
var item = _context.Items
    .Include(x => x.RelatedEntity)
    .FirstOrDefault(x => x.Id == id);
```

### Update
```csharp
var entity = _context.Items.Find(id);
entity.Property = newValue;
_context.SaveChanges();
```

### Delete
```csharp
var entity = _context.Items.Find(id);
_context.Items.Remove(entity);
_context.SaveChanges();
```

## Česte Greške

❌ **Zaboravljeni virtual** - navigation properties moraju biti virtual
❌ **Nedostaje ForeignKey atribut** - EF ne može mapirati relacije
❌ **Nema parametarskog konstruktora** - EF Core trebа prazan konstruktor
❌ **Pozvan ToList() prije Include()** - eager loading ne radi
❌ **Nedostaje SaveChanges()** - promjene se ne spremaju

