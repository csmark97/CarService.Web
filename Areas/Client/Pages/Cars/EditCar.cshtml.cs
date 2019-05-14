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
using Microsoft.AspNetCore.Identity;
using CarService.Web.Helper;
using CarService.Bll.Helper;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace CarService.Web.Areas.Client.Pages.Cars
{
    public class EditCarModel : PageModel
    {
        private readonly CarServiceDbContext _context;

        public EditCarModel(CarServiceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car Car { get; set; }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car = await _context.Cars
                .Include(c => c.ClientUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Car == null)
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
            Car.ClientUserId = userId;

            await FileHelpers.UploadAsync("C:/Users/csmar/source/repos/CarService/CarService.Web/wwwroot/pictures/", FileUpload);

            Car.Picture = FileUpload.FileName;
            Car.PictureSize = FileUpload.Length;
            Car.PictureUploadDT = DateTime.UtcNow;

            _context.Cars.Add(Car);

            _context.Attach(Car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(Car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./BrowseCar");
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
