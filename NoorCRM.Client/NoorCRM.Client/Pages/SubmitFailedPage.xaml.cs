using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitFailedPage : ContentPage
    {
        private readonly Customer _customer;
        public event PageClosedEventHandler PageClosed;

        public SubmitFailedPage(Customer customer)
        {
            InitializeComponent();
            picReason.Items.Add("نامعلوم");
            picReason.Items.Add("کار با سایر برندها");
            picReason.Items.Add("عدم فروش سفارش قبلی");
            picReason.Items.Add("کیفیت پایین");
            picReason.SelectedIndex = 0;
            _customer = customer;
        }

        private void OnPageClosed(bool successful, CustomerLog log)
        {
            PageClosed?.Invoke(successful, log);
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
                return;

            FailureReason reason = FailureReason.Unknown;
            if (picReason.SelectedIndex == 1)
                reason = FailureReason.HasAnotherBrand;
            else if (picReason.SelectedIndex == 2)
                reason = FailureReason.NotSoldLastOrder;
            else if (picReason.SelectedIndex == 3)
                reason = FailureReason.LowQuality;

            var flog = new FailedLog()
            {
                Description = txtDescription.Text,
                FailureReason = reason,
                CreationDate = DateTime.Now,
                CreatorUserId = App.MainViewModel.OnlineUser.Id,
                CustomerId = _customer.Id,
                IsVisitorsLog = true
            };

            var log = await App.ApiService.InsertNewLogAsync(flog).ConfigureAwait(true);
            if (log != null)
            {
                OnPageClosed(successful: true, log);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن علت عدم خرید با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
            else
            {
                OnPageClosed(successful: false, null);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن علت عدم خرید با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }

            await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
        }
    }
}