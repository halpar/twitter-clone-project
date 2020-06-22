using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace twitter_project.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }

        public string TweetText { get; set; }

        public DateTime SendTime { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public int ReTweets { get; set; }

        public int UserId { get; set; } //nullable olmasını önler
        public User User { get; set; } //Fk
    }
}
