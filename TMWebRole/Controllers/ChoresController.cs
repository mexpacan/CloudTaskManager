using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMWebRole;
using TMWebRole.Models;
using TMWebRole.Persistence;

namespace TMWebRole.Controllers
{
    /// <summary>
    /// This is the class controler for the Chores
    /// </summary>
    public class ChoresController : Controller
    {
        private TaskManagerContext db = new TaskManagerContext(); //DB Context
        private BlobService blobService = new BlobService("StorageConnection","chorefile"); //Blob operations

        // GET: Chores
        public ActionResult Index()
        {
            //Only show chores releated to current user
            return View(db.Chores.ToList().Where(chore => chore.User == User.Identity.Name));
        }

        // GET: Chores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // GET: Chores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoreId,Name,Note,StartDate,DueDate,Reminder,Recurrence,Priority,Status,Location,Attachment,User")] Chore chore, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {  
                //We verify if there is a file to be uploaded.
                if (file != null)
                {
                    chore.Attachment = blobService.UploadFile(file);
                }
                chore.User = User.Identity.Name;   //We asing the current user to the chore
                db.Chores.Add(chore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chore);
        }

        // GET: Chores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoreId,Name,Note,StartDate,DueDate,Reminder,Recurrence,Priority,Status,Location,Attachment,User")] Chore chore, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //First we verify if there is a new file to be uploaded
                if (file != null)
                {
                    //Secondly we verify if there is an attachment already saved.
                    //If so, we proceed to delete it first before uploading the new file.
                    if (!string.IsNullOrWhiteSpace(chore.Attachment))
                    {
                        blobService.DeleteFile(chore.Attachment);
                    }
                    chore.Attachment = blobService.UploadFile(file);
                }
                chore.User = User.Identity.Name;   //We asing the current user to the chore
                db.Entry(chore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chore);
        }

        // GET: Chores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chore chore = db.Chores.Find(id);

            //We verify if there is an attachment so we proceed to delete it as well.
            if (!string.IsNullOrWhiteSpace(chore.Attachment))
            {
                blobService.DeleteFile(chore.Attachment);
            }

            db.Chores.Remove(chore);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Chores/Download/5
        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            else
            {
                //We verify if there is an attachment we can download
                //If so, we send a message to the queue in roder for 
                //the worker role to download the file. Otherwise
                //we do nothing and go back to the index view.
                if (!string.IsNullOrWhiteSpace(chore.Attachment))
                {
                    sendDownload(chore.Attachment);
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sends a message to a queue containing the url of the file
        /// to be downloaded from the blob storage.
        /// </summary>
        /// <param name="fileUrl"></param>
        private void sendDownload(string fileUrl)
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

                // Create a message and add it to the queue.
                CloudQueueMessage message = new CloudQueueMessage(fileUrl);
                queue.AddMessage(message);

                Console.WriteLine("SEND MESSAGE: " + message.AsString);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.ToString());
            }
        }
    }
}
