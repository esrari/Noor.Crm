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
        public event CustomerCreatedEventHandler CustomerCreated;

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

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(_createCustomerViewModel.CustomerName))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "لطفا نام فروشنده را وارد نمایید.",
                                            msDuration: MaterialSnackbar.DurationLong);
                return;
            }
            if (string.IsNullOrWhiteSpace(_createCustomerViewModel.PhoneNo))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "لطفا شماره تلفن فروشگاه را وارد نمایید.",
                                            msDuration: MaterialSnackbar.DurationLong);
                return;
            }
            if (string.IsNullOrWhiteSpace(_createCustomerViewModel.CityName))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "لطفا شهر را تعیین کنید.",
                                            msDuration: MaterialSnackbar.DurationLong);
                return;
            }
            _createCustomerViewModel.IsAccepted = true;
            await App.NavigationPage.Navigation.PopModalAsync();
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
        public string CityName { get; set; }
        public string Address { get; set; }

        public CreateCustomerViewModel()
        {
            IsAccepted = false;
        }
    }

    public delegate void CustomerCreatedEventHandler(CreateCustomerViewModel newCustomer);
}