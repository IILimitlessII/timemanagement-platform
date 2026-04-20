using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimemanagementPlatform.Web.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        public string SemesterName { get; set; } = null!;
        public int SemesterWeeks { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; } // Foreign key
        public ApplicationUser? User { get; set; }

        // Navigation property for the relationship with Module
        public List<Module>? Modules { get; set; }

        // Navigation property for the relationship with ModuleHour
        public List<ModuleHour>? ModuleHours { get; set; }
    }
}
