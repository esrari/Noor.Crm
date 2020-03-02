using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageListUC : ContentView
    {
        public ObservableCollection<Message> Messages
        {
            get { return (ObservableCollection<Message>)GetValue(MessagesProperty); }
            set { SetValue(MessagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty MessagesProperty =
            BindableProperty.Create(
                nameof(Messages),
                typeof(ObservableCollection<Message>),
                typeof(MessageListUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleMessagesChanged);

        private static ObservableCollection<MessageBoxViewModel> _messageBoxes;
        private static MessageListUC lluc;
        private static async void HandleMessagesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var messages = newValue as ObservableCollection<Message>;
            if (messages != null)
            {
                messages.CollectionChanged += Messages_CollectionChanged;
                _messageBoxes = new ObservableCollection<MessageBoxViewModel>();
                foreach (var item in messages)
                    _messageBoxes.Add(new MessageBoxViewModel(item));

                lluc = bindable as MessageListUC;
                BindableLayout.SetItemsSource(lluc.lstMessages, _messageBoxes);
                //await lluc.scvScroller.ScrollToAsync(lluc.lstMessages, ScrollToPosition.Start, false).ConfigureAwait(true);
            }
        }

        private static async void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Message item in e.NewItems)
                    _messageBoxes.Add(new MessageBoxViewModel(item));

                await lluc.scvScroller.ScrollToAsync(lluc.lstMessages, ScrollToPosition.End, false).ConfigureAwait(false);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Message item in e.OldItems)
                {
                    var m = _messageBoxes.Where(c => ReferenceEquals( c.Message, item)).FirstOrDefault();
                    if (m != null)
                        _messageBoxes.Remove(m);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Message item in e.NewItems)
                {
                    foreach (Message oitem in e.OldItems)
                    {
                        var m = _messageBoxes.Where(c => ReferenceEquals(c.Message, oitem)).FirstOrDefault();
                        m.Update(item);
                    }
                }
            }
        }

        public MessageListUC()
        {
            InitializeComponent();
        }

        private async void lstMessages_SizeChanged(object sender, EventArgs e)
        {
            await lluc.scvScroller.ScrollToAsync(lluc.lstMessages, ScrollToPosition.End, false).ConfigureAwait(false);
        }
    }

    public class MessageBoxViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isVisible;
        private bool _isSelfMessage;
        private Color _borderColor;
        private Color _backgroundColor;

        public Message Message { get; private set; }
        public string MessageText => Message?.Text;
        public DateTime CreationDate => Message != null ? Message.CreateDate : DateTime.Now;
        public string CtreatorName => Message?.User?.FullName;
        public bool IsSended => Message == null ? false : (Message.Id != 0) && IsSelfMessage;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (_backgroundColor == value)
                    return;
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value)
                    return;
                _borderColor = value;
                OnPropertyChanged();
            }
        }
        public bool IsSelfMessage
        {
            get => _isSelfMessage;
            set
            {
                if (_isSelfMessage == value)
                    return;
                _isSelfMessage = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value)
                    return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxViewModel(Message message)
        {
            setValues(message);
        }

        public void Update(Message message)
        {
            setValues(message);

            OnPropertyChanged(nameof(MessageText));
            OnPropertyChanged(nameof(CreationDate));
            OnPropertyChanged(nameof(CtreatorName));
            OnPropertyChanged(nameof(IsSended));
        }

        private void setValues(Message message)
        {
            Message = message;
            if (message != null)
            {
                if (message.IsDeleted)
                    IsVisible = false;
                else
                    IsVisible = true;
                // is online user owner of this message? 
                IsSelfMessage = message.UserId == App.MainViewModel.OnlineUser.Id;

                if(IsSelfMessage)
                    BackgroundColor = Color.LightSkyBlue;
                else
                    BackgroundColor = Color.LightGreen;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}