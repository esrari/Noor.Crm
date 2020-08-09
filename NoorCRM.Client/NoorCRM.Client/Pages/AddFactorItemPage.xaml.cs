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
        private List<Product> _selectedProducts = null;
        public List<Product> SelectedProducts => _selectedProducts;

        public AddFactorItemPage()
        {
            InitializeComponent();
            productList.Products = App.MainViewModel.Products;
        }


        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            productList.Search(e.NewTextValue, false);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            _selectedProducts = null;
            App.NavigationPage.Navigation.PopModalAsync();
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            _selectedProducts = productList.GetSelectedProducts();
            App.NavigationPage.Navigation.PopModalAsync();
        }
    }
}