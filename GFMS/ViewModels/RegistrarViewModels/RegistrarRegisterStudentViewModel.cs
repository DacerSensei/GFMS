using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.ViewModels.Modals;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarRegisterStudentViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel ErrorsViewModel;
        public RegistrarRegisterStudentViewModel()
        {
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;
            GradeLevel = new ObservableCollection<string>()
            {
                "PRE SCHOOL", "ELEMENTARY", "JUNIOR HIGH SCHOOL", "SENIOR HIGH SCHOOL"
            };
            ClassLevelList = new ObservableCollection<string>();
            SexList = new ObservableCollection<string>()
            {
                "MALE", "FEMALE"
            };
            RequirementList = new ObservableCollection<Requirement>();
            DeleteCommand = new Command(obj =>
            {
                Requirement? requirement = obj as Requirement;
                if (requirement != null)
                {
                    RequirementList.Remove(requirement);
                }
            });
            AddCommand = new DialogCommand(async obj =>
            {
                RequirementDialog Dialog = new RequirementDialog()
                {
                    DataContext = new RequirementDialogViewModel()
                };
                var result = await DialogHost.Show(Dialog, "RootDialog");
                if (result != null)
                {
                    Requirement? requirement = result as Requirement;
                    if (requirement != null)
                    {
                        if (!RequirementList.Any(r => r.Description == requirement.Description))
                        {
                            RequirementList.Add(requirement);
                        }
                    }
                }
            });
            ChangePictureCommand = new Command(obj =>
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Choose Picture(*.jpg;*.png)|*.jpg;*png";
                if (fileDialog.ShowDialog() == true)
                {
                    ProfilePicture = new BitmapImage(new Uri(fileDialog.FileName));
                }
            });
            RegisterCommand = new Command(async obj =>
            {
                ValidateGradeLevel();
                ValidateAll();
                if (ErrorsViewModel.HasErrors)
                {
                    return;
                }
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
                LoginCredentials credentials = new LoginCredentials();
                if (await credentials.RegisterStudentAsync(request, "student"))
                {
                    List<Task> taskList = new List<Task>();
                    string Id = credentials.GetLastInsertedId().ToString();
                    PreviousSchool school = new PreviousSchool()
                    {
                        student_id = Id,
                        school_name = NameOfSchool,
                        school_address = PrevSchoolAddress,
                        school_mobile = PrevSchoolMobile,
                        guidance_name = NameOfGuidance,
                        guidance_mobile = GuidanceMobile,
                        principal_name = NameOfPrincipal,
                        principal_mobile = PrincipalMobile,
                        adviser_name = NameOfAdviser,
                        adviser_mobile = AdviserMobile,
                    };
                    taskList.Add(Task.Run(async () =>
                    {
                        await credentials.RegisterStudentAsync(school, "previous_school");
                    }));

                    if (ProfilePicture is BitmapSource bitmapSource)
                    {
                        // Create an encoder to encode the BitmapSource to a memory stream
                        BitmapEncoder encoder = new PngBitmapEncoder(); // You can choose a different format if needed

                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            byte[] img = ms.ToArray();
                            Registration registration = new Registration()
                            {
                                Student_Id = Id,
                                Year = SchoolYear,
                                Registration_Date = DateTime.Now.ToString("MM/dd/yyyy"),
                                Grade = ClassLevel,
                                Pic = img.ToString(),
                                Status = "0"
                            };
                            taskList.Add(Task.Run(async () =>
                            {
                                await credentials.RegisterStudentAsync(registration, "registration");
                            }));
                        }
                    }
                    foreach (var item in RequirementList)
                    {
                        item.Student_ID = Convert.ToInt32(Id);
                        taskList.Add(Task.Run(async () =>
                        {
                            await credentials.RegisterStudentAsync(item, "student_requirements");
                        }));
                    }
                    await Task.WhenAll(taskList);
                    MessageDialog Dialog = new MessageDialog("Notice", "Student Registered Successfully");
                    await DialogHost.Show(Dialog, "RootDialog");
                    ClearForm();
                }
                else
                {
                    Debug.WriteLine("Student Failed");
                }

            });
        }

        private void ClearForm()
        {
            SelectedGradeLevel = null;
            SchoolYear = string.Empty;
            LRN = string.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            NickName = string.Empty;
            Address = string.Empty;
            BirthPlace = string.Empty;
            MajorInterest = string.Empty;
            NameOfFather = string.Empty;
            NameOfMother = string.Empty;
            Gender = null;
            MyMobileNumber = string.Empty;
            Religion = string.Empty;
            Citizenship = string.Empty;
            HealthIssues = string.Empty;
            NoOfSiblings = string.Empty;
            OrderInFamily = string.Empty;
            FatherWork = string.Empty;
            MotherWork = string.Empty;
            ClassLevel = string.Empty;
            FatherMobile = string.Empty;
            MotherMobile = string.Empty;
            NameOfSchool = string.Empty;
            NameOfGuidance = string.Empty;
            NameOfPrincipal = string.Empty;
            NameOfAdviser = string.Empty;
            PrevSchoolAddress = string.Empty;
            PrevSchoolMobile = string.Empty;
            GuidanceMobile = string.Empty;
            PrincipalMobile = string.Empty;
            AdviserMobile = string.Empty;
            BirthDate = DateTime.Now;
            ProfilePicture = new BitmapImage(new Uri("pack://application:,,,/GFMS;component/Assets/Logo.png"));
            ErrorsViewModel.ClearErrors(nameof(SelectedGradeLevel));
            ErrorsViewModel.ClearErrors(nameof(Gender));
            RequirementList!.Clear();
        }

        private ImageSource _profilePicture = new BitmapImage(new Uri("pack://application:,,,/GFMS;component/Assets/Logo.png"));
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


        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ChangePictureCommand { get; }

        public static ObservableCollection<Requirement>? RequirementList { get; set; }
        public ObservableCollection<string> GradeLevel { get; set; }
        public ObservableCollection<string> ClassLevelList { get; set; }
        public ObservableCollection<string> SexList { get; set; }

        private void ValidateAll()
        {
            if (string.IsNullOrWhiteSpace(SchoolYear))
            {
                ErrorsViewModel.AddError(nameof(SchoolYear), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(SchoolYear));
            }

            if (string.IsNullOrWhiteSpace(LRN))
            {
                ErrorsViewModel.AddError(nameof(LRN), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(LRN));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorsViewModel.AddError(nameof(LastName), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(LastName));
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorsViewModel.AddError(nameof(FirstName), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(FirstName));
            }

            if (string.IsNullOrWhiteSpace(MiddleName))
            {
                ErrorsViewModel.AddError(nameof(MiddleName), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(MiddleName));
            }

            if (string.IsNullOrWhiteSpace(NickName))
            {
                ErrorsViewModel.AddError(nameof(NickName), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NickName));
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                ErrorsViewModel.AddError(nameof(Address), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Address));
            }

            if (string.IsNullOrWhiteSpace(BirthPlace))
            {
                ErrorsViewModel.AddError(nameof(BirthPlace), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(BirthPlace));
            }

            if (string.IsNullOrWhiteSpace(MajorInterest))
            {
                ErrorsViewModel.AddError(nameof(MajorInterest), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(MajorInterest));
            }

            if (string.IsNullOrWhiteSpace(NameOfFather))
            {
                ErrorsViewModel.AddError(nameof(NameOfFather), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfFather));
            }

            if (string.IsNullOrWhiteSpace(NameOfMother))
            {
                ErrorsViewModel.AddError(nameof(NameOfMother), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfMother));
            }

            if (string.IsNullOrWhiteSpace(Age))
            {
                ErrorsViewModel.AddError(nameof(Age), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Age));
            }

            if (string.IsNullOrWhiteSpace(Gender))
            {
                ErrorsViewModel.AddError(nameof(Gender), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Gender));
            }

            if (string.IsNullOrWhiteSpace(MyMobileNumber))
            {
                ErrorsViewModel.AddError(nameof(MyMobileNumber), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(MyMobileNumber));
            }

            if (string.IsNullOrWhiteSpace(Religion))
            {
                ErrorsViewModel.AddError(nameof(Religion), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Religion));
            }

            if (BirthDate == null)
            {
                ErrorsViewModel.AddError(nameof(BirthDate), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(BirthDate));
            }

            if (string.IsNullOrWhiteSpace(Citizenship))
            {
                ErrorsViewModel.AddError(nameof(Citizenship), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Citizenship));
            }

            if (string.IsNullOrWhiteSpace(HealthIssues))
            {
                ErrorsViewModel.AddError(nameof(HealthIssues), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(HealthIssues));
            }

            if (string.IsNullOrWhiteSpace(NoOfSiblings))
            {
                ErrorsViewModel.AddError(nameof(NoOfSiblings), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NoOfSiblings));
            }

            if (string.IsNullOrWhiteSpace(OrderInFamily))
            {
                ErrorsViewModel.AddError(nameof(OrderInFamily), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(OrderInFamily));
            }

            if (string.IsNullOrWhiteSpace(FatherWork))
            {
                ErrorsViewModel.AddError(nameof(FatherWork), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(FatherWork));
            }

            if (string.IsNullOrWhiteSpace(MotherWork))
            {
                ErrorsViewModel.AddError(nameof(MotherWork), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(MotherWork));
            }

            if (string.IsNullOrWhiteSpace(ClassLevel))
            {
                ErrorsViewModel.AddError(nameof(ClassLevel), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(ClassLevel));
            }

            if (string.IsNullOrWhiteSpace(FatherMobile))
            {
                ErrorsViewModel.AddError(nameof(FatherMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(FatherMobile));
            }

            if (string.IsNullOrWhiteSpace(MotherMobile))
            {
                ErrorsViewModel.AddError(nameof(MotherMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(MotherMobile));
            }

            if (string.IsNullOrWhiteSpace(NameOfSchool))
            {
                ErrorsViewModel.AddError(nameof(NameOfSchool), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfSchool));
            }

            if (string.IsNullOrWhiteSpace(NameOfGuidance))
            {
                ErrorsViewModel.AddError(nameof(NameOfGuidance), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfGuidance));
            }

            if (string.IsNullOrWhiteSpace(NameOfPrincipal))
            {
                ErrorsViewModel.AddError(nameof(NameOfPrincipal), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfPrincipal));
            }

            if (string.IsNullOrWhiteSpace(NameOfAdviser))
            {
                ErrorsViewModel.AddError(nameof(NameOfAdviser), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NameOfAdviser));
            }

            if (string.IsNullOrWhiteSpace(PrevSchoolMobile))
            {
                ErrorsViewModel.AddError(nameof(PrevSchoolMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(PrevSchoolMobile));
            }

            if (string.IsNullOrWhiteSpace(GuidanceMobile))
            {
                ErrorsViewModel.AddError(nameof(GuidanceMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(GuidanceMobile));
            }

            if (string.IsNullOrWhiteSpace(PrincipalMobile))
            {
                ErrorsViewModel.AddError(nameof(PrincipalMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(PrincipalMobile));
            }

            if (string.IsNullOrWhiteSpace(AdviserMobile))
            {
                ErrorsViewModel.AddError(nameof(AdviserMobile), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(AdviserMobile));
            }

            if (string.IsNullOrWhiteSpace(PrevSchoolAddress))
            {
                ErrorsViewModel.AddError(nameof(PrevSchoolAddress), "This field is cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(PrevSchoolAddress));
            }
        }

        private void ValidateGradeLevel()
        {
            if (string.IsNullOrWhiteSpace(SelectedGradeLevel))
            {
                ErrorsViewModel.AddError(nameof(SelectedGradeLevel), "Please select your grade level");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(SelectedGradeLevel));
            }
        }

        private string? _selectedGradeLevel;
        public string? SelectedGradeLevel
        {
            get { return _selectedGradeLevel; }
            set
            {
                if (value == _selectedGradeLevel)
                {
                    return;
                }
                _selectedGradeLevel = value;
                ValidateGradeLevel();
                ChangeClassList(value);
                OnPropertyChanged(nameof(SelectedGradeLevel));
            }
        }

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

        private string? _schoolYear = "2023-2024";
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
    }
}
