using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoorCRM.Client.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerInfoPage : ContentPage
    {
        private EditCustomerViewModel _viewModel;
        public CustomerInfoPage(Customer customer)
        {
            InitializeComponent();
            if (customer != null)
            {
                _viewModel = new EditCustomerViewModel(customer);
                BindingContext = _viewModel;

                if (customer.HasLocation)
                {
                    var pins = new ObservableCollection<Pin>();
                    pins.Add(new Pin()
                    {
                        Position = new Position(customer.Latitude, customer.Longitude),
                        Address = customer.Address,
                        Label = customer.CreateCustomerTitle()
                    });

                    map.Pins = pins;
                }
            }
        }
    }
}