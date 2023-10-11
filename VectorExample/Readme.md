# Vector example
---
This example shows how to create vector fields.  It also ties into Azure OpenAI to generate the embeddings for the vector fields.
The database is created using code migrations and will run automatically be default, so please **setup the database and 
the appsettings.json before you run it.**


# Setup
---
## PostgreSQL Server Setup
1. Create a 'Azure Database for PostgreSQL flexible server' resource in the Azure Portal.
   - Basic options
      - Server Name: [something unique]
      - Region: [closest to you]
      - PostgreSQL version: 15
      - Workload type: Development
      - Compute storage: [took the default...Bursable, B1ms]
      - Availablity Zone: No preference
      - Enable high availability: [left UNchecked]
      - Authentication method: PosgreSQL authentication only
      - Admin username: [your choice...needed below in connection string]
      - Password: [your choice...needed below in connection string]
   - Networking
      - Check "Allow public access from any Azure service within Azure to this server"
      - Add your client IP now or do it below
   - Security (took defaults)
   - Tags (took defaults)
   - Review + Create
      - Left-click "Create" button
2. Add an extension to the PostgreSQL database by doing the following
   - Under Settings, left-click "Server parameters"
   - Search for 'extensions' or 'azure.extensions'
   - In the drop down under the VALUE column, place a check in the "VECTOR" checkbox.
   - Left-click the "Save" button at the top, which will kick off a deployment.
   - Reference:
	 - [Microsoft docs on 'How to use PostgreSQL extensions'](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-extensions#how-to-use-postgresql-extensions)
3. Allow yourself access by doing the following
   - Under Settings, left-click "Networking"
   - Place a check in the "Allow public access from any Azure service within Azure to this server" checkbox.
   - Left click "Add current client IP address" or "Add 0.0.00 - 255.255.255.255" 
   - Left-click the "Save" button at the top.
4. Get a connection string and adjust it.
   - Under Settings, left-click "Connect"
   - Expand the "Connect from your app" accordian thingy
   - Copy the ADO.NET connection string ```Server=yourdbnamehere.postgres.database.azure.com;Database=MyStuff;Port=5432;User Id=youridhere;Password={your_password};Ssl Mode=Require;```
   - Adjust the Ssl Mode so that it looks like this ```Server=yourdbnamehere.postgres.database.azure.com;Database=MyStuff;Port=5432;User Id=youridhere;Password={your_password};Ssl Mode=VerifyCA;```
   - Finally, adjust your server name, user id and password to be what you used put in for Step 1 above.
5. TODO: See if this step is really needed => Go to Overview and restart the server.
 
## PostgreSQL Database setup
Short version:
Run the project.

Long winded version:
In order to use vector fields, we are following the instructions given with the 
[pgvector NuGet package](https://github.com/pgvector/pgvector-dotnet#entity-framework-core).
They call for use to execute a specific command after creating the database:
```CREATE EXTENSION vector;```

I've added that command to the 20231010223222_initial.cs migration file by hand.

Run the project.

# Tools
---
- You can connect to the the PostgreSQL database using [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=redhat-install%2Credhat-uninstall) 
  with the PostgreSQL extension from Microsoft (add the way you add VSCode extension since Azure Data Studio uses the same shell as VSCode it should be very familiar).
- 




