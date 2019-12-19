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
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                productList.Products = App.MainViewModel.Products;
            else
                productList.Products = App.MainViewModel.Products
                    .Where(p => p.Title.Contains(e.NewTextValue.Trim()));
        }

        public event ProductSelectedEventHandler ProductSelected;
        public void OnProductSelected(SelectedProduct selectedProduct)
        {
            ProductSelected?.Invoke(selectedProduct);
        }
    }
}