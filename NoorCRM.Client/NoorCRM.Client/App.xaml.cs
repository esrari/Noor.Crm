using NoorCRM.API.Models;
using NoorCRM.Client.Data;
using NoorCRM.Client.Pages.Menu;
using NoorCRM.Client.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client
{
    public partial class App : Application
    {
        public static ServiceManager ApiService { get; private set; }
        public static MasterDetailPage RootPage { get; private set; }
        public static NavigationPage NavigationPage { get; private set; }
        public static MainPage _MainPage { get; set; }
        public static MainViewModel MainViewModel { get; set; }
        public MenuPageViewModel MenuPageViewModel { get; set; }

        public static bool MenuIsPresented
        {
            get { return RootPage.IsPresented; }
            set { RootPage.IsPresented = value; }
        }

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this, "Material.Configuration");
            //Material.PlatformConfiguration.ChangeStatusBarColor(new Color(255, 0, 0));

            MenuPageViewModel = new MenuPageViewModel();
            ApiService = new ServiceManager(new RestService());
            ApiService.OnlineUserFetched += apiService_OnlineUserFetched;
            ApiService.ExtractedUserPhoneNo = "9125464496";
            //ApiService.ExtractedUserPhoneNo = "9125554444";
            Task.Run(() => ApiService.GetOnlineUserAsync());

            callMain();
        }

        private async void apiService_OnlineUserFetched(User user)
        {
            // Set AppViewModel with catched data
            MenuPageViewModel.UserTitle = user.FullName;
            MenuPageViewModel.UserPhoneNo = user.PhoneNo;
            _MainPage.OnlineUserFetched = true;
            MainViewModel.Customers = await ApiService.GetUserCustomersAsync();
            //MainViewModel.NewCourses = await ApiService.GetNewCoursesAsync();
            //MainViewModel.Categories = await ApiService.GetRootCategoreisAsync();
        }

        private void callMain()
        {
            MainViewModel = new MainViewModel();
            _MainPage = new MainPage(MainViewModel);
            var menuPage = new MenuPage(MenuPageViewModel);
            NavigationPage = new NavigationPage(_MainPage);
            RootPage = new MasterDetailPage()
            {
                FlowDirection = FlowDirection.RightToLeft,
                Master = menuPage,
                Detail = NavigationPage
            };
            MainPage = RootPage;
        }
    }
}
