using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;

namespace CarService.Web.Areas.Client.Pages.MyServices.Works
{
    public class DeleteModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public DeleteModel(CarService.Dal.CarServiceDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Work = await _context.Works.FindAsync(id);

            if (Work != null)
            {
                _context.Works.Remove(Work);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Client/MyServices");
        }
    }
}
