using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityHospitalAutomationApp.Web.Extensions;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.Services;
using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailService;

        private readonly HospitalAppDbContext _hospitalAppDbContext;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, HospitalAppDbContext hospitalAppDbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _hospitalAppDbContext = hospitalAppDbContext;
        }

        public IActionResult Index()
        {
            //if (User.Identity!.IsAuthenticated)
            //{
            //    return RedirectToAction("Index","Home");
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null!)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Action("Index", "Home")!;

                var hasUser = await _userManager.FindByEmailAsync(model.Email!);

                if (hasUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Email veya şifre yanlış!");
                    return View();
                }

                var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);

                if (signInResult.Succeeded)
                {
                    return Redirect("/Member/Index");
                }

                if (signInResult.IsLockedOut)
                {
                    ModelState.AddModelErrorList(new List<string>() { "Hesabınız 3 dakika boyunca kilitli kalacaktır!" });
                    return View();
                }

                ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış!", $"(Başarısız giriş sayısı:{await _userManager.GetAccessFailedCountAsync(hasUser)})" });


                return View();
            }
            catch (Exception ex)
            {

                throw new Exception("Bilinmeyen bir hata meydana geldi!" +" "+ ex.Message.ToString());
            }

        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var user = new AppUser { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email };
                var identityResult = await _userManager.CreateAsync(user, request.PasswordConfirm!);

                if (identityResult.Succeeded)
                {
                    // Assign the user the "Patient" role
                    await _userManager.AddToRoleAsync(user, "Patient");

                    TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarı ile gerçekleştirilmiştir.";

                    return RedirectToAction(nameof(HomeController.SignUp));
                }
                else
                {
                    ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                }

                return View();
            }
            catch (Exception)
            {
                throw new Exception("Bilinmeyen bir hata meydana geldi!");
            }
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            try
            {
                var hasUser = await _userManager.FindByEmailAsync(request.Email!);

                if (hasUser == null)
                {
                    ModelState.AddModelError(String.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır!");
                    return View();
                }

                string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

                var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, token = passwordResetToken }, HttpContext.Request.Scheme);
                //link örnek
                

                await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

                TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";

                return RedirectToAction(nameof(ForgetPassword));
            }
            catch (Exception)
            {

                throw;
            }


        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            try
            {
                var userId = TempData["userId"];
                var token = TempData["token"];
                TempData.Keep("userId");
                TempData.Keep("token");

                //var userId = TempData.Peek("userId") as string;
                //var token = TempData.Peek("token") as string;


                if (userId == null || token == null) { return View("Index"); }

                var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

                if (hasUser == null)
                {
                    ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamıştır!");
                    return View();
                }

                var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password!);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Şifreniz başarı ile yenilenmiştir.";
                }
                else
                {
                    ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
                }

                return View();

                /*CookieTempDataProviderOptions*/


                //if (ModelState.IsValid)
                //{
                //    
                //    string? userId = TempData["userId"] as string;
                //    string? token = TempData["token"] as string;

                //    if (userId == null || token == null)
                //    {
                //        return View("Error");
                //    }

                //    var user = await _userManager.FindByIdAsync(userId);
                //    if (user == null)
                //    {
                //        return View("Error");
                //    }

                //    var result = await _userManager.ResetPasswordAsync(user, token, request.Password!);
                //    if (result.Succeeded)
                //    {
                //        TempData["SuccessMessage"] = "Şifreniz başarı ile yenilenmiştir.";
                //    }

                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
                //    }
                //}

                //return View(request);




            }
            catch (NullReferenceException ex)
            {
                throw new Exception("Bir hata meydana geldi.Lütfen tekrar şifre sıfırlama talebinde bulunun!" + " " + ex.Message);

            }
            catch (Exception)
            {
                throw new Exception("Bilinmeyen bir hata meydana geldi!");
            }
        }

        public IActionResult About() 
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogDetails()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }

        public IActionResult Doctors()
        {
            return View();
        }

        public IActionResult SubmitForm( )
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(FormViewModel formViewModel)
        {
            

            
            if (string.IsNullOrEmpty(formViewModel.FullName) ||
                string.IsNullOrEmpty(formViewModel.EmailAddress) ||
                string.IsNullOrEmpty(formViewModel.Number) ||
                string.IsNullOrEmpty(formViewModel.Message))
            {
               
                TempData["alert"] = "Lütfen tüm alanları doldurunuz!";
                return RedirectToAction("Index");
            }

            var context = _hospitalAppDbContext;

            
            if (context.Forms.Any(f => f.EmailAddress == formViewModel.EmailAddress) ||
                context.Forms.Any(f => f.Number == formViewModel.Number))
            {
                
                TempData["alert"] = "Bu e-posta adresi veya telefon numarası zaten kullanılmış.";
                return RedirectToAction("Index");
            }

            
            var formModel = new FormViewModel
            {
                FullName = formViewModel.FullName,
                EmailAddress = formViewModel.EmailAddress,
                Date = formViewModel.Date,
                SelectedBranch = formViewModel.SelectedBranch,
                Number = formViewModel.Number,
                Message = formViewModel.Message
            };

            
            context.Forms.Add(formModel);

            
            await context.SaveChangesAsync();

            TempData["success"] = "Form başarılı bir şekilde gönderildi.";
            return RedirectToAction("Index");
        }

        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl })!;

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);

            return new ChallengeResult("Facebook", properties);
        }

        public IActionResult GoogleLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl })!;

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", RedirectUrl);

            return new ChallengeResult("Google", properties);
        }

        public IActionResult MicrosoftLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl })!;

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", RedirectUrl);

            return new ChallengeResult("Microsoft", properties);
        }

        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);

                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    AppUser user = new AppUser();

                    user.Email = info.Principal.FindFirst(ClaimTypes.Email)!.Value;

                    string ExtarnalUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                    if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
                    {
                        string userName = info.Principal.FindFirst(ClaimTypes.Name)!.Value;

                        userName = userName.Replace(' ', '_').ToLower() + ExtarnalUserId.Substring(0, 5).ToString();

                        user.UserName = userName;
                    }
                    else
                    {
                        user.UserName = info.Principal.FindFirst(ClaimTypes.Email)!.Value;
                    }

                    AppUser user2 = await _userManager.FindByEmailAsync(user.Email);

                    if (user2 == null)
                    {
                        IdentityResult createResult = await _userManager.CreateAsync(user);

                        if (createResult.Succeeded)
                        {
                            IdentityResult loginResault = await _userManager.AddLoginAsync(user, info);

                            if (loginResault.Succeeded)
                            {
                                //await signInManager.SignInAsync(user, true);
                                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);


                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                AddModelError(loginResault);
                            }
                        }
                        else
                        {
                            AddModelError(createResult);
                        }
                    }
                    else
                    {
                        IdentityResult loginResault = await _userManager.AddLoginAsync(user2, info);

                        await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);

                        return Redirect(ReturnUrl);
                    }

                }
            }
            List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList();

            return View("Error", errors);
        }

        public ActionResult UserDataDeletion()
        {
            return View();
        }

        public ActionResult Policy()
        {
            return View();
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}




