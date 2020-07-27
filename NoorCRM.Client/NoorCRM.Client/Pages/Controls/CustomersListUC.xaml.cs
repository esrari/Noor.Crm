using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
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
        private static SortedList<string, CustomerCardInfo> cardInfos;
        private static ObservableCollection<Customer> customers;
        private static int selectedCity = -1;
        private static string searchfilter = "";
        private static async void HandleCustomersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            cluc = (CustomersListUC)bindable;
            customers = newValue as ObservableCollection<Customer>;
            customers.CollectionChanged += Customers_CollectionChanged;
            if (customers != null)
                setToList(customers);
        }

        private static void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //var cardInfos = new List<CustomerCardInfo>(BindableLayout.GetItemsSource(cluc.stkContainer).Cast<CustomerCardInfo>());

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer item in e.NewItems)
                {
                    var cci = new CustomerCardInfo(item, App.NavigationPage.Navigation);
                    cardInfos.Add(getCustomerKey(item), cci);
                    cci.TapCommand.Execute(cci.Customer);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer item in e.OldItems)
                {
                    var cust = cardInfos.Where(c => ReferenceEquals(c.Value.Customer, item)).FirstOrDefault();
                    if (cust.Value != null)
                        cardInfos.Remove(getCustomerKey(item));
                }
            }
        }



        public CustomersListUC()
        {
            InitializeComponent();
        }

        public int Filter(string filter)
        {
            searchfilter = filter.Trim();
            return dualFilter();
        }

        public int CityFilter(int cityId)
        {
            selectedCity = cityId;
            return dualFilter();
        }

        private int dualFilter()
        {
            List<Customer> fcust = new List<Customer>();
            if (string.IsNullOrWhiteSpace(searchfilter) && selectedCity == -1)
            {
                setToList(customers);
                return customers.Count;
            }
            else if (string.IsNullOrWhiteSpace(searchfilter))
            {
                foreach (var item in customers)
                    if (item.CityId == selectedCity)
                        fcust.Add(item);
            }
            else if (selectedCity == -1)
            {
                foreach (var item in customers)
                {
                    if(((item.ManagerName != null) && (item.ManagerName.Contains(searchfilter))) || 
                        ((item.StoreName != null) && (item.StoreName.Contains(searchfilter))))
                    {
                        fcust.Add(item);
                        continue;
                    }
                    if (item.PhoneNos != null)
                    {
                        foreach (var ph in item.PhoneNos)
                            if (ph.Number != null)
                            {
                                string numberEn = ph.Number.WithEnglishDigits();
                                string filterEn = searchfilter.WithEnglishDigits();
                                if (numberEn.Contains(filterEn))
                                {
                                    fcust.Add(item);
                                    break;
                                }
                            }
                    }
                }
            }
            else
            {
                foreach (var item in customers)
                    if (item.ManagerName.Contains(searchfilter) || item.StoreName.Contains(searchfilter))
                        if (item.CityId == selectedCity)
                            fcust.Add(item);
            }

            setToList(fcust);
            return fcust.Count;
        }

        public void RefreshItems()
        {
            if (cardInfos != null)
                foreach (var item in cardInfos)
                    item.Value.CustomerChanged();
        }

        private static void setToList(IEnumerable<Customer> customers)
        {
            cardInfos = new SortedList<string, CustomerCardInfo>();
            foreach (var c in customers)
                cardInfos.Add(getCustomerKey(c), new CustomerCardInfo(c, App.NavigationPage.Navigation));
            cluc.stkContainer.ItemsSource = cardInfos.Values;
        }

        private static string getCustomerKey(Customer c)
        {
            return c.ManagerName + "(" + c.StoreName + ")-" + c.Id;
        }

        private void stkContainer_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var card = e.Item as CustomerCardInfo;
            if (card != null)
            {
                card.TapCommand.Execute(card.Customer);
            }
        }
    }
}