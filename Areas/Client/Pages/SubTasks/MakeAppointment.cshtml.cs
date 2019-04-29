using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarService.Web.Areas.Client.Pages.SubTasks
{
    public class MakeAppointmentModel : PageModel
    {
        private readonly CarServiceDbContext _context;

        public MakeAppointmentModel(CarServiceDbContext context)
        {
            _context = context;
        }

        public Work Work { get; set; }
        public SubTask SubTask { get; set; }
        public IDictionary<DayOfWeek, OpeningDay> Opening { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubTask = await _context.SubTasks.FirstOrDefaultAsync(s => s.Id == id);

            Opening opening = new Opening();

            if (SubTask.CompanyUser.Opening != null)
            {
                opening = SubTask.CompanyUser.Opening;

                Opening = new Dictionary<DayOfWeek, OpeningDay>();

                Opening.Add(DayOfWeek.Monday, opening.Monday);
                Opening.Add(DayOfWeek.Tuesday, opening.Tuesday);
                Opening.Add(DayOfWeek.Wednesday, opening.Wednesday);
                Opening.Add(DayOfWeek.Thursday, opening.Thursday);
                Opening.Add(DayOfWeek.Friday, opening.Friday);
                Opening.Add(DayOfWeek.Saturday, opening.Saturday);
                Opening.Add(DayOfWeek.Sunday, opening.Sunday);
            }            

            if (SubTask == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.Claims.Single(c => c.Type == UserHelper.NameIdentifierString).Value;
            ClientUser clientUser = await _context.ClientUsers.FirstOrDefaultAsync(u => u.Id == userId);

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Cars.Add(Car);

            //_context.Attach(Car).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CarExists(Car.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./BrowseCar");
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}