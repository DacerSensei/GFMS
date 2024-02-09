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
                    var errorResult = await DialogHost.Show(new MessageDialog("Notice", "Please input exact Amount."), "SecondaryDialog");
                    return;
                }
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to pay?"), "SecondaryDialog");
                if ((bool)result! == false)
                {
                    return;
                }

                TuitionDetails tuitionDetails = new TuitionDetails()
                {
                    Balance = TotalBalance,
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
                    IsPartial = IsPartial.ToString(),
                    // Modification starts here
                    TuitionPayment = TuitionPayment,
                    RegistrationPayment = RegistrationPayment,
                    BooksPayment = BooksPayment,
                    UniformPayment = UniformPayment,
                    OtherFeePayment = OtherFeesPayment,
                    TuitionBalance = TuitionBalance,
                    RegistrationBalance = RegistrationBalance,
                    BooksBalance = BooksBalance,
                    UniformBalance = UniformBalance,
                    OtherFeesBalance = OtherFeesBalance
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
            NotifyBalance();
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
                    IsRegistrationSet = !string.IsNullOrWhiteSpace(lastPayment.RegistrationFee) ? true : false;
                }
            }

            var firstPaymentThatArePartial = student.TuitionDetailsList.FirstOrDefault(student => Convert.ToBoolean(student.IsPartial));
            var firstPaymentIndex = student.TuitionDetailsList.FindIndex(student => Convert.ToBoolean(student.IsPartial));
            TuitionDetails? lastPaymentInThePartial = student.TuitionDetailsList.LastOrDefault();
            if (firstPaymentThatArePartial != null && lastPaymentInThePartial != null)
            {
                IsPartial = Convert.ToBoolean(firstPaymentThatArePartial.IsPartial);
            }

        }

        public ObservableCollection<TuitionDetails> HistoryList { get; set; }

        public ObservableCollection<string> DiscountList { get; set; } = new();
        public ObservableCollection<PartialTuition> PartialList { get; set; } = new();
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
                OnPropertyChanged(nameof(TotalBalance));
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
                OnPropertyChanged(nameof(TotalBalance));
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
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(OtherFeesBalance));
                OnPropertyChanged(nameof(OtherFeesCanPay));
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
                OnPropertyChanged(nameof(TotalBalance));
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
                OnPropertyChanged(nameof(TotalBalance));
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
                    var firstPaymentThatArePartial = student.TuitionDetailsList.FirstOrDefault(student => Convert.ToBoolean(student.IsPartial));
                    var firstPaymentIndex = student.TuitionDetailsList.FindIndex(student => Convert.ToBoolean(student.IsPartial));
                    TuitionDetails? lastPaymentInThePartial = student.TuitionDetailsList.LastOrDefault();
                    decimal totalPaymentBeforePartial = 0;
                    decimal totalPaymentAfterPartial = 0;
                    DateTime firstPaymentDate;
                    if (firstPaymentThatArePartial != null && lastPaymentInThePartial != null)
                    {
                        firstPaymentDate = Convert.ToDateTime(firstPaymentThatArePartial.Date); // Your first date payment of partial
                        List<TuitionDetails> beforePartialPaymentList = new();
                        List<TuitionDetails> afterPartialPaymentList = new();
                        if (firstPaymentIndex > 0)
                        {
                            beforePartialPaymentList = student.TuitionDetailsList.GetRange(0, firstPaymentIndex); // List of before partial Tuition
                            totalPaymentBeforePartial = beforePartialPaymentList.Sum(student => Convert.ToDecimal(student.Payment)); // Total Payments before partial Tuition
                        }
                        if (firstPaymentIndex >= 0 && firstPaymentIndex < student.TuitionDetailsList.Count)
                        {
                            afterPartialPaymentList = student.TuitionDetailsList.Skip(firstPaymentIndex).Where(item => !string.IsNullOrEmpty(item.TuitionPayment)).ToList(); // List of after partial Tuition then sort the payment that have tuition
                            totalPaymentAfterPartial = afterPartialPaymentList.Sum(student => Convert.ToDecimal(student.TuitionPayment)); // Total Payments of partial Tuition
                        }

                        decimal totalPartialTuition = (Convert.ToDecimal(string.IsNullOrWhiteSpace(TotalTuitionFee) ? "0" : TotalTuitionFee)); // Overall total of how much you will pay partial
                        decimal partialTuitionMonthly = totalPartialTuition / 10; // Monthly Partial
                        int totalOfPaymentsInPartial = 0;

                            totalOfPaymentsInPartial = (int)Math.Floor(totalPaymentAfterPartial / partialTuitionMonthly); // Number of how many did you already pay in

                        for (int i = 0; i < 10; i++)
                        {
                            PartialList.Add(new PartialTuition { Dues = $"{firstPaymentDate.AddMonths(i).ToShortDateString()} = {partialTuitionMonthly}", IsPaid = i < totalOfPaymentsInPartial ? true : false }); ;
                        }
                    }
                    else
                    {
                        totalPaymentBeforePartial = student.TuitionDetailsList.Sum(student => Convert.ToDecimal(student.Payment));
                        totalPaymentAfterPartial = student.TuitionDetailsList.Sum(student => Convert.ToDecimal(student.Payment));
                        firstPaymentDate = Convert.ToDateTime(Date);

                        var totalComputations = (Convert.ToDecimal(string.IsNullOrWhiteSpace(TotalTuitionFee) ? "0" : TotalTuitionFee)) / 10;

                        for (int i = 0; i < 10; i++)
                        {
                            PartialList.Add(new PartialTuition { Dues = $"{firstPaymentDate.AddMonths(i).ToShortDateString()} = {totalComputations}", IsPaid = false }); ;
                        }
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
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(RegistrationBalance));
                OnPropertyChanged(nameof(RegistrationCanPay));
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
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(BooksBalance));
                OnPropertyChanged(nameof(BooksCanPay));
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
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(UniformBalance));
                OnPropertyChanged(nameof(UniformCanPay));
                OnPropertyChanged(nameof(Uniform));
            }
        }

        public string TotalAmount
        {
            get
            {
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

        public string TotalBalance
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

        public string Payment
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TuitionPayment) && string.IsNullOrWhiteSpace(RegistrationPayment) && string.IsNullOrWhiteSpace(BooksPayment) &&
                     string.IsNullOrWhiteSpace(UniformPayment) && string.IsNullOrWhiteSpace(OtherFeesPayment))
                {
                    return string.Empty;
                }
                else
                {
                    return (Convert.ToDecimal(!string.IsNullOrWhiteSpace(TuitionPayment) ? TuitionPayment : "0") +
                           Convert.ToDecimal(!string.IsNullOrWhiteSpace(RegistrationPayment) ? RegistrationPayment : "0") +
                           Convert.ToDecimal(!string.IsNullOrWhiteSpace(BooksPayment) ? BooksPayment : "0") +
                           Convert.ToDecimal(!string.IsNullOrWhiteSpace(UniformPayment) ? UniformPayment : "0") +
                           Convert.ToDecimal(!string.IsNullOrWhiteSpace(OtherFeesPayment) ? OtherFeesPayment : "0")).ToString("N2");
                }
            }
        }

        private bool isRegistrationSet = false;
        public bool IsRegistrationSet
        {
            get => isRegistrationSet;
            set
            {
                isRegistrationSet = value;
                OnPropertyChanged(nameof(IsRegistrationSet));
            }
        }

        public bool CanPay
        {
            get
            {
                if (string.IsNullOrEmpty(TotalAmount))
                {
                    return false;
                }
                return true;
            }
        }

        public string Description
        {
            get
            {
                StringBuilder description = new StringBuilder("");

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

        public class PartialTuition
        {
            public bool IsPaid { get; set; }
            public string? Dues { get; set; }
        }

        // Modification for new design
        // Payments

        private string tuitionPayment = string.Empty;
        public string TuitionPayment
        {
            get { return tuitionPayment; }
            set
            {
                tuitionPayment = value;
                OnPropertyChanged(nameof(TuitionBalance));
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(Payment));
                OnPropertyChanged(nameof(TuitionPayment));
            }
        }

        private string registrationPayment = string.Empty;
        public string RegistrationPayment
        {
            get { return registrationPayment; }
            set
            {
                registrationPayment = value;
                OnPropertyChanged(nameof(RegistrationBalance));
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(Payment));
                OnPropertyChanged(nameof(RegistrationPayment));
            }
        }

        private string booksPayment = string.Empty;
        public string BooksPayment
        {
            get { return booksPayment; }
            set
            {
                booksPayment = value;
                OnPropertyChanged(nameof(BooksBalance));
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(Payment));
                OnPropertyChanged(nameof(BooksPayment));
            }
        }

        private string uniformPayment = string.Empty;
        public string UniformPayment
        {
            get { return uniformPayment; }
            set
            {
                uniformPayment = value;
                OnPropertyChanged(nameof(UniformBalance));
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(Payment));
                OnPropertyChanged(nameof(UniformPayment));
            }
        }

        private string otherfeesPayment = string.Empty;
        public string OtherFeesPayment
        {
            get { return otherfeesPayment; }
            set
            {
                otherfeesPayment = value;
                OnPropertyChanged(nameof(OtherFeesBalance));
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged(nameof(Payment));
                OnPropertyChanged(nameof(OtherFeesPayment));
            }
        }

        // Balance

        public string TuitionBalance
        {
            get
            {
                return BalanceComputation(TuitionPayment, TotalTuitionFee, TypeOfPayment.TUITION);
            }
        }

        public string RegistrationBalance
        {
            get
            {
                return BalanceComputation(RegistrationPayment, RegistrationFee, TypeOfPayment.REGISTRATION);
            }
        }

        public string BooksBalance
        {
            get
            {
                return BalanceComputation(BooksPayment, Books, TypeOfPayment.BOOKS);
            }
        }

        public string UniformBalance
        {
            get
            {
                return BalanceComputation(UniformPayment, Uniform, TypeOfPayment.UNIFORM);
            }
        }

        public string OtherFeesBalance
        {
            get
            {
                return BalanceComputation(OtherFeesPayment, OtherFees, TypeOfPayment.OTHER);
            }
        }

        private string BalanceComputation(string Payment, string TotalAmount, TypeOfPayment typeOfPayment)
        {
            decimal payment = 0m;
            decimal totalAmount = 0m;
            decimal total = 0m;
            if (HistoryList != null && HistoryList.Count > 0)
            {
                var lastPayment = HistoryList.LastOrDefault();
                if (lastPayment != null)
                {
                    if (!string.IsNullOrEmpty(Payment))
                    {
                        switch (typeOfPayment)
                        {
                            case TypeOfPayment.TUITION:
                                if (string.IsNullOrEmpty(lastPayment.TuitionBalance))
                                {
                                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                else
                                {
                                    totalAmount = Convert.ToDecimal(lastPayment.TuitionBalance ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                break;
                            case TypeOfPayment.REGISTRATION:
                                if (string.IsNullOrEmpty(lastPayment.RegistrationBalance))
                                {
                                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                else
                                {
                                    totalAmount = Convert.ToDecimal(lastPayment.RegistrationBalance ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(RegistrationFee) ? "0" : RegistrationFee) > Convert.ToDecimal(lastPayment.RegistrationFee))
                                    {
                                        totalAmount += Convert.ToDecimal(RegistrationFee) - Convert.ToDecimal(lastPayment.RegistrationFee);
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                    else
                                    {
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                }
                                break;
                            case TypeOfPayment.BOOKS:
                                if (string.IsNullOrEmpty(lastPayment.BooksBalance))
                                {
                                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                else
                                {
                                    totalAmount = Convert.ToDecimal(lastPayment.BooksBalance ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(Books) ? "0" : Books) > Convert.ToDecimal(lastPayment.Books))
                                    {
                                        totalAmount += Convert.ToDecimal(Books) - Convert.ToDecimal(lastPayment.Books);
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                    else
                                    {
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                }
                                break;
                            case TypeOfPayment.UNIFORM:
                                if (string.IsNullOrEmpty(lastPayment.UniformBalance))
                                {
                                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                else
                                {
                                    totalAmount = Convert.ToDecimal(lastPayment.UniformBalance ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(Uniform) ? "0" : Uniform) > Convert.ToDecimal(lastPayment.Uniform))
                                    {
                                        totalAmount += Convert.ToDecimal(Uniform) - Convert.ToDecimal(lastPayment.Uniform);
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                    else
                                    {
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                }
                                break;
                            case TypeOfPayment.OTHER:
                                if (string.IsNullOrEmpty(lastPayment.OtherFeesBalance))
                                {
                                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    total = payment > totalAmount ? 0 : totalAmount - payment;
                                }
                                else
                                {
                                    totalAmount = Convert.ToDecimal(lastPayment.OtherFeesBalance ?? "0");
                                    payment = Convert.ToDecimal(Payment ?? "0");
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(OtherFees) ? "0" : OtherFees) > Convert.ToDecimal(lastPayment.OtherFees))
                                    {
                                        totalAmount += Convert.ToDecimal(OtherFees) - Convert.ToDecimal(lastPayment.OtherFees);
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                    else
                                    {
                                        total = payment > totalAmount ? 0 : totalAmount - payment;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (typeOfPayment)
                        {
                            case TypeOfPayment.TUITION:
                                return lastPayment.TuitionBalance ?? string.Empty;
                            case TypeOfPayment.REGISTRATION:
                                if (!string.IsNullOrEmpty(lastPayment.RegistrationFee))
                                {
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(RegistrationFee) ? "0" : RegistrationFee) > Convert.ToDecimal(lastPayment.RegistrationFee))
                                    {
                                        return ((Convert.ToDecimal(RegistrationFee) - Convert.ToDecimal(lastPayment.RegistrationFee)) + Convert.ToDecimal(lastPayment.RegistrationBalance)).ToString("N2");
                                    }
                                }
                                return lastPayment.RegistrationBalance ?? string.Empty;
                            case TypeOfPayment.BOOKS:
                                if (!string.IsNullOrEmpty(lastPayment.Books))
                                {
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(Books) ? "0" : Books) > Convert.ToDecimal(lastPayment.Books))
                                    {
                                        return ((Convert.ToDecimal(Books) - Convert.ToDecimal(lastPayment.Books)) + Convert.ToDecimal(lastPayment.BooksBalance)).ToString("N2");
                                    }
                                }
                                return lastPayment.BooksBalance ?? string.Empty;
                            case TypeOfPayment.UNIFORM:
                                if (!string.IsNullOrEmpty(lastPayment.Uniform))
                                {
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(Uniform) ? "0" : Uniform) > Convert.ToDecimal(lastPayment.Uniform))
                                    {
                                        return ((Convert.ToDecimal(Uniform) - Convert.ToDecimal(lastPayment.Uniform)) + Convert.ToDecimal(lastPayment.UniformBalance)).ToString("N2");
                                    }
                                }
                                return lastPayment.UniformBalance ?? string.Empty;
                            case TypeOfPayment.OTHER:
                                if (!string.IsNullOrEmpty(lastPayment.OtherFees))
                                {
                                    if (Convert.ToDecimal(string.IsNullOrEmpty(OtherFees) ? "0" : OtherFees) > Convert.ToDecimal(lastPayment.OtherFees))
                                    {
                                        return ((Convert.ToDecimal(OtherFees) - Convert.ToDecimal(lastPayment.OtherFees)) + Convert.ToDecimal(lastPayment.OtherFeesBalance)).ToString("N2");
                                    }
                                }
                                return lastPayment.OtherFeesBalance ?? string.Empty;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Payment))
                {
                    payment = Convert.ToDecimal(Payment ?? "0");
                    totalAmount = Convert.ToDecimal(TotalAmount ?? "0");
                }
                else
                {
                    return string.Empty;
                }
                total = payment > totalAmount ? 0 : totalAmount - payment;
            }
            return total.ToString("N2");
        }

        // Can Pay

        public bool TuitionCanPay
        {
            get
            {
                if (string.IsNullOrEmpty(TotalTuitionFee))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(TuitionBalance))
                {
                    if (Convert.ToDecimal(TuitionBalance) == 0m)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool RegistrationCanPay
        {
            get
            {
                if (string.IsNullOrEmpty(RegistrationFee))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(RegistrationBalance))
                {
                    if (Convert.ToDecimal(RegistrationBalance) == 0m)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool BooksCanPay
        {
            get
            {
                if (string.IsNullOrEmpty(Books))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(BooksBalance))
                {
                    if (Convert.ToDecimal(BooksBalance) == 0m)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool UniformCanPay
        {
            get
            {
                if (string.IsNullOrEmpty(Uniform))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(UniformBalance))
                {
                    if (Convert.ToDecimal(UniformBalance) == 0m)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool OtherFeesCanPay
        {
            get
            {
                if (string.IsNullOrEmpty(OtherFees))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(OtherFeesBalance))
                {
                    if (Convert.ToDecimal(OtherFeesBalance) == 0m)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void NotifyBalance()
        {
            OnPropertyChanged(nameof(TuitionBalance));
            OnPropertyChanged(nameof(RegistrationBalance));
            OnPropertyChanged(nameof(BooksBalance));
            OnPropertyChanged(nameof(UniformBalance));
            OnPropertyChanged(nameof(OtherFeesBalance));
        }

        public enum TypeOfPayment
        {
            TUITION,
            REGISTRATION,
            BOOKS,
            UNIFORM,
            OTHER
        }
    }
}
