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
using CarService.Web.Helper;

namespace CarService.Web.Areas.Worker.Pages.ManageSubTasks
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
            var userId = User.Claims.Single(c => c.Type == UserHelper.NameIdentifierString).Value;
            //WorkerUser workerUser = await _context.WorkerUsers.FirstOrDefaultAsync(u => u.Id == userId);

            SubTasks = _context.SubTasks.ToList();
                //.Where(u => u.CompanyUserId == workerUser.CompanyUserId)
                //.ToListAsync();
        }
    }
}
