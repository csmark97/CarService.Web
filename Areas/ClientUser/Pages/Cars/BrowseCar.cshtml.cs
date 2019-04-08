using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarService.Web.Areas.ClientUser.Pages.Cars
{
    public class BrowseCarModel : PageModel
    {        
        private readonly CarServiceDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BrowseCarModel(CarServiceDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Car> Car { get;set; }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            Car = await _context.Cars.Where(u => u.ApplicationUserId == userId).ToListAsync();
        }
    }
}
