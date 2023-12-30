# Data Migration Commands

## Prerequisites
You need the (Entity Framework CLI)[https://docs.microsoft.com/en-us/ef/core/cli/dotnet] tool installed so run this command. 
It should be automatically installed when restoring nuget packages for this solution.

check with this command:
```
dotnet tool list
```
or in case you install it globally
```
dotnet tool list --global
```

install with this command:
```
dotnet tool install dotnet-ef --local
OR
dotnet tool install dotnet-ef --global
```

update with this command:
```
dotnet tool update --global dotnet-ef --version 8.0.0
```

Notes
- If you get a "Cannot find a manifest file." As the errors states, use ```dotnet new tool-manifest```, usually in the repo root directory.

# dotnet-ef commands
Get some help
```
dotnet ef -h
```

If you are in the top most directory at the prompt and want to create a new migration
after making changes, change MyChangeDescription to your change name and use this command:
```
dotnet ef migrations add initial -p Example.Repository -s Example
```

# Create the database if I move code to new machine
Do this from the same location as the solution file.
```
dotnet ef database update -p Example.Repository -s Example
```
