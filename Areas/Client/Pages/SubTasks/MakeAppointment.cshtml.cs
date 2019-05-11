using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Users;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService.Web.Areas.Client.Pages.SubTasks
{
    public class MakeAppointmentModel : PageModel
    {
        private UserLogic _appUserManager;
        private readonly AppointmentLogic _appointmentManager;

        public MakeAppointmentModel(CarServiceDbContext context)
        {
            _appUserManager = new UserLogic(context);
            _appointmentManager = new AppointmentLogic(context);
        }

        public Service Service { get; set; }

        [BindProperty]
        public SubTask SubTask { get; set; }

        [BindProperty]
        public IList<SelectListItem> Cars { get; set; }

        public IDictionary<DayOfWeek, OpeningDay> Opening { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string AppointmentTime { get; set; }
            public string AppointmentDay { get; set; }

            [Display(Name = "V�lassza ki aut�j�t!")]
            public int CarId { get; set; }
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

            await AppointmentLogic.MakeAppointmentAsync(appointment, Input.CarId, SubTask);           

            return RedirectToPage("./BrowseSubTasks");
        }        
    }
}