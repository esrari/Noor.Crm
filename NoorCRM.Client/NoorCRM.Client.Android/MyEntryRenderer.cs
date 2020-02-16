using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NoorCRM.Client.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace NoorCRM.Client.Droid
{
    [Obsolete]
    public class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }
        }
    }
}