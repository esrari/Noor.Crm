using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls;
using NoorCRM.Client.Sources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage
    {
        private readonly Customer _customer;
        private List<CustomerLog> _logs;
        private AddReminderPage _reminderPage;
        private CustomerViewModel _viewModel;

        public CustomerPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            App.ApiService.CustomerLogsFetched += ApiService_CustomerLogsFetched;
            _ = App.ApiService.GetCustomerLogsAync(customer.Id, 20, 0);

            _viewModel = new CustomerViewModel(customer);
            BindingContext = _viewModel;
        }

        private void ApiService_CustomerLogsFetched(IEnumerable<CustomerLog> logs)
        {
            if (logs == null)
                return;

            _logs = new List<CustomerLog>(logs);
            logList.CustomerLogs = _logs;
        }

        private void BtnAddLog_Clicked(object sender, EventArgs e)
        {
            var addLogPage = new AddLogPage(_customer);
            addLogPage.PageClosed += AddLogPage_PageClosed;
            App.NavigationPage.Navigation.PushAsync(addLogPage);
        }

        private void AddLogPage_PageClosed(bool successful, CustomerLog log)
        {
            if (successful)
            {
                log.CreatorUser = App.MainViewModel.OnlineUser;
                log.CreatorUser.Id = App.MainViewModel.OnlineUser.Id;
                if (log != null)
                {
                    _logs.Add(log);
                    LogListUC.AddLog(logList, log);
                }

                if (log != null && log is SuccessfulLog)
                {
                    var f = (log as SuccessfulLog).Factor;
                    App.MainViewModel.LastFactors.Insert(0, f);
                    App.MainViewModel.LastFactors = new ObservableCollection<Factor>(App.MainViewModel.LastFactors);
                    _viewModel.HasAnyFactor = true;
                }
            }
        }

        private void TapGestureRecognizer_btnEdit(object sender, EventArgs e)
        {
            var editCustomerPage = new CreateCustomerPage(_customer);
            editCustomerPage.CustomerEditDone += EditCustomerPage_CustomerEditDone; ;
            App.NavigationPage.Navigation.PushModalAsync(editCustomerPage);
        }

        private async void EditCustomerPage_CustomerEditDone(CreateCustomerViewModel newCustomer)
        {
            var city = App.MainViewModel.OnlineUser.VisitCities
                .Where(vc => vc.Name == newCustomer.CityName)
                .FirstOrDefault();
            if (city == null)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "شهر مورد نظر موجود نمی باشد و یا به آن دسترسی ندارید.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                return;
            }

            _customer.ManagerName = newCustomer.CustomerName;
            _customer.StoreName = newCustomer.StoreName;
            _customer.Address = newCustomer.Address;
            _customer.CityId = city.Id;
            _customer.City = city;
            _customer.CreationDate = DateTime.Now;
            _customer.IsActive = true;
            if (_customer.PhoneNos == null || _customer.PhoneNos.Count == 0)
            {
                _customer.PhoneNos = new[] { new PhoneNo() { Title = newCustomer.PhoneTitle1, Number = newCustomer.PhoneNo1 },
                                   new PhoneNo() { Title = newCustomer.PhoneTitle2, Number = newCustomer.PhoneNo2 },
                                   new PhoneNo() { Title = newCustomer.PhoneTitle3, Number = newCustomer.PhoneNo3 }};
            }
            else
            {
                var phList = new List<PhoneNo>(_customer.PhoneNos);
                if (phList.Count == 1)
                {
                    phList[0].Title = newCustomer.PhoneTitle1;
                    phList[0].Number = newCustomer.PhoneNo1;
                    phList.Add(new PhoneNo() { Title = newCustomer.PhoneTitle2, Number = newCustomer.PhoneNo2 });
                    phList.Add(new PhoneNo() { Title = newCustomer.PhoneTitle3, Number = newCustomer.PhoneNo3 });
                }
                else if (phList.Count == 2)
                {
                    phList[0].Title = newCustomer.PhoneTitle1;
                    phList[0].Number = newCustomer.PhoneNo1;
                    phList[1].Title = newCustomer.PhoneTitle2;
                    phList[1].Number = newCustomer.PhoneNo2;
                    phList.Add(new PhoneNo() { Title = newCustomer.PhoneTitle3, Number = newCustomer.PhoneNo3 });
                }
                else if (phList.Count == 3)
                {
                    phList[0].Title = newCustomer.PhoneTitle1;
                    phList[0].Number = newCustomer.PhoneNo1;
                    phList[1].Title = newCustomer.PhoneTitle2;
                    phList[1].Number = newCustomer.PhoneNo2;
                    phList[2].Title = newCustomer.PhoneTitle3;
                    phList[2].Number = newCustomer.PhoneNo3;
                }

                _customer.PhoneNos = phList;
            }
            var updatedCustomer = await App.ApiService.UpdateCustomerAsync(_customer).ConfigureAwait(true);

            // Send result for snack bar and add inserted customer too customers list
            if (updatedCustomer == null)
                await MaterialDialog.Instance.SnackbarAsync(message: "ویرایش مشتری با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "ویرایش مشتری با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
        }

        private async void TapGestureRecognizer_btnDelete(object sender, EventArgs e)
        {

            if (_customer.Factors != null && _customer.Factors.Any())
                await MaterialDialog.Instance.SnackbarAsync(message: "مشتری مورد نظر دارای فاکتور می باشد و قابل حذف نیست.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
            else
            {
                var confirm = await MaterialDialog.Instance.ConfirmAsync(message: "آیا از حذف این مشتری مطمئن هستید؟",
                                    confirmingText: "بله",
                                    dismissiveText: "خیر").ConfigureAwait(true);

                if (confirm.HasValue && confirm.Value)
                {
                    var result = await App.ApiService.DeleteCustomerAsync(_customer.Id).ConfigureAwait(true);
                    if (result)
                    {
                        var cust = App.MainViewModel.Customers.Where(c => c.Id == _customer.Id).FirstOrDefault();
                        if (cust != null)
                            App.MainViewModel.Customers.Remove(cust);
                        // remove from reminder list
                        var tcu = App.MainViewModel.TodayCustomers.Where(c => c.Id == _customer.Id).FirstOrDefault();
                        if (tcu != null)
                            App.MainViewModel.TodayCustomers.Remove(tcu);
                        await MaterialDialog.Instance.SnackbarAsync(message: "مشتری مورد نظر حذف شد.",
                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                        await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        private void Reminder_Tapped(object sender, EventArgs e)
        {
            _reminderPage = new AddReminderPage(_customer.Reminder);
            _reminderPage.Disappearing += Reminder_Disappearing;
            App.NavigationPage.Navigation.PushModalAsync(_reminderPage);
        }

        private async void Reminder_Disappearing(object sender, EventArgs e)
        {
            if (_reminderPage != null)
            {
                if (_reminderPage.ViewModel.IsSubmitted)
                {
                    _customer.Reminder = _reminderPage.ViewModel.SelectedDateTime;
                    var newCustomer = await App.ApiService.UpdateCustomerAsync(_customer).ConfigureAwait(true);
                    if (newCustomer != null)
                    {
                        _viewModel.Reminder = _reminderPage.ViewModel.SelectedDateTime;
                        _viewModel.HasReminder = true;
                        // just if reminder is for today or lasted days can add to list
                        if (_viewModel.Reminder.Date.CompareTo(DateTime.Today) <= 0)
                        {
                            var remCus = App.MainViewModel.TodayCustomers.Where(c => c.Id == _customer.Id)
                                .FirstOrDefault();
                            if (remCus == null)
                                App.MainViewModel.TodayCustomers.Add(_customer);
                        }
                        else
                        {
                            var remCus = App.MainViewModel.TodayCustomers.Where(c => c.Id == _customer.Id)
                                .FirstOrDefault();
                            if (remCus != null)
                                App.MainViewModel.TodayCustomers.Remove(_customer);
                        }
                        App.MainViewModel.TodayCustomers = new ObservableCollection<Customer>(App.MainViewModel.TodayCustomers.OrderBy(c => c.Reminder.Value));
                    }
                    else
                    {
                        if (_viewModel.HasReminder)
                            _customer.Reminder = _viewModel.Reminder;
                        await MaterialDialog.Instance.SnackbarAsync(message: "متاسفانه ثبت یادآور با مشکل مواجه شد.",
                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
                    }
                }
                else if (_reminderPage.ViewModel.IsRemoved)
                {
                    _customer.Reminder = null;
                    var newCustomer = await App.ApiService.UpdateCustomerAsync(_customer).ConfigureAwait(true);
                    if (newCustomer != null)
                    {
                        _viewModel.HasReminder = false;
                        var remCus = App.MainViewModel.TodayCustomers.Where(c => c.Id == _customer.Id)
                            .FirstOrDefault();
                        if (remCus != null)
                            App.MainViewModel.TodayCustomers.Remove(remCus);
                    }
                    else
                    {
                        if (_viewModel.HasReminder)
                            _customer.Reminder = _viewModel.Reminder;
                        await MaterialDialog.Instance.SnackbarAsync(message: "متاسفانه حذف یادآور با مشکل مواجه شد.",
                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
                    }

                }
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (Application.Current.Resources.TryGetValue("Secondary", out var secColor))
                App.NavigationPage.BarBackgroundColor = (Color)secColor;
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            App.NavigationPage.BarBackgroundColor = Color.White;
        }
    }

    public class CustomerViewModel : INotifyPropertyChanged
    {
        private bool _hasReminder;
        private DateTime _reminder;
        private bool _hasAnyFactor;
        private string _tel;

        public Customer Customer { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }

        public string Tel
        {
            get => _tel;
            set
            {
                if (_tel == value)
                    return;
                _tel = value;
                OnPropertyChanged();
            }
        }
        public bool HasAnyFactor
        {
            get => _hasAnyFactor;
            set
            {
                if (_hasAnyFactor == value)
                    return;
                _hasAnyFactor = value;
                OnPropertyChanged();
            }
        }
        public bool HasReminder
        {
            get => _hasReminder; set
            {
                if (_hasReminder == value)
                    return;
                _hasReminder = value;
                OnPropertyChanged();
            }
        }
        public DateTime Reminder
        {
            get => _reminder; set
            {
                if (_reminder == value)
                    return;
                _reminder = value;
                OnPropertyChanged();
            }
        }

        public CustomerViewModel(Customer customer)
        {
            Customer = customer;
            Title = Helper.CreateCustomerTitle(customer);
            HasAnyFactor = customer.Factors?.Count > 0;
            IsActive = customer.IsActive;
            if (customer.PhoneNos != null)
                foreach (var item in customer.PhoneNos)
                    if (!string.IsNullOrWhiteSpace(item.Number))
                    {
                        Tel = item.Number;
                        break;
                    }
            if (customer.Reminder.HasValue)
            {
                HasReminder = true;
                Reminder = customer.Reminder.Value;
            }
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}