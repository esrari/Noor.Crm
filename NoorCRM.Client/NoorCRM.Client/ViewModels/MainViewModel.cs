using NoorCRM.API.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace NoorCRM.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel This => this;
        public bool SplashScreenSuccessfulClosed { get; set; }

        public string Conn
        {
            get => _conn;
            set
            {
                _conn = value;
                OnPropertyChanged();
            }
        }

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
                    OnHomeTabShowed();
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

        #region Products
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                if (ReferenceEquals(_products, value))
                    return;
                _products = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Last Factors
        private ObservableCollection<Factor> _lastFactors = new ObservableCollection<Factor>();
        public ObservableCollection<Factor> LastFactors
        {
            get => _lastFactors;
            set
            {
                if (ReferenceEquals(_lastFactors, value))
                    return;
                _lastFactors = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Customers
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                if (ReferenceEquals(_customers, value))
                    return;
                _customers = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Online User
        private User _onlineUser;
        private City _defaultCity;
        private string _conn;

        public User OnlineUser
        {
            get => _onlineUser;
            set
            {
                if (ReferenceEquals(_onlineUser, value))
                    return;
                _onlineUser = value;
                OnPropertyChanged();
            }
        }

        public City DefaultCity
        {
            get => _defaultCity;
            set
            {
                if (ReferenceEquals(_defaultCity, value))
                    return;
                _defaultCity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region INotifyPropertyChanged
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnHomeTabShowed([CallerMemberName] string propertyName = null)
        {
            HomeTabShowed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler HomeTabShowed;
        #endregion

        public MainViewModel()
        {
            CheckWifiOnStart();
            CheckWifiContinuously();
        }

        public void CheckWifiOnStart()
        {
            Conn = CrossConnectivity.Current.IsConnected ? "wifi_on.png" : "wifi_off.png";
        }

        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                Conn = args.IsConnected ? "wifi_on.png" : "wifi_off.png";
            };
        }
    }
}
