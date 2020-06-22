using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace twitter_project.Models
{
    public class Follower
    {
        public int Id { get; set; }

        public int FollowId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
