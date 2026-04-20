using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimemanagementPlatform.Web.Models
{
    public class ModuleHour
    {
        [Key]
        public int ModuleHourId { get; set; }  // Unique identifier for the study time entry PK
        public int Hours { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [ForeignKey("Module")]
        public int ModuleId { get; set; } // Foreign key
        public Module? Module { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; } = null!; // Foreign key
        public ApplicationUser? User { get; set; }

        public int SemesterId { get; set; } // Foreign key
        public Semester? Semester { get; set; }
    }
}
