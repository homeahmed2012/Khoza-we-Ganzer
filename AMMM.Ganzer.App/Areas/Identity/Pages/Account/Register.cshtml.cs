#nullable disable

using System.ComponentModel.DataAnnotations;
using AMMM.Ganzer.App.Helpers;
using AMMM.Ganzer.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMM.Ganzer.App.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            webHostEnvironment = hostEnvironment;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        //public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Required(ErrorMessage = "اجباري")]
            [EmailAddress]
            [Display(Name = "البريد الاكتروني")]
            public string Email { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [StringLength(100, ErrorMessage = "يجب الا يقل عن 6 احرف والا يزيد عن 100 حرف", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "غير متطابق مع الباسورد")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "الاسم")]
            public string Name { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "تاريخ الميلاد")]
            public string BirthDate { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "المحافظة")]
            public Governate Governate { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "منطقة السكن")]
            public District District { get; set; }

            [Required(ErrorMessage ="اجباري")]
            [Display(Name = "مستوي اللياقة البدنية الحالي (مقارنة بباقي الجروب)")]
            public FitnessLevel FitnessLevel { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "رقم التلفون")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "رقم تلفون الطوارئ")]
            public string EmergencyPhone { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "بتركب عجل بانظام من امتي")]
            public string DateOfRidingBike { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "تاريخ الانضمام لخوذة وجنزير")]
            public string DateOfJoiningTheGroup { get; set; }

            [Required(ErrorMessage = "اجباري")]
            [Display(Name = "صورة شخصية")]
            public IFormFile ProfilePic { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser(); // == new ApplicationUser()
                // check if phone number already exist
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.Name = Input.Name;
                user.BirthDate = DateOnly.Parse(Input.BirthDate);
                user.Governorate = (int)Input.Governate;
                user.District = (int)Input.District;
                user.FitnessLevel = (int)Input.FitnessLevel;
                user.PhoneNumber = Input.PhoneNumber;
                user.EmergencyPhone = Input.EmergencyPhone;
                user.DateOfRidingBike = DateOnly.Parse(Input.DateOfRidingBike);
                user.DateOfJoiningTheGroup = DateOnly.Parse(Input.DateOfJoiningTheGroup);
                user.Points = 0;
                user.ProfilePicture = UploadedFile(Input);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    //var adminRole = new IdentityRole
                    //{
                    //    Name = "Admin"
                    //};

                    //var res1 = await _roleManager.CreateAsync(adminRole);
                    //var res2 = await _userManager.AddToRoleAsync(user, "Admin");


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        private string UploadedFile(InputModel model)
        {
            string uniqueFileName = null;

            if (model.ProfilePic != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePic.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePic.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
