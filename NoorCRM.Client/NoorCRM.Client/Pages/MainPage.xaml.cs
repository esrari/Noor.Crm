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
        private bool _onlineUserFetched;
        private MainViewModel MainViewModel { get; set; }

        public MainPage(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            mainViewModel.HomeTabSelected = true;
            InitializeComponent();
            BindingContext = MainViewModel;

            bottomMenuItemsCreation();
        }

        public bool OnlineUserFetched
        {
            get => _onlineUserFetched;
            set
            {
                _onlineUserFetched = value;
                MainViewModel.UserCourses = App.ApiService.OnlineUserCourses;
                //For test
                MainViewModel.FirstAdBannerCourse = App.ApiService.OnlineUserCourses[0];
            }
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
                    Title = "خانه",
                    Icon = MaterialIcons.HomeOutline,
                    TextColor = bottomMenuSelectedColor,
                    TapCommand = new Command(OnHomeTapped),
                    GridColumnIndex = 0
                },
                new BottomMenuItem()
                {
                    Title = "دسته ها",
                    Icon = MaterialIcons.FolderOpenOutline,
                    TextColor = bottomMenuNormalColor,
                    TapCommand = new Command(OnSecondTapped),
                    GridColumnIndex = 1
                },
                new BottomMenuItem()
                {
                    Title = "سایر",
                    Icon = MaterialIcons.Star,
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

        public void SelectHomePage()
        {
            OnHomeTapped();
        }
        #endregion
    }
}
