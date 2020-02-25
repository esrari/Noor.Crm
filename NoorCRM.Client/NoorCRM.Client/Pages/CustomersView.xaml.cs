using NoorCRM.API.Models;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            addCustomerPage.CustomerEditDone += AddCustomerPage_CustomerEditDone;
            App.NavigationPage.Navigation.PushModalAsync(addCustomerPage);
        }

        private async void AddCustomerPage_CustomerEditDone(CreateCustomerViewModel newCustomer)
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
            var customer = new Customer()
            {
                ManagerName = newCustomer.CustomerName,
                StoreName = newCustomer.StoreName,
                Address = newCustomer.Address,
                CityId = city.Id,
                CreationDate = DateTime.Now,
                IsActive = true,
                PhoneNos = new[] { new PhoneNo() { Title = newCustomer.PhoneTitle1, Number = newCustomer.PhoneNo1 },
                                   new PhoneNo() { Title = newCustomer.PhoneTitle2, Number = newCustomer.PhoneNo2 },
                                   new PhoneNo() { Title = newCustomer.PhoneTitle3, Number = newCustomer.PhoneNo3 }}
            };

            var insertedCustomer = await App.ApiService.InsertNewCustomerAsync(customer).ConfigureAwait(true);

            // Send result for snack bar and add inserted customer too customers list
            if (insertedCustomer == null)
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن مشتری جدید با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن مشتری جدید با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);

                // after insertion city don't load again
                insertedCustomer.City = city;
                insertedCustomer.CityId = city.Id;
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.MainViewModel.Customers.Add(insertedCustomer);
                });
            }
        }

        private void ContentView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
                listCustomer.RefreshItems();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            listCustomer.Filter(e.NewTextValue);
        }
    }
}