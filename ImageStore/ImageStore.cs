using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace NumberPlates.Images
{
    public class ImageStore
    {
        private static readonly string key = @"ZpTaZvATbbcSx0BlGjDtwes0eM23R6eud2qdEy7zomA7OzM3x9rvoIkbWjcZOFKLVUXGmXSZWOCF0PRKQ/akqw==";
        private static readonly string connstring = @"DefaultEndpointsProtocol=https;AccountName=numberplates;AccountKey=ZpTaZvATbbcSx0BlGjDtwes0eM23R6eud2qdEy7zomA7OzM3x9rvoIkbWjcZOFKLVUXGmXSZWOCF0PRKQ/akqw==;EndpointSuffix=core.windows.net";

        public async Task<string> StoreImage(byte[] contents)
        {
            var account = CloudStorageAccount.Parse(connstring);
            var serviceClient = account.CreateCloudBlobClient();

            // Create container. Name must be lower case.
            Console.WriteLine("Creating container...");
            var container = serviceClient.GetContainerReference("numberplates");
            await container.CreateIfNotExistsAsync();

            // write a blob to the container
            var guid = Guid.NewGuid();
            var blob = container.GetBlockBlobReference(guid.ToString() + ".jpg");
            await blob.UploadFromByteArrayAsync(contents, 0, contents.Length);
            return blob.Uri.AbsoluteUri;
        }
    }
}

