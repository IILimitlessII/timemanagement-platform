using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimemanagementPlatform.Web.Areas;
using TimemanagementPlatform.Web.Data;
using TimemanagementPlatform.Web.Models;

namespace TimemanagementPlatform.Web.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            // Retrieve the current user's id
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Filter the modules based on the current user's id
            var modules = _context.Modules
                .Include(m => m.Semester)
                .Include(m => m.User)
                .Where(m => m.UserId == currentUserId);

            return View(await modules.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .Include(m => m.Semester)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            // Retrieve the current user's id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get the semesters for the current user
            var userSemesters = _context.Semesters
                .Where(s => s.UserId == userId)
                .Select(s => new SelectListItem
                {
                    Value = s.SemesterId.ToString(),
                    Text = s.SemesterName
                })
                .ToList();

            var colours = Colour();

            ViewData["Colour"] = new SelectList(colours, "Value", "Text");
            ViewData["SemesterId"] = new SelectList(userSemesters, "Value", "Text");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,Code,Name,Credits,ClassHrsWeekly,HrsSelfStudy,HrsSelfStudyLeft,SelectedColourHex,SelectedColourName,UserId,SemesterId")] Module module)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            module.UserId = userId;
            if (ModelState.IsValid)
            {
                module.SelectedColourName = module.SelectedColourHex;
                module.SelectedColourHex = GetHexFromColorName(module.SelectedColourHex);
                module.HrsSelfStudy = Calculations.CalcSelfStudy(module.Credits, module.ClassHrsWeekly, module.SemesterId, _context);
                _context.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", module.SemesterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", module.UserId);
            return View(module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            var colours = Colour();

            var userSemesters = _context.Semesters
        .Where(s => s.UserId == @module.UserId)
        .Select(s => new SelectListItem
        {
            Value = s.SemesterId.ToString(),
            Text = s.SemesterName
        })
        .ToList();

            ViewData["SemesterIdList"] = userSemesters;

            ViewData["SemesterId"] = new SelectList(userSemesters, "Value", "Text", @module.SemesterId);

            ViewData["Colour"] = new SelectList(colours, "Value", "Text", @module.SelectedColourName);
            ViewBag.Colour = ViewData["Colour"];
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterName", @module.SemesterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @module.UserId);
            return View(@module);
        }

        public List<SelectListItem> Colour()
        {
            var colourOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Red", Text = "Red" },
                new SelectListItem { Value = "Green", Text = "Green" },
                new SelectListItem { Value = "Blue", Text = "Blue" },
                new SelectListItem { Value = "Yellow", Text = "Yellow" },
                new SelectListItem { Value = "Pink", Text = "Pink" },
                new SelectListItem { Value = "Purple", Text = "Purple" },
                new SelectListItem { Value = "Orange", Text = "Orange" },
            };
            return colourOptions;
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Code,Name,Credits,ClassHrsWeekly,HrsSelfStudy,HrsSelfStudyLeft,SelectedColourHex,SelectedColourName,UserId,SemesterId")] Module @module)
        {
            if (id != @module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    module.SelectedColourName = module.SelectedColourHex;
                    module.SelectedColourHex = GetHexFromColorName(module.SelectedColourHex);
                    module.HrsSelfStudy = Calculations.CalcSelfStudy(module.Credits, module.ClassHrsWeekly, module.SemesterId, _context);
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.ModuleId))
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

            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", @module.SemesterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @module.UserId);
            return View(@module);
        }

        public string GetHexFromColorName(string? colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
            {
                return string.Empty;
            }

            switch (colorName.ToLowerInvariant())
            {
                case "red":
                    return "#FF0000";
                case "green":
                    return "#00FF00";
                case "blue":
                    return "#0000FF";
                case "yellow":
                    return "#FFFF00";
                case "pink":
                    return "#FFC0CB";
                case "purple":
                    return "#800080";
                case "orange":
                    return "#FFA500";
                default:
                    return ""; // You may want to handle the case when the colorName is not recognized
            }
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .Include(m => m.Semester)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Modules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
            }
            var @module = await _context.Modules.FindAsync(id);
            if (@module != null)
            {
                _context.Modules.Remove(@module);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return (_context.Modules?.Any(e => e.ModuleId == id)).GetValueOrDefault();
        }
    }
}
