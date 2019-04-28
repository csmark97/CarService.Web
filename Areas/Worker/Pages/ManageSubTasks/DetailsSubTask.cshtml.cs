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
    public class DetailsSubTaskModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public DetailsSubTaskModel(CarService.Dal.CarServiceDbContext context)
        {
            _context = context;
        }

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
    }
}
