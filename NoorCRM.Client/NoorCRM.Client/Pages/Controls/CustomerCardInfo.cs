using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace NoorCRM.Client.Pages.Controls
{
    public class CustomerCardInfo: INotifyPropertyChanged
    {
        public Customer Customer { get; }
        public string Title => Helper.CreateCustomerTitle(Customer);
        public string CityName => Customer.City?.Name;
        public string Address => Customer.Address;
        public DateTime Remider => Customer.Reminder.HasValue ? Customer.Reminder.Value : DateTime.Now.AddDays(1);
        public bool HasAnyFactor => Customer.Factors?.Count > 0;
        public bool HasReminder => Customer.Reminder.HasValue;

        public ICommand TapCommand { get; set; }

        public CustomerCardInfo(Customer customer, INavigation navigation)
        {
            Customer = customer;
            
            TapCommand = new Command<Customer>(new Action<Customer>(c =>
            {
                var page = new CustomerPage(c);
                page.Disappearing += Page_Disappearing;
                navigation.PushAsync(page);
            }));
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            CustomerChanged();
        }

        public void CustomerChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(CityName));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(HasAnyFactor));
            OnPropertyChanged(nameof(HasReminder)); 
            OnPropertyChanged(nameof(Remider));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}