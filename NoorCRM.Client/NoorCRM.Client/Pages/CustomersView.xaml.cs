using NoorCRM.API.Models;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomersView : ContentView
    {
        public MainViewModel MainViewModel
        {
            get { return (MainViewModel)GetValue(MainViewModelProperty); }
            set { SetValue(MainViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty MainViewModelProperty =
            BindableProperty.Create(
                nameof(MainViewModel),
                typeof(MainViewModel),
                typeof(CustomersView),
                null,
                BindingMode.TwoWay,
                propertyChanged: handleViewModelChanged);

        private static void handleViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var viewModel = newValue as MainViewModel;
            if (viewModel != null)
            {
                var cluc = (CustomersView)bindable;
                cluc.BindingContext = viewModel;
            }
        }

        public CustomersView()
        {
            InitializeComponent();
        }

        private void BtnAddCustomer_Clicked(object sender, EventArgs e)
        {
            var addCustomerPage = new CreateCustomerPage();
            addCustomerPage.CustomerCreated += AddCustomerPage_CustomerCreated;
            App.NavigationPage.Navigation.PushModalAsync(addCustomerPage);
        }

        private async void AddCustomerPage_CustomerCreated(CreateCustomerViewModel newCustomer)
        {
            var city = App.MainViewModel.OnlineUser.VisitCities
                    .Where(vc => vc.Name == newCustomer.CityName)
                    .FirstOrDefault();
            var customer = new Customer()
            {
                ManagerName = newCustomer.CustomerName,
                StoreName = newCustomer.StoreName,
                Address = newCustomer.Address,
                CityId = city.Id,
                CreationDate = DateTime.Now,
                IsActive = true,
                PhoneNos = new[] { new PhoneNo() { Title = "تلفن", Number = newCustomer.PhoneNo } },
            };

            var insertedCustomer = await App.ApiService.InsertNewCustomerAsync(customer);

            // Send result for snack bar and add inserted customer too customers list
            if (insertedCustomer == null)
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن مشتری جدید با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong);
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن مشتری جدید با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong);

                // after inserting open customer page for action
                await App.NavigationPage.Navigation.PushAsync(new CustomerPage(insertedCustomer));
                // after insertion city don't load again
                var newList = new ObservableCollection<Customer>(App.MainViewModel.Customers);
                insertedCustomer.City = city;
                insertedCustomer.CityId = city.Id;
                newList.Add(insertedCustomer);
                App.MainViewModel.Customers = newList;
            }
        }
    }
}