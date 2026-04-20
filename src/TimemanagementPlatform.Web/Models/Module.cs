using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimemanagementPlatform.Web.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; } // Unique code for the module PK
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
        public int ClassHrsWeekly { get; set; }
        public float? HrsSelfStudy { get; set; }
        public float? HrsSelfStudyLeft { get; set; }
        public string? SelectedColourHex { get; set; }
        public string? SelectedColourName { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; } = null!; // Foreign key
        public ApplicationUser? User { get; set; }

        public int SemesterId { get; set; } // Foreign key
        public Semester? Semester { get; set; }
    }
}
