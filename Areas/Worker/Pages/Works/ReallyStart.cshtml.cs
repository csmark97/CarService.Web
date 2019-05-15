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
using Microsoft.AspNetCore.Identity.UI.Services;
using CarService.Bll.EmailService;
using CarService.Bll.Users;

namespace CarService.Web.Areas.Worker.Pages.Works
{
    public class ReallyStartModel : PageModel
    {
        private readonly WorkSheetLogic _workSheetLogic;
        private readonly IEmailSender _emailSender;
        private readonly EmailLogic _emailLogic;
        private readonly UserLogic _userLogic;

        public ReallyStartModel(CarService.Dal.CarServiceDbContext context, IEmailSender emailSender)
        {
            _workSheetLogic = new WorkSheetLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
            _userLogic = new UserLogic(context);
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

            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Work = await WorkSheetLogic.GetWorkByIdAsync(Work.Id);

            Work.StateId = 3;

            await WorkSheetLogic.ModifyWorkAsync(Work);

            await _emailLogic.SendStatusChangeEmailAsync(workerUser, Work);

            return RedirectToPage("./WorkSheets");
        }
    }
}
