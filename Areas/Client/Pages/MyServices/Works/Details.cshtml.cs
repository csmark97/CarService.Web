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
    public class DetailsModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public DetailsModel(CarService.Dal.CarServiceDbContext context)
        {
            _context = context;
        }

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
    }
}
