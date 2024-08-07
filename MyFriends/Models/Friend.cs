using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriends.Models
{
    public class Friend
    {
        public Friend() 
        {
            Images = new List<Image>();
        }
        [Key]
        public int Id { get; set; }

        [Display(Name ="שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Display(Name = "שם מלא"),NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }

        [Display(Name = "מספר טלפון"),Phone]

        public string PhoneNumber { get; set; }

        [Display(Name = "כתובת אימייל"),EmailAddress(ErrorMessage ="כתובת מייל אינה תקינה")]
        public string EmailAdress { get; set; }

        public List<Image> Images { get; set; }

        [Display(Name = "הוספת תמונה"),NotMapped]
        public IFormFile setImage { 
            get { return null;}
            set
            {
                if (value == null)
                {
                    return;
                }
                //יצירת מקום בזכרון עבור קובץ
                MemoryStream stream = new MemoryStream();

                value.CopyTo(stream);
                //המרת מקום בזכרון שיצרנו לבייטים
                byte[] streamArray = stream.ToArray();
                //הוספת התמונה לרשימת התמונות של החבר
                AddImage(streamArray);
                    }

            }


        public void AddImage(byte[] image)
        {
            Image img = new Image
            {
                //דרך אחת ליצור מופע של קלאס
                MyImages = image,
                Friend = this,
            };
            //Image img = new Image()
            //img.MyImage = image;
            //img.Friend = this;
            Images.Add(img);


        }






    }

}

