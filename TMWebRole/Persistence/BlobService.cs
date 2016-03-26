using Microsoft.Owin.Logging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TMWebRole.Persistence
{
    /// <summary>
    /// BlobService class.
    /// It performes all the task in relation to Adding and Deleting files
    /// in a Blob container.
    /// </summary>
    public class BlobService
    {
        private string storageConnection;
        private string blobContainer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="container"></param>
        public BlobService(string connection, string container)
        {
            this.storageConnection = connection;
            this.blobContainer = container;
        }

        /// <summary>
        /// Creates a Configure a Connection to
        /// Azure Storage and a Blob Container
        /// </summary>
        public void CreateAndConfigure()
        {
            try
            {
                // Retrieve storage account from connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting(storageConnection));

                //Create the blob client
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //Retrieve a reference to a container
                CloudBlobContainer container = blobClient.GetContainerReference(blobContainer);

                // Create the "files" container if it doesn't already exist.
                if (container.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container
                    container.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }
        }

        /// <summary>
        /// Deletes a given file from a Blob Container
        /// </summary>
        /// <param name="blobUrl"></param>
        public void DeleteFile(string blobUrl)
        {
            try
            {
                string file = Path.GetFileName(blobUrl);

                // Retrieve storage account from connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting(storageConnection));

                //Create the blob client
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //Retrieve a reference to a container
                CloudBlobContainer container = blobClient.GetContainerReference(blobContainer);

                // Retrieve reference to a blob named "myblob.txt".
                
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file);

                // Delete the blob.
                blockBlob.Delete();

                Trace.TraceInformation("File deleted!");
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }
        }

        /// <summary>
        /// Uploads a file to a Blob Container.
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <returns>The Uri to the file</returns>
        public string UploadFile(HttpPostedFileBase fileToUpload)
        {
            if (fileToUpload == null || fileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = string.Empty;

            try
            {
                // Retrieve storage account from connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting(storageConnection));

                //Create the blob client
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //Retrieve a reference to a container
                CloudBlobContainer container = blobClient.GetContainerReference(blobContainer);

                //Create the container if it doesn't exist
                container.CreateIfNotExists();

                // configure container for public access
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Create a unique name for the file we are about to upload
                string fileName = String.Format("{0}-{1}{2}",
                    "file",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(fileToUpload.FileName));

                // Upload file to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                blockBlob.Properties.ContentType = fileToUpload.ContentType;
                blockBlob.UploadFromStream(fileToUpload.InputStream);

                fullPath = blockBlob.Uri.ToString();

                Trace.TraceInformation("File uploaded!: " + fullPath);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }

            return fullPath;
        }

    }
}