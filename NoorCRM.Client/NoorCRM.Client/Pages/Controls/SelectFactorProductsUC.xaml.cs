using NoorCRM.API.Models;
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
    public partial class SelectFactorProductsUC : ContentView
    {
        public IEnumerable<Product> Products
        {
            get { return (IEnumerable<Product>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty ProductsProperty =
            BindableProperty.Create(
                nameof(Products),
                typeof(IEnumerable<Product>),
                typeof(SelectFactorProductsUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleProductListChanged);

        private static SelectFactorProductsUC sfpuc;
        private static List<SelectProductCardInfo> allCardInfos;
        private static void HandleProductListChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var products = newValue as IEnumerable<Product>;
            if (products != null)
            {
                var sortedcards = new SortedList<string,SelectProductCardInfo>();
                foreach (var item in products)
                    sortedcards.Add(item.Title+item.Id, new SelectProductCardInfo(item));
                allCardInfos = sortedcards.Values.ToList();
                sfpuc = (SelectFactorProductsUC)bindable;
                setToList(allCardInfos);
            }
        }

        public SelectFactorProductsUC()
        {
            InitializeComponent();
        }

        public List<Product> GetSelectedProducts()
        {
            var list = new List<Product>();
            foreach (var item in allCardInfos)
            {
                if (item.Selected)
                    list.Add(item.Product);
            }

            return list;
        }

        string filter = "";
        bool onlyExistedProducts = false;
        public void Search(string filter, bool onlyExistedProducts)
        {
            this.filter = filter;
            this.onlyExistedProducts = onlyExistedProducts;
            var list = new List<SelectProductCardInfo>();
            foreach (var item in allCardInfos)
            {
                if (!onlyExistedProducts || (onlyExistedProducts && item.ExistedQuantity > 0))
                    if (string.IsNullOrWhiteSpace(filter) || item.Title.Contains(filter.Trim()))
                        list.Add(item);
            }

            setToList( list);
        }

        public void DeselectAll()
        {
            foreach (var item in allCardInfos)
            {
                item.Selected = false;
            }
            Search(filter, onlyExistedProducts);
        }

        private static void setToList(IEnumerable<SelectProductCardInfo> products)
        {
            sfpuc.stkContainer.ItemsSource = products;
        }
    }
}