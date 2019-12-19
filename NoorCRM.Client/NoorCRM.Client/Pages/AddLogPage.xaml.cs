using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLogPage : ContentPage
    {
        private readonly Customer _customer;

        public AddLogPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
        }

        private void BtnSuccessful_Clicked(object sender, EventArgs e)
        {
            var addFactorPage = new SubmitFactorPage(_customer);
            App.NavigationPage.Navigation.PushAsync(addFactorPage);
        }
    }
}