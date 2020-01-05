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
    public partial class SubmitCommentPage : ContentPage
    {
        private readonly Customer _customer;
        public event PageClosedEventHandler PageClosed;

        public SubmitCommentPage(Customer customer)
        {
            InitializeComponent();

            picImportance.Items.Add("عادی");
            picImportance.Items.Add("مهم");
            picImportance.Items.Add("خیلی مهم");
            picImportance.SelectedIndex = 0;
            _customer = customer;
        }

        private void OnPageClosed(bool successful, CustomerLog log)
        {
            PageClosed?.Invoke(successful, log);
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtComment.Text))
                return;

            ImportanceLevel level = ImportanceLevel.Normal;
            if (picImportance.SelectedIndex == 1)
                level = ImportanceLevel.Important;
            else if (picImportance.SelectedIndex == 2)
                level = ImportanceLevel.Critical;

            var comlog = new CommentLog()
            {
                Comment = txtComment.Text,
                ImportanceLevel = level,
                CreationDate = DateTime.Now,
                CreatorUserId = App.MainViewModel.OnlineUser.Id,
                CustomerId = _customer.Id,
                IsVisitorsLog = true
            };

            var log = await App.ApiService.InsertNewLogAsync(comlog).ConfigureAwait(true);
            if (log != null)
            {
                OnPageClosed(successful: true, log);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن توضیح جدید با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
            else
            {
                OnPageClosed(successful: false, null);
                await MaterialDialog.Instance.SnackbarAsync(message: "افزودن توضیح جدید با مشکل روبرو شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }

            await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
        }
    }
}