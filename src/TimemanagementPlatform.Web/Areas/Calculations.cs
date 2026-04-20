using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TimemanagementPlatform.Web.Data;

namespace TimemanagementPlatform.Web.Areas
{
    public class Calculations
    {
        public static float CalcSelfStudy(int credits, int classhours, int semesterId, ApplicationDbContext dbContext)
        {
            int weeks = dbContext.Semesters
            .Where(s => s.SemesterId == semesterId)
            .Select(s => s.SemesterWeeks)
            .FirstOrDefault();
            float selfstudyhours = ((credits * 10) / weeks) - classhours;
            return selfstudyhours;
        }

        public static float TimeLeft(int hours, DateTime date,int moduleId)
        {
            // Get the current date
            DateTime today = DateTime.Now;

            // Calculate the difference between the current day and Monday
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;

            // Calculate the beginning of the week (Monday)
            DateTime beginningOfWeek = today.AddDays(-daysUntilMonday);

            // Calculate the end of the week (Sunday)
            DateTime endOfWeek = beginningOfWeek.AddDays(6);

            float hoursLeft = 0;
            hoursLeft -= hours;
            return hoursLeft;
        }

        public static Dictionary<DateTime, float> CalculateWeeklyStudyHours(int moduleId, ApplicationDbContext dbContext)
        {
            // Initialize the dictionary to store weekly study hours
            Dictionary<DateTime, float> weeklyStudyHours = new Dictionary<DateTime, float>();

            // Get the earliest date for the specified module
            DateTime earliestDate = dbContext.ModuleHours
                .Where(mh => mh.ModuleId == moduleId)
                .OrderBy(mh => mh.Date)
                .Select(mh => mh.Date)
                .FirstOrDefault();

            // If there are no study hours for the module, return an empty dictionary
            if (earliestDate == default)
            {
                return weeklyStudyHours;
            }

            // Calculate the beginning of the week (Monday) for the week of the earliest date
            int daysUntilEarliestMonday = ((int)DayOfWeek.Monday - (int)earliestDate.DayOfWeek + 7) % 7;
            DateTime beginningOfWeek = earliestDate.AddDays(-daysUntilEarliestMonday);

            // Get the current date
            DateTime today = DateTime.Now;

            // Iterate over the weeks starting from the week of the earliest date
            while (beginningOfWeek <= today)
            {
                // Calculate the end of the week (Sunday)
                DateTime endOfWeek = beginningOfWeek.AddDays(6);

                // Sum the hours studied for the current week
                float hoursStudied = dbContext.ModuleHours
                    .Where(mh => mh.ModuleId == moduleId && mh.Date >= beginningOfWeek && mh.Date <= endOfWeek)
                    .Sum(mh => mh.Hours);

                // Add the weekly study hours to the dictionary
                weeklyStudyHours.Add(beginningOfWeek, hoursStudied);

                // Move to the beginning of the next week
                beginningOfWeek = beginningOfWeek.AddDays(7);
            }

            return weeklyStudyHours;
        }
    }
}
