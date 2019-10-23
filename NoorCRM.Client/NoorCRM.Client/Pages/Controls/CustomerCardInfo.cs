using NoorCRM.API.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NoorCRM.Client.Pages.Controls
{
    public class CustomerCardInfo
    {
        public Customer Customer { get; }
        public string Title { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }

        public ICommand TapCommand { get; set; }

        public CustomerCardInfo(Customer customer, INavigation navigation)
        {
            Customer = customer;
            if (!string.IsNullOrWhiteSpace(customer.StoreName))
                Title = $"{customer.StoreName.Trim()} ({customer.ManagerName.Trim()})";
            else Title = customer.ManagerName.Trim();

            CityName = customer.City.Name;
            Address = customer.Address;
            TapCommand = new Command<Customer>(new Action<Customer>(c =>
            {
                navigation.PushAsync(new CustomerPage(c));
            }));
        }
    }
}