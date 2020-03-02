using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NoorCRM.Client.Pages.Menu
{
    public class MenuPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _userTitle;
        private string _userPhoneNo;

        public string UserTitle
        {
            get => _userTitle;
            set
            {
                if (_userTitle == value)
                    return;

                _userTitle = value;
                OnPropertyChanged();
            }
        }
        public string UserPhoneNo
        {
            get => _userPhoneNo;
            set
            {
                if (_userPhoneNo == value)
                    return;
                _userPhoneNo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region INotifyPropertyChanged

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands
        public ICommand GoMainCommand { get; set; }
        public ICommand GoReloadCommand { get; set; }
        //public ICommand GoCurrentCoursesCommand { get; set; }
        //public ICommand GoTaggedCoursesCommand { get; set; }
        public ICommand GoAboutUsCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MenuPageViewModel()
        {
            GoMainCommand = new Command(GoMain);
            GoReloadCommand = new Command(GoReload);
            //GoCurrentCoursesCommand = new Command(GoCurrentCourses);
            //GoTaggedCoursesCommand = new Command(GoTaggedCourses);
            GoAboutUsCommand = new Command(GoAboutUs);
            ExitCommand = new Command(Exit);
        }

        void GoMain(object obj)
        {
            App.NavigationPage.Navigation.PopToRootAsync();
            App.MainViewModel.HomeTabSelected = true;
            App.MenuIsPresented = false;
        }

        void GoReload(object obj)
        {
            App.NavigationPage.Navigation.PopToRootAsync();
            App.MainViewModel.HomeTabSelected = true;
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushModalAsync(
                new SplashPage(App.MainViewModel.OnlineUser));
        }

        //void GoCurrentCourses(object obj)
        //{
        //    App.NavigationPage.Navigation.PushAsync(new CurrentCourses());
        //    App.MenuIsPresented = false;
        //}

        //private void GoTaggedCourses(object obj)
        //{
        //    App.NavigationPage.Navigation.PushAsync(new TaggedCourses()); //the content page you wanna load on this click event 
        //    App.MenuIsPresented = false;
        //}

        private void GoAboutUs(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new AboutUsPage()); //the content page you wanna load on this click event 
            App.MenuIsPresented = false;
        }

        private void Exit(object obj)
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
