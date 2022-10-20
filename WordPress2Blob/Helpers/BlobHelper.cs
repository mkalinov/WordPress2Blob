using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPress2Blob.Models;

namespace WordPress2Blob.Helpers
{
    public static class BlobHelper
    {

        public static async Task copyFilesToBlobParrallel(string connectionString, string containerName, List<Mapping> cards, int maxDegreeOfParallelism = 10)
        {

            Parallel.ForEach(cards, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }
            , m => {

                BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
                container.Create();
                BlobClient blob = container.GetBlobClient(m.blobPath);
                //await UploadBlob(blob, m.bytes);

                using (Stream stream = new MemoryStream(m.bytes))
                {
                    blob.Upload(stream, overwrite: true);
                }

                //TO DO: add exception logging
                //write to a local file
            });

        }

        public static async Task copyFilesToBlobAsync(string connectionString, string containerName,  List<Mapping> cards)
        {

            var tasks = cards.Select(async m => {

                BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
                container.Create();
                BlobClient blob = container.GetBlobClient(m.blobPath);
                //await UploadBlob(blob, m.bytes);

                using (Stream stream = new MemoryStream(m.bytes))
                {
                    await blob.UploadAsync(stream, overwrite: true);
                }

                //TO DO: add exception logging
                //write to a local file
            });

            await Task.WhenAll(tasks);               
        }

        public static async Task UploadBlob(BlobClient blobClient, byte[] bytes)
        {

            Stream stream = new MemoryStream(bytes);
            await blobClient.UploadAsync(stream, overwrite: true);
        }

    }
}
