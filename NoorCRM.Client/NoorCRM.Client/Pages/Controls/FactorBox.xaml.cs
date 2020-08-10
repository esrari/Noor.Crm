using NoorCRM.API.Models;
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
        public FactorBoxViewModel ViewModel { get; set; }
        public FactorBox(Factor factor)
        {
            InitializeComponent();
            ViewModel = new FactorBoxViewModel(factor);
            BindingContext = ViewModel;
        }
    }
}