using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarService.Web.Areas.Company
{
    public class IndexModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public IndexModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public string EmailStatusMessage { get; set; }

        [Required]
        [BindProperty]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = Email;

            var subject = "Email Test";

            var message = "This is an automatically generated email by Márk Csordás.";

            await _emailSender.SendEmailAsync(email, subject, message);

            EmailStatusMessage = "Send test email was successful.";

            return Page();
        }
    }
}