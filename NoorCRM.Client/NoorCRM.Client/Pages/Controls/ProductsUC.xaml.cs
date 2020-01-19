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

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsUC : ContentView
    {
        public IEnumerable<Product> Products
        {
            set
            {
                if (value != null)
                {
                    stkProducts.Children.Clear();
                    foreach (var item in value)
                    {
                        var proBox = new ProductBox(item);
                        proBox.ProductSelected += ProBox_ProductSelected;
                        stkProducts.Children.Add(proBox);
                    }
                    scvScroller.ScrollToAsync(stkProducts, ScrollToPosition.Start, false).ConfigureAwait(false);
                }
            }
        }

        public ObservableCollection<Product> ProductList
        {
            get { return (ObservableCollection<Product>)GetValue(ProductListProperty); }
            set { SetValue(ProductListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty ProductListProperty =
            BindableProperty.Create(
                nameof(ProductList),
                typeof(ObservableCollection<Product>),
                typeof(ProductsUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleProductListChanged);

        private static async void HandleProductListChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var products = newValue as ObservableCollection<Product>;
            if (products != null)
            {
                var pruc = (ProductsUC)bindable;
                pruc.stkProducts.Children.Clear();
                foreach (var item in products)
                {
                    var proBox = new ProductBox(item);
                    pruc.stkProducts.Children.Add(proBox);
                }

                await Task.Delay(3).ConfigureAwait(true);
                await pruc.scvScroller.ScrollToAsync(pruc.stkProducts, ScrollToPosition.Start, false).ConfigureAwait(false);
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

        public void Filter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                foreach (ProductBox item in stkProducts.Children)
                    item.IsVisible = true;
            else
            {
                var trimFilter = filter.Trim();
                foreach (ProductBox item in stkProducts.Children)
                {
                    if (item.ViewModel.Title.Contains(trimFilter))
                        item.IsVisible = true;
                    else item.IsVisible = false;
                }
            }
        }

        public event ProductSelectedEventHandler ProductSelected;
        public void OnProductSelected(SelectedProduct selectedProduct)
        {
            ProductSelected?.Invoke(selectedProduct);
        }
    }
}