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
        public event PageClosedEventHandler PageClosed;

        public AddLogPage(Customer customer)
        {
            InitializeComponent();
            App.NavigationPage.BarBackgroundColor = Color.White;
            _customer = customer;
        }

        private void BtnSuccessful_Clicked(object sender, EventArgs e)
        {
            var addFactorPage = new SubmitFactorPage(_customer);
            addFactorPage.PageClosed += AddNewLogPage_PageClosed;
            App.NavigationPage.Navigation.PushAsync(addFactorPage);
        }

        private void AddNewLogPage_PageClosed(bool successful, CustomerLog log)
        {
            OnPageClosed(successful, log);
            App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
        }

        private void btnComment_Clicked(object sender, EventArgs e)
        {
            var addCommentPage = new SubmitCommentPage(_customer);
            addCommentPage.PageClosed += AddNewLogPage_PageClosed;
            App.NavigationPage.Navigation.PushAsync(addCommentPage);
        }

        private void OnPageClosed(bool successful, CustomerLog log)
        {
            PageClosed?.Invoke(successful, log);
        }

        private void btnFailed_Clicked(object sender, EventArgs e)
        {
            var addFailedPage = new SubmitFailedPage(_customer);
            addFailedPage.PageClosed += AddNewLogPage_PageClosed;
            App.NavigationPage.Navigation.PushAsync(addFailedPage);
        }
    }

    public delegate void PageClosedEventHandler(bool successful, CustomerLog log);
    public delegate void FactorDeletedEventHandler(Factor factor);
}