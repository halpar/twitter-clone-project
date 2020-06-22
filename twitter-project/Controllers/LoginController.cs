using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using twitter_project.Models;
using twitter_project.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace twitter_project.Controllers
{

    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        // GET: /<controller>/

        TwitterDbContext context;

        public LoginController(TwitterDbContext _context ,IWebHostEnvironment hostEnvironment)
        {
            context = _context;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User usr)
        {
            if (context.Users.Any(x => x.Username == usr.Username && x.Password == usr.Password))  //İstenen kayıt varsa true yoksa false döndürür
            {

                var user = context.Users.FirstOrDefault(x => x.Username == usr.Username && x.Password == usr.Password);


                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Password", user.Password );
                HttpContext.Session.SetString("Phone", user.Phone);
                HttpContext.Session.SetString("Email", user.Email);
                
                return Json(new { message = "Giriş yapılıyor" });
            }
            else
            {
                return Json(new { message = "Hatalı parola veya kullanıcı adı" });
            }
        }

        [HttpPost]
        public IActionResult Register(UserViewModel usrvm)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.Any(x => x.Username == usrvm.Username && x.Password == usrvm.Password || x.Email == usrvm.Email))  // Any istenen kayıt varsa true yoksa false döndürür
                {
                    return Json(new { message = "Bu kullanıcı zaten kayıtlı" });
                }
                else
                {
                   string uniqueFileName ="default_profile.png";
                    User usr = new User
                    {
                        Username = usrvm.Username,
                        Name = usrvm.Name,
                        Password = usrvm.Password,
                        Phone = usrvm.Phone,
                        Email=usrvm.Email,
                        ProfileImage = uniqueFileName,
                    };


                    context.Add(usr);
                    context.SaveChanges();
                    return Json(new { message = "Kayıt başarılı" });
                }

            }
            else
            {
                ModelState.AddModelError("", "Hatalı giriş");
                return Json(new { message = "Verilerde hata var." });
            }
        }


        //private string FetchDefaultPhoto(UserViewModel usrvm)
        //{
        //    string uniqueFilename = null;
        //    if (usrvm.ProfileImage == null)
        //    { 

        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

        //        string filePath = Path.GetFullPath(@"C:\Users\alper\source\repos\twitter-project\twitter-project\wwwroot\images\defaultprofile.png");

        //        usrvm.ProfileImage

        //    }
            
        //    return uniqueFilename;
        //}
        //private string UploadedFile(UserViewModel usrvm)
        //{
        //    string uniqueFileName = null;

        //    if (usrvm.ProfileImage != null)
        //    {
        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + usrvm.ProfileImage.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            usrvm.ProfileImage.CopyTo(fileStream);
        //        }
        //    }

        //    return uniqueFileName;
        //}
    }
}
