using File_Automation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Security.Claims;
using System.Data.SqlClient;
using File_Automation.DbContexts;
using Microsoft.AspNetCore.Authorization;

namespace File_Automation.Controllers
{
    
    public class LoginController : Controller
    {
        SqlCommand com = new SqlCommand();
        //SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        private readonly ILogger<LoginController> _logger;
        private readonly ApplicationDbContext _db;
        private IConfiguration Configuration;
        public LoginController(ILogger<LoginController> logger, ApplicationDbContext db, IConfiguration _configuration)
        {
            _logger = logger;
            _db = db;
            Configuration = _configuration;
            con.ConnectionString = this.Configuration.GetConnectionString("DefaultConnection");
        }
        
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach(var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Serializable]
        public IActionResult Login(Login model)
        {
            //FetchData(model.UserName,model.Password);
            string username = model.UserName;
            string password = model.Password;
            string user = string.Empty;



            try
            {
                con.Open();
                com.Connection = con;
                //string querry = "SELECT * FROM [FileAutomation].[dbo].[Logins] where Username='" + username + "' AND password= '" + password + "'";
                string querry = "SELECT * FROM [dbo].[Logins] where Username='" + username + "' AND password= '" + password + "'";
                //     SqlDataAdapter---Represents a set of data commands and a database connection that are used to
                //     fill the System.Data.DataSet and update a SQL Server database. This class cannot
                //     be inherited.
                SqlDataAdapter sda = new SqlDataAdapter(querry, con);

                //     DataTable---Represents one table of in-memory data.

                DataTable dtable = new DataTable();
                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,username)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();
                    HttpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                    //              var claims = new List<Claim>
                    //              {

                    //              };

                    //              var identity = new ClaimsIdentity(claims,
                    //CookieAuthenticationDefaults.AuthenticationScheme);

                    //              HttpContext.SignInAsync(
                    //                CookieAuthenticationDefaults.AuthenticationScheme,
                    //                new ClaimsPrincipal(identity));

                    //string someValue = (string)Session["SessionVariableNameHere"];
                    //HttpContext.Session.["SessionVariableNameHere"] = "Flag";
                    con.Close();
                    HttpContext.Session.SetString("user", username);
                    //TempData["user"] = username;

                    return RedirectToAction("Index","Home");

                }
                else
                {
                    //return Content("UserName or Password is wrong");
                    con.Close();
                    return RedirectToAction("LoginIssue","Home");

                }
            }
            catch
            {

            }


            //con.Open();
            //com.Connection = con;
            //com.CommandText = "UPDATE [FileAutomation].[dbo].[Uploads] SET [FileAutomation].[dbo].[Uploads].[Processed]='Yes'";
            //com.ExecuteNonQuery();
            return RedirectToAction("Login");
        }
    }
}
