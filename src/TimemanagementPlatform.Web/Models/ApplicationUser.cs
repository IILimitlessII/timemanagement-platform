using Microsoft.AspNetCore.Identity;

namespace TimemanagementPlatform.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Semester> Semesters { get; set; } = new List<Semester>();

        public ICollection<Module> Modules { get; set; } = new List<Module>();

        public ICollection<ModuleHour> ModuleHours { get; set; } = new List<ModuleHour>();
    }
}
