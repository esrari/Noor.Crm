using NoorCRM.API.Models;
using System.Collections.Generic;
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
        public IEnumerable<Customer> Customers
        {
            get { return (IEnumerable<Customer>)GetValue(CustomersProperty); }
            set { SetValue(CustomersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CustomersProperty =
            BindableProperty.Create(
                nameof(Customers),
                typeof(IEnumerable<Customer>),
                typeof(CustomersListUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleCustomersChanged);

        private static async void HandleCustomersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customers = newValue as IEnumerable<Customer>;
            if (customers != null)
            {
                var cardInfos = new List<CustomerCardInfo>();
                foreach (var c in customers)
                    cardInfos.Add(new CustomerCardInfo(c, App.NavigationPage.Navigation));

                var cluc = (CustomersListUC)bindable;
                BindableLayout.SetItemsSource(cluc.stkContainer, cardInfos);
                await Task.Delay(3);
                await cluc.scvScroller.ScrollToAsync(cluc.stkContainer, ScrollToPosition.End, false);
            }
        }
        public CustomersListUC()
        {
            InitializeComponent();
        }
    }
}