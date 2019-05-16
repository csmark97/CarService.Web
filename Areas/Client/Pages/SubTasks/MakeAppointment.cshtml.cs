using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarService.Bll.EmailService;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Users;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService.Web.Areas.Client.Pages.SubTasks
{
    public class MakeAppointmentModel : PageModel
    {
        private readonly UserLogic _appUserManager;
        private readonly AppointmentLogic _appointmentManager;
        private readonly EmailLogic _emailLogic;

        public MakeAppointmentModel(CarServiceDbContext context, IEmailSender emailSender)
        {
            _appUserManager = new UserLogic(context);
            _appointmentManager = new AppointmentLogic(context);
            _emailLogic = new EmailLogic(context, emailSender);
        }

        public Service Service { get; set; }

        [BindProperty]
        public SubTask SubTask { get; set; }

        [BindProperty]
        public IList<SelectListItem> Cars { get; set; }

        public IDictionary<DayOfWeek, OpeningDay> Opening { get; set; }
        public IDictionary<DayOfWeek, Dictionary<DateTime, bool>> FinalOpening;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string AppointmentTime { get; set; }
            public string AppointmentDay { get; set; }

            [Display(Name = "Válassza ki autóját!")]
            public int CarId { get; set; }

            [Display(Name = "Megjegyzés")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClientUser clientUser = await UserLogic.GetUserAsync(User);
            
            SubTask = await AppointmentLogic.GetSubTaskByIdAsync(id.Value);

            if (SubTask == null)
            {
                return NotFound();
            }           

            Opening = AppointmentLogic.GetOpening(SubTask.CompanyUser.Opening);

            FinalOpening = await AppointmentLogic.GetFinalOpeningAsync(SubTask);

            Cars = await AppointmentLogic.GetCarsByIdAsync(clientUser.Id);           

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string[] elementsOfDate = Input.AppointmentDay.Split('-');            

            string[] elementsOfTime = Input.AppointmentTime.Split(':');            

            DateTime appointment = AppointmentLogic.CreateAppointmentDate(elementsOfDate, elementsOfTime);

            Work work = await AppointmentLogic.MakeAppointmentAsync(appointment, Input.CarId, SubTask, Input.Description);

            await _emailLogic.SendStatusChangeEmailAsync(work.WorkerUser, work);

            return RedirectToPage("./BrowseSubTasks");
        }       
    }
}