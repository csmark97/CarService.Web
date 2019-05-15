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
using CarService.Bll.Users;
using Microsoft.AspNetCore.Identity.UI.Services;
using CarService.Bll.EmailService;

namespace CarService.Web.Areas.Worker.Pages.Works
{
    public class ModifyPriceModel : PageModel
    {
        private readonly WorkSheetLogic _workSheetLogic;
        private readonly UserLogic _userLogic;
        private readonly EmailLogic _emailLogic;

        public ModifyPriceModel(CarServiceDbContext context, IEmailSender emailSender)
        {
            _workSheetLogic = new WorkSheetLogic(context);
            _userLogic = new UserLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
        }

        [BindProperty]
        public Work Work { get; set; }
        [BindProperty]
        public int Price { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Work = await WorkSheetLogic.GetWorkByIdAsync(id);
            Price = Work.Price;

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

            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Work = await WorkSheetLogic.GetWorkByIdAsync(Work.Id);
            Work.Price = Price;
            Work.StateId = 1;

            await WorkSheetLogic.ModifyWorkAsync(Work);

            await _emailLogic.SendStatusChangeEmailAsync(workerUser, Work);

            return RedirectToPage("./WorkSheets");
        }
    }
}
