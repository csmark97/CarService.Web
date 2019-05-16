using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Bll.Company;
using CarService.Bll.Users;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarService.Web.Areas.Company.Pages.Services
{    
    public class IndexModel : PageModel
    {
        private readonly UserLogic _userLogic;
        private readonly CompanyLogic _companyLogic;

        public IndexModel(CarServiceDbContext context)
        {
            _userLogic = new UserLogic(context);
            _companyLogic = new CompanyLogic(context);
        }

        public IList<Service> Services;

        public async Task OnGetAsync()
        {
            var userId = await UserLogic.GetCompanyUserAsync(User);

            Services = await CompanyLogic.GetServicesByCompanyIdAsync(userId);
        }        
    }
}