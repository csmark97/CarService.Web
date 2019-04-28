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

namespace CarService.Web.Areas.Client.Pages.SubTasks
{
    public class BrowseSubTaskModel : PageModel
    {        
        private readonly CarServiceDbContext _context;
        private readonly UserManager<User> _userManager;

        public BrowseSubTaskModel(CarServiceDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<SubTask> SubTasks { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            SubTasks = await _context.SubTasks.ToListAsync();
        }
    }
}
