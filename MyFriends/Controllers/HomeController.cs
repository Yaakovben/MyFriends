using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFriends.DAL;
using MyFriends.Models;
using System.Diagnostics;

namespace MyFriends.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
    
        // פתיחת טופס לחבר חדש
        public IActionResult Create()
        {
            return View(new Friend());
        }

        //  הוספת חבר חדש 
        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddFriend(Friend friend)
        {
            Data.Get.Friends.Add(friend);
            Data.Get.SaveChanges();
            return RedirectToAction("Friends");
        }
        //  פיתחת טופס לעדכון חבר  
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Friends");
            }
            Friend? friend = Data.Get.Friends.FirstOrDefault(friend => friend.Id == id);
            if (friend == null)
            {
                return RedirectToAction("Friends");
            }
            
            return View(friend);

        }

        // עדכון חבר
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateFriend(Friend newFriend)
        {
            if (newFriend == null)
            {
                return RedirectToAction("Friends");
            }
            Friend? existingFriend = Data.Get.Friends.FirstOrDefault(f => f.Id == newFriend.Id);

            if (existingFriend == null)
            
            {
             return RedirectToAction("Friends");
            }

                //עדכון חבר מהדאטה בייס בחבר שקיבלנו בויאו
             Data.Get.Entry(existingFriend).CurrentValues.SetValues(newFriend);

             Data.Get.SaveChanges();
             return RedirectToAction("Friends");
        }
        // מחיקת חבר
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Friend>friendList = Data.Get.Friends.ToList();

            Friend? friendToRemove = friendList.Find(friend => friend.Id == id);
            if (friendToRemove == null)
            {
                return NotFound();
            }
            Data.Get.Friends.Remove(friendToRemove);
            Data.Get.SaveChanges();
            return RedirectToAction(nameof(Friends));
            }
        

        // הצגת החברים
        public IActionResult Friends()
        {
            List<Friend> friends = Data.Get.Friends.ToList();
            return View(friends);
        }

        //הצגת פרטי החבר
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Friends");
            }
           Friend? friend =  Data.Get.Friends.Include(f => f.Images).ToList().FirstOrDefault(friend => friend.Id == id);
            if (friend == null)
            {
                return RedirectToAction("Friends");
            }
            return View(friend);
        }

        // פונקצייה להוספת תמונות בנוסף לתמונה שקיימת 
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddNewImage(Friend friend)
        {
            Friend? friendFromDb = Data.Get.Friends.FirstOrDefault(f => f.Id == friend.Id);
            if (friendFromDb == null)
            {
                return NotFound();
            }
            byte[]? firstImage = friend.Images.First().MyImages;
            if (firstImage == null)
            {
                return NotFound();
            }
            friendFromDb.AddImage(firstImage);
            Data.Get.SaveChanges();
            return RedirectToAction("Details", new { m = friend.Id });



        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
