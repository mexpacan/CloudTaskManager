using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace TMWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("TMWorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("TMWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TMWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("TMWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            
            while (!cancellationToken.IsCancellationRequested)
            {
                readMessage(); //We read the messages from the queue
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
                
            }
        }

        /// <summary>
        /// Reads the messages from the queue
        /// </summary>
        private void readMessage()
        {
            try
            {

                // Retrieve storage account from connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

                // Create the queue client
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                // Retrieve a reference to a queue
                CloudQueue queue = queueClient.GetQueueReference("chorequeue");

                // Create the queue if it doesn't already exist
                queue.CreateIfNotExists();

                // Get the next message
                CloudQueueMessage retrievedMessage = queue.GetMessage();

                // Display message.
                if (retrievedMessage == null) return;
                Trace.TraceInformation("WorkerRole Got Message!!!!!");

                DownloadFile(storageAccount, retrievedMessage.AsString); //Download file

                queue.DeleteMessage(retrievedMessage);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }
        }

        /// <summary>
        /// Downloads the given file from the blob container to
        /// a local drive (Path).
        /// </summary>
        /// <param name="storageAccount"></param>
        /// <param name="fileName"></param>
        private void DownloadFile(CloudStorageAccount storageAccount, string fileUrl)
        {
            string file = Path.GetFileName(fileUrl);

            try
            {
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("chorefile");

                // Retrieve reference to a blob named "file".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file);

                string filePath;

                //The file is downloaded to a local Specialized Folder "MyDocuments"
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path = path + "\\";

                filePath = path + file;

                // Save blob contents to a file.
                using (var fileStream = System.IO.File.OpenWrite(@filePath))
                {
                    blockBlob.DownloadToStream(fileStream);
                }

                Trace.TraceInformation("File download succesfull!");

            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }
        }
    }
}
