using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Web.Helper;

namespace CarService.Web.Areas.Worker.Pages.ManageSubTasks
{
    public class EditSubTaskModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public EditSubTaskModel(CarService.Dal.CarServiceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubTask SubTask { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubTask = await _context.SubTasks.FirstOrDefaultAsync(m => m.Id == id);

            if (SubTask == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.Claims.Single(c => c.Type == UserHelper.NameIdentifierString).Value;

            string company = await _context.WorkerUsers.Where(u => u.Id == userId)
                .Select(s => s.CompanyUserId)
                .SingleOrDefaultAsync();

            SubTask.CompanyUserId = company;                     

            _context.Attach(SubTask).State = EntityState.Modified;            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubTaskExists(SubTask.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./BrowseSubTasks");
        }

        private bool SubTaskExists(int id)
        {
            return _context.SubTasks.Any(e => e.Id == id);
        }
    }
}
