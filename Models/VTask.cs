using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VihoTask.Models
{
    public class VTask
    {
        public long VTaskID { get; set; }

        public string VTaskName { get; set; }

        public string VTaskDescription { get; set;}

        public string VUserID { get; set; }

        public VUser VUser { get; set; }

        public string VTaskStatus { get; set;}

        public string VTaskType { get; set;}

        public string VTaskDateStart { get; set;}

        public string VTaskDateEnd { get; set;} 

        public string VTaskPriority { get; set;}

        public byte[] VTaskFile { get; set;}

        public string VTaskFileExtension { get; set; }

        [NotMapped]
        public IFormFile FileUpdate { get; set; }

    }
}
