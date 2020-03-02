using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace NoorCRM.Client.Sources
{
    public static class ExtensionMethods
    {
        public static bool Replace<T>(this ObservableCollection<T> collection, T oldItem, T newItem)
        {
            if (collection != null && oldItem != null && newItem != null)
                if (collection.Contains(oldItem))
                {
                    collection[collection.IndexOf(oldItem)] = newItem;
                    return true;
                }

            return false;
        }

        public static ImageSource ToImageSource(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            try
            {
                if (bytes != null && bytes.Length > 0)
                    return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            catch (Exception e)
            {
                Debug.WriteLine("byte array cannot convert to ImageSource" + Environment.NewLine + e.Message);
            }

            return null;
        }
    }
}
