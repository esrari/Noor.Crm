using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls.Logs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogBox : ContentView
    {
        private readonly CustomerLog _log;
        private LogBoxViewModel _viewModel;

        public LogBox(CustomerLog log)
        {
            InitializeComponent();
            _log = log;
            _viewModel = new LogBoxViewModel();

            if (log != null)
            {
                _viewModel.LogType = log.LogType;
                _viewModel.LogTypePersianName = log.LogType.PersianName();
                _viewModel.CreationDate = log.CreationDate;
                _viewModel.CtreatorName = log.CreatorUser?.FullName;
                if (Application.Current.Resources.TryGetValue("BackgroundGrayColor", out var defaultColor))
                    _viewModel.BackgroundColor = (Color)defaultColor;

                switch (log.LogType)
                {
                    case LogType.Successful:
                        SuccessfulLogSettings(log, _viewModel);
                        break;
                    case LogType.Failed:
                        FailedLogSettings(log, _viewModel);
                        break;
                    case LogType.Comment:
                        CommentLogSettings(log, _viewModel);
                        break;
                    case LogType.RejectFactor:
                        RejectFactorLogSettings(log, _viewModel);
                        break;
                    case LogType.SubmitFactor:
                        SubmitFactorLogSettings(log, _viewModel);
                        break;
                    case LogType.PhoneCall:
                        break;
                    case LogType.StoreClosed:
                        break;
                    case LogType.ManagerName:
                        break;
                    case LogType.StoreName:
                        break;
                    case LogType.StoreReplaced:
                        break;
                    case LogType.WorkTimeChanged:
                        break;
                    case LogType.Photo:
                        break;
                    case LogType.PhoneNo:
                        break;
                    case LogType.VisitorChanged:
                        break;
                    case LogType.CityChanged:
                        break;
                    default:
                        throw new InvalidCastException("نوع لاگ تعریف نشده است.");
                }
            }

            BindingContext = _viewModel;
        }

        private async void SuccessfulLogSettings(CustomerLog customerLog, LogBoxViewModel model)
        {
            var log = customerLog as SuccessfulLog;
            model.Description = log.Description;
            if (Application.Current.Resources.TryGetValue("PrimaryLight", out var color))
                model.BackgroundColor = (Color)color;
            model.Factor = await App.ApiService.GetFactorAsync(log.FactorId).ConfigureAwait(true);
        }

        private void FailedLogSettings(CustomerLog customerLog, LogBoxViewModel model)
        {
            var log = customerLog as FailedLog;
            model.Description = log.Description;
            model.ExtraDescription = log.FailureReason.PersianName();
            model.ExtraDescriptionLabel = "علت رد خرید";
        }

        private void CommentLogSettings(CustomerLog customerLog, LogBoxViewModel model)
        {
            var log = customerLog as CommentLog;
            model.Description = log.Comment;
            model.ExtraDescription = log.ImportanceLevel.PersianName();
            model.ExtraDescriptionLabel = "درجه اهمیت";
        }

        private void RejectFactorLogSettings(CustomerLog customerLog, LogBoxViewModel model)
        {
            var log = customerLog as RejectFactorLog;
            model.Description = log.Description;
            if (Application.Current.Resources.TryGetValue("Secondary", out var color))
                model.BackgroundColor = (Color)color;
        }

        private void SubmitFactorLogSettings(CustomerLog customerLog, LogBoxViewModel model)
        {
            var log = customerLog as SubmitFactorLog;
            model.Description = log.Description;
            if (Application.Current.Resources.TryGetValue("PrimaryDark", out var color))
                model.BackgroundColor = (Color)color;
        }
    }

    public class LogBoxViewModel : INotifyPropertyChanged
    {
        private Factor _factor;

        public event PropertyChangedEventHandler PropertyChanged;

        public LogType LogType { get; set; }
        public Color BackgroundColor { get; set; }
        public string Description { get; set; }
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public string ExtraDescription { get; set; }
        public string ExtraDescriptionLabel { get; set; }
        public bool HasExtraDescription => !string.IsNullOrWhiteSpace(ExtraDescription);

        public DateTime CreationDate { get; set; }
        public string CtreatorName { get; set; }
        public string LogTypePersianName { get; set; }

        public bool FactorTapable => Factor != null;
        public Factor Factor
        {
            get => _factor;
            set
            {
                _factor = value;
                if (value == null)
                    TapCommand = null;
                else
                    TapCommand = new Command<Factor>(new Action<Factor>(f =>
                    {
                        if (f != null)
                            App.NavigationPage.Navigation.PushAsync(new SubmitFactorPage(f));
                    }));

                OnPropertyChanged();
                OnPropertyChanged(nameof(FactorTapable));
                OnPropertyChanged(nameof(TapCommand));
            }
        }
        public Command TapCommand { get; private set; }

        public LogBoxViewModel()
        {

        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}