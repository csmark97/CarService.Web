using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CarService.Dal.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarService.Dal;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CarService.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly CarServiceDbContext _context;

        public List<SelectListItem> Options { get; set; }

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            CarServiceDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;

            SelectListItem client = new SelectListItem
            {
                Value = UserType.CLIENT.ToString(),
                Text = EnumHelper<UserType>.GetDisplayValue(UserType.CLIENT)
            };
            SelectListItem worker = new SelectListItem
            {
                Value = UserType.WORKER.ToString(),
                Text = EnumHelper<UserType>.GetDisplayValue(UserType.WORKER)
            };
            SelectListItem company = new SelectListItem
            {
                Value = UserType.COMPANY.ToString(),
                Text = EnumHelper<UserType>.GetDisplayValue(UserType.COMPANY)
            };

            Options = new List<SelectListItem>
            {
                client,
                worker,
                company
            };
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Jelszó")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Jelszó újra")]
            [Compare("Password", ErrorMessage = "A jelszavak nem egyeznek!")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Név")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Felhasználói fiók típusa")]
            public UserType UserType { get; set; }

            [Display(Name = "Telefonszám")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Irányítószám")]
            public string Zip { get; set; }

            [Display(Name = "Város")]
            public string City { get; set; }

            [Display(Name = "Út")]
            public string Street { get; set; }

            [Display(Name = "Házszám")]
            public string HouseNumber { get; set; }

            [Display(Name = "Privát kulcs")]
            public string PrivateKey { get; set; }

            [Display(Name = "Hétfő")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartMonday { get; set; }

            [Display(Name = "Kedd")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartTuesday { get; set; }

            [Display(Name = "Szerda")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartWednesday { get; set; }

            [Display(Name = "Csütörtök")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartThursday { get; set; }

            [Display(Name = "Péntek")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartFriday { get; set; }

            [Display(Name = "Szombat")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartSaturday { get; set; }            

            [Display(Name = "Vasárnap")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartSunday { get; set; }
            
            [Display(Name = "Hétfő")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndMonday { get; set; }

            [Display(Name = "Kedd")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndTuesday { get; set; }

            [Display(Name = "Szerda")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndWednesday { get; set; }

            [Display(Name = "Csütörtök")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndThursday { get; set; }

            [Display(Name = "Péntek")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndFriday { get; set; }

            [Display(Name = "Szombat")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndSaturday { get; set; }

            [Display(Name = "Vasárnap")]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]        
            public DateTime EndSunday { get; set; }

            [Display(Name = "Zárva")]
            public bool SaturdayOpen { get; set; }

            [Display(Name = "Zárva")]
            public bool SundayOpen { get; set; }            
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                User user;
                string role;

                if (Input.UserType == UserType.COMPANY)
                {
                    Opening opening = new Opening
                    {
                        StartMonday = Input.StartMonday,
                        StartTuesday = Input.StartTuesday,
                        StartWednesday = Input.StartWednesday,
                        StartThursday = Input.StartThursday,
                        StartFriday = Input.StartFriday,
                        StartSaturday = Input.StartSaturday,
                        StartSunday = Input.StartSunday,

                        EndMonday = Input.EndMonday,
                        EndTuesday = Input.EndTuesday,
                        EndWednesday = Input.EndWednesday,
                        EndThursday = Input.EndThursday,
                        EndFriday = Input.EndFriday,
                        EndSaturday = Input.EndSaturday,
                        EndSunday = Input.EndSunday,

                        SaturdayOpen = Input.SaturdayOpen,
                        SundayOpen = Input.SundayOpen
                    };

                    await _context.Openings.AddAsync(opening);

                    await _context.SaveChangesAsync();

                    int openingId = opening.Id;

                    user = new CompanyUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Name = Input.Name,
                        PhoneNumber = Input.PhoneNumber,
                        Address = new Address
                        {
                            Zip = int.Parse(Input.Zip),
                            City = Input.City,
                            Street = Input.Street,
                            HouseNumber = int.Parse(Input.HouseNumber)
                        },
                        PrivateKey = Input.PrivateKey,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        OpeningId = openingId
                    };
                    role = Roles.Companies;
                }
                else if (Input.UserType == UserType.WORKER)
                {
                    var company = await _context.CompanyUsers
                        .Where(c => c.PrivateKey == Input.PrivateKey).ToListAsync();

                    CompanyUser attachedCompany = company.FirstOrDefault();

                    user = new WorkerUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Name = Input.Name,
                        PhoneNumber = Input.PhoneNumber,
                        CompanyUserId = attachedCompany.Id,
                        Address = new Address {},
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    role = Roles.Workers;
                }
                else if (Input.UserType == UserType.CLIENT)
                {
                    var company = await _context.CompanyUsers
                        .Where(c => c.PrivateKey == Input.PrivateKey).ToListAsync();

                    user = new ClientUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Name = Input.Name,
                        PhoneNumber = Input.PhoneNumber,
                        Address = new Address
                        {
                            Zip = int.Parse(Input.Zip),
                            City = Input.City,
                            Street = Input.Street,
                            HouseNumber = int.Parse(Input.HouseNumber)
                        },
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    role = Roles.Clients;
                }
                else //Never happen
                {
                    user = new CompanyUser();
                    role = "";
                }

                var userResult = await _userManager.CreateAsync(user, Input.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, role);

                if (userResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
