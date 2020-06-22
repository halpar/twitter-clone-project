using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using twitter_project.Models;
using twitter_project.ViewModels;

namespace twitter_project.Controllers
{
    public class HomeController : Controller
    {
        TwitterDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(TwitterDbContext _context , IWebHostEnvironment hostEnvironment)
        {
            context = _context;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult HomePage()
        {
            var userName = HttpContext.Session.GetString("Username");
            var sessionId = HttpContext.Session.GetInt32("UserId");


            if (context.Users.Any(x => x.Username == userName))  //İstenen kayıt varsa true yoksa false döndürür
            {
                var user = context.Users.Where(x => x.Username == userName)
                    .Include("Tweets")
                    .Include("Followers")
                    .Include("Tweets.Likes")
                    .FirstOrDefault();

                var allUsers = context.Users.Select(x => x)
                    .Include("Tweets")
                    .Include("Followers")
                    .Include("Tweets.Likes").ToList(); //Db deki tüm userlar

                var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();


                var notFollowingUsers = otherUsers.Where(x => !user.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

                var _connectedUsers = allUsers.Where(x => !notFollowingUsers.Select(y => y.UserId).Contains(x.UserId)).ToList();  // Ben ve takip ettiklerim

                var vmHomePage = new HomepageViewModel()
                {
                    connectedUsers = _connectedUsers,
                    otherUsers = notFollowingUsers,
                    sessionUser = user
                };

                ViewBag.notFollowing = notFollowingUsers;

                return View(vmHomePage);

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public IActionResult Follow(User user)
        {

            Follower follower = new Follower();
            follower.UserId = (int)HttpContext.Session.GetInt32("UserId");
            follower.FollowId = user.UserId;
            context.Followers.Add(follower);
            context.SaveChanges();


            return RedirectToAction("HomePage", "Home");
        }


        [HttpPost]
        public IActionResult Tweet(Tweet tweet)
        {
            var userName = HttpContext.Session.GetString("Username");
            if (context.Users.Any(x => x.Username == userName))  //İstenen kayıt varsa true yoksa false döndürür
            {
                var user = context.Users.FirstOrDefault(x => x.Username == userName);

                tweet.UserId = user.UserId;
                context.Tweets.Add(tweet);
                context.SaveChanges();
            }


            return RedirectToAction("HomePage");
        }

        [HttpPost]
        public IActionResult Like(Tweet twt)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (context.Likes.Any(x => x.UserId == userId && x.TweetId == twt.TweetId))
            {
                var like = context.Likes.SingleOrDefault(x => x.UserId == userId && x.TweetId == twt.TweetId);

                context.Likes.Remove(like);
                context.SaveChanges();

            }
            else
            {
                Like like = new Like();
                like.TweetId = twt.TweetId;
                like.UserId = (int)userId;
                context.Likes.Add(like);
                context.SaveChanges();
            }

            return RedirectToAction("HomePage");
        }

        public IActionResult Notifications()
        {
            return View();
        }

        public IActionResult Bookmarks()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Profile()
        {

            var sessionId = HttpContext.Session.GetInt32("UserId");

            var sessionUser = context.Users.FirstOrDefault(x => x.UserId == sessionId);

            var myTweets = context.Tweets.Where(x => x.UserId == sessionId).ToList();

            var currentUser = context.Users   //Tıkladığım kişinin bilgileri eager loading
               .Where(x => x.UserId == sessionId)
               .Include("Tweets")
               .Include("Followers")
               .Include("Tweets.Likes")
               .FirstOrDefault();


            var followers = context.Followers.Where(x => x.FollowId == currentUser.UserId)
                .Include("User").ToList();

            ViewBag.followers = followers.Count;


            var allUsers = context.Users.Select(x => x)
                .Include("Tweets")
                .Include("Followers")
                .Include("Tweets.Likes").ToList(); //Db deki tüm userlar    

            var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();


            var notFollowingUsers = otherUsers.Where(x => !sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

            ViewBag.notFollowing = notFollowingUsers;

            var usrvm = new UserViewModel()
            {
                CurrentUser = currentUser
            };


            return View(usrvm);
        }

        [HttpPost]
        public IActionResult Profile(UserViewModel usr)
        {
            var sessionName = HttpContext.Session.GetString("Username");



            if (usr.Username == sessionName)
            {
                return RedirectToAction("Profile");
            }

            else
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == sessionName);
                var myTweets = context.Tweets.Where(x => x.UserId == selectedUser.UserId).ToList();
                return View();
            }
        }


        [HttpPost]
        public IActionResult OtherProfile(User usr)
        {
            var sessionId = HttpContext.Session.GetInt32("UserId");


            if (usr.UserId == sessionId)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                var _currentUser = context.Users   //Tıkladığım kişinin bilgileri eager loading
                   .Where(x => x.UserId == usr.UserId)
                   .Include("Tweets")
                   .Include("Followers")
                   .FirstOrDefault();


                var _sessionUser = context.Users.Where(x => x.UserId == sessionId)
                    .Include("Followers")
                    .FirstOrDefault();



                var vmOther = new OtherUsersViewModel()
                {
                    sessionUser = _sessionUser,
                    currentUser = _currentUser
                };

                var followingUser = _currentUser.Followers.Where(x => x.UserId == _currentUser.UserId).ToList();

                var followers = context.Followers.Where(x => x.FollowId == _currentUser.UserId)
                                .Include("User").ToList();

                ViewBag.followers = followers.Count;


                var allUsers = context.Users.Select(x => x)
                    .Include("Tweets")
                    .Include("Followers")
                    .Include("Tweets.Likes").ToList(); //Db deki tüm userlar    

                var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();


                var notFollowingUsers = otherUsers.Where(x => !_sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

                ViewBag.notFollowing = notFollowingUsers;

                var followStatus = _sessionUser.Followers.Where(x => x.FollowId == _currentUser.UserId).ToList();

                if (followStatus.Count == 0)
                {
                    ViewBag.followStatus = false;
                }
                else
                {
                    ViewBag.followStatus = true;
                }
                return View(vmOther);
            }
        }



        [HttpPost]
        public IActionResult Followers(User _clickedUser)
        {
            var _sessionUser = context.Users.Where(x => x.UserId == _clickedUser.UserId)
                   .Include("Followers")
                   .FirstOrDefault();


            var allUsers = context.Users.Select(x => x)
                .Include("Tweets")
                .Include("Followers")
                .Include("Tweets.Likes").ToList(); //Db deki tüm userlar    

            var otherUsers = allUsers.Where(x => x.UserId != _sessionUser.UserId).ToList();


            var notFollowingUsers = otherUsers.Where(x => !_sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

            var followerUsers = otherUsers.Where(x => context.Followers.Select(y => y.UserId).Contains(x.UserId)).ToList();


            var followingUsers = otherUsers.Where(x => _sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList();  // Ben ve takip ettiklerim


            ViewBag.notFollowing = notFollowingUsers;

            ViewBag.following = followingUsers;

            ViewBag.follower = followerUsers;

            return View(_sessionUser);
        }

        [HttpPost]
        public IActionResult UnFollow(User _clickedUser)
        {
            int sessionId = (int)HttpContext.Session.GetInt32("UserId");

            var sessionUser = context.Users.Where(x => x.UserId == sessionId);

            //var _sessionUser = context.Users.Where(x => x.UserId == _clickedUser.UserId)
            //       .Include("Followers")
            //       .FirstOrDefault();

            var deleteFollow = context.Followers.FirstOrDefault(x => x.UserId == sessionId && x.FollowId == _clickedUser.UserId);

            context.Followers.Remove(deleteFollow);
            context.SaveChanges();

            return RedirectToAction("HomePage");
        }

        [HttpPost]
        public IActionResult UpdateUser(UserViewModel usr)
        {
            int sessionId = (int)HttpContext.Session.GetInt32("UserId");
            
            string  uniqueFileName = UploadedFile(usr);

            var sessionUser = context.Users.Where(x => x.UserId == sessionId).FirstOrDefault();

            sessionUser.Name = usr.Name;
            sessionUser.PersonalInformation = usr.PersonalInformation;
            sessionUser.Location = usr.Location;
            sessionUser.WebPage = usr.WebPage;
            sessionUser.BirthDate = usr.BirthDate;
            sessionUser.ProfileImage = uniqueFileName;
            context.SaveChanges();

            return RedirectToAction("Profile");
        }

        private string UploadedFile(UserViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }


}
