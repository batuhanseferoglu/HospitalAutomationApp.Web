using AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.Services;
using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



namespace AspNetCoreIdentityHospitalAutomationApp.Web.Controllers
{
    [Authorize(Roles = "Doctor")]

    public class DoctorController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailService;

        private readonly HospitalAppDbContext _hospitalAppDbContext;

        private readonly string _connectionString;



        public DoctorController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, HospitalAppDbContext hospitalAppDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _hospitalAppDbContext = hospitalAppDbContext;
            _connectionString = configuration.GetConnectionString("DefaultAzureConnectionString");

        }

        public IActionResult Index()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            DoctorViewModel doctorViewModel = user.Adapt<DoctorViewModel>();

            return View(doctorViewModel);
        }

        public IActionResult DoctorIndex()
        {
            return View();
        }

        public IActionResult DoctorEdit()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            DoctorViewModel doctorViewModel = user.Adapt<DoctorViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            //ViewBag.Branch = new SelectList(Enum.GetNames(typeof(Branches)).Select(name => name.Replace("_", " ")));

            ViewBag.Branch = Enum.GetValues(typeof(Branches))
            .Cast<int>()
            .Select(value => new SelectListItem
            {
                Text = Enum.GetName(typeof(Branches), value)?.Replace("_", " "),
                Value = value.ToString()
            });

            return View(doctorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DoctorEdit(DoctorViewModel doctorViewModel, IFormFile userPicture)
        {


            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirm");

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            //ViewBag.Branch = new SelectList(Enum.GetNames(typeof(Branches)).Select(name => name.Replace("_", " ")));

            ViewBag.Branch = Enum.GetValues(typeof(Branches)).Cast<int>().Select(value => new SelectListItem { Text = Enum.GetName(typeof(Branches), value).Replace("_", " "), Value = value.ToString() });

            if (!ModelState.IsValid)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                string phone = _userManager.GetPhoneNumberAsync(user).Result!;

                if (phone != doctorViewModel.PhoneNumber)
                {
                    if (_userManager.Users.Any(u => u.PhoneNumber == doctorViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");

                        return View(doctorViewModel);
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



                user.UserName = doctorViewModel.UserName;
                user.Email = doctorViewModel.Email;
                user.PhoneNumber = doctorViewModel.PhoneNumber;
                user.City = doctorViewModel.City;
                user.Gender = (int)doctorViewModel.Gender;
                user.Branch = (int)doctorViewModel.Branch;
                user.BirthDay = doctorViewModel.BirthDay;



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

                if (phone != doctorViewModel.PhoneNumber)
                {
                    if (_userManager.Users.Any(u => u.PhoneNumber == doctorViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");

                        return View(doctorViewModel);
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

                //string branchName = ViewBag.Branch.ToString().Replace(" ", "_");
                //user.Branch = (int)Enum.Parse(typeof(Branches), branchName);

                user.UserName = doctorViewModel.UserName;
                user.Email = doctorViewModel.Email;
                user.PhoneNumber = doctorViewModel.PhoneNumber;
                user.City = doctorViewModel.City;
                user.Gender = (int)doctorViewModel.Gender;
                user.Branch = (int)doctorViewModel.Branch;
                user.BirthDay = doctorViewModel.BirthDay;



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
            return View(doctorViewModel);
        }

        public IActionResult AddMedicine()
        {

            var model = new MedicineViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddMedicine(MedicineViewModel model)
        {

            if (ModelState.IsValid)
            {

                _hospitalAppDbContext.Meds.Add(new MedicineViewModel
                {
                    MedName = model.MedName,
                    Description = model.Description,
                    Price = model.Price
                });
                _hospitalAppDbContext.SaveChanges();


                return RedirectToAction("ListMedicine");
            }


            return View(model);
        }

        public IActionResult EditMedicine(int id)
        {

            var medicine = _hospitalAppDbContext.Meds.Find(id);
            if (medicine == null)
            {

                return NotFound();
            }


            var model = new MedicineViewModel
            {
                Id = medicine.Id,
                MedName = medicine.MedName,
                Description = medicine.Description,
                Price = medicine.Price
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditMedicine(MedicineViewModel model)
        {

            if (ModelState.IsValid)
            {

                var medicine = _hospitalAppDbContext.Meds.Find(model.Id);
                if (medicine == null)
                {

                    return NotFound();
                }
                medicine.MedName = model.MedName;
                medicine.Description = model.Description;
                medicine.Price = model.Price;
                _hospitalAppDbContext.SaveChanges();


                return RedirectToAction("ListMedicine");
            }


            return View(model);
        }

        public IActionResult DeleteMedicine(int id)
        {

            var medicine = _hospitalAppDbContext.Meds.Find(id);
            if (medicine == null)
            {

                return NotFound();
            }


            var model = new MedicineViewModel
            {
                Id = medicine.Id,
                MedName = medicine.MedName,
                Description = medicine.Description,
                Price = medicine.Price
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteMedicine(int id, MedicineViewModel model)
        {

            var medicine = _hospitalAppDbContext.Meds.Find(id);
            if (medicine == null)
            {

                return NotFound();
            }
            _hospitalAppDbContext.Meds.Remove(medicine);
            _hospitalAppDbContext.SaveChanges();


            return RedirectToAction("ListMedicine");
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



        public async Task<IActionResult> PatientList()
        {
            var patientList = await _userManager.Users.ToListAsync();

            var memberViewModelList = patientList.Select(x => new AppUser()
            {
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            }).ToList();

            return View(memberViewModelList);
        }

        public async Task<IActionResult> Patients()
        {
            var roleName = "Patient";
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return View(usersInRole);
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                ViewBag.Message = "Dosya başarılı bir şekilde yüklendi";
            }
            else
            {
                ViewBag.Message = "Bir dosya seçmediniz.";
            }
            return View();
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

        [HttpPost]
        public ActionResult DeleteFile(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                ViewBag.Message = "File deleted successfully";
            }
            else
            {
                ViewBag.Message = "File not found";
            }
            return View();
        }
        [HttpGet]
        public ActionResult UploadPatientFile(string patientId)
        {
            var model = new PatientFileViewModel
            {
                PatientId = patientId
            };
            return View();
        }

        [HttpPost]
        public IActionResult UploadPatientFile(string patientId, IFormFile file)
        {


       
            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

           
            var fileData = System.IO.File.ReadAllBytes(filePath);

            
            System.IO.File.Delete(filePath);

           
            var patientFile = new PatientFileViewModel
            {
                PatientId = patientId,
                Data = fileData,
                ContentType = file.ContentType,
                FileName = file.FileName
            };

            
            using (var context = _hospitalAppDbContext)
            {
                context.Add(patientFile);
                context.SaveChanges();
            }
            TempData["success"] = "Form başarılı bir şekilde gönderildi.";
            return RedirectToAction("PatientList");
        }

        public IActionResult ListPatientFiles(string patientId)
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




        public FileResult Dispatch()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExampleFiles", "Sevk Dilekçe Örneği.docx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", "Sevk Dilekçe Örneği.docx");


        }



        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();

        }
    }
}





