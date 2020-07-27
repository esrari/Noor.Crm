using NoorCRM.API.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        private string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/phoneNo.txt";

        public event OnlineUserFetchedEventHandler OnlineUserFetched;
        public SplashPage()
        {
            InitializeComponent();

            App.ApiService.OnlineUserFetched += apiService_OnlineUserFetched;
            _ = getUserPassAsync();
        }

        public SplashPage(User user)
        {
            InitializeComponent();
            if (user != null)
                loadAllOtherData(user).ConfigureAwait(false);
            else
            {
                App.ApiService.OnlineUserFetched += apiService_OnlineUserFetched;
                _ = getUserPassAsync();
            }
        }

        private async void apiService_OnlineUserFetched(User user)
        {
            if (user != null)
            {
                App.MainViewModel.OnlineUser = user;
                App.MenuPageViewModel.UserTitle = user.FullName;
                App.MenuPageViewModel.UserPhoneNo = user.PhoneNo;
                await loadAllOtherData(user).ConfigureAwait(false);
            }
        }

        private async Task loadAllOtherData(User user)
        {
            // Set AppViewModel with catched data
            App.MainViewModel.Customers = new ObservableCollection<Customer>(await App.ApiService.GetUserCustomersAsync().ConfigureAwait(true));
            //App.MainViewModel.TodayCustomers = new ObservableCollection<Customer>(await App.ApiService.GetUserTodayCustomersAsync().ConfigureAwait(true));
            App.MainViewModel.Products = new ObservableCollection<Product>(await App.ApiService.GetAllProductsAsync().ConfigureAwait(false));
            App.MainViewModel.LastFactors = new ObservableCollection<Factor>(await App.ApiService.GetLastVisitorFactorsAsync(user.Id, 20).ConfigureAwait(false));
            App.MainViewModel.Messages = new ObservableCollection<Message>(await App.ApiService.GetNewMessagesAsync(20).ConfigureAwait(false));

            Random random = new Random();
            // get today customers from all customers
            SortedDictionary<DateTime, Customer> todayCustomersDict = new SortedDictionary<DateTime, Customer>();
            foreach (Customer item in App.MainViewModel.Customers)
                if (item.Reminder.HasValue)
                {
                    if (todayCustomersDict.ContainsKey(item.Reminder.Value))
                        item.Reminder = item.Reminder.Value.AddMilliseconds(random.Next(1, 10));
                    todayCustomersDict.Add(item.Reminder.Value, item);
                }
            App.MainViewModel.TodayCustomers = new ObservableCollection<Customer>(todayCustomersDict.Values);

            // Set first city if exist as default city
            if (user.VisitCities != null && user.VisitCities.Count > 0)
                App.MainViewModel.DefaultCity = (user.VisitCities as IList<City>)[0];

            OnlineUserFetched?.Invoke();
            App.MainViewModel.SplashScreenSuccessfulClosed = true;
            await App.NavigationPage.Navigation.PopModalAsync().ConfigureAwait(false);
        }

        private async Task getUserPassAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                lblnet.IsVisible = true;
                lblRefresh.IsVisible = true;
                indicator.IsVisible = false;

                return;
            }
            lblnet.IsVisible = false;
            lblRefresh.IsVisible = false;
            indicator.IsVisible = true;

            User user = null;
            if (App.ApiService.OnlineUser == null)
            {
                if (File.Exists(fileName))
                {
                    var lines = new List<string>(File.ReadLines(fileName));
                    if (lines.Count > 1)
                    {
                        string username = lines[0].Trim();
                        string password = lines[1].Trim();
                        user = await checkNumber(username, password).ConfigureAwait(true);
                    }
                }
            }
            else
            {
                user = App.ApiService.OnlineUser;
                await loadAllOtherData(user).ConfigureAwait(false);
            }

            if (user == null)
            {
                btnSubmit.IsVisible = true;
                txtUsername.IsVisible = true;
                txtPassword.IsVisible = true;
                indicator.IsRunning = false;
            }
        }

        private async Task<User> checkNumber(string username, string password)
        {
            btnSubmit.IsVisible = false;
            txtUsername.IsVisible = false;
            txtPassword.IsVisible = false;
            indicator.IsRunning = true;
            App.ApiService.ExtractedUsername = username;
            App.ApiService.ExtractedPassword = password;
            return await App.ApiService.GetOnlineUserAsync().ConfigureAwait(true);
        }

        private async void MaterialButton_Clicked(object sender, EventArgs e)
        {
            User user = await checkNumber(txtUsername.Text.Trim(), txtPassword.Text).ConfigureAwait(true);
            if (user != null)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                File.WriteAllText(fileName, $"{txtUsername.Text.Trim()}{Environment.NewLine}{txtPassword.Text.Trim()}");
            }
            else
            {
                btnSubmit.IsVisible = true;
                txtUsername.IsVisible = true;
                txtPassword.IsVisible = true;
                indicator.IsRunning = false;
                await MaterialDialog.Instance.SnackbarAsync(message: "نام کاربری یا رمز عبور صحیح نمی باشد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await getUserPassAsync().ConfigureAwait(false);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            App.MainViewModel.SplashScreenSuccessfulClosed = false;
        }
    }

    public delegate void OnlineUserFetchedEventHandler();
}