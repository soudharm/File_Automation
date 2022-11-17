using File_Automation.DbContexts;
using File_Automation.Models;
using Microsoft.AspNetCore.Mvc;

namespace File_Automation.Controllers
{
    public class UploadController:Controller
    {
        private readonly ApplicationDbContext _db;
        public UploadController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public IActionResult Upload(Upload model)
        //{
            //string environment = model.environment;
            //return View(env);
            //return RedirectToAction(Script);
            //return RedirectToAction(Runscript);
            //return View(model);
            //return environment;
        //}
    }
}
