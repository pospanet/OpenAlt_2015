using System;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace StorageClient
{
    class Program
    {
        private const string StorageConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=<account_name>;AccountKey=<account_key>;BlobEndpoint=https://<account_name>.blob.core.windows.net/;TableEndpoint=https://<account_name>.table.core.windows.net/;QueueEndpoint=https://<account_name>.queue.core.windows.net/;FileEndpoint=https://<account_name>.file.core.windows.net/";

        static void Main(string[] args)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (FileStream fileStream = File.OpenRead(@"path\myfile"))
            {
                blockBlob.UploadFromStream(fileStream);
            }


            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof (CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob) item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                }
                else if (item.GetType() == typeof (CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob) item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);
                }
                else if (item.GetType() == typeof (CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory) item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }


            // Retrieve reference to a blob named "photo1.jpg".
            blockBlob = container.GetBlockBlobReference("photo1.jpg");

            // Save blob contents to a file.
            using (FileStream fileStream = File.OpenWrite(@"path\myfile"))
            {
                blockBlob.DownloadToStream(fileStream);
            }


            // Retrieve reference to a blob named "myblob.txt"
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference("myblob.txt");

            string text;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                blockBlob2.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }


            // Retrieve reference to a blob named "myblob.txt".
            blockBlob = container.GetBlockBlobReference("myblob.txt");

            // Delete the blob.
            blockBlob.Delete();
        }
    }
}