using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFactorItemPage : ContentPage
    {
        public AddFactorItemPage()
        {
            InitializeComponent();
            productList.ProductSelected += ProductList_ProductSelected;
            productList.Products = App.MainViewModel.Products;
        }



        private void ProductList_ProductSelected(SelectedProduct selectedProduct)
        {
            OnProductSelected(selectedProduct);
            txtSearch.Text = "";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            productList.Filter(e.NewTextValue);
        }

        public event ProductSelectedEventHandler ProductSelected;
        public void OnProductSelected(SelectedProduct selectedProduct)
        {
            ProductSelected?.Invoke(selectedProduct);
        }
    }
}