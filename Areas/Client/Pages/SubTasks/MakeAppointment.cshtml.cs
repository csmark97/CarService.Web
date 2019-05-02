using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly CarServiceDbContext _context;

        public MakeAppointmentModel(CarServiceDbContext context)
        {
            _context = context;
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

            [Display(Name = "Válassza ki autóját!")]
            public Car Car { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = User.Claims.Single(c => c.Type == UserHelper.NameIdentifierString).Value;
            ClientUser clientUser = await _context.ClientUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (id == null)
            {
                return NotFound();
            }

            SubTask = await _context.SubTasks.FirstOrDefaultAsync(s => s.Id == id);            

            Opening opening = new Opening();

            if (SubTask.CompanyUser.Opening != null)
            {
                opening = SubTask.CompanyUser.Opening;

                Opening = new Dictionary<DayOfWeek, OpeningDay>
                {
                    { DayOfWeek.Monday, opening.Monday },
                    { DayOfWeek.Tuesday, opening.Tuesday },
                    { DayOfWeek.Wednesday, opening.Wednesday },
                    { DayOfWeek.Thursday, opening.Thursday },
                    { DayOfWeek.Friday, opening.Friday },
                    { DayOfWeek.Saturday, opening.Saturday },
                    { DayOfWeek.Sunday, opening.Sunday }
                };
            }

            IList<Car> cars = await _context.Cars.Where(w => w.ClientUserId == userId).ToListAsync();

            Cars = new List<SelectListItem>();
            foreach (var car in cars)
            {
                SelectListItem carItem = new SelectListItem
                {
                    Value = car.Id.ToString(),
                    Text = car.Model + " " + car.YearOfManufacture
                };
                Cars.Add(carItem);
            }

            if (SubTask == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.Claims.Single(c => c.Type == UserHelper.NameIdentifierString).Value;
            ClientUser clientUser = await _context.ClientUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string[] elementsOfDate = Input.AppointmentDay.Split('-');
            int year, month, day;
            year = int.Parse(elementsOfDate[0]);
            month = int.Parse(elementsOfDate[1]);
            day = int.Parse(elementsOfDate[2]);

            string[] elementsOfTime = Input.AppointmentTime.Split(':');
            int hour, minute;
            hour = int.Parse(elementsOfTime[0]);
            minute = int.Parse(elementsOfTime[1]);

            DateTime appointment = new DateTime(year, month, day, hour, minute, 0);

            IList<WorkerUser> workerUsers = await _context.WorkerUsers.ToArrayAsync();

            workerUsers.Shuffle();

            WorkerUser workerForTheJob = new WorkerUser();

            foreach (var worker in workerUsers)
            {
                bool thisWorkerIsFree = true;
                var workingTimes = worker.Works.Select(t => new
                {
                    t.StartingTime,
                    t.EndTime
                }).ToList();

                foreach (var interval in workingTimes)
                {
                    if (appointment >= interval.StartingTime && appointment < interval.EndTime)
                    {
                        thisWorkerIsFree = false;
                    }
                }

                if (thisWorkerIsFree)
                {
                    workerForTheJob = worker;
                    break;
                }
                else
                {
                    throw new Exception("Erre az idõpontra, nincs szabad munkatársunk!");
                }
            }

            int openServiceId = ServiceExists(2/*Input.Car.Id*/);

            Service service;

            if (openServiceId == 0)
            {
                service = new Service
                {
                    StartingTime = appointment,
                    EndTime = appointment.AddMinutes(SubTask.EstimtedTime),
                    TotalPrice = SubTask.EstimatedPrice,
                    CarId = 2/*Input.Car.Id*/
                };

                _context.Services.Add(service);
                //_context.Attach(service).State = EntityState.Added;

                await _context.SaveChangesAsync();                
            }
            else
            {
                service = await _context.Services.Where(w => w.Id == openServiceId).FirstOrDefaultAsync();
                service.TotalPrice += SubTask.EstimatedPrice;

                if (appointment.AddMinutes(SubTask.EstimtedTime) > service.EndTime)
                {
                    service.EndTime = appointment.AddMinutes(SubTask.EstimtedTime);
                }

                _context.Services.Add(service);

                _context.Attach(service).State = EntityState.Modified;
            }

            Work work = new Work
            {
                StartingTime = appointment,
                EndTime = appointment.AddMinutes(SubTask.EstimtedTime),
                Price = SubTask.EstimatedPrice,
                SubTaskId = SubTask.Id,
                ServiceId = service.Id,
                StateId = 2,
                WorkerUserId = workerForTheJob.Id
            };

            _context.Works.Add(work);
            await _context.SaveChangesAsync();

            return RedirectToPage("./BrowseSubTasks");
        }

        private int ServiceExists(int carId)
        {
            bool carHasService = _context.Services.Any(e => e.Id == carId);

            if (carHasService)
            {
                var services = _context.Services
                    .Where(e => e.Id == carId)
                    .Select(w => new
                    {
                        w.Id,
                        works = w.Works
                    })
                    .ToList();

                List<Work> works = new List<Work>();

                foreach (var service in services)
                {
                    foreach (var work in service.works)
                    {
                        if (!work.State.Equals("finished"))
                        {
                            return service.Id;
                        }
                    }
                }
            }

            return 0;
        }
    }
}