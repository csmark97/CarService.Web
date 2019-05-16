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
using CarService.Bll.Works;
using System.ComponentModel.DataAnnotations;
using CarService.Bll.Users;

namespace CarService.Web.Areas.Client.Pages.MyServices
{
    public class MessagesModel : PageModel
    {
        private readonly WorkLogic _workLogic;
        private readonly UserLogic _userLogic;

        public MessagesModel(CarServiceDbContext context)
        {
            _workLogic = new WorkLogic(context);
            _userLogic = new UserLogic(context);
        }

        [BindProperty]
        public Work Work { get; set; }

        [BindProperty]
        public ClientUser ClientUser { get; set; }

        [BindProperty]
        [Display(Name = "Üzenet szövege")]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Work = await WorkLogic.GetWorkByIdAsync(id.Value);
            ClientUser = await UserLogic.GetUserAsync(User);

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

            ClientUser = await UserLogic.GetUserAsync(User);

            Message.SenderId = ClientUser.Id;
            Message.WorkId = Work.Id;
            Message.Time = DateTime.Now;

            await WorkLogic.SaveMessageAsync(Message);

            return RedirectToPage("Index");
        }        
    }
}
