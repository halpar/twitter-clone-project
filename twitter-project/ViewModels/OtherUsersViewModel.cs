using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twitter_project.Models;

namespace twitter_project.ViewModels
{
    public class OtherUsersViewModel
    {
        public User currentUser { get; set; }
        public User sessionUser { get; set; }
    }
}
