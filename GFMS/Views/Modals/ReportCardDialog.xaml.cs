using GFMS.Commands;
using GFMS.Models;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Aspose.Words.Replacing;
using Aspose.Words;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Diagnostics;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for ReportCardDialog.xaml
    /// </summary>
    public partial class ReportCardDialog : Window, INotifyPropertyChanged
    {
        static char GetAlphabetLetter(int orderNumber)
        {
            // Ensure the order number is within the valid range
            if (orderNumber < 1 || orderNumber > 26)
            {
                throw new ArgumentException("Invalid order number. It should be between 1 and 26.");
            }

            // Convert the order number to the ASCII value of the corresponding letter
            int asciiValue = orderNumber + (int)'A' - 1;

            // Convert the ASCII value to a character
            char letter = (char)asciiValue;

            return letter;
        }
        private void SaveGridAsImage(Grid grid, string filePath)
        {
            // Measure and arrange the grid to ensure it's fully rendered
            grid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            grid.Arrange(new Rect(0, 0, grid.DesiredSize.Width, grid.DesiredSize.Height));

            // Create a RenderTargetBitmap to render the entire grid
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)grid.ActualWidth,
                (int)grid.ActualHeight,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            // Set pixel alignment mode and snap to device pixels
            RenderOptions.SetBitmapScalingMode(renderTargetBitmap, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(renderTargetBitmap, EdgeMode.Aliased);

            // Render the entire grid onto the RenderTargetBitmap
            renderTargetBitmap.Render(grid);

            // Create a PngBitmapEncoder and add the RenderTargetBitmap to it
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            // Save the image to the specified file path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        private void SaveWindowAsImage(string filePath)
        {
            // Measure and arrange the window to ensure it's fully rendered
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(0, 0, ActualWidth, ActualHeight));

            // Create a RenderTargetBitmap to render the entire window
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)ActualWidth,
                (int)ActualHeight,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            // Set pixel alignment mode and snap to device pixels
            RenderOptions.SetBitmapScalingMode(renderTargetBitmap, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(renderTargetBitmap, EdgeMode.Aliased);

            // Render the entire window onto the RenderTargetBitmap
            renderTargetBitmap.Render(this);

            // Create a PngBitmapEncoder and add the RenderTargetBitmap to it
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            // Save the image to the specified file path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        Document doc = new Document("../../../Documents/ReportCard.docx");
        Document doc_jhs = new Document("../../../Documents/JuniorHighSchool.docx");
        Document doc_shs = new Document("../../../Documents/SeniorHighSchool.docx");
        Document doc_elem = new Document("../../../Documents/Elementary.docx");
        Document doc_pre = new Document("../../../Documents/PreSchool.docx");
        FindReplaceOptions findReplaceOptions = new FindReplaceOptions(FindReplaceDirection.Forward);
        public ObservableCollection<Subject> SubjectList { get; set; } = new ObservableCollection<Subject>();
        public ObservableCollection<Behavior> BehaviorList { get; set; } = new ObservableCollection<Behavior>();
        public ObservableCollection<Attendance> AttendanceList { get; set; } = new ObservableCollection<Attendance>();
        public ObservableCollection<Narrative> NarrativeList { get; set; } = new ObservableCollection<Narrative>();
        private readonly LoginCredentials Credentials = new LoginCredentials();
        public ReportCardDialog(StudentReport student, Users teacher, Users principal, bool isEditable = false, bool isVisible = true, bool toPrint = false, bool isPaid = false)
        {
            InitializeComponent();
            LoadedCommand = new Command(obj =>
            {

            });
            if (!isVisible)
            {
                SaveVisibility = Visibility.Hidden;
            }
            if (isEditable)
            {
                IsTeacher = "SAVE";
                IsReadOnly = false;
            }
            else
            {
                IsTeacher = "REQUEST PRINT REPORT CARD";
                IsTeacherForm = "REQUEST PRINT FORM 137";
            }
            LoadAllDataAsync(student, teacher, principal, toPrint);
            SaveCommand = new Command(async obj =>
            {
                if (isEditable)
                {
                    var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to save?"), "SecondaryDialog");
                    if ((bool)result! == false)
                    {
                        return;
                    }
                    ReportCard reportCard = new ReportCard()
                    {
                        Attendance = JsonConvert.SerializeObject(AttendanceList),
                        Behavior = JsonConvert.SerializeObject(BehaviorList),
                        Narrative = JsonConvert.SerializeObject(NarrativeList),
                        Subjects = JsonConvert.SerializeObject(SubjectList)
                    };
                    await Credentials.UpdateStudentAsync(reportCard, new { id = student.ReportCard!.Id }, "studentgrades");
                    DialogResult = true;
                    Close();
                }
                else
                {
                    int countOfNull = SubjectList.Count(subject => subject.FirstGrading == null);
                    if (countOfNull > 0)
                    {
                        await DialogHost.Show(new MessageDialog("Notice", "You cannot request because you have empty grades on first grading."), "SecondaryDialog");
                        return;
                    }
                    if (isPaid)
                    {
                        var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to send a request for this student's REPORT CARD to the principal?"), "SecondaryDialog");
                        if ((bool)result! == false)
                        {
                            return;
                        }
                        await Credentials.RegisterStudentAsync(new Notification { User_Id = MainWindow.User!.Id.ToString(), Type = "Report Card", Student_Id = student.Student.id.ToString(), Status = "Pending", Requested = DateTime.Now.ToShortDateString(), Approved = "Unknown" }, "notification");
                        await DialogHost.Show(new MessageDialog("Notice", "You just sent a request to the principal.", true), "SecondaryDialog");
                        Close();
                    }
                    else
                    {
                        await DialogHost.Show(new MessageDialog("Notice", "You cannot request because you have balance to be paid."), "SecondaryDialog");
                    }
                }
            });
            FormCommand = new Command(async obj =>
            {
                int countOfNull = SubjectList.Count(subject => subject.FirstGrading == null) + 
                                  SubjectList.Count(subject => subject.SecondGrading == null) +
                                  SubjectList.Count(subject => subject.ThirdGrading == null) +
                                  SubjectList.Count(subject => subject.FourthGrading == null);
                if (countOfNull > 0)
                {
                    await DialogHost.Show(new MessageDialog("Notice", "You cannot request because you need to complete report card first."), "SecondaryDialog");
                    return;
                }
                if (isPaid)
                {
                    var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to send a request for this student's FORM 137 to the principal?"), "SecondaryDialog");
                    if ((bool)result! == false)
                    {
                        return;
                    }
                    await Credentials.RegisterStudentAsync(new Notification { User_Id = MainWindow.User!.Id.ToString(), Type = "FORM 137", Student_Id = student.Student.id.ToString(), Status = "Pending", Requested = DateTime.Now.ToShortDateString(), Approved = "Unknown" }, "notification");
                    await DialogHost.Show(new MessageDialog("Notice", "You just sent a request to the principal.", true), "SecondaryDialog");
                    Close();
                }
                else
                {
                    await DialogHost.Show(new MessageDialog("Notice", "You cannot request because you have balance to be paid."), "SecondaryDialog");
                }
            });
            CancelCommand = new Command(obj =>
            {
                //SaveGridAsImage(deezGrid, "output.png");
                //SaveWindowAsImage("ow.png");
                DialogResult = false;
                Close();
            });
            DataContext = this;
        }

        private void LoadAllDataAsync(StudentReport student, Users teacher, Users principal, bool toPrint = false)
        {
            FullName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
            doc_shs.Range.Replace("<LNAME>", student.Student!.LastName, findReplaceOptions);
            doc_shs.Range.Replace("<FNAME>", student.Student!.FirstName, findReplaceOptions);
            doc_shs.Range.Replace("<MNAME>", student.Student!.MiddleName, findReplaceOptions);
            string FullNamee = $"{student.Student!.LastName} {student.Student.FirstName}";
            doc.Range.Replace("<NAME>", FullName, findReplaceOptions);
            Sex = student.Student.Gender;
            doc.Range.Replace("<GENDER>", Sex, findReplaceOptions);
            doc_shs.Range.Replace("<GENDER>", Sex, findReplaceOptions);
            LRN = student.Student.LRN;
            doc.Range.Replace("<LRN>", LRN, findReplaceOptions);
            doc_shs.Range.Replace("<LRN>", LRN, findReplaceOptions);
            Grade = student.Registration!.Grade;
            doc.Range.Replace("<GRADE>", Grade, findReplaceOptions);
            doc_shs.Range.Replace("<GRADE>", Grade, findReplaceOptions);
            doc_shs.Range.Replace("<TRACK>", Grade, findReplaceOptions);
            doc_shs.Range.Replace("<SECTION>", Grade, findReplaceOptions);
            if (teacher != null)
            {
                Adviser = $"{teacher.FirstName} {teacher.LastName}";
                doc.Range.Replace("<ADVISER>", Adviser, findReplaceOptions);
            }
            if (principal != null)
            {
                Principal = $"{principal.FirstName} {principal.LastName}";
            }
            DateTime today = DateTime.Today;
            var BirthDate = Convert.ToDateTime(student.Student!.Birthdate);
            int ageValue = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-ageValue))
            {
                ageValue--;
            }
            Age = ageValue.ToString();
            doc.Range.Replace("<AGE>", Age, findReplaceOptions);
            doc_shs.Range.Replace("<BIRTH>", student.Student!.Birthdate, findReplaceOptions);

            if (student.ReportCard != null)
            {
                if (student.ReportCard.Behavior != null)
                {
                    List<Behavior>? list = JsonConvert.DeserializeObject<List<Behavior>>(student.ReportCard.Behavior);
                    int x = 0;
                    foreach (var item in list!)
                    {
                        x++;
                        BehaviorList.Add(item);
                        doc.Range.Replace("<V" + x.ToString() + "1>", item.FirstGrading == null ? "" : item.FirstGrading, findReplaceOptions);
                        doc.Range.Replace("<V" + x.ToString() + "2>", item.SecondGrading == null ? "" : item.SecondGrading, findReplaceOptions);
                        doc.Range.Replace("<V" + x.ToString() + "3>", item.ThirdGrading == null ? "" : item.ThirdGrading, findReplaceOptions);
                        doc.Range.Replace("<V" + x.ToString() + "4>", item.FourthGrading == null ? "" : item.FourthGrading, findReplaceOptions);

                    }
                }
                if (student.ReportCard.Subjects != null)
                {
                    List<Subject>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subject>>(student.ReportCard.Subjects);
                    int x = 0;
                    foreach (var item in list!)
                    {
                        x++;
                        SubjectList.Add(item);
                        doc.Range.Replace("<SU" + x + ">", item.SubjectName == null ? "" : item.SubjectName, findReplaceOptions);
                        doc_shs.Range.Replace("<SU" + x + ">", item.SubjectName == null ? "" : item.SubjectName, findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "1", item.FirstGrading == null ? "   " : item.FirstGrading.ToString(), findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "2", item.SecondGrading == null ? "   " : item.SecondGrading.ToString(), findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "3", item.ThirdGrading == null ? "   " : item.ThirdGrading.ToString(), findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "4", item.FourthGrading == null ? "   " : item.FourthGrading.ToString(), findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "5", item.FinalRating == null ? "   " : item.FinalRating.ToString(), findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(x).ToString() + "6", item.Remarks == null ? "   " : item.Remarks.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "1", item.FirstGrading == null ? "   " : item.FirstGrading.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "2", item.SecondGrading == null ? "   " : item.SecondGrading.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "3", item.ThirdGrading == null ? "   " : item.ThirdGrading.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "4", item.FourthGrading == null ? "   " : item.FourthGrading.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "5", item.FinalRating == null ? "   " : item.FinalRating.ToString(), findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(x).ToString() + "6", item.Remarks == null ? "   " : item.Remarks.ToString(), findReplaceOptions);

                    }
                    for (int y = x; y <= 13; y++)
                    {
                        doc.Range.Replace("<SU" + y + ">", "", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "1", "   ", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "2", "   ", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "3", "   ", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "4", "   ", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "5", "   ", findReplaceOptions);
                        doc.Range.Replace("" + GetAlphabetLetter(y).ToString() + "6", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("<SU" + y + ">", "", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "1", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "2", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "3", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "4", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "5", "   ", findReplaceOptions);
                        doc_shs.Range.Replace("" + GetAlphabetLetter(y).ToString() + "6", "   ", findReplaceOptions);
                    }
                }
                if (student.ReportCard.Attendance != null)
                {
                    List<Attendance>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Attendance>>(student.ReportCard.Attendance);
                    int x = 3;
                    foreach (var item in list!)
                    {
                        x++;
                        AttendanceList.Add(item);

                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "0>", item.Aug == null ? "" : item.Aug.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "1>", item.Sept == null ? "" : item.Sept.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "2>", item.Oct == null ? "" : item.Oct.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "3>", item.Nov == null ? "" : item.Nov.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "4>", item.Dec == null ? "" : item.Dec.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "5>", item.Jan == null ? "" : item.Jan.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "6>", item.Feb == null ? "" : item.Feb.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "7>", item.Mar == null ? "" : item.Mar.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "8>", item.Apr == null ? "" : item.Apr.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "9>", item.May == null ? "" : item.May.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "10>", item.Jun == null ? "" : item.Jun.ToString(), findReplaceOptions);
                        doc.Range.Replace("<" + GetAlphabetLetter(x) + "11>", item.Total == null ? "" : item.Total.ToString(), findReplaceOptions);
                    }
                }
                if (student.ReportCard.Narrative != null)
                {
                    List<Narrative>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Narrative>>(student.ReportCard.Narrative);
                    int x = 0;
                    foreach (var item in list!)
                    {
                        x++;
                        NarrativeList.Add(item);
                        doc.Range.Replace("<" + x.ToString() + "NARR>", item.Message, findReplaceOptions);

                    }
                }
            }
            saveGenDocc(FullNamee);
            saveGenDocc_shs(FullNamee);
        }
        public void saveGenDocc(string fn)
        {
            MemoryStream stream = new MemoryStream();
            doc.Save(stream, SaveFormat.Pdf);
            byte[] fileBytes = stream.ToArray();
            FileStream fileStream = new FileStream((BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(fn))).Replace("-", "").ToLower()) + ".dtt", FileMode.Create, FileAccess.ReadWrite);
            fileStream.Write(fileBytes, 0, fileBytes.Length);
            fileStream.Flush();
            fileStream.Close();
            stream.Close();
        }
        public void saveGenDocc_shs(string fn)
        {
            MemoryStream stream = new MemoryStream();
            doc_shs.Save(stream, SaveFormat.Pdf);
            byte[] fileBytes = stream.ToArray();
            FileStream fileStream = new FileStream((BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(fn))).Replace("-", "").ToLower()) + ".f137", FileMode.Create, FileAccess.ReadWrite);
            fileStream.Write(fileBytes, 0, fileBytes.Length);
            fileStream.Flush();
            fileStream.Close();
            stream.Close();
        }
        public void saveGenDoc(string fn)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF document (*.pdf)|*.pdf";
            //dialog.FileName = fn + " ReportCard " + DateTime.Now.Date.ToString("MM-dd-yyyy");
            dialog.FileName = (BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(fn))).Replace("-", "").ToLower()) + ".dtt";
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
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand FormCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }

        public string? AverageFirstGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.FirstGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageSecondGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.SecondGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageThirdGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.ThirdGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageFourthGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.FourthGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageFinalRating
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFirstGrading) && !string.IsNullOrWhiteSpace(AverageSecondGrading) && !string.IsNullOrWhiteSpace(AverageThirdGrading) && !string.IsNullOrWhiteSpace(AverageFourthGrading))
                {
                    return $"{(Convert.ToInt16(AverageFirstGrading) + Convert.ToInt16(AverageSecondGrading) + Convert.ToInt16(AverageThirdGrading) + Convert.ToInt16(AverageFourthGrading)) / 4}";
                }
                return string.Empty;
            }
        }
        public string? AverageRemarks
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFinalRating))
                {
                    if (Convert.ToInt16(AverageFinalRating) > 74)
                    {
                        return "PASSED";
                    }
                    else
                    {
                        return "FAILED";
                    }
                }

                return string.Empty;
            }
        }

        public string? AverageRemarksColor
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFinalRating))
                {
                    if (Convert.ToInt16(AverageFinalRating) > 74)
                    {
                        return "LimeGreen";
                    }
                    else
                    {
                        return "#c12d2b";
                    }
                }

                return "#c12d2b";
            }
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        private string? _age;
        public string? Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        private string? _sex;
        public string? Sex
        {
            get { return _sex; }
            set { _sex = value; OnPropertyChanged(nameof(Sex)); }
        }

        private string? _lrn;
        public string? LRN
        {
            get { return _lrn; }
            set { _lrn = value; OnPropertyChanged(nameof(LRN)); }
        }

        private string? _grade;
        public string? Grade
        {
            get { return _grade; }
            set { _grade = value; OnPropertyChanged(nameof(Grade)); }
        }

        private string? _adviser;
        public string? Adviser
        {
            get { return _adviser; }
            set { _adviser = value; OnPropertyChanged(nameof(Adviser)); }
        }

        private string? _principal;
        public string? Principal
        {
            get { return _principal; }
            set { _principal = value; OnPropertyChanged(nameof(Principal)); }
        }

        private string isTeacher;

        public string IsTeacher
        {
            get { return isTeacher; }
            set { isTeacher = value; OnPropertyChanged(nameof(IsTeacher)); }
        }

        private string isTeacherForm;

        public string IsTeacherForm
        {
            get { return isTeacherForm; }
            set { isTeacherForm = value; OnPropertyChanged(nameof(isTeacherForm)); }
        }

        private Visibility saveVisibility = Visibility.Visible;

        public Visibility SaveVisibility
        {
            get { return saveVisibility; }
            set { saveVisibility = value; OnPropertyChanged(nameof(SaveVisibility)); }
        }

        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
