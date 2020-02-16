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
        public ProductViewModel ViewModel { get; private set; }
        private SelectedProduct SelectedProduct { get; set; }

        public ProductBox(Product product)
        {
            InitializeComponent();
            ViewModel = new ProductViewModel(product);
            BindingContext = ViewModel;
        }

        private void BtnPrice1_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price1);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice2_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price2);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice3_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price3);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice1ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price1ch);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice2ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price2ch);
            OnProductSelected();
            App.NavigationPage.Navigation.PopAsync();
        }

        private void BtnPrice3ch_Clicked(object sender, EventArgs e)
        {
            SelectedProduct = new SelectedProduct(ViewModel.Product, ViewModel.Price3ch);
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