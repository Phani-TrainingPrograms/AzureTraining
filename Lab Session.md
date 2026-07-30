## Azure Hands-On Lab: Deploying a Linux Web Server from Windows
Welcome to your first Linux deployment on Azure. As a Windows programmer, you can think of a Linux Virtual Machine (VM) as a cloud-based server without a graphical user interface (GUI). Instead of clicking buttons, we interact with it using text commands.
This lab manual will guide you through creating a Linux server, connecting to it from your Windows command prompt, installing the Nginx web server (similar to IIS), and hosting a simple webpage. [1, 2] 
------------------------------
## Prerequisites
Before starting, ensure you have the following ready on your Windows machine:

* Azure Account: An active Azure subscription with permissions to create resources.
* Windows Terminal or Command Prompt: Built-in tools (Windows 10/11 have SSH client enabled by default).
* Web Browser: Microsoft Edge, Google Chrome, or any browser of your choice. [3, 4] 

------------------------------
## Step 1: Create the Linux Virtual Machine in Azure
We will use the Azure Portal to provision a virtual machine running Ubuntu, a popular and user-friendly version (distribution) of Linux. [5] 

   1. Log in to the Azure Portal. [6] 
   2. In the search bar at the top, type Virtual Machines and select it from the services list. [7, 8] 
   3. Click Create > Azure virtual machine. [9] 
   4. Configure the Basics tab with the following settings:
   * Subscription: Select your active subscription.
      * Resource Group: Click Create new, name it RG-Linux-Demo, and click OK.
      * Virtual machine name: LinuxWebVM
      * Region: Select a region close to you (e.g., East US).
      * Availability options: No infrastructure redundancy required.
      * Security type: Standard.
      * Image: Select Ubuntu Server 22.04 LTS - x64 Gen2 (Think of this as selecting Windows Server 2022).
      * Size: Choose a low-cost option like Standard_B1s or Standard_B2s (adequate for testing). [10, 11, 12, 13, 14] 
   5. Configure the Administrator account section:
   * Authentication type: Select SSH public key. (This is a highly secure alternative to passwords using a matching pair of cryptographic keys).
      * Username: azureuser
      * SSH key pair source: Select Generate new key pair.
      * Key pair name: LinuxWebVM_key [15, 16, 17, 18, 19] 
   6. Configure Inbound port rules:
   * Public inbound ports: Select Allow selected ports.
      * Select inbound ports: Check both SSH (22) and HTTP (80).
      * Note: Port 22 allows remote command-line access. Port 80 allows public web traffic. [20, 21, 22, 23, 24] 
      7. Click Review + create at the bottom of the page. [25] 
   8. Once validation passes, click Create. [26] 
   9. A pop-up window will appear titled Generate new key pair. Click Download private key and create resource.
   * Crucial Step: A file named LinuxWebVM_key.pem will download to your Windows machine (usually in your Downloads folder). Treat this file like a master password; do not lose it.
   
------------------------------
## Step 2: Retrieve Your VM's Public IP Address
Once the deployment finishes, we need the server's public address to connect to it. [27] 

   1. Click Go to resource once the deployment states "Your deployment is complete".
   2. On the Overview page, look at the right side of the screen.
   3. Locate the Public IP address field.
   4. Copy this IP address (e.g., 52.184.23.141) to your notepad. [28, 29] 

------------------------------
## Step 3: Connect to the Linux VM from Windows Command Prompt
We will now use Secure Shell (SSH) to access the Linux command line directly from Windows. [30] 

   1. Open Command Prompt or PowerShell on your Windows computer.
   2. Navigate to the folder where your private key was downloaded (usually your Downloads folder) by typing:
   
   cd %USERPROFILE%\Downloads
   
   3. Secure the Key file (Windows Security Rule): Linux requires that your private key file has restricted permissions. Run this command to strip away extra Windows permissions from the file:
   
   icacls LinuxWebVM_key.pem /inheritance:r /grant:r "%username%:R"
   
   4. Connect to your VM using the ssh command. Replace <Your_Public_IP> with the IP address you copied in Step 2:
   
   ssh -i LinuxWebVM_key.pem azureuser@<Your_Public_IP>
   
   5. The prompt will display a warning: "The authenticity of host... can't be established. Are you sure you want to continue connecting?"
   6. Type yes and hit Enter.
   7. Your command line prompt will change to something like azureuser@LinuxWebVM:~$. You are now successfully inside the Linux operating system! [31, 32, 33] 

