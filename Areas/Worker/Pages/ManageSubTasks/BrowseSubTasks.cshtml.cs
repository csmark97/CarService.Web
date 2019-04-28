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

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            //TODO: extension methodba kitenni
            var userId = User.Claims.Single(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            WorkerUser workerUser = await _context.WorkerUsers.FirstOrDefaultAsync(u => u.Id == userId);

            SubTasks = await _context.SubTasks
                .Where(u => u.CompanyId == workerUser.CompanyUserId)
                .ToListAsync();
        }
    }
}
