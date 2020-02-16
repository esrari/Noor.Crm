using NoorCRM.API.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomersListUC : ContentView
    {
        public ObservableCollection<Customer> Customers
        {
            get { return (ObservableCollection<Customer>)GetValue(CustomersProperty); }
            set { SetValue(CustomersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CustomersProperty =
            BindableProperty.Create(
                nameof(Customers),
                typeof(ObservableCollection<Customer>),
                typeof(CustomersListUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleCustomersChanged);

        private static CustomersListUC cluc;
        private static ObservableCollection<CustomerCardInfo> cardInfos;
        private static async void HandleCustomersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customers = newValue as ObservableCollection<Customer>;
            customers.CollectionChanged += Customers_CollectionChanged;
            if (customers != null)
            {
                cardInfos = new ObservableCollection<CustomerCardInfo>();
                foreach (var c in customers)
                    cardInfos.Add(new CustomerCardInfo(c, App.NavigationPage.Navigation));

                cluc = (CustomersListUC)bindable;
                BindableLayout.SetItemsSource(cluc.stkContainer, cardInfos);
                await Task.Delay(3).ConfigureAwait(true);
                await cluc.scvScroller.ScrollToAsync(cluc.stkContainer, ScrollToPosition.Start, false).ConfigureAwait(false);
            }
        }

        private static void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //var cardInfos = new List<CustomerCardInfo>(BindableLayout.GetItemsSource(cluc.stkContainer).Cast<CustomerCardInfo>());

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer item in e.NewItems)
                {
                    cardInfos.Add(new CustomerCardInfo(item, App.NavigationPage.Navigation));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer item in e.OldItems)
                {
                    var cust = cardInfos.Where(c => ReferenceEquals(c.Customer, item)).FirstOrDefault();
                    if (cust != null)
                        cardInfos.Remove(cust);
                }
            }

            //BindableLayout.SetItemsSource(cluc.stkContainer, cardInfos);
        }

        public CustomersListUC()
        {
            InitializeComponent();
        }

        public void RefreshItems()
        {
            if (cardInfos != null)
                foreach (var item in cardInfos)
                {
                    item.CustomerChanged();
                }
        }
    }
}