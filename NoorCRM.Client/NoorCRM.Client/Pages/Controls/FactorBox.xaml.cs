using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FactorBox : ContentView
    {
        public FactorBox(Factor factor)
        {
            InitializeComponent();
            BindingContext = new FactorBoxViewModel(factor);
        }
    }

    public class FactorBoxViewModel
    {
        public Factor Factor { get; set; }
        public string Title { get; set; }
        public string CityName { get; set; }
        public double TotalPrice { get; set; }
        public DateTime RegisterDate { get; set; }
        public Command TapCommand { get; set; }

        public Color MainColor { get; set; }
        public Color BackColor { get; set; }

        public FactorBoxViewModel()
        {

        }

        public FactorBoxViewModel(Factor factor)
        {
            // Fill card from factor
            Factor = factor;
            Title = factor?.Customer?.GetTitle();
            CityName = factor.Customer?.City?.Name;
            TotalPrice = factor.GetTotalPrice();
            RegisterDate = factor.CreateDate;
            TapCommand = new Command<Factor>(new Action<Factor>(f =>
            {
                App.NavigationPage.Navigation.PushAsync(new SubmitFactorPage(f));
            }));

            MainColor = Color.Black;
            BackColor = Color.White;
            // Get colors from resources
            object mainColorObj, backColorObj;
            switch (factor.Status)
            {
                case FactorStatus.New:
                    if (Application.Current.Resources.TryGetValue("Primary", out mainColorObj))
                        MainColor = (Color)mainColorObj;
                    if (Application.Current.Resources.TryGetValue("PrimaryLight", out backColorObj))
                        BackColor = (Color)backColorObj;
                    break;
                case FactorStatus.Proceeded:
                    if (Application.Current.Resources.TryGetValue("Green", out mainColorObj))
                        MainColor = (Color)mainColorObj;
                    if (Application.Current.Resources.TryGetValue("LightGreen", out backColorObj))
                        BackColor = (Color)backColorObj;
                    break;
                case FactorStatus.Rejected:
                    if (Application.Current.Resources.TryGetValue("Secondary", out mainColorObj))
                        MainColor = (Color)mainColorObj;
                    if (Application.Current.Resources.TryGetValue("SecondaryLight", out backColorObj))
                        BackColor = (Color)backColorObj;
                    break;
                case FactorStatus.Printed:
                    if (Application.Current.Resources.TryGetValue("BorderGrayColor", out mainColorObj))
                        MainColor = (Color)mainColorObj;
                    if (Application.Current.Resources.TryGetValue("BackgroundGrayColor", out backColorObj))
                        BackColor = (Color)backColorObj;
                    break;
                default:
                    break;
            }

        }
    }
}