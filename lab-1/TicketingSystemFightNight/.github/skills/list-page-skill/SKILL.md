---
name: "List Page Skill"
description: "Guide for creating list/index pages with EF data retrieval and Bootstrap tables"
---

# List Page Skill

Koristi za kreiranje list/index stranica s:
- Dohvatom podataka iz Entity Framework-a
- Razor view s petljom kroz podatke
- Bootstrap tablicom
- Link-ovima na details/edit/delete akcije

## Controller Pattern

```csharp
[Route("resursi")]
public class ResourceController : Controller
{
    private readonly IRepository<Resource> _repository;

    public ResourceController(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    [HttpGet("")]
    [HttpGet("svi")]
    public IActionResult Index()
    {
        var resources = _repository.GetAll();
        return View(resources);
    }

    [HttpGet("detalji/{id:int}")]
    public IActionResult Details(int id)
    {
        var resource = _repository.GetById(id);
        if (resource == null)
            return NotFound();
        return View(resource);
    }
}
```

## View Pattern (Razor)

```html
@model IEnumerable<Resource>

<div class="container mt-4">
    <h1>Resursi</h1>
    <a href="/resursi/edit" class="btn btn-primary mb-3">Dodaj novi</a>

    @if (Model.Any())
    {
        <table class="table table-striped">
            <thead class="table-dark">
                <tr>
                    <th>ID</th>
                    <th>Naziv</th>
                    <th>Akcije</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td>@item.Id</td>
                        <td>@item.Name</td>
                        <td>
                            <a href="/resursi/detalji/@item.Id" class="btn btn-sm btn-info">Detalji</a>
                            <a href="/resursi/uredi/@item.Id" class="btn btn-sm btn-warning">Uredi</a>
                            <a href="/resursi/obrisi/@item.Id" class="btn btn-sm btn-danger">Obriši</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    }
    else
    {
        <p class="alert alert-info">Nema dostupnih resursa.</p>
    }
</div>
```

## EF Query Best Practices

```csharp
// Eager loading relacija
var items = _context.Items
    .Include(x => x.RelatedEntity)
    .ToList();

// Read-only upiti
var items = _context.Items
    .AsNoTracking()
    .Where(x => x.IsActive)
    .ToList();

// Filtering i sorting
var items = _context.Items
    .Where(x => x.Year == 2024)
    .OrderBy(x => x.Name)
    .ToList();
```

