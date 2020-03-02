using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageBox : ContentView
    {
        public MessageBox(MessageBoxViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }


}