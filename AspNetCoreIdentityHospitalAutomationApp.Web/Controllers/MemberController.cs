using AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalAutomationApp.Web.ViewModels;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailService;

        private readonly HospitalAppDbContext _hospitalAppDbContext;

       

        public MemberController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, HospitalAppDbContext hospitalAppDbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _hospitalAppDbContext = hospitalAppDbContext;
           
        }

        public IActionResult Index()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            MemberViewModel memberViewModel = user.Adapt<MemberViewModel>();

            return View(memberViewModel);
        }

        public IActionResult MemberEdit()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            MemberViewModel memberViewModel = user.Adapt<MemberViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(memberViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MemberEdit(MemberViewModel memberViewModel, IFormFile userPicture)
        {


            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirm");

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (!ModelState.IsValid)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                string phone = _userManager.GetPhoneNumberAsync(user).Result!;

                if (phone != memberViewModel.PhoneNumber)
                {
                    if (_userManager.Users.Any(u => u.PhoneNumber == memberViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");

                        return View(memberViewModel);
                    }
                }

                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName); // kullanıcı resimlerini rastgele isimler ve dosya uzantısı ile kaydedecek

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName); // resimleri veri tabanında kaydedeceğimiz yolu belirledik

                    using var stream = new FileStream(path, FileMode.Create);
                    await userPicture.CopyToAsync(stream);

                    user.Picture = "/UserPicture/" + fileName;


                }

                user.UserName = memberViewModel.UserName;
                user.Email = memberViewModel.Email;
                user.PhoneNumber = memberViewModel.PhoneNumber;
                user.City = memberViewModel.City;
                user.Gender = (int)memberViewModel.Gender;
                user.BirthDay = memberViewModel.BirthDay;



                IdentityResult result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user); // security stamp ve cookie bilgilerini kullanıcı adı dğeiştirildek sonra güncellemek için 
                    await _signInManager.SignOutAsync();

                    await _signInManager.SignInAsync(user, true);

                    ViewBag.success = "true";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                string phone = _userManager.GetPhoneNumberAsync(user).Result!;

                if (phone != memberViewModel.PhoneNumber)
                {
                    if (_userManager.Users.Any(u => u.PhoneNumber == memberViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");

                        return View(memberViewModel);
                    }
                }

                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName); // kullanıcı resimlerini rastgele isimler ve dosya uzantısı ile kaydedecek

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName); // resimleri veri tabanında kaydedeceğimiz yolu belirledik

                    using var stream = new FileStream(path, FileMode.Create);
                    await userPicture.CopyToAsync(stream);

                    user.Picture = "/UserPicture/" + fileName;


                }

                user.UserName = memberViewModel.UserName;
                user.Email = memberViewModel.Email;
                user.PhoneNumber = memberViewModel.PhoneNumber;
                user.City = memberViewModel.City;
                user.Gender = (int)memberViewModel.Gender;
                user.BirthDay = memberViewModel.BirthDay;



                IdentityResult result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user); // security stamp ve cookie bilgilerini kullanıcı adı dğeiştirildek sonra güncellemek için 
                    await _signInManager.SignOutAsync();

                    await _signInManager.SignInAsync(user, true);

                    ViewBag.success = "true";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(memberViewModel);
        }

        public IActionResult MemberIndex()
        {
            return View();
        }

       

        public async Task<IActionResult> DoctorList()
        {
            var doctorList = await _userManager.Users.ToListAsync();

            var doctorViewModelList = doctorList.Select(x => new DoctorViewModel()
            {
                UserName = x.UserName,
                Email = x.Email,
                Branch = (Branches)x.Branch ,
            }).ToList();

            return View(doctorViewModelList);
        }

        public async Task <IActionResult> Doctors()
        {
            var roleName = "Doctor";
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

           
            return View(usersInRole);
        }

        public async Task<IActionResult> ListMedicine()
        {
           
            var medicines = await _hospitalAppDbContext.Meds.ToListAsync();

            var medicineViewModelList = medicines.Select(x => new MedicineViewModel()
            {
                MedName = x.MedName,
                Description = x.Description,
                Price = x.Price,
            }).ToList();

            
            return View(medicines);
        }

        public IActionResult PatientHistory(string patientId)
        {
            
            List<PatientFileViewModel> files;
            using (var context = _hospitalAppDbContext)
            {
                //files = context.Files
                //    .Where(f => f.PatientId == patientId)
                //    .ToList();
                files = context.Files.ToList();
            }

            
            return View(files);
        }

        public ActionResult ListFiles()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
            var files = Directory.GetFiles(directory).Select(Path.GetFileName);
            return View(files);
        }

        public FileResult DownloadFile(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        public IActionResult DownloadRecipeFile(int fileId)
        {
            
            var file = _hospitalAppDbContext.Files.Find(fileId);
            if (file == null)
            {
                return NotFound();
            }

            
            return File(file.Data, "application/octet-stream", file.FileName);
        }

        public async Task<IActionResult> CreateAppointment() 
        {
            var roleName = "Doctor";
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            ViewBag.Doctors = new SelectList(usersInRole);
            ViewBag.Branches = Enum.GetValues(typeof(Branches))
            .Cast<int>()
            .Select(value => new SelectListItem
            {
                Text = Enum.GetName(typeof(Branches), value)?.Replace("_", " "),
                Value = value.ToString()
            });

            return View();
        }


        [HttpPost]
        public IActionResult CreateAppointment(MemberEventViewModel memberEventViewModel)
        {
            
            ViewBag.Branch = Enum.GetValues(typeof(Branches)).Cast<int>().Select(value => new SelectListItem { Text = Enum.GetName(typeof(Branches), value).Replace("_", " "), Value = value.ToString() });
            if (memberEventViewModel.Branch.ToString() != "Belirtilmemiş" && !_hospitalAppDbContext.MemberEvents.Where(a => a.Branch == memberEventViewModel.Branch && a.SelectedDate == memberEventViewModel.SelectedDate).Any())
            {
                _hospitalAppDbContext.Add(memberEventViewModel);
                _hospitalAppDbContext.SaveChanges();
                TempData["success"] = "Randevu başarı ile oluşturuldu!";
            }
            else
            {
                TempData["error"] = "Bu bölümde aynı saat için zaten randevu var, lütfen farklı bir doktor veya saat seçin!";
            }

            return View();


        }

        [HttpGet]
        public IActionResult GetDoctorsByBranch(int branchId)
        {
            var doctors = _hospitalAppDbContext.Users
                .Where(d => d.Branch == branchId)
                .Select(d => new { d.Id, d.UserName });

            return Json(doctors);
        }

        public IActionResult ListAppointments()
        {
            var appointments = _hospitalAppDbContext.MemberEvents.ToList();
            return View(appointments);
        }


        public IActionResult DeleteAppointment(int id)
        {

            var appointment = _hospitalAppDbContext.MemberEvents.Find(id);
            if (appointment == null)
            {

                return NotFound();
            }


            var model = new MemberEventViewModel
            {
                Id = appointment.Id,
                UserName = appointment.UserName,
                SelectedDate= appointment.SelectedDate,
                Branch = appointment.Branch
                
                
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteAppointment(int Id , MemberEventViewModel model)
        {
            var appointment = _hospitalAppDbContext.MemberEvents.Find(Id);
            if (appointment == null)
            {

                return NotFound();
            }
            _hospitalAppDbContext.MemberEvents.Remove(appointment);
            _hospitalAppDbContext.SaveChanges();
            return RedirectToAction("ListAppointments");
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();

        }
    }
}



   


           
