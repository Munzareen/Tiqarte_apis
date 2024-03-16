using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace WebAPI.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private static BlobContainerClient blobContainer;

        public BlobService(string _blobConnectionString)
        {
            _blobServiceClient = new BlobServiceClient(_blobConnectionString);
        }

        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, string gifFilePath)
        {
            try
            {
                string fileName = Path.GetFileName(gifFilePath);
                string contentType = string.Empty;
                MemoryStream content = new MemoryStream();
                using (FileStream file = new FileStream(gifFilePath, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(content);
                    content.Position = 0;
                }

                if (blobContainer == null)
                    blobContainer = GetContainerClient(blobContainerName);
                var blobClient = blobContainer.GetBlobClient(fileName);
                var result = blobClient.Upload(content, new BlobHttpHeaders { ContentType = contentType });
                return blobClient.Uri;
            }
            catch (Exception Ex)
            {
            }
            return null;
        }

        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, string fileName, MemoryStream ms)
        {
            try
            {
                string contentType = string.Empty;
                ms.Position = 0;
                if (blobContainer == null)
                    blobContainer = GetContainerClient(blobContainerName);

                var blobClient = blobContainer.GetBlobClient(fileName);
                var result = blobClient.Upload(ms, new BlobHttpHeaders { ContentType = contentType });
                return blobClient.Uri;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName)
        {
            // Get refrence of the container 
            var containerClient = GetContainerClient(blobContainerName);
            // create a space for a file in the container.
            var blobClient = containerClient.GetBlobClient(fileName);
            // upload the bytes of the file in that space
            await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
            // retutrns the URI  of the file create.
            return blobClient.Uri;
        }

        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11;
                var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
                containerClient.CreateIfNotExists(PublicAccessType.Blob);
                return containerClient;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
    }
}