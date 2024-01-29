using GFMS.Commands;
using GFMS.Models;
using GFMS.Services;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for RecieptPayment.xaml
    /// </summary>
    public partial class RecieptPayment : Window, INotifyPropertyChanged
    {
        private readonly LoginCredentials Credentials = new LoginCredentials();
        int revealView = 0;
        private StudentAccounting student;
        public RecieptPayment(StudentAccounting student)
        {
            InitializeComponent();
            this.student = student;
            revealView = 0;
            OtherFeeVisOne = "Hidden";
            OtherFeeVisTwo = "Hidden";
            AddFeeText = "Add FEE";
            LoadedCommand = new Command(obj =>
            {

            });

            AddFeeCommand = new Command(obj =>
            {
                if (revealView == 0)
                {
                    OtherFeeVisOne = "Visible";
                    OtherFeeVisTwo = "Hidden";
                    OtherFeeOne = "";
                    DscFeeOne = "";
                    AddFeeText = "Add FEE";
                    revealView++;
                }
                else if (revealView == 1)
                {
                    OtherFeeVisOne = "Visible";
                    OtherFeeVisTwo = "Visible";
                    OtherFeeTwo = "";
                    DscFeeTwo = "";
                    AddFeeText = "Remove all addtl. FEES";
                    revealView++;
                }
                else
                {
                    OtherFeeVisOne = "Hidden";
                    OtherFeeVisTwo = "Hidden";
                    OtherFeeOne = "";
                    DscFeeOne = "";
                    OtherFeeTwo = "";
                    DscFeeTwo = "";
                    AddFeeText = "Add FEE";
                    revealView = 0;
                }
            });
            LoadAllDataAsync(student);
            PayCommand = new Command(async obj =>
            {
                if (string.IsNullOrEmpty(TotalAmount))
                {
                    var errorResult = await DialogHost.Show(new MessageDialog("Notice", "You cannot pay with empty data"), "SecondaryDialog");
                    return;
                }
                if (string.IsNullOrEmpty(Payment))
                {
                    var errorResult = await DialogHost.Show(new MessageDialog("Notice", "Your payment is empty"), "SecondaryDialog");
                    return;
                }
                if (Convert.ToDecimal(Payment) > Convert.ToDecimal(TotalAmount))
                {
                    var errorResult = await DialogHost.Show(new MessageDialog("Notice", "Your payment is too high"), "SecondaryDialog");
                    return;
                }
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to pay?"), "SecondaryDialog");
                if ((bool)result! == false)
                {
                    return;
                }

                TuitionDetails tuitionDetails = new TuitionDetails()
                {
                    Balance = Balance,
                    Books = Books,
                    DiscountedTuition = Discounted,
                    DiscountType = SelectedDiscount ?? "",
                    Inclusion = Inclusion,
                    ModeOfPayment = SelectedModeOfPayment,
                    OtherFees = OtherFees,
                    Payment = Payment,
                    RegistrationFee = RegistrationFee,
                    TotalAmount = TotalAmount,
                    Uniform = Uniform,
                    TuitionFee = TuitionFee,
                    TotalTuitionFee = TotalTuitionFee,
                    Description = Description,
                    AddFeeOne = OtherFeeOne,
                    AddFeeTwo = OtherFeeTwo,
                    AddFeeDscOne = DscFeeOne,
                    AddFeeDscTwo = DscFeeTwo,
                    Date = DateTime.Now.ToShortDateString(),
                    IsPartial = IsPartial.ToString()
                };
                Accounting accounting = new Accounting()
                {
                    Registration_Id = student.Registration!.Id.ToString(),
                    Payment = JsonConvert.SerializeObject(tuitionDetails),
                    Created_At = DateTime.Now.ToShortDateString()
                };
                await Credentials.RegisterStudentAsync(accounting, "accounting");
                GeneratePDF.GeneratePaymentReciept(tuitionDetails, student);
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
        private void LoadAllDataAsync(StudentAccounting student)
        {
            DiscountList = new ObservableCollection<string>()
                {
                    "FULL PAID TUITION", "SIBLING DISCOUNT", "VALEDICTORIAN", "SALUTATORIAN"
                };
            ModeOfPaymentList = new ObservableCollection<string>()
                {
                    "CASH ON HAND", "GCASH"
                };
            TuitionFee = "39500";
            SelectedModeOfPayment = ModeOfPaymentList.FirstOrDefault() ?? string.Empty;
            CompleteName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
            GradeLevel = student.Registration.Grade ?? "";
            FinanceName = $"{MainWindow.User!.FirstName} {MainWindow.User.LastName}";
            HistoryList = new ObservableCollection<TuitionDetails>(student.TuitionDetailsList!);
            if (HistoryList != null && HistoryList.Count > 0)
            {
                var lastPayment = HistoryList.LastOrDefault();
                if (lastPayment != null)
                {
                    SelectedDiscount = !string.IsNullOrWhiteSpace(lastPayment.DiscountType) ? lastPayment.DiscountType : "";
                    RegistrationFee = !string.IsNullOrWhiteSpace(lastPayment.RegistrationFee) ? lastPayment.RegistrationFee : "";
                    Books = !string.IsNullOrWhiteSpace(lastPayment.Books) ? lastPayment.Books : "";
                    Uniform = !string.IsNullOrWhiteSpace(lastPayment.Uniform) ? lastPayment.Uniform : "";
                    OtherFees = !string.IsNullOrWhiteSpace(lastPayment.OtherFees) ? lastPayment.OtherFees : "";
                }
            }
            var firstPayment = student.TuitionDetailsList.FirstOrDefault();
            DateTime firstPaymentDate;
            if (firstPayment != null)
            {
                firstPaymentDate = Convert.ToDateTime(firstPayment.Date);
                IsPartial = Convert.ToBoolean(firstPayment.IsPartial);
            }
            else
            {
                firstPaymentDate = Convert.ToDateTime(Date);
            }
            for (int i = 0; i < 10; i++)
            {
                PartialList.Add($"{firstPaymentDate.AddMonths(i).ToShortDateString()} = {(Convert.ToDecimal(TotalAmount) + student.DecimalPaid) / 10}");
            }

        }

        public ObservableCollection<TuitionDetails> HistoryList { get; set; }

        public ObservableCollection<string> DiscountList { get; set; } = new();
        public ObservableCollection<string> PartialList { get; set; } = new();
        public ObservableCollection<string> ModeOfPaymentList { get; set; } = new();

        public ICommand PayCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }
        public ICommand AddFeeCommand { get; }

        private string completeName;

        public string CompleteName
        {
            get { return completeName; }
            set { completeName = value; OnPropertyChanged(nameof(CompleteName)); }
        }

        private string financeName;

        public string FinanceName
        {
            get { return financeName; }
            set { financeName = value; OnPropertyChanged(nameof(FinanceName)); }
        }

        private string gradeLevel;

        public string GradeLevel
        {
            get { return gradeLevel; }
            set { gradeLevel = value; OnPropertyChanged(nameof(GradeLevel)); }
        }

        private string date = DateTime.Now.Date.ToShortDateString();

        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }

        private string tuitionFee = string.Empty;

        public string TuitionFee
        {
            get { return tuitionFee; }
            set
            {
                tuitionFee = value;
                OnPropertyChanged(nameof(TotalTuitionFee));
                OnPropertyChanged(nameof(Discounted));
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(TuitionFee));
            }
        }

        public string TotalTuitionFee
        {
            get
            {
                if (string.IsNullOrEmpty(TuitionFee))
                {
                    return string.Empty;
                }
                if (!string.IsNullOrEmpty(SelectedDiscount))
                {
                    decimal tuition = Convert.ToDecimal(TuitionFee);
                    if (SelectedDiscount.ToLower() == "FULL PAID TUITION".ToLower())
                    {
                        decimal discountPercentage = 0.05m;
                        decimal discountAmount = tuition - (tuition * discountPercentage);
                        return discountAmount.ToString("N2");
                    }
                    else if (SelectedDiscount.ToLower() == "SIBLING DISCOUNT".ToLower())
                    {
                        decimal discountPercentage = 0.1m;
                        decimal discountAmount = tuition - (tuition * discountPercentage);
                        return discountAmount.ToString("N2");
                    }
                    else if (SelectedDiscount.ToLower() == "VALEDICTORIAN".ToLower())
                    {
                        decimal discountPercentage = 1m;
                        decimal discountAmount = tuition - (tuition * discountPercentage);
                        return discountAmount.ToString("N2");
                    }
                    else if (SelectedDiscount.ToLower() == "SALUTATORIAN".ToLower())
                    {
                        decimal discountPercentage = 0.5m;
                        decimal discountAmount = tuition - (tuition * discountPercentage);
                        return discountAmount.ToString("N2");
                    }
                }
                return TuitionFee;
            }
        }

        private string selectedDiscount;

        public string SelectedDiscount
        {
            get { return selectedDiscount; }
            set
            {
                selectedDiscount = value;
                OnPropertyChanged(nameof(TotalTuitionFee));
                OnPropertyChanged(nameof(Discounted));
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SelectedDiscount));
            }
        }

        public string Discounted
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectedDiscount))
                {
                    if (!string.IsNullOrEmpty(TuitionFee))
                    {
                        decimal tuition = Convert.ToDecimal(TuitionFee);
                        if (SelectedDiscount.ToLower() == "FULL PAID TUITION".ToLower())
                        {
                            decimal discountPercentage = 0.05m;
                            decimal discountAmount = tuition * discountPercentage;
                            return discountAmount.ToString("N2");
                        }
                        else if (SelectedDiscount.ToLower() == "SIBLING DISCOUNT".ToLower())
                        {
                            decimal discountPercentage = 0.1m;
                            decimal discountAmount = tuition * discountPercentage;
                            return discountAmount.ToString("N2");
                        }
                        else if (SelectedDiscount.ToLower() == "VALEDICTORIAN".ToLower())
                        {
                            decimal discountPercentage = 1m;
                            decimal discountAmount = tuition * discountPercentage;
                            return discountAmount.ToString("N2");
                        }
                        else if (SelectedDiscount.ToLower() == "SALUTATORIAN".ToLower())
                        {
                            decimal discountPercentage = 0.5m;
                            decimal discountAmount = tuition * discountPercentage;
                            return discountAmount.ToString("N2");
                        }
                    }
                }
                return string.Empty;
            }
        }

        private string otherFees = string.Empty;

        public string OtherFees
        {
            get { return otherFees; }
            set
            {
                otherFees = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(OtherFees));
            }
        }

        private string otherFeeOne = string.Empty;

        public string OtherFeeOne
        {
            get { return otherFeeOne; }
            set
            {
                otherFeeOne = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(OtherFees));
            }
        }

        private string otherFeeTwo = string.Empty;

        public string OtherFeeTwo
        {
            get { return otherFeeTwo; }
            set
            {
                otherFeeTwo = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(OtherFees));
            }
        }

        private string dscFeeOne = string.Empty;

        public string DscFeeOne
        {
            get { return dscFeeOne; }
            set { dscFeeOne = value; OnPropertyChanged(nameof(DscFeeOne)); }
        }

        private string dscFeeTwo = string.Empty;

        public string DscFeeTwo
        {
            get { return dscFeeTwo; }
            set { dscFeeTwo = value; OnPropertyChanged(nameof(DscFeeTwo)); }
        }

        private string otherFeeVisOne = string.Empty;

        public string OtherFeeVisOne
        {
            get { return otherFeeVisOne; }
            set { otherFeeVisOne = value; OnPropertyChanged(nameof(OtherFeeVisOne)); }
        }

        private string otherFeeVisTwo = string.Empty;

        public string OtherFeeVisTwo
        {
            get { return otherFeeVisTwo; }
            set { otherFeeVisTwo = value; OnPropertyChanged(nameof(OtherFeeVisTwo)); }
        }

        private string addFeeText = string.Empty;
        public string AddFeeText
        {
            get { return addFeeText; }
            set { addFeeText = value; OnPropertyChanged(nameof(AddFeeText)); }
        }

        private string inclusion = string.Empty;

        public string Inclusion
        {
            get { return inclusion; }
            set { inclusion = value; OnPropertyChanged(nameof(Inclusion)); }
        }

        private string selectedModeOfPayment;

        public string SelectedModeOfPayment
        {
            get { return selectedModeOfPayment; }
            set { selectedModeOfPayment = value; OnPropertyChanged(nameof(SelectedModeOfPayment)); }
        }

        private bool isPartial = false;

        public bool IsPartial
        {
            get { return isPartial; }
            set
            {
                isPartial = value;
                if (value == true)
                {
                    PartialList.Clear();
                    var firstPayment = student.TuitionDetailsList.FirstOrDefault();
                    DateTime firstPaymentDate;
                    if (firstPayment != null)
                    {
                        firstPaymentDate = Convert.ToDateTime(firstPayment.Date);
                    }
                    else
                    {
                        firstPaymentDate = Convert.ToDateTime(Date);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        PartialList.Add($"{firstPaymentDate.AddMonths(i).ToShortDateString()} = {(Convert.ToDecimal(TotalAmount) + student.DecimalPaid) / 10}");
                    }
                }
                OnPropertyChanged(nameof(IsPartial));
                OnPropertyChanged(nameof(IsDueVisible));
            }
        }

        public string IsDueVisible
        {
            get
            {
                if (IsPartial == true)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }

        private string registrationFee = string.Empty;

        public string RegistrationFee
        {
            get { return registrationFee; }
            set
            {
                registrationFee = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(RegistrationFee));
            }
        }

        private string books = string.Empty;

        public string Books
        {
            get { return books; }
            set
            {
                books = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(Books));
            }
        }

        private string uniform = string.Empty;

        public string Uniform
        {
            get { return uniform; }
            set
            {
                uniform = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(CanPay));
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(Uniform));
            }
        }

        public string TotalAmount
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectedDiscount))
                {
                    if (SelectedDiscount.ToLower() == "VALEDICTORIAN".ToLower())
                    {
                        return new decimal(0).ToString("N2");
                    }
                }
                decimal totalTuitionFee = 0m;
                decimal registrationFee = 0m;
                decimal booksFee = 0m;
                decimal uniformFee = 0m;
                decimal otherFees = 0m;
                decimal feeOne = 0m;
                decimal feeTwo = 0m;
                if (!string.IsNullOrEmpty(TotalTuitionFee))
                {
                    totalTuitionFee = Convert.ToDecimal(TotalTuitionFee ?? "0");
                }
                if (!string.IsNullOrEmpty(RegistrationFee))
                {
                    registrationFee = Convert.ToDecimal(RegistrationFee ?? "0");
                }
                if (!string.IsNullOrEmpty(Books))
                {
                    booksFee = Convert.ToDecimal(Books ?? "0");
                }
                if (!string.IsNullOrEmpty(Uniform))
                {
                    uniformFee = Convert.ToDecimal(Uniform ?? "0");
                }
                if (!string.IsNullOrEmpty(OtherFees))
                {
                    otherFees = Convert.ToDecimal(OtherFees ?? "0");
                }
                if (!string.IsNullOrEmpty(OtherFeeOne))
                {
                    feeOne = Convert.ToDecimal(OtherFeeOne ?? "0");
                }
                if (!string.IsNullOrEmpty(OtherFeeTwo))
                {
                    feeTwo = Convert.ToDecimal(OtherFeeTwo ?? "0");
                }
                decimal total = totalTuitionFee + registrationFee + booksFee + uniformFee + otherFees + feeOne + feeTwo - student.DecimalPaid;
                return total > 0 ? total.ToString("N2") : string.Empty;
            }
        }

        public string Balance
        {
            get
            {
                decimal payment = 0m;
                decimal totalAmount = 0m;
                if (!string.IsNullOrEmpty(Payment))
                {
                    payment = Convert.ToDecimal(Payment ?? "0");
                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                }
                else
                {
                    return string.Empty;
                }
                decimal total = payment > totalAmount ? 0 : totalAmount - payment;
                return total.ToString("N2");
            }
        }

        private string payment = string.Empty;

        public string Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(Payment));
            }
        }

        public bool CanPay
        {
            get
            {
                if (string.IsNullOrEmpty(TotalAmount))
                {
                    Payment = string.Empty;
                    return false;
                }
                return true;
            }
        }

        public string Description
        {
            get
            {
                StringBuilder description = new StringBuilder("You pay in ");

                AppendDescription(description, "Tuition", !string.IsNullOrEmpty(TotalTuitionFee));
                AppendDescription(description, "Registration Fee", !string.IsNullOrEmpty(RegistrationFee));
                AppendDescription(description, "Books", !string.IsNullOrEmpty(Books));
                AppendDescription(description, "Uniform", !string.IsNullOrEmpty(Uniform));
                AppendDescription(description, "Other Fees", !string.IsNullOrEmpty(OtherFees));

                return description.ToString();
            }
        }

        private void AppendDescription(StringBuilder description, string descriptionName, bool condition)
        {
            if (condition)
            {
                description.Append($"{descriptionName}, ");
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
