using NoorCRM.API.Models;
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
    public partial class CommentLogBox : ContentView
    {
        public CommentLogBox(CommentLog commentLog)
        {
            InitializeComponent();

            switch(commentLog.ImportanceLevel)
            {
                case ImportanceLevel.Important:
                    lblImportance.Text = "مهم";
                    lblImportance.TextColor = Color.Orange;
                    break;
                case ImportanceLevel.Critical:
                    lblImportance.Text = "خیلی مهم";
                    lblImportance.TextColor = Color.Red;
                    break;
            }

            lblComment.Text = commentLog.Comment;
            lblDate.Text = commentLog.CreationDate.ToString();
            lblCreatorName.Text = commentLog.CreatorUser?.FullName;
        }
    }
}