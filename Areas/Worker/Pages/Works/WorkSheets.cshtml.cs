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
    public class WorkCalendarModel : PageModel
    {
        private readonly UserLogic _appUserLogic;
        private readonly AppointmentLogic _appointmentLogic;
        private readonly WorkSheetLogic _calendarLogic;

        public WorkCalendarModel(CarServiceDbContext context)
        {
            _appUserLogic = new UserLogic(context);
            _appointmentLogic = new AppointmentLogic(context);
            _calendarLogic = new WorkSheetLogic(context);
        }

        public IDictionary<DayOfWeek, OpeningDay> Opening { get; set; }

        [BindProperty]
        public IList<Work> Works { get; set; }

        [BindProperty]
        public Work NextWork { get; set; }


        public async Task OnGetAsync()
        {
            WorkerUser workerUser = await UserLogic.GetWorkerUserAsync(User);

            Works = await WorkSheetLogic.GetRemainingWorksByWorkerIdAsync(workerUser.Id);

            NextWork = WorkSheetLogic.GetNextWork(Works);
        }
    }
}