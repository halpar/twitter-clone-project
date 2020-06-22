using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twitter_project.Models;

namespace twitter_project.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PersonalInformation { get; set; }
        public string Location { get; set; }
        public string WebPage { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string BannerImage { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public User CurrentUser { get; set; }
    }
}
