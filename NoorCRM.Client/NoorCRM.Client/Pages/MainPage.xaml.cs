using NoorCRM.Client.Pages;
using NoorCRM.Client.Sources;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NoorCRM.Client
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainViewModel MainViewModel { get; set; }

        public MainPage(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            MainViewModel.HomeTabSelected = true;
            mainViewModel.HomeTabShowed += MainViewModel_HomeTabShowed;
            App.MainViewModel.SplashScreenSuccessfulClosed = true;
            InitializeComponent();
            bottomMenuItemsCreation();

            var splash = new SplashPage();
            splash.Disappearing += Splash_Disappearing;
            Navigation.PushModalAsync(splash)
                            .ConfigureAwait(true);

            BindingContext = MainViewModel;
        }

        private void Splash_Disappearing(object sender, EventArgs e)
        {
            if (!App.MainViewModel.SplashScreenSuccessfulClosed)
                Environment.Exit(0);
        }

        private void MainViewModel_HomeTabShowed(object sender, PropertyChangedEventArgs e)
        {
            OnHomeTapped();
        }

        #region Bottom Menu
        private Color bottomMenuNormalColor = Color.White;
        private Color bottomMenuSelectedColor = Color.Black;

        private void bottomMenuItemsCreation()
        {
            // Get colors from resources
            if (Application.Current.Resources.TryGetValue("BodyTextColor", out var menuNorObj))
                bottomMenuNormalColor = (Color)menuNorObj;
            if (Application.Current.Resources.TryGetValue("Primary", out var menuSelObj))
                bottomMenuSelectedColor = (Color)menuSelObj;

            MainViewModel.BottomMenuItems = new List<BottomMenuItem>()
            {
                new BottomMenuItem()
                {
                    Title = "کالا",
                    Icon = MaterialIcons.LibraryBooks,
                    TextColor = bottomMenuSelectedColor,
                    TapCommand = new Command(OnHomeTapped),
                    GridColumnIndex = 0
                },
                new BottomMenuItem()
                {
                    Title = "مشتریان",
                    Icon = MaterialIcons.AccountGroup,
                    TextColor = bottomMenuNormalColor,
                    TapCommand = new Command(OnSecondTapped),
                    GridColumnIndex = 1
                },
                new BottomMenuItem()
                {
                    Title = "سفارشات",
                    Icon = MaterialIcons.CartOutline,
                    TextColor = bottomMenuNormalColor,
                    TapCommand = new Command(OnThirdTapped),
                    GridColumnIndex = 2
                },
            };
        }

        private void OnHomeTapped()
        {
            setBottomMenuSelectedItem(0);
            MainViewModel.HomeTabSelected = true;
        }

        private void OnSecondTapped()
        {
            setBottomMenuSelectedItem(1);
            MainViewModel.SecondTabSelected = true;
        }

        private void OnThirdTapped()
        {
            setBottomMenuSelectedItem(2);
            MainViewModel.ThirdTabSelected = true;
        }

        private void setBottomMenuSelectedItem(int selectedIndex)
        {
            foreach (var item in MainViewModel.BottomMenuItems)
            {
                item.TextColor = bottomMenuNormalColor;
            }

            if (MainViewModel.BottomMenuItems.Count > selectedIndex)
                MainViewModel.BottomMenuItems[selectedIndex].TextColor = bottomMenuSelectedColor;
        }
        #endregion

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SplashPage())
                .ConfigureAwait(true);
        }
    }
}
