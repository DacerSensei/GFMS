using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GFMS.Models
{
    public class StudentReport
    {
        public Student? Student { get; set; }
        public Registration? Registration { get; set; }
        public ReportCard? ReportCard { get; set; }
        public List<Requirement>? Requirement { get; set; }
        public string? StudentName
        {
            get
            {
                return $"{Student!.LastName} {Student!.FirstName}";
            }
        }

        public string? Status
        {
            get
            {
                if (Convert.ToInt16(Registration!.Status) == 1)
                {
                    return "Officially Enrolled";
                }
                else
                {
                    return "Temporary Enrolled";
                }
            }
        }
        public string? StatusColor
        {
            get
            {
                
                if (Convert.ToInt16(Registration!.Status) == 1)
                {
                    return "#3dc03c";
                }
                else
                {
                    return "#ffb302";
                }
            }
        }

        public ImageSource? ProfilePicture
        {
            get
            {
                try
                {
                    if (Registration != null)
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
