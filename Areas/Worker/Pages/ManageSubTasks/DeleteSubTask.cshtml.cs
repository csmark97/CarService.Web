using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;

namespace CarService.Web.Areas.Worker.Pages.ManageSubTasks
{
    public class DeleteSubTaskModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public DeleteSubTaskModel(CarService.Dal.CarServiceDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubTask = await _context.SubTasks.FindAsync(id);

            if (SubTask != null)
            {
                _context.SubTasks.Remove(SubTask);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./BrowseSubTasks");
        }
    }
}
