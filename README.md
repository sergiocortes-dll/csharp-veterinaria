# Sistema Veterinaria San Miguel

## ¿Como empezar?

### Instalar dependencias

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add Microsoft.EntityFrameworkCore.Design
dotnet add Microsoft.Extensions.Configuration
dotnet add Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.Extensions.Configuration.Json
```

### Como hacer una migración

Corre los comandos

**Verifica que dotnet-ef está instalado.**

```bash
dotnet tool install --global dotnet-ef
```

Construye la migración, donde <Nombre Migración> será el nombre.
Imagina que es un commit de git, pero el nombre debe ser unico.

```bash
dotnet ef migrations add <Nombre Migración>
```

Actualiza la base de datos.

```bash
dotnet ef database update 
```