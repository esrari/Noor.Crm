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
    public partial class CreateCustomerPage : ContentPage
    {
        private CreateCustomerViewModel _createCustomerViewModel;
        public event CustomerCreatedEventHandler CustomerCreated;

        public CreateCustomerPage()
        {
            InitializeComponent();

            _createCustomerViewModel = new CreateCustomerViewModel();
            BindingContext = _createCustomerViewModel;
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            _createCustomerViewModel.IsAccepted = true;
            App.NavigationPage.Navigation.PopModalAsync();
            OnCustomerCreated(_createCustomerViewModel);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            _createCustomerViewModel.IsAccepted = false;
            App.NavigationPage.Navigation.PopModalAsync();
        }

        private void OnCustomerCreated(CreateCustomerViewModel newCustomer)
        {
            CustomerCreated?.Invoke(newCustomer);
        }
    }

    public class CreateCustomerViewModel
    {
        public bool IsAccepted { get; set; }
        public string CustomerName { get; set; }
        public string StoreName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        public CreateCustomerViewModel()
        {
            IsAccepted = false;
        }
    }

    public delegate void CustomerCreatedEventHandler(CreateCustomerViewModel newCustomer);
}