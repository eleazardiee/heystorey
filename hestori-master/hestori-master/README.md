LocalHistoryWebsite - Setup and Run Guide
This guide provides step-by-step instructions for cloning, setting up, and running the LocalHistoryWebsite application on a new device.

Prerequisites
Before you begin, ensure you have the following installed on your system:

- Visual Studio 2022 (Community edition is fine)
- .NET 8.0 SDK
- SQL Server LocalDB (included with Visual Studio)
- Git

Step 1: Clone the Repository
1. Open a command prompt or terminal
2. Navigate to the directory where you want to store the project
3. Clone the repository using Git:
  bash
  - git clone https://github.com/ASENDIENTEKevinVance/LocalHistoryWebsite.git
4. Navigate into the project directory:
  bash
  - cd LocalHistoryWebsite

Step 2: Open the Project in Visual Studio
1. Launch Visual Studio 2022
2. Select "Open a project or solution"
3. Navigate to the cloned repository folder
4. Select the solution file LocalHistoryWebsite.sln and click "Open"

Step 3: Restore NuGet Packages
1. In Visual Studio, right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for the restoration process to complete
   
Step 4: Set Up the Database
1. Open the Package Manager Console in Visual Studio:
  - Go to Tools > NuGet Package Manager > Package Manager Console
2. Run the following command to create the database:
  - Update-Database
This command will:
  *Create a LocalDB database named "LocalHistoryWebsite"
  *Apply all the existing migrations to create the database schema
-If you encounter any errors about missing Entity Framework tools, run these commands first:
  * - Install-Package Microsoft.EntityFrameworkCore.Tools
  * - Install-Package Microsoft.EntityFrameworkCore.SqlServer
   
Step 5: Configure the Application
1. Check the connection string in appsettings.json:
  - Ensure it's set to use LocalDB:
    json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LocalHistoryWebsite;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
2. Create necessary folders for file uploads:
  - In Solution Explorer, right-click on wwwroot
  - Select Add > New Folder
  - Create folders: uploads and inside that another folder historyImages

Step 6: Build and Run the Application
1. Build the solution:
  - Go to Build > Build Solution (or press Ctrl+Shift+B)
2. Run the application:
  - Press F5 to start the application in debug mode
  - Or press Ctrl+F5 to run without debugging
3. The browser should open automatically with your application running

Step 7: Use the Application
Once the application is running:
1. Click the "Share Your Local History" button to create a new post
2. Fill in the required information:
  - Title of the story
  - Description/documentation
  - Upload images (optional)
3. Click "Submit" to create your post
4. View your posts on the home page
5. Use the reaction buttons (Heart, Like, Dislike) to react to posts
6. Click "Read More" to view the full details of a post
7. Use the "Delete" button to remove posts

Troubleshooting
*If you encounter database connection issues:
1. Verify LocalDB is installed and running:
  - Open a command prompt and run: sqllocaldb info
  - If no instances are listed, run: sqllocaldb create MSSQLLocalDB
2. Check if the database exists:
  - Open SQL Server Object Explorer in Visual Studio
  - Look for the LocalHistoryWebsite database under (localdb)\MSSQLLocalDB
3. If migrations fail:
  - Try running Add-Migration InitialCreate -Force and then Update-Database

If image uploads don't work:
1. Check that the upload directories exist:
  - Ensure wwwroot/uploads/historyImages folder exists
  - Make sure the application has write permissions to this folder
2. Check the HistoryImage model:
  - Ensure the Description property is nullable or a default value is provided

