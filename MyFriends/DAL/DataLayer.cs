using Microsoft.EntityFrameworkCore;
using MyFriends.Models;

namespace MyFriends.DAL
{
    // DbContext קלאס שמייצג את שכבת הנתונים,יורש מקלאס בשם 
    public class DataLayer : DbContext
    {
        //קונסטרקטור שמקבל מחרוזת חיבור ומעביר אותה לקונסטרקטור של הקלאס האב
        public DataLayer(string connectinString) : base(GetOptions(connectinString))
        {
            //פונקצייה שמוודאת שהדאטה בייס נוצר
            Database.EnsureCreated();

            //להכניס נתונים בפעם הראשונה
            Seed();
            
        }
        private void Seed() 
        { 
            // אם כבר יש חברים בטבלה
            if(Friends.Count() >0)
            {     
                return;
            }  
            Friend firsFriend = new Friend();
            firsFriend.FirstName = "גקי";
            firsFriend.LastName = "לוי";
            firsFriend.EmailAdress = "gekilevi@gmail.com";
            firsFriend.PhoneNumber = "1234567890";

            Friends.Add(firsFriend);
            SaveChanges();
        }


        //יצירת טבלאות עם כל הנתונים
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Image> Images { get; set; }

        // פונקציה שמחזירה את אפשרויות ההתחברות למסד הנתונים
        private static DbContextOptions GetOptions(string connectinString)
        {
            return SqlServerDbContextOptionsExtensions 
                .UseSqlServer(new DbContextOptionsBuilder(), connectinString)
                .Options;
                
        }

    }
}
