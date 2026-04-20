using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TimemanagementPlatform.Web.Data;
using TimemanagementPlatform.Web.Models;

namespace TimemanagementPlatform.Web.Controllers
{
    [Authorize]
    public class ModuleHoursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleHoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModuleHours
        public async Task<IActionResult> Index()
        {
            // Retrieve the current user's id
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Filter the ModuleHours based on the current user's id
            var moduleHours = _context.ModuleHours
                .Include(mh => mh.Module)
                .Include(mh => mh.Semester)
                .Include(mh => mh.User)
                .Where(mh => mh.UserId == currentUserId);

            return View(await moduleHours.ToListAsync());
        }

        // GET: ModuleHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ModuleHours == null)
            {
                return NotFound();
            }

            var moduleHour = await _context.ModuleHours
                .Include(m => m.Module)
                .Include(m => m.Semester)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ModuleHourId == id);
            if (moduleHour == null)
            {
                return NotFound();
            }

            return View(moduleHour);
        }

        // GET: ModuleHours/Create
        public IActionResult Create()
        {
            // Retrieve the current user's id
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Populate the Module dropdown with Module Names associated with the current user
            ViewData["ModuleId"] = new SelectList(_context.Modules.Where(m => m.UserId == currentUserId), "ModuleId", "Name");

            return View();
        }

        // POST: ModuleHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleHourId,Hours,Date,ModuleId,UserId,SemesterId")] ModuleHour moduleHour)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user's id
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Find the associated module to get the SemesterId
                var associatedModule = await _context.Modules.FindAsync(moduleHour.ModuleId);

                // Check if associatedModule is null
                if (associatedModule == null)
                {
                    // Handle the situation where associatedModule is null
                    ModelState.AddModelError("ModuleId", "Invalid Module selected");
                    return View(moduleHour);
                }

                // Set the UserId and SemesterId for moduleHour
                moduleHour.UserId = currentUserId;
                moduleHour.SemesterId = associatedModule.SemesterId;

                // Add and save the moduleHour
                _context.Add(moduleHour);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Populate the Module dropdown initially (before any selection)
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "Name", moduleHour.ModuleId);

            // Populate the Semester dropdown with Semester Names associated with the current user
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterName", moduleHour.SemesterId);

            return View(moduleHour);
        }

        // GET: ModuleHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ModuleHours == null)
            {
                return NotFound();
            }

            var moduleHour = await _context.ModuleHours.FindAsync(id);
            if (moduleHour == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", moduleHour.ModuleId);
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", moduleHour.SemesterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", moduleHour.UserId);
            return View(moduleHour);
        }

        // POST: ModuleHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleHourId,Hours,Date,ModuleId,UserId,SemesterId")] ModuleHour moduleHour)
        {
            if (id != moduleHour.ModuleHourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleHour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleHourExists(moduleHour.ModuleHourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", moduleHour.ModuleId);
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", moduleHour.SemesterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", moduleHour.UserId);
            return View(moduleHour);
        }

        // GET: ModuleHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ModuleHours == null)
            {
                return NotFound();
            }

            var moduleHour = await _context.ModuleHours
                .Include(m => m.Module)
                .Include(m => m.Semester)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ModuleHourId == id);
            if (moduleHour == null)
            {
                return NotFound();
            }

            return View(moduleHour);
        }

        // POST: ModuleHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ModuleHours == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ModuleHours'  is null.");
            }
            var moduleHour = await _context.ModuleHours.FindAsync(id);
            if (moduleHour != null)
            {
                _context.ModuleHours.Remove(moduleHour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleHourExists(int id)
        {
            return (_context.ModuleHours?.Any(e => e.ModuleHourId == id)).GetValueOrDefault();
        }

        public IActionResult Graph()
        {
            // Retrieve the current user's id
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Populate the Module dropdown with Module Names associated with the current user
            var moduleSelectList = new SelectList(_context.Modules.Where(m => m.UserId == currentUserId), "ModuleId", "Name");

            // Create an empty view model
            var viewModel = new ModuleHourGraphViewModel
            {
                Dates = new List<string>(),
                ModuleHours = new List<int>(),
                SelfStudyHours = 0,
                ModuleSelectList = moduleSelectList
            };

            return View(viewModel);
        }

        [HttpGet]
        public JsonResult GetModuleChartData(int moduleId)
        {
            try
            {
                // Retrieve the current user's id
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Query the database for ModuleHours associated with the selected moduleId
                var moduleHours = _context.ModuleHours
                    .Include(mh => mh.Module)
                    .Where(mh => mh.UserId == currentUserId && mh.ModuleId == moduleId)
                    .OrderBy(mh => mh.Date)
                    .ToList();

                var module = _context.Modules
                    .Where(m => m.UserId == currentUserId && m.ModuleId == moduleId)
                    .FirstOrDefault();

                // Check if module is null
                if (module == null)
                {
                    throw new InvalidOperationException($"Module with id {moduleId} not found for the current user.");
                }

                var semesterId = module.SemesterId;

                var semesterInfo = _context.Semesters
                    .Where(s => s.SemesterId == semesterId)
                    .FirstOrDefault();

                // Check if semesterInfo is null
                if (semesterInfo == null)
                {
                    throw new InvalidOperationException($"Semester with id {semesterId} not found.");
                }

                DateTime semesterStartDate = semesterInfo.StartDate;
                int semesterWeeks = semesterInfo.SemesterWeeks;

                // Calculate the end date by adding weeks to the start date
                DateTime semesterEndDate = semesterStartDate.AddDays(semesterWeeks * 7);

                // Calculate total study hours for the module
                var totalStudyHours = module?.HrsSelfStudy ?? 0;

                // Create a list of all weeks in the semester
                var allWeeks = Enumerable.Range(1, semesterWeeks).ToList();

                // Calculate the week of the semester for each ModuleHour date
                var weekNumberModuleHours = moduleHours
                    .GroupBy(mh => CalculateWeekNumber(mh.Date, semesterStartDate))
                    .Select(group => new
                    {
                        WeekNumber = group.Key,
                        TotalHours = group.Sum(mh => mh.Hours)
                    })
                    .ToList();

                // Merge the calculated weekNumberModuleHours with all weeks in the semester
                var mergedWeeks = allWeeks
                    .GroupJoin(weekNumberModuleHours,
                        allWeek => allWeek,
                        moduleWeek => moduleWeek.WeekNumber,
                        (allWeek, moduleWeeks) => new
                        {
                            WeekNumber = allWeek,
                            TotalHours = moduleWeeks.Any() ? moduleWeeks.Sum(mw => mw.TotalHours) : 0
                        })
                    .ToList();

                // Populate the ModuleHourGraphViewModel
                var chartData = new ModuleHourGraphViewModel
                {
                    Dates = mergedWeeks.Select(wn => $"{wn.WeekNumber}").ToList(),
                    ModuleHours = mergedWeeks.Select(wn => wn.TotalHours).ToList(),
                    SelfStudyHours = totalStudyHours,
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                
                //return an error response
                return Json(new { error = ex.Message });
            }
        }


        // Helper method to calculate the week number within the semester
        private int CalculateWeekNumber(DateTime date, DateTime semesterStartDate)
        {
            // Calculate the number of days between the date and the semester start date
            int days = (int)(date - semesterStartDate).TotalDays;

            // Calculate the week number by dividing the days by 7
            int weekNumber = (days / 7) + 1;

            return weekNumber;
        }

        public ActionResult Calendar()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve module hours for the current user
            var moduleHours = _context.ModuleHours
                .Include(mh => mh.Module)  
                .Where(mh => mh.UserId == currentUserId)
                .ToList();

            // Map module hours to events view model
            var events = moduleHours.Select(mh => new EventsViewModel
            {
                title = $"{mh.Module?.Code}, {mh.Hours}",  
                start = mh.Date,
                end = mh.Date.AddDays(1),
                color = mh.Module?.SelectedColourHex ?? "#6c1c6a"
            }).ToList();

            // Serialize events to JSON string
            var eventsJson = JsonSerializer.Serialize(events);

            // Pass the events JSON string to the view
            ViewBag.EventsJson = eventsJson;

            return View();
        }


    }
}
