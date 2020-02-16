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
            if (Application.Current.Resources.TryGetValue("Secondary", out var secColor))
                App.NavigationPage.BarBackgroundColor = (Color)secColor;
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

        }

        private async void TapGestureRecognizer_btnDelete(object sender, EventArgs e)
        {
            if (_customer.Factors != null && _customer.Factors.Any())
                await MaterialDialog.Instance.SnackbarAsync(message: "مشتری مورد نظر دارای فاکتور می باشد و قابل حذف نیست.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
            else
            {
                var result = await App.ApiService.DeleteCustomerAsync(_customer.Id).ConfigureAwait(true);
                if (result)
                {
                    var cust = App.MainViewModel.Customers.Where(c => c.Id == _customer.Id).FirstOrDefault();
                    if (cust != null)
                        App.MainViewModel.Customers.Remove(cust);
                    await MaterialDialog.Instance.SnackbarAsync(message: "مشتری مورد نظر حذف شد.",
                        msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                    await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
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
    }

    public class CustomerViewModel : INotifyPropertyChanged
    {
        private bool _hasReminder;
        private DateTime _reminder;
        private bool _hasAnyFactor;

        public Customer Customer { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool HasAnyFactor
        {
            get => _hasAnyFactor; set
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