using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using NoorCRM.Client.Pages;
using NoorCRM.Client.Pages.Menu;
using NoorCRM.Client.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public static MenuPageViewModel MenuPageViewModel { get; set; }
        public static AddFactorItemPage AddItemPage { get; set; }

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

            callMain();
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
