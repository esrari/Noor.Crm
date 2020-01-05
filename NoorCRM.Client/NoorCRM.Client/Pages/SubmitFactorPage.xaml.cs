using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls;
using NoorCRM.Client.ViewModels;
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
    public partial class SubmitFactorPage : ContentPage
    {
        private readonly Customer _customer;
        private FactorViewModel _viewModel;
        public event PageClosedEventHandler PageClosed;

        public SubmitFactorPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            _viewModel = new FactorViewModel(customer);
            BindingContext = _viewModel;
        }

        private void BtnAddItem_Clicked(object sender, EventArgs e)
        {
            var addItemPage = new AddFactorItemPage();
            addItemPage.ProductSelected += AddItemPage_ProductSelected;
            App.NavigationPage.Navigation.PushAsync(addItemPage);
        }

        private void AddItemPage_ProductSelected(SelectedProduct selectedProduct)
        {
            var item = new FactorItemViewModel()
            {
                Product = selectedProduct.Product,
                SelectedPrice = selectedProduct.SelectedPrice
            };
            _viewModel.AddItem(item);
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.FactorItems.Any())
                return;
            var factor = _viewModel.GetSubmitedFactor();
            var successLog = new SuccessfulLog()
            {
                CreationDate = DateTime.Now,
                CreatorUserId = App.MainViewModel.OnlineUser.Id,
                CustomerId = factor.CustomerId,
                IsVisitorsLog = true
            };

            var sucLog = await App.ApiService.InsertNewFactorAsync(factor, successLog).ConfigureAwait(true);
            if (sucLog != null)
            {
                OnPageClosed(successful: true, sucLog);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن فاکتور جدید با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
            else
            {
                OnPageClosed(successful: false, null);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن فاکتور جدید با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }

            await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
        }

        private void OnPageClosed(bool successful, SuccessfulLog log)
        {
            PageClosed?.Invoke(successful, log);
        }
    }
}