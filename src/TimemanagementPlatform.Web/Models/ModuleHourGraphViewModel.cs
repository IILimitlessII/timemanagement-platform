using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimemanagementPlatform.Web.Models
{
    public class ModuleHourGraphViewModel
    {
        public List<string>? Dates { get; set; }
        public List<int>? ModuleHours { get; set; }
        public float? SelfStudyHours { get; set; }
        public SelectList? ModuleSelectList { get; set; }
    }
}
