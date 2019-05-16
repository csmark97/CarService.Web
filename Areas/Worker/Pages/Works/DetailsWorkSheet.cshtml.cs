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
    public class DetailsWorkSheetModel : PageModel
    {
        private readonly UserLogic _appUserLogic;
        private readonly AppointmentLogic _appointmentLogic;
        private readonly WorkSheetLogic _calendarLogic;
        private readonly WorkLogic _workLogic;
        private readonly EmailLogic _emailLogic;

        public DetailsWorkSheetModel(CarServiceDbContext context, IEmailSender emailSender)
        {
            _appUserLogic = new UserLogic(context);
            _appointmentLogic = new AppointmentLogic(context);
            _calendarLogic = new WorkSheetLogic(context);
            _workLogic = new WorkLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
        }

        [BindProperty]
        public Work Work { get; set; }

        [BindProperty]
        [Display(Name = "Üzenet szövege")]
        public Message Message { get; set; }


        public async Task OnGetAsync(int? id)
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Work = await WorkSheetLogic.GetWorkByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Message.SenderId = workerUser.Id;
            Message.WorkId = Work.Id;
            Message.Time = DateTime.Now;

            await WorkLogic.SaveMessageAsync(Message);

            string emailMessage = $"Tisztelt {Work.Service.Car.ClientUser.Name}!<br /><br />" +
                 $"Tájékoztatjuk, hogy {Work.SubTask.Name} ({Work.Service.Car.Brand} {Work.Service.Car.Model}) " +
                 $"feladathoz, új üzenet érkezett rendszerünkben {Message.Time} időpontban!<br /><br />" +
                 $"Üzenet szövege a következő<br />\"{Message.Text}\"<br /><br />" +
                 $"Válaszolni a rendszerünkben tud, a feladathoz tartozó üzeneteknél!" +
                 $"Kérjük, hogy erre az e-mailre ne válaszoljon!<br /><br /><br />" +
                 $"Üdvözlettel:<br />{Work.SubTask.CompanyUser.Name}";

            await _emailLogic.SendNotificationAsync(Work, emailMessage);

            return RedirectToPage("Index");
        }
    }
}