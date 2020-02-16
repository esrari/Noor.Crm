using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DevProtocol.Xam.Demos.SpaceNav.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(SpaceNavigationPageRenderer), typeof(SpaceNavigationPageRenderer))]
namespace DevProtocol.Xam.Demos.SpaceNav.Droid.Renderers
{
    [Obsolete]
    public class SpaceNavigationPageRenderer : NavigationPageRenderer
    {
        private Android.Support.V7.Widget.Toolbar toolbar;

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);
            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                toolbar = (Android.Support.V7.Widget.Toolbar)child;
                toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
            }
        }

        private void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var view = e.Child.GetType();
            if (e.Child.GetType() == typeof(Android.Widget.TextView))
            {
                var textView = (Android.Widget.TextView)e.Child;
                var spaceFont = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "IRANSansMobile(FaNum).ttf");
                textView.Typeface = spaceFont;
                toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }
        }
    }
}