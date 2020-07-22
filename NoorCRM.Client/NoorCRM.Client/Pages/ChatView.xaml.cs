using NoorCRM.API.Models;
using NoorCRM.Client.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentView, IDisposable
    {
        private bool _checkerRun = false;
        private Timer _timer;
        public ChatView()
        {
            InitializeComponent();
            _timer = new Timer(5 * 60 * 1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(!_checkerRun)
                await CheckForNewMessagesAsync().ConfigureAwait(true);
        }

        private async Task CheckForNewMessagesAsync()
        {
            _checkerRun = true;
            var m = App.MainViewModel.Messages.Last(l => l.Id != 0);
            if (m.Id != 0)
            {
                var messages = await App.ApiService.GetNewMessagesAsync(20, m.Id).ConfigureAwait(true);
                if (messages != null)
                    foreach (var item in messages)
                        App.MainViewModel.Messages.Add(item);
            }
            _checkerRun = false;
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
                    await CheckForNewMessagesAsync().ConfigureAwait(true);
        }

        protected virtual void Dispose(bool type)
        {
            Dispose();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}