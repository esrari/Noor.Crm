using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NoorCRM.Client.Pages.Menu
{
    public class MenuPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _userTitle;
        private string _userPhoneNo;
        private double _userAccountBalance;

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
        public double UserAccountBalance
        {
            get => _userAccountBalance;
            set
            {
                if (_userAccountBalance == value)
                    return;
                _userAccountBalance = value;
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
        //public ICommand GoPayedCoursesCommand { get; set; }
        //public ICommand GoCurrentCoursesCommand { get; set; }
        //public ICommand GoTaggedCoursesCommand { get; set; }
        public ICommand GoAboutUsCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MenuPageViewModel()
        {
            GoMainCommand = new Command(GoMain);
            //GoPayedCoursesCommand = new Command(GoPayedCourses);
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

        //void GoPayedCourses(object obj)
        //{
        //    App.NavigationPage.Navigation.PushAsync(new PayedCourses()); //the content page you wanna load on this click event 
        //    App.MenuIsPresented = false;
        //}

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
