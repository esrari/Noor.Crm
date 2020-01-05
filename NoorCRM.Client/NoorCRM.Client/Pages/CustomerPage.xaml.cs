using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage
    {
        private readonly Customer _customer;
        private List<CustomerLog> _logs;

        public CustomerPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            App.ApiService.CustomerLogsFetched += ApiService_CustomerLogsFetched;
            _ = App.ApiService.GetCustomerLogsAync(customer.Id);

            BindingContext = new CustomerViewModel(customer);
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
            if(successful)
            {
                _logs.Add(log);
                logList.CustomerLogs = _logs;
            }
        }
    }

    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }

        public CustomerViewModel(Customer customer)
        {
            Customer = customer;
            Title = Helper.CreateCustomerTitle(customer);
            IsActive = customer.IsActive;
        }
    }
}