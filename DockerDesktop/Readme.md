# Summary
This folder contains a docker compose file to create a local instance of PostgreSQL that I can attach to.

# Requirements
Docker Desktop is installed.

# Usage
At a command prompt inside this directory, use one of these two ways

## Way 1: It takes control of the prompt till your done
Start command: 
```docker compose up```

Stop command:
Use CTRL-C, which will stop it.

## Way 2: It frees the prompt immediately and runs as a deamon 
Start command: 
```docker compose up -d```

Stop command:
```docker compose down```

# Connection string
If you use the defaults, the connection string for should be
```
Server=localhost;Database=example_DB;Port=5432;User Id=root;Password=frog363root;
```

Reference
- https://www.connectionstrings.com/postgresql/