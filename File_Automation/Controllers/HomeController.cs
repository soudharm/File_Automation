﻿using File_Automation.DbContexts;
using File_Automation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
//using Microsoft.Data.SqlClient;
using System.Management.Automation;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Azure.Storage.Sas;
using Azure;
using System.Reflection.Metadata;
using System.Data;
using System.ComponentModel;
using System.Web;
using System.Collections;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Session;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace File_Automation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SqlCommand com=new SqlCommand();
        //SqlDataReader dr;
        SqlConnection con=new SqlConnection();
        //List<Upload> Uploads=new List<Upload>();
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private IConfiguration Configuration;
        static string Test_aznesttdaveehstaging1; 
        static string Test_aznesttdaveadls1;
        static string Dev_aznestddaveehstaging1;
        static string Dev_aznestddaveadls1;
        static string Prod_aznestpdaveehstaging1;
        static string Prod_aznestpdaveadls1;
        static string Prod_diadifstorage002;
        static string Test_diadifstorage001;
        static string logpath; //= "C:\\Users\\soupatil\\source\\repos\\File_Automation\\File_Automation\\wwwroot\\images\\out.txt";
        //static string logpath = "~/images/out.txt";
        


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration _configuration)
        {
            _logger = logger;
            _db = db;
            Configuration = _configuration;
            con.ConnectionString = this.Configuration.GetConnectionString("DefaultConnection");
            Test_aznesttdaveehstaging1 = Configuration.GetValue<string>("StorageAccounts:Test_aznesttdaveehstaging1");
            Test_aznesttdaveadls1 = Configuration.GetValue<string>("StorageAccounts:Test_aznesttdaveadls1");
            Dev_aznestddaveehstaging1 = Configuration.GetValue<string>("StorageAccounts:Dev_aznestddaveehstaging1");
            Dev_aznestddaveadls1 = Configuration.GetValue<string>("StorageAccounts:Dev_aznestddaveadls1");
            Prod_aznestpdaveehstaging1 = Configuration.GetValue<string>("StorageAccounts:Prod_aznestpdaveehstaging1");
            Prod_aznestpdaveadls1 = Configuration.GetValue<string>("StorageAccounts:Prod_aznestpdaveadls1");
            Prod_diadifstorage002 = Configuration.GetValue<string>("StorageAccounts:Prod_diadifstorage002");
            Test_diadifstorage001 = Configuration.GetValue<string>("StorageAccounts:Test_diadifstorage001");
            logpath = Configuration.GetValue<string>("LogFile:logpath");
        }

        //--Get Index Action
        public IActionResult Index()
        {
            return View();
        }

        //--Get Upload Action
        public IActionResult Upload()
        {
            return View();
        }

        //--Post Upload Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(Upload model)
        {
            FileStream filestream = new FileStream(logpath, FileMode.Create);
            StreamWriter streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            try
            {
                string containerName = model.DestContainer;
                //List<string> folderpath = new List<model.LocalPath.FileName>;

                //foreach (var file in folderpath)
                //{
                //    Console.WriteLine(file);
                //}
                //var files = Directory.GetFiles(folderpath, model.FileName, SearchOption.AllDirectories);

                //BlobContainerClient containerClient = new BlobContainerClient(connection(model.environment, model.storage), containerName);
                BlobServiceClient blobServiceClient = new BlobServiceClient(connection(model.environment, model.storage));
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                

                //BlobClient blob=containerClient.GetBlobClient(folderpath);
                foreach (IFormFile path in model.LocalPath)
                {
                    var filePathOverCloud = model.AzFolderName + "/" + path.FileName;


                    using (Stream stream = path.OpenReadStream())
                    {
                        //blob.Upload(stream);
                        BlobClient blobClient = containerClient.GetBlobClient(filePathOverCloud);
                        blobClient.Upload(stream, overwrite: true);
                    }
                    Console.WriteLine(model.AzFolderName + "/" + path.FileName + " Uploaded");
                }

                streamwriter.Close();
            }
            catch
            {
                streamwriter.Close();
                TempData["alertMessage"] = "Please Provide the details Correctly";
                return RedirectToAction("UploadIssue");
                //string message = ex.Message;
                //Console.WriteLine(message);
            }
            streamwriter.Close();
            _db.Uploads.Add(model);
            _db.SaveChanges();

            //System.Threading.Thread.Sleep(1000);

            return RedirectToAction("Logs");        

        }

        public IActionResult UploadIssue()
        {
            return View();
        }

        public IActionResult Move()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(Move model)
        {
            FileStream filestream = new FileStream(logpath, FileMode.Create);
            StreamWriter streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            try
            {
                BlobContainerClient sourceContainerClient = new BlobContainerClient(connection(model.SourceEnvironment, model.SourceStorage), model.SourceContainer);
                BlobContainerClient targetContainerClient = new BlobContainerClient(connection(model.TargetEnvironment, model.DestinationStorage), model.DestinationContainer);
                

                //Console.WriteLine("Sending copy blob request....");

                var blobs = sourceContainerClient.GetBlobs(prefix: model.SourceAzFolderName + "//");

                //IAsyncEnumerable<BlobItem> segment = sourceContainerClient.GetBlobsAsync(prefix: model.File);
                //await foreach (BlobItem blobItem in segment)
                foreach (BlobItem blobItem in blobs)
                {
                    //Console.WriteLine(blobItem.Name);
                    //Console.WriteLine("before");
                    if (model.File != null)
                    {
                        if (model.DestAzFolderName != null)
                        {
                            if (blobItem.Name.Contains(model.File) && blobItem.Name.Contains("."))
                            {
       
                                string createfile = Getfilename(blobItem.Name);
                                var result = await targetContainerClient.GetBlobClient(model.DestAzFolderName + "/" + createfile).StartCopyFromUriAsync(GetSharedAccessUri(blobItem.Name, sourceContainerClient));

                                Console.WriteLine(model.DestAzFolderName + "/" + createfile+" copied.");
                            }
                        }
                        else if (model.DestAzFolderName == null)
                        {
                            if (blobItem.Name.Contains(model.File) && blobItem.Name.Contains("."))
                            {
                                var result = await targetContainerClient.GetBlobClient(blobItem.Name).StartCopyFromUriAsync(GetSharedAccessUri(blobItem.Name, sourceContainerClient));
                                Console.WriteLine(blobItem.Name + " copied.");
                            }
                        }

                    }
                    else if (model.File == null)
                    {
                        if (model.DestAzFolderName != null)
                        {
                            if (blobItem.Name.Contains("."))
                            {
                                
                                string createfile = Getfilename(blobItem.Name);
                                //Console.WriteLine("after" + "//" + createfile);
                                var result = await targetContainerClient.GetBlobClient(model.DestAzFolderName + "/" + createfile).StartCopyFromUriAsync(GetSharedAccessUri(blobItem.Name, sourceContainerClient));

                                Console.WriteLine(model.DestAzFolderName + "/" + createfile + " copied.");
                            }
                        }
                        else if (model.DestAzFolderName == null)
                        {
                            if (blobItem.Name.Contains("."))
                            {
                                var result = await targetContainerClient.GetBlobClient(blobItem.Name).StartCopyFromUriAsync(GetSharedAccessUri(blobItem.Name, sourceContainerClient));
                                Console.WriteLine(blobItem.Name + " copied.");
                            }
                        }
                    }
                }
                streamwriter.Close();
            }
            catch//(Exception ex)
            {
                streamwriter.Close();
                TempData["alertMessage"] = "Please Provide the details Correctly";
                return RedirectToAction("UploadIssue");
                //string message = ex.Message;
                //Console.WriteLine(message);
            }
            streamwriter.Close();
            _db.Copies.Add(model);
            _db.SaveChanges();

            //System.Threading.Thread.Sleep(1000);
            return RedirectToAction("Logs");
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Delete model)
        {
            FileStream filestream = new FileStream(logpath, FileMode.Create);
            StreamWriter streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            try
            {
                BlobContainerClient containerClient = new BlobContainerClient(connection(model.environment, model.storage), model.Container);

                //ArrayList list = new ArrayList();
                //List<BlobItem> blobItems = new List<BlobItem>();
                BlobClient sourceBlob = containerClient.GetBlobClient(model.AzFolderName);
                

                var blobs = containerClient.GetBlobs(prefix: model.AzFolderName + "//");

                foreach (var blob in blobs)
                {
                    //Console.WriteLine(blob);
                    var blobClient = containerClient.GetBlobClient(blob.Name);
                    if (model.FileName!=null)
                    {
                        if (blob.Name.Contains(model.FileName) && blob.Name.Contains("."))
                        {
                            blobClient.Delete();
                            Console.WriteLine(blob.Name + " is deleted");
                        }
                    }
                    else if(model.FileName == null)
                    {
                        if (blob.Name.Contains("."))
                        {
                            blobClient.Delete();
                            Console.WriteLine(blob.Name + " is deleted");
                        }
                    }
                        
                }
                bool blobExisted = await sourceBlob.DeleteIfExistsAsync();
                //if (blobExisted==false)
                //{
                //TempData["alertMessage"] = "Please Provide the details Correctly";
                //return RedirectToAction("UploadIssue");
                //}
                streamwriter.Close();
            }
            //catch (DirectoryIsNotEmpty)
            //{
            //    Console.WriteLine("Hello");
            //}
            catch (Exception ex)
            {
                
                if (ex.Message.Contains("DirectoryIsNotEmpty"))
                {
                    Console.WriteLine("");
                    //We get this exception when the it tries to delete more files from the specified directory.
                }
                else
                {
                    streamwriter.Close();
                    //string message = ex.Message;
                    //Console.WriteLine(message);
                    TempData["alertMessage"] = "Please Provide the details Correctly";
                    return RedirectToAction("UploadIssue");
                }
                
            }
            streamwriter.Close();
            _db.Deletes.Add(model);
            _db.SaveChanges();

            System.Threading.Thread.Sleep(1000);
            return RedirectToAction("Logs");
        }

        public IActionResult Logs()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginIssue()
        {
            return View();
        }

        public string connection(string env, string src)
        {
            if (env == "Test" && src == "aznesttdaveehstaging1")
            {
                return Test_aznesttdaveehstaging1;
            }
            else if (env == "Test" && src == "aznesttdaveadls1")
            {
                return Test_aznesttdaveadls1;
            }
            else if (env == "Dev" && src == "aznestddaveehstaging1")
            {
                return Dev_aznestddaveehstaging1;
            }
            else if (env == "Dev" && src == "aznestddaveadls1")
            {
                return Dev_aznestddaveadls1;
            }
            else if (env == "Prod" && src == "aznestpdaveehstaging1")
            {
                return Prod_aznestpdaveehstaging1;
            }
            else if (env == "Prod" && src == "aznestpdaveadls1")
            {
                return Prod_aznestpdaveadls1;
            }
            else if (env == "Prod" && src == "diadifstorage002")
            {
                return Prod_diadifstorage002;
            }
            else if (env == "Test" && src == "diadifstorage001")
            {
                return Test_diadifstorage001;
            }
            return null;
        }

        public string Getfilename(string filename)
        {
            int index = filename.IndexOf("/");
            string returnname=filename.Substring(index+1);
            return returnname;
        }
        


        private static Uri GetSharedAccessUri(string blobName, BlobContainerClient container)
        {
            DateTimeOffset expiredOn = DateTimeOffset.UtcNow.AddMinutes(60);

            BlobClient blob = container.GetBlobClient(blobName);
            Uri sasUri = blob.GenerateSasUri(BlobSasPermissions.Read, expiredOn);

            return sasUri;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
