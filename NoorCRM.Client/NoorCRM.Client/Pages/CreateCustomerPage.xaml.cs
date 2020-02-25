using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCustomerPage : ContentPage
    {
        private CreateCustomerViewModel _createCustomerViewModel;
        private readonly Customer _customer;
        private bool _isEdit = false;

        public event CustomerCreatedEventHandler CustomerEditDone;

        public CreateCustomerPage()
        {
            InitializeComponent();

            var cities = from city in App.MainViewModel.OnlineUser.VisitCities
                         select city.Name;
            picCities.ItemsSource = new List<string>(cities);
            _createCustomerViewModel = new CreateCustomerViewModel()
            { CityName = App.MainViewModel.DefaultCity?.Name };
            BindingContext = _createCustomerViewModel;
        }

        public CreateCustomerPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            _isEdit = true;

            var cities = from city in App.MainViewModel.OnlineUser.VisitCities
                         select city.Name;
            picCities.ItemsSource = new List<string>(cities);
            _createCustomerViewModel = new CreateCustomerViewModel(customer);
            BindingContext = _createCustomerViewModel;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_createCustomerViewModel.CustomerName))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "لطفا نام فروشنده را وارد نمایید.",
                                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
                return;
            }
            if (string.IsNullOrWhiteSpace(_createCustomerViewModel.CityName))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "لطفا شهر را تعیین کنید.",
                                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(false);
                return;
            }
            _createCustomerViewModel.IsAccepted = true;
            await App.NavigationPage.Navigation.PopModalAsync().ConfigureAwait(false);
            OnCustomerEditDone(_createCustomerViewModel);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            _createCustomerViewModel.IsAccepted = false;
            App.NavigationPage.Navigation.PopModalAsync();
        }

        private void OnCustomerEditDone(CreateCustomerViewModel newCustomer)
        {
            CustomerEditDone?.Invoke(newCustomer);
        }

    }

    public class CreateCustomerViewModel
    {
        public bool IsAccepted { get; set; }
        public string CustomerName { get; set; }
        public string StoreName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneTitle1 { get; set; }
        public string PhoneNo2 { get; set; }
        public string PhoneTitle2 { get; set; }
        public string PhoneNo3 { get; set; }
        public string PhoneTitle3 { get; set; }

        public CreateCustomerViewModel()
        {
            IsAccepted = false;
        }

        public CreateCustomerViewModel(Customer customer)
        {
            IsAccepted = false;

            if (customer != null)
            {
                CustomerName = customer.ManagerName;
                StoreName = customer.StoreName;
                CityName = customer.City?.Name;
                Address = customer.Address;
                if (customer.PhoneNos != null)
                {
                    var numbers = new List<PhoneNo>(customer.PhoneNos);
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        if (i == 0)
                        {
                            PhoneTitle1 = numbers[i].Title;
                            PhoneNo1 = numbers[i].Number;
                        }
                        else if(i==1)
                        {
                            PhoneTitle2 = numbers[i].Title;
                            PhoneNo2 = numbers[i].Number;
                        }
                        else if (i == 2)
                        {
                            PhoneTitle3 = numbers[i].Title;
                            PhoneNo3 = numbers[i].Number;
                        }
                    }
                }
            }
        }
    }

    public delegate void CustomerCreatedEventHandler(CreateCustomerViewModel newCustomer);
}