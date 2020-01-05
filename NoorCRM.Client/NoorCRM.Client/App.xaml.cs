using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Menu;
using NoorCRM.Client.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NoorCRM.Client
{

    // Release with this options 
    // https://docs.microsoft.com/en-us/xamarin/android/deploy-test/release-prep/?tabs=windows#Specify_the_Application_Icon
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
            ApiService = new ServiceManager(new RestService(SoftwareSettings.BaseAddress));
            ApiService.OnlineUserFetched += apiService_OnlineUserFetched;
            ApiService.ExtractedUserPhoneNo = "9125464496";
            //ApiService.ExtractedUserPhoneNo = "9125554444";
            Task.Run(() => ApiService.GetOnlineUserAsync());

            callMain();
        }

        private async void apiService_OnlineUserFetched(User user)
        {
            // Set AppViewModel with catched data
            MainViewModel.OnlineUser = user;
            MenuPageViewModel.UserTitle = user.FullName;
            MenuPageViewModel.UserPhoneNo = user.PhoneNo;
            _MainPage.OnlineUserFetched = true;
            MainViewModel.Customers = new ObservableCollection<Customer>( await ApiService.GetUserCustomersAsync().ConfigureAwait(true));
            MainViewModel.Products = new ObservableCollection<Product>(await ApiService.GetAllProductsAsync().ConfigureAwait(false));
            //MainViewModel.Categories = await ApiService.GetRootCategoreisAsync();


            // Set first city if exist as default city
            if (user.VisitCities != null && user.VisitCities.Count > 0)
                MainViewModel.DefaultCity = (user.VisitCities as IList<City>)[0];
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
