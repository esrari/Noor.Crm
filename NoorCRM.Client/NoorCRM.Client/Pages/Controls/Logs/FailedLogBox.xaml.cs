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
    public partial class FailedLogBox : ContentView
    {
        public FailedLogBox(FailedLog failedLog)
        {
            InitializeComponent();

            if (failedLog != null)
            {
                switch (failedLog.FailureReason)
                {
                    case FailureReason.Unknown:
                        lblReason.Text = "نامشخص";
                        break;
                    case FailureReason.HasAnotherBrand:
                        lblReason.Text = "کار با سایر برندها";
                        break;
                    case FailureReason.NotSoldLastOrder:
                        lblReason.Text = "عدم فروش سفارش قبلی";
                        break;
                    case FailureReason.LowQuality:
                        lblReason.Text = "کیفیت پایین جنس";
                        break;
                }

                lblDescription.Text = failedLog.Description;
                lblDate.Text = failedLog.CreationDate.GetDateTimeString(
                    DateOrTime.DateTime, (int)DateTimeFormat.yymmddHHmm, isPersian: true);
                lblCreatorName.Text = failedLog.CreatorUser?.FullName;
            }
        }
    }
}