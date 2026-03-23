# Copilot Instructions

## Directrices del proyecto
- Preferencias/proyecto: usar ApplicationDbContext para persistencia, inyección mediante constructores primarios (Primary Constructors), usar EF Core con .AsNoTracking() en consultas de solo lectura, paginación con .Skip() y .Take(), sin lógica de negocio/validaciones en repositorios, objetivo C# 14 / .NET 10.