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

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitFactorPage : ContentPage
    {
        private readonly Customer _customer;
        private FactorViewModel _viewModel;
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
            await App.ApiService.InsertNewFactorAsync(factor);
            await App.NavigationPage.Navigation.PopAsync();
            await App.NavigationPage.Navigation.PopAsync();
        }
    }
}