using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace NoorCRM.Client.ViewModels
{
    public class BottomMenuItem : INotifyPropertyChanged
    {
        private Color _textColor;

        public string Title { get; set; }
        public string Icon { get; set; }
        public Command TapCommand { get; set; }
        public int GridColumnIndex { get; set; }
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor == value)
                    return;
                _textColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}