------------------------------
## Step 4: Install Nginx Web Server
Linux servers utilize a package manager (similar to a command-line app store) to install software. On Ubuntu, this tool is called apt. [34] 

   1. Update the software index: This tells Linux to fetch the latest list of available software packages.
   
   sudo apt update
   
   Terminology Check: sudo stands for "Superuser Do". It is the Linux equivalent of right-clicking a program and choosing "Run as Administrator". [35] 
   2. Install Nginx: Run the installation command.
   
   sudo apt install nginx -y
   
   Terminology Check: The -y flag automatically answers "yes" to the confirmation prompt during installation. [36] 

------------------------------
## Step 5: Deploy a Simple Web Application
In Linux, Nginx serves web pages out of a specific directory (folder) path: /var/www/html. We will navigate to this folder and replace the default page with our own HTML code. [37, 38, 39] 

   1. Navigate to the web directory:
   
   cd /var/www/html
   
   [40] 
   2. Delete the default Nginx welcome page:
   
   sudo rm index.nginx-debian.html
   
   Terminology Check: rm stands for "remove", which deletes files. [41] 
   3. Create a new HTML file: We will use a basic built-in Linux text editor called nano.
   
   sudo nano index.html
   
   [42] 
   4. Your command prompt screen will turn into a text editor. Paste the following HTML code block directly into the window:
   '''
   <!DOCTYPE html>
   <html>
   <head>
       <title>Azure Linux Demo</title>
       <style>
           body { font-family: sans-serif; text-align: center; margin-top: 50px; background-color: #f4f4f9; }
           h1 { color: #0078d4; }
       </style>
   </head>
   <body>
       <h1>Hello Windows Developers!</h1>
       <p>This simple web application is running successfully on an Azure Linux VM.</p>
   </body>
   </html>
   '''
   5. Save and exit the editor:
   * Press Ctrl + O (Write Out) and hit Enter to save the file.
      * Press Ctrl + X to exit the Nano editor and return to the command prompt.
   
------------------------------
## Step 6: Test Your Web Application in a Browser

   1. Open a web browser on your local Windows machine.
   2. In the URL address bar, type your VM's public IP address (from Step 2) and press Enter:
   
   http://<Your_Public_IP>
   
   3. You should instantly see your styled webpage displaying "Hello Windows Developers!". [43, 44, 45] 

------------------------------
## Step 7: Clean Up Resources (Crucial)
To prevent ongoing charges on your Azure account, delete the resources created during this lab.

   1. Return to the Azure Portal.
   2. Search for Resource groups and click on RG-Linux-Demo.
   3. Click Delete resource group at the top menu.
   4. Type the name of the resource group (RG-Linux-Demo) to confirm, then click Delete. [46, 47, 48] 

------------------------------
Here are the step-by-step instructions to create an Azure Storage Account and upload files directly through your web browser using the Azure Portal (Dashboard Console). 
For Windows programmers, think of an Azure Storage Account as a secure, cloud-based file share system, and a BLOB (Binary Large Object) as simply a file (like an image, log, text, or configuration file).
------------------------------
## Step 1: Create the Storage Account in the Azure Portal

   1. Log in to the Azure Portal.
   2. In the top search bar, type Storage accounts and select it under Services. [8, 9, 10] 
   3. Click the + Create button in the top left. [11] 
   4. On the Basics tab, configure the following:
   * Subscription: Select your active subscription.
      * Resource Group: Select RG-Linux-Demo (the same one used for your VM) to keep your lab organized.
      * Storage account name: Enter a unique name (e.g., winprogstorage2026).
      * Crucial Rule: This name must be completely unique across all of Azure, use lowercase letters and numbers only, and be between 3 to 24 characters.
      * Region: Select the same region as your VM (e.g., East US).
      * Primary service: Azure Blob Storage or Azure Virtual Machines.
      * Performance: Select Standard (ideal for general file storage).
      * Redundancy: Select Locally-redundant storage (LRS). This is the lowest-cost option, making it perfect for labs and testing. [12, 13, 14, 15, 16] 
   5. Click the Review + create button at the bottom of the screen. [17] 
   6. Once validation passes, click Create. This process usually takes less than a minute. [18, 19] 

------------------------------
## Step 2: Create a Container (The Folder System)
Before you can upload files (BLOBs), you must create a Container. Think of a Container exactly like a root folder or a directory inside a file drive. [20] 

   1. Once the deployment finishes, click Go to resource. [21] 
   2. On the left-hand navigation menu, under the Data storage section, click on Containers. [22] 
   3. Click the + Container button at the top. [23] 
   4. In the right-hand panel that opens:
   * Name: Type config-files (use lowercase letters and hyphens only).
      * Anonymous access level: Leave it at Private (no anonymous access). This keeps your configuration files secure from the public web. [24, 25, 26, 27, 28] 
   5. Click Create. Your new container folder will now appear in the list. [29] 

