using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace NoorCRM.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel This => this;

        #region AdBanner Command
        private Command _firstAdBannerCommand;
        private Course _firstAdBannerCourse;
        public Command FirstAdBannerCommand
        {
            get { return _firstAdBannerCommand; }
            set
            {
                if (ReferenceEquals(_firstAdBannerCommand, value))
                    return;
                _firstAdBannerCommand = value;
                OnPropertyChanged();
            }
        }
        public Course FirstAdBannerCourse
        {
            get { return _firstAdBannerCourse; }
            set
            {
                if (ReferenceEquals(_firstAdBannerCourse, value))
                    return;
                _firstAdBannerCourse = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Bottom Menu
        #region Bottom Tab Visiblity
        private bool _homeTabSelected;
        private bool _secondTabSelected;
        private bool _thirdTabSelected;

        public bool HomeTabSelected
        {
            get { return _homeTabSelected; }
            set
            {
                if (_homeTabSelected == value)
                    return;
                _homeTabSelected = value;
                // Manage other tabs
                if (value)
                {
                    SecondTabSelected = false;
                    ThirdTabSelected = false;
                }
                OnPropertyChanged();
            }
        }

        public bool SecondTabSelected
        {
            get { return _secondTabSelected; }
            set
            {
                if (_secondTabSelected == value)
                    return;
                _secondTabSelected = value;
                // Manage other tabs
                if (value)
                {
                    HomeTabSelected = false;
                    ThirdTabSelected = false;
                }
                OnPropertyChanged();
            }
        }

        public bool ThirdTabSelected
        {
            get { return _thirdTabSelected; }
            set
            {
                if (_thirdTabSelected == value)
                    return;
                _thirdTabSelected = value;
                // Manage other tabs
                if (value)
                {
                    HomeTabSelected = false;
                    SecondTabSelected = false;
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region Button List
        private IList<BottomMenuItem> _bottomMenuItems;
        public IList<BottomMenuItem> BottomMenuItems
        {
            get => _bottomMenuItems;
            set
            {
                if (ReferenceEquals(_bottomMenuItems, value))
                    return;

                _bottomMenuItems = value;
                OnPropertyChanged();
            }
        } 
        #endregion
        #endregion

        #region User Courses
        private IEnumerable<Course> _userCourses = new List<Course>();
        public IEnumerable<Course> UserCourses
        {
            get => _userCourses;
            set
            {
                if (ReferenceEquals(_userCourses, value))
                    return;
                _userCourses = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Free Courses
        private IEnumerable<Course> _freeCourses = new List<Course>();
        public IEnumerable<Course> FreeCourses
        {
            get => _freeCourses;
            set
            {
                if (ReferenceEquals(_freeCourses, value))
                    return;
                _freeCourses = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region New Courses
        private IEnumerable<Course> _newCourses = new List<Course>();
        public IEnumerable<Course> NewCourses
        {
            get => _newCourses;
            set
            {
                if (ReferenceEquals(_newCourses, value))
                    return;
                _newCourses = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Categories
        private IEnumerable<Category> _categories = new List<Category>();
        public IEnumerable<Category> Categories
        {
            get => _categories;
            set
            {
                if (ReferenceEquals(_categories, value))
                    return;
                _categories = value;
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
    }
}
