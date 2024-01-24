using Aspose.Words.Replacing;
using Aspose.Words;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFMS.Models;
using GFMS.ViewModels;
using GFMS.Views;

namespace GFMS.Services
{
    public static class GeneratePDF
    {
        public static void GeneratePaymentReciept(TuitionDetails tuitionDetails, StudentAccounting student)
        {
            string DateIssued = DateTime.Now.Date.ToShortDateString();
            string CompleteName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
            //int DateNumber = Convert.ToInt32(DateIssued.Date.ToShortDateString + " " + DateIssued.Date.ToShortTimeString);
            //string OrdinalSuffix = GetOrdinalSuffix(DateNumber);
            Document doc = new Document("../../../Documents/PaymentReceipt.docx");

            FindReplaceOptions findReplaceOptions = new FindReplaceOptions(FindReplaceDirection.Forward);

            // You can close the stream now, it is no longer needed because the document is in memory.
            // Find and replace text in the document
            doc.Range.Replace("<Name>", CompleteName, findReplaceOptions);
            doc.Range.Replace("<Grade>", student.Registration!.Grade, findReplaceOptions);
            doc.Range.Replace("<Finance Name>", $"{MainWindow.User!.FirstName} {MainWindow.User!.LastName}", findReplaceOptions);
            doc.Range.Replace("<TFA>", tuitionDetails.TuitionFee, findReplaceOptions);
            doc.Range.Replace("<TTF>", tuitionDetails.TotalTuitionFee, findReplaceOptions);
            doc.Range.Replace("<OF>", tuitionDetails.OtherFees, findReplaceOptions);
            doc.Range.Replace("<Inclusion>", tuitionDetails.Inclusion, findReplaceOptions);
            doc.Range.Replace("<DiscountType>", tuitionDetails.DiscountType, findReplaceOptions);
            doc.Range.Replace("<DA>", tuitionDetails.DiscountedTuition, findReplaceOptions);
            doc.Range.Replace("<MP>", tuitionDetails.ModeOfPayment, findReplaceOptions);
            doc.Range.Replace("<RF>", tuitionDetails.RegistrationFee, findReplaceOptions);
            doc.Range.Replace("<Books>", tuitionDetails.Books, findReplaceOptions);
            doc.Range.Replace("<Uniform>", tuitionDetails.Books, findReplaceOptions);
            doc.Range.Replace("<TA>", tuitionDetails.TotalAmount, findReplaceOptions);
            doc.Range.Replace("<Balance>", tuitionDetails.Balance, findReplaceOptions);
            doc.Range.Replace("<Payment>", tuitionDetails.Payment, findReplaceOptions);
            if (tuitionDetails.AddFeeDscOne == null || tuitionDetails.AddFeeDscOne.ToString() == string.Empty)
            {
                doc.Range.Replace("<ADD1>", "  ", findReplaceOptions);
                doc.Range.Replace("<ADDD1>", "  ", findReplaceOptions);
            } else
            {
                doc.Range.Replace("<ADD1>", "Addtl. Fee: " + tuitionDetails.AddFeeDscOne, findReplaceOptions);
                doc.Range.Replace("<ADDD1>", tuitionDetails.AddFeeOne, findReplaceOptions);
            }
            if (tuitionDetails.AddFeeDscTwo == null || tuitionDetails.AddFeeDscTwo.ToString() == string.Empty)
            {
                doc.Range.Replace("<ADD2>", "  ", findReplaceOptions);
                doc.Range.Replace("<ADDD2>", "  ", findReplaceOptions);
            }
            else
            {
                doc.Range.Replace("<ADD2>", "Addtl. Fee: " + tuitionDetails.AddFeeDscTwo, findReplaceOptions);
                doc.Range.Replace("<ADDD2>", tuitionDetails.AddFeeTwo, findReplaceOptions);
            }

            doc.Range.Replace("<Date>", DateIssued, new FindReplaceOptions(FindReplaceDirection.Forward));
            // Save the Word document

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF document (*.pdf)|*.pdf";
            dialog.FileName = CompleteName + " " + DateTime.Now.Date.ToString("MM-dd-yyyy");
            var result = dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (result == true)
            {
                MemoryStream stream = new MemoryStream();
                doc.Save(stream, SaveFormat.Pdf);
                byte[] fileBytes = stream.ToArray();
                FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                fileStream.Write(fileBytes, 0, fileBytes.Length);
                fileStream.Flush();
                fileStream.Close();
                stream.Close();
            }
        }
    }
}