------------------------------
## Step 3: Upload Files from Windows using the Azure Console
Now we will upload an HTML file and a sample .config text file from your local Windows machine straight into your new cloud container.
## 1. Prepare Your Local Windows Files
If you do not have files ready, quickly create them on your Windows desktop:

* File 1: An HTML file named index.html.
* File 2: Open Notepad, type Setting1=Value1, and save it as app.config. [30] 

## 2. Upload via the Browser Console

   1. Inside the Azure Portal, click on the name of your new container: config-files.
   2. Click the Upload button located at the top menu.
   3. A right-hand panel titled Upload blob will slide out.
   4. Click the Browse for files folder icon.
   5. In the standard Windows file picker window that pops up, navigate to your files, select both index.html and app.config, and click Open.
   6. Expand the Advanced section in that panel if you want to explore settings, but for standard files, the default options are perfect.
   7. Click the Upload button at the bottom of the panel. 

------------------------------
## Step 4: Verify Your Uploaded BLOBs

   1. The panel will close, and you will see index.html and app.config listed inside your container grid.
   2. Click on app.config from the file list.
   3. Go to the Edit tab at the top of the file details panel.
   4. You can see, edit, and modify the text contents of your configuration file directly inside the web browser without downloading it back to Windows. Click Save if you make any changes! [34, 35] 

------------------------------
# Reading the Config from a C# App....
------------------------------
## Step 1: Grant Your Account Permissions in Azure
By default, even if you are the owner of the subscription, your code needs explicit data permissions to read the contents inside a storage container.

   1. Open the Azure Portal.
   2. Navigate to your Storage Account (winprogstorage2026).
   3. Click on Access Control (IAM) in the left-hand menu.
   4. Click + Add > Add role assignment.
   5. Search for the role Storage Blob Data Reader (this allows your code to read files but not delete or modify them) and select it. Click Next.
   6. Select User, group, or service principal.
   7. Click + Select members, search for your own Azure login email address, select it, and click Select.
   8. Click Review + assign. 

------------------------------
## Step 2: Create the C# Console Application

   1. Open Visual Studio or VS Code on your Windows machine.
   2. Create a new project: Console App (.NET 6.0, .NET 8.0, or later).
   3. Name your project AzureStorageReader. 

------------------------------
## Step 3: Install the Required NuGet Packages
You need two official Microsoft library packages to interact with Azure Blob Storage securely. Open your NuGet Package Manager Console (Tools > NuGet Package Manager > Package Manager Console) and run these two commands: [10, 11, 12] 

Install-Package Azure.Storage.Blobs
Install-Package Azure.Identity

