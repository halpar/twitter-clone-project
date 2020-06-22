using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace twitter_project.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string PersonalInformation { get; set; }
        
        public string Location { get; set; }
        
        public string WebPage { get; set; }
        
        public string ProfileImage { get; set; }
        
        public string BannerImage { get; set; }
        
        public string BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Tweet> Tweets { get; set; }

        public virtual ICollection<Follower> Followers { get; set; }

    }
}
