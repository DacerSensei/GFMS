using GFMS.Commands;
using GFMS.Models;
using GFMS.ViewModels;
using GFMS.ViewModels.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using SharpVectors.Dom.Svg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for EditOfficiallyEnrolled.xaml
    /// </summary>
    public partial class EditOfficiallyEnrolled : Window, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private readonly ErrorsViewModel ErrorsViewModel;
        public string StudentID;

        public EditOfficiallyEnrolled(RegisteredStudent student)
        {
            InitializeComponent();
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;
            ClassLevelList = new ObservableCollection<string>();
            ChangeClassList(student.Registration.Level);
            SexList = new ObservableCollection<string>()
            {
                "MALE", "FEMALE"
            };
            FillForm(student);
            SaveCommand = new Command(async obj =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure?"), "SecondaryDialog");
                if ((bool)result! == false)
                {
                    return;
                }
                List<Task> taskList = new List<Task>();
                LoginCredentials credentials = new LoginCredentials();
                Student request = new Student
                {
                    LRN = LRN,
                    LastName = LastName,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    NickName = NickName,
                    Address = Address,
                    BirthPlace = BirthPlace,
                    Gender = Gender,
                    Mobile = MyMobileNumber,
                    Religion = Religion,
                    Birthdate = BirthDate!.Value.Date.ToString("MM/dd/yyyy"),
                    Citizenship = Citizenship,
                    Siblings = NoOfSiblings,
                    OrderFamily = OrderInFamily,
                    Interest = MajorInterest,
                    Health = HealthIssues,
                    Father_name = NameOfFather,
                    Father_work = FatherWork,
                    Father_mobile = FatherMobile,
                    Mother_name = NameOfMother,
                    Mother_work = MotherWork,
                    Mother_mobile = MotherMobile
                };
                NewPreviousSchool previousSchool = new NewPreviousSchool()
                {
                    school_name = NameOfSchool,
                    school_address = PrevSchoolAddress,
                    school_mobile = PrevSchoolMobile,
                    guidance_name = NameOfGuidance,
                    guidance_mobile = GuidanceMobile,
                    principal_name = NameOfPrincipal,
                    principal_mobile = PrincipalMobile,
                    adviser_name = NameOfAdviser,
                    adviser_mobile = AdviserMobile
                };
                string Id = credentials.GetLastInsertedId().ToString();
                taskList.Add(Task.Run(async () =>
                {
                    await credentials.UpdateStudentAsync(previousSchool, new { id = student.PreviousSchool!.id }, "previous_school");
                }));
                taskList.Add(Task.Run(async () =>
                {
                    await credentials.UpdateStudentAsync(request, new { id = student.Student!.id }, "student");
                }));
                await Task.WhenAll(taskList);
                DialogResult = true;
                Close();
            }); 
            CancelCommand = new Command(obj =>
            {
                DialogResult = false;
                Close();
            });
            DataContext = this;
        }

        private void FillForm(RegisteredStudent student)
        {
            SchoolYear = student.Registration!.Year;
            LRN = student.Student!.LRN;
            LastName = student.Student.LastName;
            FirstName = student.Student.FirstName;
            MiddleName = student.Student.MiddleName;
            NickName = student.Student.NickName;
            Address = student.Student.Address;
            BirthPlace = student.Student.BirthPlace;
            MajorInterest = student.Student.Interest;
            NameOfFather = student.Student.Father_name;
            NameOfMother = student.Student.Mother_name;
            Gender = student.Student.Gender;
            MyMobileNumber = student.Student.Mobile;
            Religion = student.Student.Religion;
            Citizenship = student.Student.Citizenship;
            HealthIssues = student.Student.Health;
            NoOfSiblings = student.Student.Siblings;
            OrderInFamily = student.Student.OrderFamily;
            FatherWork = student.Student.Father_work;
            MotherWork = student.Student.Mother_work;
            ClassLevel = student.Registration.Grade;
            FatherMobile = student.Student.Father_mobile;
            MotherMobile = student.Student.Mother_mobile;
            NameOfSchool = student.PreviousSchool!.school_name;
            NameOfGuidance = student.PreviousSchool.guidance_name;
            NameOfPrincipal = student.PreviousSchool.principal_name;
            NameOfAdviser = student.PreviousSchool.adviser_name;
            PrevSchoolAddress = student.PreviousSchool.school_address;
            PrevSchoolMobile = student.PreviousSchool.school_mobile;
            GuidanceMobile = student.PreviousSchool.guidance_mobile;
            PrincipalMobile = student.PreviousSchool.principal_mobile;
            AdviserMobile = student.PreviousSchool.adviser_mobile;
            BirthDate = DateTime.Parse(student.Student.Birthdate!);
            ProfilePicture = StringToImageSource(student.Registration.Pic!);
        }

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get { return _profilePicture; }
            set
            {
                if (_profilePicture == value) return;
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<string> ClassLevelList { get; set; }
        public ObservableCollection<string> SexList { get; set; }

        private void ChangeClassList(string? value)
        {
            ClassLevelList.Clear();
            if (value == null) return;
            switch (value.ToUpper())
            {
                case "PRE SCHOOL":
                    ClassLevelList.Add("TOODLER");
                    ClassLevelList.Add("NURSERY");
                    ClassLevelList.Add("KINDER 1");
                    ClassLevelList.Add("KINDER 2");
                    break;
                case "ELEMENTARY":
                    {
                        ClassLevelList.Add("Grade 1");
                        ClassLevelList.Add("Grade 2");
                        ClassLevelList.Add("Grade 3");
                        ClassLevelList.Add("Grade 4");
                        ClassLevelList.Add("Grade 5");
                        ClassLevelList.Add("Grade 6");
                        break;
                    }
                case "JUNIOR HIGH SCHOOL":
                    {
                        ClassLevelList.Add("Grade 7");
                        ClassLevelList.Add("Grade 8");
                        ClassLevelList.Add("Grade 9");
                        ClassLevelList.Add("Grade 10");
                        break;
                    }
                case "SENIOR HIGH SCHOOL":
                    {
                        ClassLevelList.Add("Grade 11 - ABM");
                        ClassLevelList.Add("Grade 11 - HUMSS");
                        ClassLevelList.Add("Grade 11 - STEM");
                        ClassLevelList.Add("Grade 11 - GA");
                        ClassLevelList.Add("Grade 12 - ABM");
                        ClassLevelList.Add("Grade 12 - HUMMS");
                        ClassLevelList.Add("Grade 12 - STEM");
                        ClassLevelList.Add("Grade 12 - GAS");
                        break;
                    }
                default: break;
            }
        }

        private string? _schoolYear;
        public string? SchoolYear
        {
            get { return _schoolYear; }
            set
            {
                if (value == _schoolYear)
                {
                    return;
                }
                _schoolYear = value;
                OnPropertyChanged(nameof(SchoolYear));
            }
        }

        private string? _lrn;
        public string? LRN
        {
            get { return _lrn; }
            set
            {
                if (value == _lrn)
                {
                    return;
                }
                _lrn = value;
                OnPropertyChanged(nameof(LRN));
            }
        }

        private string? _lastName;
        public string? LastName
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName)
                {
                    return;
                }
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string? _firstName;
        public string? FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName)
                {
                    return;
                }
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string? _middleName;
        public string? MiddleName
        {
            get { return _middleName; }
            set
            {
                if (value == _middleName)
                {
                    return;
                }
                _middleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }

        private string? _nickName;
        public string? NickName
        {
            get { return _nickName; }
            set
            {
                if (value == _nickName)
                {
                    return;
                }
                _nickName = value;
                OnPropertyChanged(nameof(NickName));
            }
        }

        private string? _address;
        public string? Address
        {
            get { return _address; }
            set
            {
                if (value == _address)
                {
                    return;
                }
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string? _birthPlace;
        public string? BirthPlace
        {
            get { return _birthPlace; }
            set
            {
                if (value == _birthPlace)
                {
                    return;
                }
                _birthPlace = value;
                OnPropertyChanged(nameof(BirthPlace));
            }
        }

        private string? _majorInterest;
        public string? MajorInterest
        {
            get { return _majorInterest; }
            set
            {
                if (value == _majorInterest)
                {
                    return;
                }
                _majorInterest = value;
                OnPropertyChanged(nameof(MajorInterest));
            }
        }

        private string? _nameOfFather;
        public string? NameOfFather
        {
            get { return _nameOfFather; }
            set
            {
                if (value == _nameOfFather)
                {
                    return;
                }
                _nameOfFather = value;
                OnPropertyChanged(nameof(NameOfFather));
            }
        }

        private string? _nameOfMother;
        public string? NameOfMother
        {
            get { return _nameOfMother; }
            set
            {
                if (value == _nameOfMother)
                {
                    return;
                }
                _nameOfMother = value;
                OnPropertyChanged(nameof(NameOfMother));
            }
        }

        private string? _age;
        public string? Age
        {
            get
            {
                if (BirthDate != null)
                {
                    DateTime today = DateTime.Today;
                    int ageValue = today.Year - BirthDate.Value.Year;
                    if (BirthDate.Value.Date > today.AddYears(-ageValue))
                    {
                        ageValue--;
                    }
                    _age = ageValue.ToString();
                }
                else
                {
                    _age = string.Empty;
                }
                return _age;
            }
            set
            {
                if (value == _age)
                {
                    return;
                }
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string? _gender;
        public string? Gender
        {
            get { return _gender; }
            set
            {
                if (value == _gender)
                {
                    return;
                }
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string? _myMobileNumber;
        public string? MyMobileNumber
        {
            get { return _myMobileNumber; }
            set
            {
                if (value == _myMobileNumber)
                {
                    return;
                }
                _myMobileNumber = value;
                OnPropertyChanged(nameof(MyMobileNumber));
            }
        }

        private string? _religion;
        public string? Religion
        {
            get { return _religion; }
            set
            {
                if (value == _religion)
                {
                    return;
                }
                _religion = value;
                OnPropertyChanged(nameof(Religion));
            }
        }

        private DateTime? _birthDate = DateTime.Now;
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                if (value == _birthDate)
                {
                    return;
                }
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(Age));
            }
        }

        private string? _citizenship;
        public string? Citizenship
        {
            get { return _citizenship; }
            set
            {
                if (value == _citizenship)
                {
                    return;
                }
                _citizenship = value;
                OnPropertyChanged(nameof(Citizenship));
            }
        }

        private string? _healthIssues;
        public string? HealthIssues
        {
            get { return _healthIssues; }
            set
            {
                if (value == _healthIssues)
                {
                    return;
                }
                _healthIssues = value;
                OnPropertyChanged(nameof(HealthIssues));
            }
        }

        private string? _noOfSiblings;
        public string? NoOfSiblings
        {
            get { return _noOfSiblings; }
            set
            {
                if (value == _noOfSiblings)
                {
                    return;
                }
                _noOfSiblings = value;
                OnPropertyChanged(nameof(NoOfSiblings));
            }
        }

        private string? _orderInFamily;
        public string? OrderInFamily
        {
            get { return _orderInFamily; }
            set
            {
                if (value == _orderInFamily)
                {
                    return;
                }
                _orderInFamily = value;
                OnPropertyChanged(nameof(OrderInFamily));
            }
        }

        private string? _fatherWork;
        public string? FatherWork
        {
            get { return _fatherWork; }
            set
            {
                if (value == _fatherWork)
                {
                    return;
                }
                _fatherWork = value;
                OnPropertyChanged(nameof(FatherWork));
            }
        }

        private string? _motherWork;
        public string? MotherWork
        {
            get { return _motherWork; }
            set
            {
                if (value == _motherWork)
                {
                    return;
                }
                _motherWork = value;
                OnPropertyChanged(nameof(MotherWork));
            }
        }

        private string? _classLevel;
        public string? ClassLevel
        {
            get { return _classLevel; }
            set
            {
                if (value == _classLevel)
                {
                    return;
                }
                _classLevel = value;
                OnPropertyChanged(nameof(ClassLevel));
            }
        }

        private string? _fatherMobile;
        public string? FatherMobile
        {
            get { return _fatherMobile; }
            set
            {
                if (value == _fatherMobile)
                {
                    return;
                }
                _fatherMobile = value;
                OnPropertyChanged(nameof(FatherMobile));
            }
        }

        private string? _motherMobile;
        public string? MotherMobile
        {
            get { return _motherMobile; }
            set
            {
                if (value == _motherMobile)
                {
                    return;
                }
                _motherMobile = value;
                OnPropertyChanged(nameof(MotherMobile));
            }
        }

        // This part is the previous School
        private string? _nameOfSchool;
        public string? NameOfSchool
        {
            get { return _nameOfSchool; }
            set
            {
                if (value == _nameOfSchool)
                {
                    return;
                }
                _nameOfSchool = value;
                OnPropertyChanged(nameof(NameOfSchool));
            }
        }

        private string? _nameOfGuidance;
        public string? NameOfGuidance
        {
            get { return _nameOfGuidance; }
            set
            {
                if (value == _nameOfGuidance)
                {
                    return;
                }
                _nameOfGuidance = value;
                OnPropertyChanged(nameof(NameOfGuidance));
            }
        }

        private string? _nameOfPrincipal;
        public string? NameOfPrincipal
        {
            get { return _nameOfPrincipal; }
            set
            {
                if (value == _nameOfPrincipal)
                {
                    return;
                }
                _nameOfPrincipal = value;
                OnPropertyChanged(nameof(NameOfPrincipal));
            }
        }

        private string? _nameOfAdviser;
        public string? NameOfAdviser
        {
            get { return _nameOfAdviser; }
            set
            {
                if (value == _nameOfAdviser)
                {
                    return;
                }
                _nameOfAdviser = value;
                OnPropertyChanged(nameof(NameOfAdviser));
            }
        }

        private string? _prevSchoolAddress;
        public string? PrevSchoolAddress
        {
            get { return _prevSchoolAddress; }
            set
            {
                if (value == _prevSchoolAddress)
                {
                    return;
                }
                _prevSchoolAddress = value;
                OnPropertyChanged(nameof(PrevSchoolAddress));
            }
        }

        private string? _prevSchoolMobile;
        public string? PrevSchoolMobile
        {
            get { return _prevSchoolMobile; }
            set
            {
                if (value == _prevSchoolMobile)
                {
                    return;
                }
                _prevSchoolMobile = value;
                OnPropertyChanged(nameof(PrevSchoolMobile));
            }
        }

        private string? _guidanceMobile;
        public string? GuidanceMobile
        {
            get { return _guidanceMobile; }
            set
            {
                if (value == _guidanceMobile)
                {
                    return;
                }
                _guidanceMobile = value;
                OnPropertyChanged(nameof(GuidanceMobile));
            }
        }

        private string? _principalMobile;
        public string? PrincipalMobile
        {
            get { return _principalMobile; }
            set
            {
                if (value == _principalMobile)
                {
                    return;
                }
                _principalMobile = value;
                OnPropertyChanged(nameof(PrincipalMobile));
            }
        }

        private string? _adviserMobile;
        public string? AdviserMobile
        {
            get { return _adviserMobile; }
            set
            {
                if (value == _adviserMobile)
                {
                    return;
                }
                _adviserMobile = value;
                OnPropertyChanged(nameof(AdviserMobile));
            }
        }

        public bool CanCreate => !HasErrors;

        public bool HasErrors => ErrorsViewModel.HasErrors;

        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsViewModel.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private ImageSource StringToImageSource(string imageString)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(imageString);

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
            catch (FormatException ex)
            {
                Console.WriteLine("Error converting base64 string: " + ex.Message);
                return null;
            }
        }

        private class NewPreviousSchool
        {
            public string? school_name { get; set; }
            public string? school_address { get; set; }
            public string? school_mobile { get; set; }
            public string? guidance_name { get; set; }
            public string? guidance_mobile { get; set; }
            public string? principal_name { get; set; }
            public string? principal_mobile { get; set; }
            public string? adviser_name { get; set; }
            public string? adviser_mobile { get; set; }
        }
    }
}
