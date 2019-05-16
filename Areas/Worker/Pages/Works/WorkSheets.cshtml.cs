using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarService.Bll.EmailService;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Users;
using CarService.Bll.WorkCalendar;
using CarService.Bll.Works;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarService.Web.Areas.Worker.Pages.Works
{
    public class WorkSheetModel : PageModel
    {
        private readonly UserLogic _appUserLogic;
        private readonly AppointmentLogic _appointmentLogic;
        private readonly WorkSheetLogic _calendarLogic;
        private readonly WorkLogic _workLogic;
        private EmailLogic _emailLogic;

        public WorkSheetModel(CarServiceDbContext context, IEmailSender emailSender)
        {
            _appUserLogic = new UserLogic(context);
            _appointmentLogic = new AppointmentLogic(context);
            _calendarLogic = new WorkSheetLogic(context);
            _workLogic = new WorkLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
        }

        [BindProperty]
        public IList<Work> Works { get; set; }

        [BindProperty]
        public Work NextWork { get; set; }

        [BindProperty]
        [Display(Name = "Üzenet szövege")]
        public Message Message { get; set; }

        public async Task OnGetAsync()
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Works = await WorkSheetLogic.GetRemainingWorksByWorkerIdAsync(workerUser.Id);

            NextWork = WorkSheetLogic.GetNextWork(Works);            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Works = await WorkSheetLogic.GetRemainingWorksByWorkerIdAsync(workerUser.Id);

            NextWork = WorkSheetLogic.GetNextWork(Works);

            Message.SenderId = workerUser.Id;
            Message.WorkId = NextWork.Id;
            Message.Time = DateTime.Now;

            await WorkLogic.SaveMessageAsync(Message);

            string emailMessage = $"Tisztelt {NextWork.Service.Car.ClientUser.Name}!<br /><br />" +
                $"Tájékoztatjuk, hogy {NextWork.SubTask.Name} ({NextWork.Service.Car.Brand} {NextWork.Service.Car.Model}) " +
                $"feladathoz, új üzenet érkezett rendszerünkben {Message.Time} időpontban!<br /><br />" +
                $"Üzenet szövege a következő<br />\"{Message.Text}\"<br /><br />" +
                $"Válaszolni a rendszerünkben tud, a feladathoz tartozó üzeneteknél!" +
                $"Kérjük, hogy erre az e-mailre ne válaszoljon!<br /><br /><br />" +
                $"Üdvözlettel:<br />{NextWork.SubTask.CompanyUser.Name}";

            await _emailLogic.SendNotificationAsync(NextWork, emailMessage);

            return Page();
        }
    }
}