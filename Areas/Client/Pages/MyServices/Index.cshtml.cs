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
using CarService.Bll.Works;


namespace CarService.Web.Areas.Client.Pages.MyServices
{
    public class IndexModel : PageModel
    {
        private UserLogic _appUserManager;
        private ServiceLogic _appointmentManager;

        public IndexModel(CarServiceDbContext context)
        {
            _appUserManager = new UserLogic(context);
            _appointmentManager = new ServiceLogic(context);
        }

        public IList<Service> Service { get;set; }

        public async Task OnGetAsync()
        {
            ClientUser clientUser = await UserLogic.GetUserAsync(User);
            Service = await ServiceLogic.GetMyServices(clientUser.Id);
        }
    }
}
