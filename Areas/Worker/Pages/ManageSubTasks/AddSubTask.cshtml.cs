using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using CarService.Web.Helper;

namespace CarService.Web.Areas.Worker.Pages.ManageSubTasks
{
    public class AddSubTaskModel : PageModel
    {
        private readonly CarService.Dal.CarServiceDbContext _context;

        public AddSubTaskModel(CarService.Dal.CarServiceDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SubTask SubTask { get; set; }

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

            SubTask.CompanyId = company;

            _context.SubTasks.Add(SubTask);
            await _context.SaveChangesAsync();

            return RedirectToPage("./BrowseSubTasks");
        }
    }
}