
using System.ComponentModel.DataAnnotations;





namespace File_Automation.Models
{
    public class Upload
    {
        
    //Login login=new Login();
    //public Upload() : base() { }
    //public Upload(string UserName) : base(UserName)
    //{
    //WebUserName = UserName;
    //}

    //public Login Login { get; set; }
    [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Operation { get; set; }
        public string environment { get; set; }
        public string storage { get; set; }
        public string LocalPath { get; set; }
        public string DestContainer { get; set; }
        public string AzFolderName { get; set; }
        public string FileName { get; set; }

        //DateTime myDateTime = DateTime.Now;
        public string currentDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");



        //public string UserName { get; set; }
        //public string WebUserName { get; set; } //= login.UserName;
        //var UserName = Login.UserName;
        //public string WebUserName { get; set; }//= TempData["user"];
        //{
        //get
        //{ return WebUserName = Login.UserName; }
        //set { }
        //{
        //  WebUserName=login.UserName;
        //}
        //} //= User.Identity.GetUseId();
        //public DateTime currentDateTime = DateTime.Now;
        //public string Password { get; set; }

        
    }
}
