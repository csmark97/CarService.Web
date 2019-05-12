﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Bll.Users;
using CarService.Bll.Works;


namespace CarService.Web.Areas.Client.Pages.MyServices
{
    public class IndexModel : PageModel
    {
        private readonly UserLogic _appUserManager;
        private readonly ServiceLogic _serviceLogic;
        private readonly WorkLogic _workLogic;

        public IndexModel(CarServiceDbContext context)
        {
            _appUserManager = new UserLogic(context);
            _serviceLogic = new ServiceLogic(context);
            _workLogic = new WorkLogic(context);
        }

        public IDictionary<Work, Service> History { get; set; }
        public IList<Service> Services { get; set; }
        public IList<Service> ActiveServices { get; set; }
        public IList<Service> FinishedServices { get; set; }

        public async Task OnGetAsync()
        {
            ClientUser clientUser = await UserLogic.GetUserAsync(User);
            Services = await ServiceLogic.GetMyServices(clientUser.Id);
            History = new Dictionary<Work, Service>();
            ActiveServices = new List<Service>();
            FinishedServices = new List<Service>();

            foreach (var service in Services)
            {
                foreach (var work in service.Works)
                {
                    if (!work.State.Equals("Finished") || !work.State.Equals("FinishedAndPaid"))
                    {
                        ActiveServices.Add(service);
                        break;
                    }
                }
            }

            foreach (var service in Services)
            {
                bool serviceIsActive = false;
                foreach (var activeService in ActiveServices)
                {
                    if (service.Id == activeService.Id)
                    {
                        serviceIsActive = true;
                    }
                }

                if (!serviceIsActive)
                {
                    FinishedServices.Add(service);
                }
            }

            foreach (var service in Services)
            {
                IList<Work> works = await WorkLogic.GetWorksByServiceIdAsync(service.Id);
                foreach (var work in works)
                {
                    History.Add(work, service);
                }
            }

        }
    }
}
