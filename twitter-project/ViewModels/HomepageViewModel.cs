using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twitter_project.Models;

namespace twitter_project.ViewModels
{
    public class HomepageViewModel
    {
        public List<User> connectedUsers { get; set; }
        public List<User> otherUsers { get; set; }
        public User sessionUser { get; set; }
    }
}
