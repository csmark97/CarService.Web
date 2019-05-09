using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Bll.User;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Works;

namespace CarService.Web.Areas.Client.Pages.MyServices.Works
{
    public class IndexModel : PageModel
    {
        private AppUserManager _appUserManager;
        private WorkManager _workManager;

        public IndexModel(CarServiceDbContext context)
        {
            _appUserManager = new AppUserManager(context);
            _workManager = new WorkManager(context);
        }

        public IList<Work> Work { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Work = await WorkManager.GetWorkByServiceAsync(id.Value);

            if (Work == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
