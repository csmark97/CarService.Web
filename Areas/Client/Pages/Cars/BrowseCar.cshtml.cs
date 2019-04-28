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

namespace CarService.Web.Areas.Client.Pages.Cars
{
    public class BrowseCarModel : PageModel
    {        
        private readonly CarServiceDbContext _context;
        private readonly UserManager<User> _userManager;

        public BrowseCarModel(CarServiceDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Car> Car { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            Car = await _context.Cars.Where(u => u.ClientUserId == userId).ToListAsync();
        }
    }
}
