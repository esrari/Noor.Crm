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

        private static ProductsUC puc;
        private static List<ProductViewModel> allProInfos;
        private static async void HandleProductListChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var products = newValue as ObservableCollection<Product>;
            if (products != null)
            {
                puc = bindable as ProductsUC;
                var sortedcards = new SortedList<string, Product>();
                foreach (var item in products)
                    sortedcards.Add(item.Title + item.Id, item);

                allProInfos = new List<ProductViewModel>();
                foreach (var item in sortedcards.Values)
                    allProInfos.Add(new ProductViewModel(item));

                setToList(allProInfos);
            }
        }

        public ProductsUC()
        {
            InitializeComponent();
        }

        public void Filter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                setToList(allProInfos);
            else
            {
                var trimFilter = filter.Trim();
                var products = new List<ProductViewModel>();
                foreach (var item in allProInfos)
                    if (item.Title.Contains(trimFilter))
                        products.Add(item);

                setToList(products);
            }
        }

        private static void setToList(IEnumerable<ProductViewModel> products)
        {
            puc.stkProducts.ItemsSource = products;
        }
    }
}