using System.ComponentModel.DataAnnotations;

namespace File_Automation.Models
{
    public class Delete
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Operation { get; set; }
        public string environment { get; set; }
        public string storage { get; set; }
        public string Container { get; set; }
        public string AzFolderName { get; set; }
        public string FileName { get; set; }
        public string currentDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
