using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls.Logs;
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
    public partial class FactorsListUC : ContentView
    {
        public IEnumerable<Factor> Factors
        {
            get { return (IEnumerable<Factor>)GetValue(FactorsProperty); }
            set { SetValue(FactorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty FactorsProperty =
            BindableProperty.Create(
                nameof(Factors),
                typeof(IEnumerable<Factor>),
                typeof(FactorsListUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleFactorsChanged);

        private static FactorsListUC facList;
        private static List<FactorBoxViewModel> allBoxInfos;
        private static async void HandleFactorsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var facts = newValue as IEnumerable<Factor>;
            if (facts != null)
            {
                facList = bindable as FactorsListUC;
                allBoxInfos = new List<FactorBoxViewModel>();
                var revFacts = facts.Reverse();
                foreach (var item in revFacts)
                    allBoxInfos.Add(new FactorBoxViewModel(item));

                setToList(allBoxInfos);
            }
        }

        public FactorsListUC()
        {
            InitializeComponent();
        }

        public void Filter(string filter)
        {

            if (string.IsNullOrWhiteSpace(filter))
                setToList(allBoxInfos);
            else
            {
                var trimFilter = filter.Trim();
                List<FactorBoxViewModel> factors = new List<FactorBoxViewModel>();
                foreach (FactorBoxViewModel item in allBoxInfos)
                    if (item.Title.Contains(trimFilter))
                        factors.Add(item);

                setToList(factors);
            }
        }

        private static void setToList(IEnumerable<FactorBoxViewModel> products)
        {
            facList.stkFactors.ItemsSource = products;
        }

        private void stkFactors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var box = e.SelectedItem as FactorBoxViewModel;
            if (box != null)
                box.TapCommand.Execute(box.Factor);
        }
    }
}