using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
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
    public partial class ChatView : ContentView
    {
        private bool _checkerRun = false;
        public ChatView()
        {
            InitializeComponent();
        }

        private async Task CheckForNewMessagesAsync()
        {
            var m = App.MainViewModel.Messages.Last(l => l.Id != 0);
            if (m.Id != 0)
            {
                var messages = await App.ApiService.GetNewMessagesAsync(20, m.Id).ConfigureAwait(true);
                if (messages != null)
                    foreach (var item in messages)
                        App.MainViewModel.Messages.Add(item);
            }
            await Task.Delay(60000).ConfigureAwait(true);
            await CheckForNewMessagesAsync().ConfigureAwait(true);
        }

        private async Task sendMessageAsync(Message message = null)
        {
            if (message == null)
            {
                string text = txtMessage.Text.Trim();
                if (string.IsNullOrWhiteSpace(text))
                    return;

                message = new Message()
                {
                    Id = 0,
                    CreateDate = DateTime.Now,
                    Text = text,
                    UserId = App.MainViewModel.OnlineUser.Id,
                };
                // Add new message to end of message with id=0
                // it means this message not sended yet
                App.MainViewModel.Messages.Add(message);
            }
            Message sendedMessage = await App.ApiService.SendMessageAsync(message).ConfigureAwait(true);
            if (sendedMessage != null)
            {
                sendedMessage.User = App.MainViewModel.OnlineUser;
                App.MainViewModel.Messages.Replace(message, sendedMessage);
                txtMessage.Text = "";
            }
            else
            {
                var confirm = await MaterialDialog.Instance.ConfirmAsync(message: "ارسال پیام ناموفق بود. آیا تمایل به ارسال مجدد دارید؟",
                    confirmingText: "بله",
                    dismissiveText: "خیر").ConfigureAwait(true);

                if (confirm.HasValue && confirm.Value)
                    await sendMessageAsync(message).ConfigureAwait(false);
                else
                    App.MainViewModel.Messages.Remove(message);
            }
        }

        private async void lblSend_Tapped(object sender, EventArgs e)
        {
            await sendMessageAsync().ConfigureAwait(false);
        }

        private async void ContentView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsVisible))
                if (IsVisible && !_checkerRun)
                {
                    _checkerRun = true;
                    await CheckForNewMessagesAsync().ConfigureAwait(true);
                }
        }
    }
}