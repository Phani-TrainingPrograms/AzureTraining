namespace AzureStorageReaderApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string accountName = "winprogstorage2026";
            const string blobServiceUrl = $"https://{accountName}.blob.core.windows.net";
            const string containerName = "config-files";
            const string blobName = "App.config";
            const string connectionString = "UR ConnectionString taken from Access keys";

            Console.WriteLine("Connecting to Azure blob Storage");
            try
            {
                //Authenticate the user
                //var serviceClient = new Azure.Storage.Blobs.BlobServiceClient(new Uri(blobServiceUrl), new Azure.Identity.DefaultAzureCredential());
                var serviceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                Console.WriteLine($"Downloading stream for blob: {blobName}");

                var downloadStream = await blobClient.DownloadStreamingAsync();

                using(var reader = new StreamReader(downloadStream.Value.Content))
                {
                    string content = await reader.ReadToEndAsync();
                    Console.WriteLine($"Content of blob {blobName}:\n{content}");
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occured: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
