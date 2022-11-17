using System.ComponentModel.DataAnnotations;

namespace File_Automation.Models
{
    public class Move
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Operation { get; set; }
        public string SourceEnvironment { get; set; }
        public string TargetEnvironment { get; set; }
        public string SourceStorage { get; set; }
        public string DestinationStorage { get; set; }
        public string SourceContainer { get; set; }
        public string DestinationContainer { get; set; }
        public string SourceAzFolderName { get; set; }
        public string DestAzFolderName { get; set; }
        public string File { get; set; }
        public string currentDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
