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
using CarService.Bll.EmailService;
using Microsoft.AspNetCore.Identity.UI.Services;
using CarService.Bll.Company;

namespace CarService.Web.Areas.Company.Pages.Services
{
    public class ReallyPaidModel : PageModel
    {
        private readonly WorkSheetLogic _workSheetLogic;
        private readonly EmailLogic _emailLogic;
        private readonly UserLogic _userLogic;
        private readonly CompanyLogic _companyLogic;

        public ReallyPaidModel(CarServiceDbContext context, IEmailSender emailSender)
        {
            _workSheetLogic = new WorkSheetLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
            _userLogic = new UserLogic(context);
            _companyLogic = new CompanyLogic(context);
        }

        [BindProperty]
        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = await CompanyLogic.GetServiceByIdAsync(id.Value);

            if (Service == null)
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

            Service = await CompanyLogic.GetServiceByIdAsync(Service.Id);

            foreach (var work in Service.Works)
            {
                work.StateId = 6;
                await WorkSheetLogic.ModifyWorkAsync(work);
                
            }

            await _emailLogic.SendStatusChangeEmailAsync(Service);

            return RedirectToPage("./Index");
        }
    }
}
