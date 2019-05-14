using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using CarService.Bll.Helper;
using Microsoft.AspNetCore.Http.Internal;

namespace CarService.Web.Areas.Client.Pages.Cars
{
    public class AddCarModel : PageModel
    {
        private const string UploadFiles = "UploadFiles";
        private readonly CarServiceDbContext _context;
        private readonly UserManager<User> _userManager;
        public AddCarModel(CarServiceDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Car Car { get; set; }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public string userId { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {           
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Car.ClientUserId = user.Id;

            await FileHelpers.UploadAsync("C:/Users/csmar/source/repos/CarService/CarService.Web/wwwroot/pictures/", FileUpload);            

            Car.Picture = FileUpload.FileName;
            Car.PictureSize = FileUpload.Length;
            Car.PictureUploadDT = DateTime.UtcNow;

            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            return RedirectToPage("./BrowseCar");
        }        
    }
}