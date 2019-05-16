using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using CarService.Bll.Works;

namespace CarService.Web.Areas.Client.Pages.MyServices
{
    public class DetailsModel : PageModel
    {
        private readonly WorkLogic _workLogic;

        public DetailsModel(CarServiceDbContext context)
        {
            _workLogic = new WorkLogic(context);
        }

        public Work Work { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Work = await WorkLogic.GetWorkByIdAsync(id.Value);            

            if (Work == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
