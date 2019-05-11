using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Bll.Users;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Works;

namespace CarService.Web.Areas.Client.Pages.MyServices.Works
{
    public class IndexModel : PageModel
    {
        private UserLogic _appUserManager;
        private WorkLogic _workManager;

        public IndexModel(CarServiceDbContext context)
        {
            _appUserManager = new UserLogic(context);
            _workManager = new WorkLogic(context);
        }

        public IList<Work> Work { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Work = await WorkLogic.GetWorkByServiceIdAsync(id.Value);

            if (Work == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
