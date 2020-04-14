using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCustomerPage : ContentPage
    {
        private EditCustomerViewModel _createCustomerViewModel;
        private readonly Customer _customer;
        private bool _isEdit = false;

        public event CustomerCreatedEventHandler CustomerEditDone;

        public CreateCustomerPage()
        {
            InitializeComponent();

            var cities = from city in App.MainViewModel.OnlineUser.VisitCities
                         select city.Name;
            picCities.ItemsSource = new List<string>(cities);
            _createCustomerViewModel = new EditCustomerViewModel()
            { CityName = App.MainViewModel.DefaultCity?.Name };
            BindingContext = _createCustomerViewModel;
        }

        public CreateCustomerPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            _isEdit = true;
            if (customer != null)
            {
                var cities = from city in App.MainViewModel.OnlineUser.VisitCities
                             select city.Name;
                picCities.ItemsSource = new List<string>(cities);
                _createCustomerViewModel = new EditCustomerViewModel(customer);
                BindingContext = _createCustomerViewModel;

                if (customer.HasLocation)
                {
                    setLocation(new Position(customer.Latitude, customer.Longitude));
                    btnLocation.Text = "تغییر موقعیت";
                }
            }
        }

        private void setLocation(Position? position)
        {
            var pins = new ObservableCollection<Pin>();
            if (position.HasValue)
            {
                pins.Add(new Pin()
                {
                    Position = position.Value,
                    Address = _customer.Address,
                    Label = _customer.CreateCustomerTitle()
                });
                map.IsVisible = true;
                btnLocation.Text = "تغییر موقعیت";
            }
            else
            {
                map.IsVisible = false;
                btnLocation.Text = "ثبت موقعیت";
            }
            map.Pins = pins;
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

        private void OnCustomerEditDone(EditCustomerViewModel newCustomer)
        {
            CustomerEditDone?.Invoke(newCustomer);
        }

        AddPinPage _pinPage;
        private void btnLocation_Clicked(object sender, EventArgs e)
        {
            if (_customer.HasLocation)
                _pinPage = new AddPinPage(new Position(_customer.Latitude, _customer.Longitude));
            else
                _pinPage = new AddPinPage();

            _pinPage.Disappearing += PinPage_Disappearing;
            App.NavigationPage.Navigation.PushModalAsync(_pinPage);
        }

        private void PinPage_Disappearing(object sender, EventArgs e)
        {
            if (_pinPage.PageSubmitted)
            {
                setLocation(_pinPage.Position);
                if (_pinPage.Position.HasValue)
                {
                    _customer.HasLocation = true;
                    _customer.Latitude = _pinPage.Position.Value.Latitude;
                    _customer.Longitude = _pinPage.Position.Value.Longitude;
            } }
        }
    }

    public class EditCustomerViewModel
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

        public bool HasLocation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public EditCustomerViewModel()
        {
            IsAccepted = false;
        }

        public EditCustomerViewModel(Customer customer)
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
                        else if (i == 1)
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

                HasLocation = customer.HasLocation;
                Latitude = customer.Latitude;
                Longitude = customer.Longitude;
            }
        }
    }

    public delegate void CustomerCreatedEventHandler(EditCustomerViewModel newCustomer);
}