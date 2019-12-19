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

            logList.CustomerLogs = logs;
        }

        private void BtnAddLog_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new AddLogPage(_customer));
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