---
name: "Edit Form Skill"
description: "Guide for creating create/edit forms with validation and model binding"
---

# Edit Form Skill

Koristi za create/edit forme s:
- asp-for tag helperima (model binding)
- Model validacijom (data annotations)
- POST akcijom za spremanje u bazu
- Redirekcijom nakon uspjeha

## Controller Pattern

```csharp
[HttpGet("uredi/{id:int}")]
public IActionResult Edit(int id)
{
    var entity = _repository.GetById(id);
    if (entity == null)
        return NotFound();
    return View(entity);
}

[HttpPost("uredi/{id:int}")]
public IActionResult Edit(int id, Entity model)
{
    if (!ModelState.IsValid)
        return View(model);

    try
    {
        _repository.Update(id, model);
        return RedirectToAction("Index");
    }
    catch (Exception)
    {
        ModelState.AddModelError("", "Greška pri spremanju.");
        return View(model);
    }
}

[HttpGet("kreiraj")]
public IActionResult Create()
{
    return View();
}

[HttpPost("kreiraj")]
public IActionResult Create(Entity model)
{
    if (!ModelState.IsValid)
        return View(model);

    try
    {
        _repository.Add(model);
        return RedirectToAction("Index");
    }
    catch (Exception)
    {
        ModelState.AddModelError("", "Greška pri kreiranju.");
        return View(model);
    }
}
```

## Model s Data Annotations

```csharp
public class Entity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Naziv je obavezan")]
    [StringLength(100, MinimumLength = 3, 
        ErrorMessage = "Naziv mora biti između 3 i 100 znakova")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email je obavezan")]
    [EmailAddress(ErrorMessage = "Nevaljani email")]
    public string Email { get; set; } = null!;

    [Range(0, 1000, ErrorMessage = "Vrijednost mora biti između 0 i 1000")]
    public int Quantity { get; set; }
}
```

## View Pattern (Razor)

```html
@model Entity

<div class="container mt-4">
    <h1>@(Model.Id > 0 ? "Uredi" : "Kreiraj")</h1>

    <form method="post">
        <input type="hidden" asp-for="Id" />

        <div class="mb-3">
            <label asp-for="Name" class="form-label">Naziv:</label>
            <input type="text" asp-for="Name" class="form-control" />
            <span asp-validation-for="Name" class="text-danger"></span>
        </div>

        <div class="mb-3">
            <label asp-for="Email" class="form-label">Email:</label>
            <input type="email" asp-for="Email" class="form-control" />
            <span asp-validation-for="Email" class="text-danger"></span>
        </div>

        <div class="mb-3">
            <label asp-for="Quantity" class="form-label">Količina:</label>
            <input type="number" asp-for="Quantity" class="form-control" />
            <span asp-validation-for="Quantity" class="text-danger"></span>
        </div>

        <button type="submit" class="btn btn-primary">Spremi</button>
        <a href="/resources" class="btn btn-secondary">Poništi</a>
    </form>

    @section Scripts {
        <partial name="_ValidationScriptsPartial" />
    }
</div>
```

## Validacijski Best Practices

- Uvijek validiraj `ModelState.IsValid` prije spremanja
- Koristi `[Required]` za obavezna polja
- Koristi `[StringLength]` za tekstualna polja
- Koristi `[EmailAddress]` za email polja
- Koristi `[Range]` za numerička polja
- Prikazuj greške korisniku kroz `<span asp-validation-for="">`

