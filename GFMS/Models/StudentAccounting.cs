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
    public class StudentAccounting
    {
        public Student? Student { get; set; }
        public Registration? Registration { get; set; }
        public List<Accounting>? PaymentList { get; set; }
        public string? StudentName
        {
            get
            {
                return $"{Student!.LastName} {Student!.FirstName}";
            }
        }

        public string TotalPaid
        {
            get
            {
                decimal totalPaid = 0m;
                if (PaymentList != null && PaymentList.Count > 0)
                {
                    for (int i = 0; i < PaymentList.Count; i++)
                    {
                        totalPaid += Convert.ToDecimal(PaymentList[i].Payment);
                    }
                }
                return totalPaid.ToString("N2");
            }
        }

        public string? Status
        {
            get
            {
                if (Convert.ToDecimal(TotalPaid) > 15000m)
                {
                    return "Paid";
                }
                return "Unpaid";
            }
        }
        public string? StatusColor
        {
            get
            {
                if (Convert.ToDecimal(TotalPaid) > 15000m)
                {
                    return "#3dc03c";
                }
                return "#ff2147";
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
