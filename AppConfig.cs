using Newtonsoft.Json;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryNotes
{
    public class AppConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonProperty("ShowInAltTab")]
        public bool ShowInAltTab
        {
            get => _showInAltTab;
            set
            {
                if (_showInAltTab != value)
                {
                    _showInAltTab = value;
                    Changed();
                }
            }
        }

        private bool _showInAltTab = false;

        protected void Changed([CallerMemberName] string member = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
