using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Bll.WorkCalendar;

namespace CarService.Web.Areas.Worker.Pages.Works
{
    public class ReallyStartModel : PageModel
    {
        private readonly WorkSheetLogic _workSheetLogic;

        public ReallyStartModel(CarService.Dal.CarServiceDbContext context)
        {
            _workSheetLogic = new WorkSheetLogic(context);
        }

        [BindProperty]
        public Work Work { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Work = await WorkSheetLogic.GetWorkByIdAsync(id);

            if (Work == null)
            {
                return NotFound();
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Work = await WorkSheetLogic.GetWorkByIdAsync(Work.Id);

            Work.StateId = 3;

            await WorkSheetLogic.ModifyWorkAsync(Work);            

            return RedirectToPage("./WorkSheets");
        }
    }
}
