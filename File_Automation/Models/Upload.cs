
using System.ComponentModel.DataAnnotations;





namespace File_Automation.Models
{
    public class Upload
    {

        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Operation { get; set; }
        public string environment { get; set; }
        public string storage { get; set; }

        public List<IFormFile> LocalPath { get; } = new List<IFormFile>();
        public string DestContainer { get; set; }
        public string AzFolderName { get; set; }
        //public string FileName { get; set; }

        //DateTime myDateTime = DateTime.Now;
        public string currentDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