------------------------------
## Step 4: Write the C# Code
Replace all the code inside your Program.cs file with the following snippet. [13, 14] 
Make sure to replace the placeholder URL with your actual Azure Storage Account URL (You can find this URL in the Azure Portal under your storage account's Endpoints tab, or just substitute your storage account name into the string below). [15] 
'''
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Define your storage endpoints and target file details
        string accountName = "winprogstorage2026"; // <-- Replace with your storage account name
        string blobServiceUri = $"https://{accountName}.blob.core.windows.net";
        string containerName = "config-files";
        string blobName = "app.config";

        Console.WriteLine("Connecting to Azure Blob Storage securely...");

        try
        {
            // 2. Authenticate using DefaultAzureCredential.
            // This safely grabs your active visual studio/azure CLI login credentials on Windows.
            var serviceClient = new BlobServiceClient(new Uri(blobServiceUri), new DefaultAzureCredential());
            
            // 3. Drill down into the specific container and file (BLOB)
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            Console.WriteLine($"Downloading stream for: {blobName}...");

            // 4. Download the file content into a memory stream
            var downloadResponse = await blobClient.DownloadStreamingAsync();
            
            // 5. Read the content line by line using a standard StreamReader
            using (var reader = new StreamReader(downloadResponse.Value.Content))
            {
                string fileContent = await reader.ReadToEndAsync();
                
                Console.WriteLine("\n--- File Content Successfully Read ---");
                Console.WriteLine(fileContent);
                Console.WriteLine("--------------------------------------\n");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
'''
------------------------------
## Step 5: Run Your Code locally on Windows
Before running the application, your local Windows environment needs to know who you are so DefaultAzureCredential can grab your identity: [16] 

   1. In Visual Studio, ensure you are logged into the same account you used for the Azure Portal (check the top-right corner of the IDE).
   2. Hit F5 or click Run.
   3. The console app will open, authenticate silently behind the scenes, fetch the app.config file from the cloud, and print Setting1=Value1 right into your command screen.

## Hard code the Secret key to access from Visual Studio account

Hardcode a Local Secret (For Lab Testing Only)If you cannot change your account logins due to company machine restrictions, you can bypass the automated identity lookups by passing a temporary connection string or secret into the code.Warning: Never use this method in production code.1. Get your Access Key from AzureGo to the Azure Portal and open your Storage Account (winprogstorage2026).On the left sidebar menu under Security + networking, click Access keys.Click Show next to Connection string under key1, and copy the full string to your clipboard.2. Update Your C# CodeModify your Program.cs code to use the connection string directly, bypassing DefaultAzureCredential completely:csharp// Replace this block in your old code:
```
// var serviceClient = new BlobServiceClient(new Uri(blobServiceUri), new DefaultAzureCredential());

// With this simplified block (Paste your actual connection string here):
string connectionString = "DefaultEndpointsProtocol=https;AccountName=winprogstorage2026;AccountKey=...[rest of your key]...";
var serviceClient = new BlobServiceClient(connectionString);
Use code with caution.
```
------------------------------
Using Azure Key Vault to store secured data.
------------------------------
## Step 1: Create an Azure Key Vault via the Portal

   1. Log in to the Azure Portal.
   2. In the top search bar, type Key vaults and select it from the services list. [10, 11, 12] 
   3. Click the + Create button. [13] 
   4. On the Basics tab, configure the following settings:
   * Subscription: Select your active subscription.
      * Resource Group: Select your existing resource group RG-Linux-Demo.
      * Key vault name: Enter a unique name (e.g., winprog-vault-2026). Must be unique across all of Azure.
      * Region: Select the same region as your virtual machine and storage account (e.g., East US).
      * Pricing tier: Select Standard.
      * Days to retain deleted vaults: Leave at the default (90).
      * Purge protection: Select Disable. [14, 15, 16, 17, 18] 
   5. Click Next to move to the Access configuration tab. [19] 

------------------------------
## Step 2: Configure Permissions (Azure RBAC)
Azure uses strict identity models to determine who can read a vault's secret. [20] 

   1. On the Access configuration tab, under Permission model, select Azure role-based access control (recommended).
   2. Click Review + create at the bottom, then click Create once validation passes.
   3. Once the deployment finishes, click Go to resource.
   4. In the left-hand menu, click Access Control (IAM).
   5. Click + Add > Add role assignment.
   6. Search for the role Key Vault Secrets User (this allows reading secret values) and select it. Click Next.
   7. Select User, group, or service principal, click + Select members, search for your own Azure portal email address, and select it.
   8. Click Review + assign. [21, 22, 23, 24, 25] 

------------------------------
## Step 3: Add Your Connection String Secret to Key Vault

   1. In the left-hand menu of your Key Vault, under Objects, click on Secrets. [26] 
   2. Click the + Generate/Import button at the top. [27] 
   3. Configure the secret settings:
   * Upload options: Manual
      * Name: DbConnectionString
      * Secret value: Paste your database connection string here (e.g., Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;).
      * Leave all other settings at their defaults. [28, 29, 30, 31] 
   4. Click Create. Your secret is now securely encrypted at rest inside Azure. [32] 

------------------------------
## Step 4: Install Required NuGet Packages in Visual Studio
Open your NuGet Package Manager Console (Tools > NuGet Package Manager > Package Manager Console) in Visual Studio and run this command to install the Key Vault client library: [33, 34] 

Install-Package Azure.Security.KeyVault.Secrets

(We will reuse the Azure.Identity package we installed earlier for secure authentication). [35] 
------------------------------
## Step 5: Write the C# Code to Fetch the Secret
Update your Program.cs file. The code uses DefaultAzureCredential to verify your identity and directly requests the secret by its name without processing any local text files or configuration parsing. [36, 37, 38] 
'''
using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Point to your unique Key Vault URL endpoint
        string vaultName = "winprog-vault-2026"; // <-- Replace with your exact Key Vault name
        string vaultUri = $"https://{vaultName}.vault.azure.net/";
        string secretName = "DbConnectionString";

        Console.WriteLine("Connecting securely to Azure Key Vault...");

        try
        {
            // 2. Authenticate using your active Visual Studio / Azure CLI user account
            var client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());

            Console.WriteLine($"Retrieving secret key '{secretName}' from the safe...");

            // 3. Request the secret value asynchronously from Azure
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);

            // 4. Output the value to confirm it works
            Console.WriteLine("\n--- Key Vault Secret Successfully Retrieved ---");
            Console.WriteLine($"Secret Value: {secret.Value}");
            Console.WriteLine("------------------------------------------------\n");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
'''
------------------------------
## Step 6: Test the Secure Application

   1. Ensure your active user profile in Visual Studio matches the email address you granted permissions to in Step 2.
   2. Run the application (F5).
   3. Your application will pull your secure database connection string safely out of the encrypted vault directly into your application's volatile memory. [39] 

------------------------------



