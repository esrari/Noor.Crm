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
    public partial class ProductsUC : ContentView
    {
        public IEnumerable<Product> Products
        {
            set {
                if (value != null)
                {
                    stkProducts.Children.Clear();
                    foreach (var item in value)
                    {
                        var proBox = new ProductBox(item);
                        proBox.ProductSelected += ProBox_ProductSelected;
                        stkProducts.Children.Add(proBox);
                    }
                }
            }
        }

        private void ProBox_ProductSelected(SelectedProduct selectedProduct)
        {
            OnProductSelected(selectedProduct);
        }

        public ProductsUC()
        {
            InitializeComponent();
        }

        public event ProductSelectedEventHandler ProductSelected;
        public void OnProductSelected(SelectedProduct selectedProduct)
        {
            ProductSelected?.Invoke(selectedProduct);
        }
    }
}