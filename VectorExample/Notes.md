# Vector support
---
According to [Microsoft's PostgreSQL site](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-extensions#postgres-15-extensions), 
the pgvector extension is allows for vector fields.  The github for [pgvector](https://github.com/pgvector/pgvector) is here and has [information about 
adding and using Entity Framework with their extension](https://github.com/pgvector/pgvector-dotnet).  You'll need to use the following NuGet package
```
dotnet add package Pgvector.EntityFrameworkCore
```


## Setup error
```
No suitable constructor was found for entity type 'Vector'. The following constructors had parameters that could not be bound to properties of the entity type:
    Cannot bind 'v' in 'Vector(float[] v)'
    Cannot bind 's' in 'Vector(string s)'
Note that only mapped properties can be bound to constructor parameters. Navigations to related entities, including references to owned types, cannot be bound.
```
Problem was that I missed the step from the [EF instructions](https://github.com/pgvector/pgvector-dotnet#entity-framework-core) where you add UseVector to the options!
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseNpgsql("connString", o => o.UseVector());
}
```