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
using CarService.Bll.Works;


namespace CarService.Web.Areas.Client.Pages.MyServices
{
    public class IndexModel : PageModel
    {
        private AppUserManager _appUserManager;
        private ServiceManager _appointmentManager;

        public IndexModel(CarServiceDbContext context)
        {
            _appUserManager = new AppUserManager(context);
            _appointmentManager = new ServiceManager(context);
        }

        public IList<Service> Service { get;set; }

        public async Task OnGetAsync()
        {
            ClientUser clientUser = await AppUserManager.GetUserAsync(User);
            Service = await ServiceManager.GetMyServices(clientUser.Id);
        }
    }
}
