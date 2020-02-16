using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReminderPage : ContentPage
    {
        private ReminderViewModel _viewModel;
        public ReminderViewModel ViewModel => _viewModel;
        public AddReminderPage(DateTime? selected)
        {
            InitializeComponent();
            if (selected.HasValue)
                _viewModel = new ReminderViewModel(selected.Value);
            else
                _viewModel = new ReminderViewModel(DateTime.Now);

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    wlThisWeek.Children.Remove(chipSun);
                    break;
                case DayOfWeek.Monday:
                    wlThisWeek.Children.Remove(chipSun);
                    wlThisWeek.Children.Remove(chipMon);
                    break;
                case DayOfWeek.Tuesday:
                    wlThisWeek.Children.Remove(chipSun);
                    wlThisWeek.Children.Remove(chipMon);
                    wlThisWeek.Children.Remove(chipTue);
                    break;
                case DayOfWeek.Wednesday:
                    wlThisWeek.Children.Remove(chipSun);
                    wlThisWeek.Children.Remove(chipMon);
                    wlThisWeek.Children.Remove(chipTue);
                    wlThisWeek.Children.Remove(chipWed);
                    break;
                case DayOfWeek.Thursday:
                    wlThisWeek.Children.Remove(chipSun);
                    wlThisWeek.Children.Remove(chipMon);
                    wlThisWeek.Children.Remove(chipTue);
                    wlThisWeek.Children.Remove(chipWed);
                    wlThisWeek.Children.Remove(chipThu);
                    break;
                case DayOfWeek.Friday:
                    wlThisWeek.Children.Remove(chipSun);
                    wlThisWeek.Children.Remove(chipMon);
                    wlThisWeek.Children.Remove(chipTue);
                    wlThisWeek.Children.Remove(chipWed);
                    wlThisWeek.Children.Remove(chipThu);
                    wlThisWeek.Children.Remove(chipFri);
                    break;
            }

            BindingContext = _viewModel;
        }

        private int getDayDistance(DayOfWeek now, DayOfWeek selected)
        {
            int today = (((int)now) + 1) % 7;
            int sel = (((int)selected) + 1) % 7;
            return sel - today;
        }

        private void Today_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now);
        }

        private void Tomarrow_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(1));
        }

        private void TowDays_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(2));
        }

        private void Week_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(7));
        }

        private void TwoWeek_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(14));
        }

        private void t10Min_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddMinutes(10);
        }

        private void t20Min_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddMinutes(20);
        }

        private void t30Min_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddMinutes(30);
        }

        private void t1hour_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddHours(1);
        }

        private void t2hour_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddHours(2);
        }

        private void t3hour_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = DateTime.Now.AddHours(3);
        }

        private void Sun_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Sunday)));
        }

        private void Mon_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Monday)));
        }

        private void Tue_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Tuesday)));
        }

        private void Wed_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Wednesday)));
        }

        private void Thu_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Thursday)));
        }

        private void Fri_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Friday)));
        }

        private void NextSat_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Saturday) + 7));
        }

        private void NextSun_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Sunday) + 7));
        }

        private void NextMon_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Monday) + 7));
        }

        private void NextTue_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Tuesday) + 7));
        }

        private void NextWed_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Wednesday) + 7));
        }

        private void NextThu_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Thursday) + 7));
        }

        private void NextFri_Tapped(object sender, EventArgs e)
        {
            _viewModel.SetDate(DateTime.Now.AddDays(
                getDayDistance(DateTime.Now.DayOfWeek, DayOfWeek.Friday) + 7));
        }

        private void upMin_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddMinutes(1);
        }

        private void upHour_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddHours(1);
        }

        private void upDay_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(1);
        }

        private void upMonth_Tapped(object sender, EventArgs e)
        {
            var day = _viewModel.SelectedDateTime.DayOfYear;
            if (day >= 51 && day < 80)
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(29);
            else if (day >= 80 && day < 265)
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(31);
            else
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(30);
        }

        private void upYear_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddYears(1);
        }

        private void downMin_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddMinutes(-1);
        }

        private void downHour_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddHours(-1);
        }

        private void downDay_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(-1);
        }

        private void downMonth_Tapped(object sender, EventArgs e)
        {
            var day = _viewModel.SelectedDateTime.DayOfYear;
            if (day >= 80 && day < 109)
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(-29);
            else if (day >= 109 && day < 296)
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(-31);
            else
                _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddDays(-30);
        }

        private void downYear_Tapped(object sender, EventArgs e)
        {
            _viewModel.SelectedDateTime = _viewModel.SelectedDateTime.AddYears(-1);
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            _viewModel.IsSubmitted = true;
            _viewModel.IsRemoved = false;
            App.NavigationPage.Navigation.PopModalAsync();
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            _viewModel.IsSubmitted = false;
            _viewModel.IsRemoved = true;
            App.NavigationPage.Navigation.PopModalAsync();
        }
    }

    public class ReminderViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDateTime;
        public bool IsSubmitted { get; set; }
        public bool IsRemoved { get; set; }

        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set
            {
                if (_selectedDateTime == value)
                    return;
                _selectedDateTime = value;
                OnPropertyChanged();
            }
        }

        public ReminderViewModel()
        {
            IsSubmitted = false;
            IsRemoved = false;
        }

        public ReminderViewModel(DateTime selected)
        {
            IsSubmitted = false;
            IsRemoved = false;
            SelectedDateTime = selected;
        }

        public void SetDate(DateTime date)
        {
            SelectedDateTime = new DateTime(date.Year, date.Month, date.Day,
                SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
        }

        public void SetTime(DateTime time)
        {
            SelectedDateTime = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month,
                SelectedDateTime.Day, time.Hour, time.Minute, time.Second);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}