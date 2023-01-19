using AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.Extensions;
using AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        

        private  UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly SignInManager<AppUser> _signInManager;

       

       

        public AppUser CurrentUser => _userManager.FindByNameAsync(User.Identity!.Name).Result;

        public HomeController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            
            _userManager = userManager;
            _signInManager = signInManager;
           
            
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();

            var userViewModelList = userList.Select(x => new UserViewModel()
            {
                Name = x.UserName,
                Email = x.Email,
                Id = x.Id,
            }).ToList();

            return View(userViewModelList);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;
            IdentityResult result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            }

            return View(roleViewModel);
        }

        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;

            }

            return RedirectToAction("Roles");

        }

        public IActionResult RoleUpdate(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                return View(role.Adapt<RoleViewModel>());


            }

            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu!");
            }
            return View(roleViewModel);


        }

        public  IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;

            AppUser user = _userManager.FindByIdAsync(id).Result;

            ViewBag.userName = user.UserName;

            IQueryable<AppRole> roles = _roleManager.Roles;

            List<string>? userroles = _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();

                r.RoleId = role.Id;
                r.RoleName = role.Name;

                if (userroles!.Contains(role.Name))
                {

                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;

                }
                roleAssignViewModels.Add(r);
            }

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"]!.ToString()).Result; // tempdata da userid'yi tutup burada yakaladık kullancının seçilebilmesi için

            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> ResetUserPassword(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            PasswordResetByAdminViewModel passwordResetByAdminViewModel = new PasswordResetByAdminViewModel();

            passwordResetByAdminViewModel.UserId = user.Id;

            return View(passwordResetByAdminViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(PasswordResetByAdminViewModel passwordResetByAdminViewModel)
        {
            AppUser user = await _userManager.FindByIdAsync(passwordResetByAdminViewModel.UserId);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user); // aşağıdaki parametre token istediği için burada bir token değeri oluşturduk

            await _userManager.ResetPasswordAsync(user, token, passwordResetByAdminViewModel.NewPassword);

            await _userManager.UpdateSecurityStampAsync(user); // kullanıncının eski şifre ile sistemde gezmesini engellemek için securitystamp değerini güncelliyoruz.

            return RedirectToAction("Users");
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

                throw new Exception("Bilinmeyen bir hata meydana geldi!" + " " + ex.Message.ToString());
            }

        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();

        }
    }

}


