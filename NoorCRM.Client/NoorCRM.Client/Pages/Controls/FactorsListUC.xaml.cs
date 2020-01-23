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

        private static void HandleFactorsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var facts = newValue as IEnumerable<Factor>;
            if (facts != null)
            {
                var facList = bindable as FactorsListUC;
                facList.stkFactors.Children.Clear();
                var revFacts = facts.Reverse();
                foreach (var item in revFacts)
                {
                    var f = new FactorBox(item);
                    facList.stkFactors.Children.Add(f);
                }
                facList.scroller.ScrollToAsync(facList.scroller, ScrollToPosition.End, false);
            }
        }

        public FactorsListUC()
        {
            InitializeComponent();
        }
    }
}