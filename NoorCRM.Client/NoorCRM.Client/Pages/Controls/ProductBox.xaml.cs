using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductBox : ContentView
    {
        private ProductViewModel _viewModel;
        private SelectedProduct SelectedProduct { get; set; }

        public ProductBox(Product product)
        {
            InitializeComponent();
            _viewModel = new ProductViewModel(product);
            BindingContext = _viewModel;
        }

        private void BtnPrice1_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price1);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice2_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price2);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice3_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price3);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice1ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price1ch);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice2ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price2ch);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice3ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(_viewModel.Product, _viewModel.Price3ch);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        public event ProductSelectedEventHandler ProductSelected;
        public void OnProductSelected()
        {
            ProductSelected?.Invoke(SelectedProduct);
        }
    }


}