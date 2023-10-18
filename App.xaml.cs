using CryLib;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace CryNotes
{
    public partial class App : Application
    {
        public static bool ShowInAltTab
        {
            get => (App.Current as CryNotes.App)?._appConfig.ShowInAltTab ?? false;
            set
            {
                CryNotes.App? _app = (App.Current as CryNotes.App);
                if (_app is null)
                    return;

                _app._appConfig.ShowInAltTab = value;
            }
        }

        private static string AppConfigsPath = CryLib.Core.Paths.AppPath + "appconfigs.cfg";
        private static string ConfigsPath = CryLib.Core.Paths.AppPath + "configs.cfg";

        private List<Window> _windows = new List<Window>();
        private ObservableCollection<NoteWindowConfig> _windowConfigs = new ObservableCollection<NoteWindowConfig>();

        private AppConfig _appConfig = new AppConfig();
        private SettingsWindow? _settingsWindow;

        private DispatcherTimer? _savetimer = null;
        private byte[] _lastSaveHash = { };

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (File.Exists(AppConfigsPath))
            {
                string json = File.ReadAllText(AppConfigsPath);
                AppConfig? temp = json.FromCryJson<AppConfig>();

                if (temp is null)
                {
                    CryMessagebox.Create("App config is corrupt. Delete file " + ConfigsPath);
                    Environment.Exit(0);
                }

                _appConfig = temp;

                // The settings is only applied on app start
                ShowInAltTab = _appConfig.ShowInAltTab;
            }

            if (File.Exists(ConfigsPath))
            {
                string json = File.ReadAllText(ConfigsPath);
                ObservableCollection<NoteWindowConfig>? temp = json.FromCryJson<ObservableCollection<NoteWindowConfig>>();

                if (temp is null)
                {
                    CryMessagebox.Create("Config is corrupt. Delete file " + ConfigsPath);
                    Environment.Exit(0);
                }

                _windowConfigs = temp;
                _OpenFromArray(_windowConfigs);

                _windowConfigs.CollectionChanged += _windowConfigs_CollectionChanged;
            }
            else
            {
                _windowConfigs.CollectionChanged += _windowConfigs_CollectionChanged;
                _CreateNewDefaultWindow();
            }

            _Save();

            Application.Current.Exit += Current_Exit;

            _CreateAndStartSaveTimer();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            _savetimer?.Stop();
            _Save();
        }

        private void _windowConfigs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
                _RemoveFromArrays(e.OldItems);

            if (e.NewItems is not null)
                _OpenFromArray(e.NewItems);
        }

        private void _OpenFromArray(System.Collections.IList configs)
        {
            foreach (NoteWindowConfig config in configs)
            {
                _CreateWindow(config);
            }
        }

        private void _RemoveFromArrays(System.Collections.IList configs)
        {
            foreach (NoteWindowConfig config in configs)
            {
                Window? win = _windows.Find(window => window.Tag is not null && window.Tag is Guid id && id == config.WindowID);

                if (win != null)
                    win.Close();
            }
        }

        private void _CreateNewDefaultWindow(double? x = null, double? y = null)
        {
            NoteWindowConfig config = new NoteWindowConfig(new ObservableCollection<Note>() { new Note("First Note") })
            {
                Width = 0.13671875,
                Height = 0.41666666,
            };

            config.X = x ?? 0.8;
            config.Y = y ?? 0.2;

            _windowConfigs.Add(config);
        }

        private void _CreateWindow(NoteWindowConfig config)
        {
            NoteWindow window = new NoteWindow()
            {
                DataContext = config
            };
            _windows.Add(window);
            window.Show();

            config.WindowID = Guid.NewGuid();
            window.Tag = config.WindowID;
        }

        private void _Save()
        {
            string json = _windowConfigs.ToCryJson();
            byte[] newSaveHash = CryLib.Core.Cryptography.Crypto.HashSha512(json);

            // To make it just a bit more efficient and do not write to the drive every 5 minutes.
            if (newSaveHash.Equals(_lastSaveHash))
                return;

            _lastSaveHash = CryLib.Core.Cryptography.Crypto.HashSha512(json);
            File.WriteAllText(ConfigsPath, json);
        }

        private void _CreateAndStartSaveTimer()
        {
            if (_savetimer is not null)
                return;

            _savetimer = new DispatcherTimer();
            _savetimer.Interval = TimeSpan.FromMinutes(5);
            _savetimer.Tick += _savetimer_Tick;
            _savetimer.Start();
        }

        private void _savetimer_Tick(object? sender, EventArgs e) => _Save();

        public void OpenNew(double x, double y) => _CreateNewDefaultWindow(x, y);

        public void DeleteNoteWindow(NoteWindowConfig config) => _windowConfigs.Remove(config);

        public void ShowSettingsWindow()
        {
            if (_settingsWindow is null)
                _settingsWindow = new SettingsWindow();

            _settingsWindow.DataContext = this._appConfig;
            _settingsWindow.Show();
            _settingsWindow.Closing += _settingsWindow_Closing;
        }

        private void _settingsWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_settingsWindow != null)
                _settingsWindow.Closing -= _settingsWindow_Closing;

            _settingsWindow = null;
        }
    }
}
