using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Bll.MakeAppointment;
using CarService.Bll.Users;
using CarService.Bll.WorkCalendar;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarService.Web.Areas.Worker.Pages.Works
{
    public class DetailsWorkSheetModel : PageModel
    {
        private readonly UserLogic _appUserLogic;
        private readonly AppointmentLogic _appointmentLogic;
        private readonly WorkSheetLogic _calendarLogic;

        public DetailsWorkSheetModel(CarServiceDbContext context)
        {
            _appUserLogic = new UserLogic(context);
            _appointmentLogic = new AppointmentLogic(context);
            _calendarLogic = new WorkSheetLogic(context);
        }

        [BindProperty]
        public Work Work { get; set; }


        public async Task OnGetAsync(int? id)
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Work = await WorkSheetLogic.GetWorkByIdAsync(id);
        }
    }
}