# Entity Framework Development Skill

## When to Use
Use this skill when working with Entity Framework models, migrations, or database operations in this ASP.NET MVC project.

## Model Annotations
- Always use [Key] on Id properties
- Use [ForeignKey(" NavigationProperty\)] for foreign keys
- Mark navigation properties as virtual for lazy loading
- Use virtual ICollection<T> for one-to-many relationships

## Migration Workflow
1. Make changes to model classes
2. Run: dotnet ef migrations add <MigrationName>
3. Review generated migration in Migrations folder
4. Apply: dotnet ef database update

## Common EF Operations
- Fetch with relationships: context.Entities.Include(e => e.Related).ToListAsync()
- Fetch single: context.Entities.FindAsync(id)
- Add: context.Entities.AddAsync(entity); await context.SaveChangesAsync()
- Update: context.Entities.Update(entity); await context.SaveChangesAsync()
- Delete: context.Entities.Remove(entity); await context.SaveChangesAsync()

## Database Context
- VjezbaDbContext inherits from DbContext
- All models registered as DbSet<T>
- SQLite database: teh_lab.db
