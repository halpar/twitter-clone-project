using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twitter_project.Models;

namespace twitter_project.Models
{
    public class TwitterDbContext : DbContext
    {
        public TwitterDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>().Property(x => x.SendTime).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<User>().Property(x => x.RegistrationDate).HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Like>().HasKey(x => x.UserId);
            modelBuilder.Entity<Like>().HasKey(x => x.LikeId);

        }

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follower> Followers { get; set; }
    }
}
