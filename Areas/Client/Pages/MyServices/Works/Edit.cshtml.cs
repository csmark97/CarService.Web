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

namespace CarService.Web.Areas.Client.Pages.MyServices.Works
{
    public class EditModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public EditModel(CarService.Dal.CarServiceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Work Work { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Work = await _context.Works
                .Include(w => w.Service)
                .Include(w => w.State)
                .Include(w => w.SubTask)
                .Include(w => w.WorkerUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Work == null)
            {
                return NotFound();
            }
           ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id");
           ViewData["StateId"] = new SelectList(_context.States, "Id", "Id");
           ViewData["SubTaskId"] = new SelectList(_context.SubTasks, "Id", "Id");
           ViewData["WorkerUserId"] = new SelectList(_context.WorkerUsers, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Work).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkExists(Work.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool WorkExists(int id)
        {
            return _context.Works.Any(e => e.Id == id);
        }
    }
}
