using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GFMS.Models
{
    public class RegisteredStudent
    {
        public Student? Student { get; set; }
        public Registration? Registration { get; set; }
        public PreviousSchool? PreviousSchool { get; set; }
        public string? StudentName
        {
            get
            {
                return $"{Student!.LastName} {Student!.FirstName}";
            }
        }
        public string? Status { get; set; }
        public string? StatusColor { get; set; } = "#ffb302";
        public ImageSource? ProfilePicture
        {
            get
            {
                try
                {
                    if(Registration != null)
                    {
                        if (Registration.Pic != null)
                        {
                            byte[] imageBytes = Convert.FromBase64String(Registration.Pic);

                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.StreamSource = ms;
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.EndInit();

                                return bitmapImage;
                            }
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Error converting base64 string: " + ex.Message);
                    return null;
                }
                return null;
            }
        }
    }
}
