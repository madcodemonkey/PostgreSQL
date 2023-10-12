# Vector example
This example shows how to create vector fields.  It also ties into Azure OpenAI to generate the embeddings for the vector fields.
The database is created using code migrations and will run automatically be default, so please **setup the database and 
the appsettings.json before you run it.**

# Setup
## PostgreSQL Server Setup
1. Create a 'Azure Database for PostgreSQL flexible server' resource in the Azure Portal.
   - Basic options
      - Server Name: [something unique]
      - Region: [closest to you]
      - PostgreSQL version: 15
      - Workload type: Development
      - Compute storage: [took the default...Burstable, B1ms]
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
   - Left click "Add current client IP address" or "Add 0.0.0.0 - 255.255.255.255" 
   - Left-click the "Save" button at the top.
4. Get a connection string and adjust it.
   - Under Settings, left-click "Connect"
   - Expand the "Connect from your app" accordian thingy
   - Copy the ADO.NET connection string ```Server=yourdbnamehere.postgres.database.azure.com;Database=MyStuff;Port=5432;User Id=youridhere;Password={your_password};Ssl Mode=Require;```
   - Adjust the Ssl Mode to "VerifyCA" so that it looks like this 
     ```Server=yourdbnamehere.postgres.database.azure.com;Database=MyStuff;Port=5432;User Id=youridhere;Password={your_password};Ssl Mode=VerifyCA;```
   - Finally, adjust your server name, user id and password to be what you used put in for Step 1 above.
5. Update appsettings.json (or override with secrets.json)
   - Update the "Repository:DatabaseConnectionString" with your connection string.
 
## PostgreSQL Database setup
Short version:
1. Run the project.
2. Important! After the Swagger screen appears, STOP the project from running (why? see long winded version below).


Long winded version:
In order to use vector fields, we are following the instructions given with the 
[pgvector NuGet package](https://github.com/pgvector/pgvector-dotnet#entity-framework-core).
They require us to execute a specific command after creating the database:
```CREATE EXTENSION vector;```

I've added that command to the 20231011175629_initial.cs migration file by hand.

1. Run the project.
2. Important: After the Swagger screen appears, STOP the project from running.
   - Why? If you start populating the CloudResource table at this point it will work, but the second you query you'll get an error 
     complaining about ```Can't cast database type .<unknown> to Vector```.  The vector extension was created as stated above, 
     but the driver cached info about the database when it first connected.  Restarting flushes its old knowledge so that when 
     it restarts it knows about vectors.

## Open AI
1. Create an Azure OpenAI resource in the portal.
   - Basics
       - Choose resource group
       - Choose region (it's limited)
       - Choose a name
       - There is only one tier (S0)
   - Network
      - Take defaults 
   - Tags
      - Take defaults 
   - Review + Create
      - Left-click "Create" button
2. Update appsettings.json (or override with secrets.json)
   - Once deployed, Left-Click on "Keys and Endpoint" 
   - Update the "OpenAI:Endpoint" with your endpoint (e.g., https://myNameFromStep1.openai.azure.com/) 
   - Update the "OpenAI:Key" with Key1 or Key2
3. Left-Click on "Model Deployments" and "Management Deployments" this will open Azure OpenAI Studio
4. Create a deployment using the "text-embedding-ada-002" model version 2 and give it a name and Left-click "Create" button
   - Extra credit given if you use the "Advanced Option" to NOT consume all the remaining tokens!
5. Update appsettings.json (or override with secrets.json)
   - Update the "OpenAI:DeploymentOrModelName" with then name you chose in step 4

# Populate the Vector fields with embeddings
1. Run the Example Web API project as your startup project.
2. Use the postman collection at the root of this repository to call the "DataManipulation" controller's "Update-Vector-Fields" POST method 
   till you consistently get back zero items updated.  You can increase the batch size if you desire and we will still save every 10th item 
   as we progress through the db records that need their vector fields updated.  Just be aware that you could timeout or get throttled by
   the OpenAI endoint as we try to generate embeddings for the fields.

# Searching
1. Run the Example Web API project as your startup project.
2. Use the postman collection at the root of this repository to call "Search" method using semantic type queries (e.g. what services use CDNs?)
   - Note 1: See the database or the AzureCatalog.json in the Example.Repository project (seeds folder) to get a feel for what questions you can ask.
   - Note 2: I'm currently searching both the title and content vectors for the nearest X items and then in memory combining the findings and
             doing a cosine comparision to find the best of those (see the code in CloudResourceRepository.cs, which lives in the Example.Repository project).

# Tools
- You can connect to the the PostgreSQL database using [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=redhat-install%2Credhat-uninstall) 
  with the PostgreSQL extension from Microsoft (add the way you add VSCode extension since Azure Data Studio uses the same shell as VSCode it should be very familiar).

# Warnings
- Update these ASAP when they go out of beta
   - I'm using a beta version of Microsoft.SemanticKernel.Core (1.0.0-beta1) so they may move the CosineSimilarity extension I used in CloudResourceRepository.cs to a different location.
   - I'm using a beta version of Azure.AI.OpenAI (1.0.0-beta.8)
  
# Notes
- There is some great advice on indexing the vector fields [here on the site where the vector extension code lives](https://github.com/pgvector/pgvector#indexing).
  The supported indext types (IVFFlat and HNSW) are explained with details on how to create them using SQL statement in the database.


