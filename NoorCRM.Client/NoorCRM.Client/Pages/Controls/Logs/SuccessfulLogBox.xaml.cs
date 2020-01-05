using NoorCRM.API.Models;
using Utility.DateTimeFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls.Logs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulLogBox : ContentView
    {
        private readonly SuccessfulLog _successfulLog;

        public SuccessfulLogBox(SuccessfulLog successfulLog)
        {
            InitializeComponent();

            if(successfulLog != null)
            {
                lblDescription.Text = successfulLog.Description;
                lblDate.Text = successfulLog.CreationDate.GetDateTimeString(
                    DateOrTime.DateTime, (int)DateTimeFormat.yymmddHHmm, isPersian: true);
                lblCreatorName.Text = successfulLog.CreatorUser?.FullName;
            }

            _successfulLog = successfulLog;
        }

        private void btnSeeFactor_Clicked(object sender, EventArgs e)
        {

        }
    }
}