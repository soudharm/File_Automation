using System.ComponentModel.DataAnnotations;

namespace File_Automation.Models
{
    //[Serializable]
    public class Login
    {
        //public Login() { }
        //public Login(string WebUserName)
        //{
        //UserName = WebUserName;
        //}
        //Login stud = new Login();
        //ViewState["CurrentStudent"] = stud;

        [Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
