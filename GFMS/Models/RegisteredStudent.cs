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
        public List<Requirement>? Requirement { get; set; }
        public List<Accounting>? PaymentList { get; set; }
        public List<TuitionDetails>? TuitionDetailsList { get; set; }

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
                if (IsPaid && Requirement.Count == 9)
                {
                    return "Enrolled";
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
                if (IsPaid && Requirement.Count == 9)
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

        private decimal DecimalPaid
        {
            get
            {
                decimal totalPaid = 0m;
                if (TuitionDetailsList != null && TuitionDetailsList.Count > 0)
                {
                    for (int i = 0; i < TuitionDetailsList.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(TuitionDetailsList[i].Payment))
                        {
                            continue;
                        }
                        totalPaid += Convert.ToDecimal(TuitionDetailsList[i].Payment);
                    }
                }
                return totalPaid;
            }
        }

        public string TotalPaid
        {
            get
            {
                return DecimalPaid.ToString("N2");
            }
        }

        public bool IsPaid
        {
            get
            {
                if (TuitionDetailsList != null && TuitionDetailsList.Count > 0 && !TuitionDetailsList.All(details => string.IsNullOrEmpty(details.TotalTuitionFee)))
                {
                    var totalTuitionFee = TuitionDetailsList.LastOrDefault();
                    var tuition = Convert.ToDecimal(!string.IsNullOrWhiteSpace(totalTuitionFee!.TotalTuitionFee) ? totalTuitionFee!.TotalTuitionFee : "0") +
                    Convert.ToDecimal(!string.IsNullOrWhiteSpace(totalTuitionFee!.Books) ? totalTuitionFee!.Books : "0") +
                    Convert.ToDecimal(!string.IsNullOrWhiteSpace(totalTuitionFee!.Uniform) ? totalTuitionFee!.Uniform : "0") +
                    Convert.ToDecimal(!string.IsNullOrWhiteSpace(totalTuitionFee!.OtherFees) ? totalTuitionFee!.OtherFees : "0") +
                    Convert.ToDecimal(!string.IsNullOrWhiteSpace(totalTuitionFee!.RegistrationFee) ? totalTuitionFee!.RegistrationFee : "0");

                    if (totalTuitionFee != null)
                    {
                        if (totalTuitionFee.TotalTuitionFee != null &&
                            DecimalPaid >= Convert.ToDecimal(tuition))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return false;
            }
        }

        public string? CanGoUp
        {
            get
            {
                if (IsPaid && Requirement.Count == 9)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }
    }
}
