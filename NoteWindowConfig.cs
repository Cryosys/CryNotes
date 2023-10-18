using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;

using Windows.UI.ApplicationSettings;

namespace CryNotes
{
    [JsonObject("WindowConfig")]
    public class NoteWindowConfig : CryBaseViewModelNDepObj
    {
        [JsonProperty("Notes")]
        public ObservableCollection<Note> Notes
        {
            get => _notes;
            private set => SetProperty(ref _notes, value);
        }

        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();

        [JsonProperty("X")]
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        private double _x = 0.0d;

        [JsonProperty("Y")]
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        private double _y = 0.0d;

        [JsonProperty("Width")]
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        private double _width = 350.0d;

        [JsonProperty("Height")]
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private double _height = 600.0d;

        [JsonIgnore]
        public Guid WindowID
        {
            get;
            set;
        }

        [JsonIgnore]
        public CryCommand AddNoteCommand
        {
            get => _addNoteCommand;
            set => SetProperty(ref _addNoteCommand, value);
        }

        [JsonIgnore]
        private CryCommand _addNoteCommand;

        [JsonIgnore]
        public CryCommand OpenNewCommand
        {
            get => _openNewCommand;
            set => SetProperty(ref _openNewCommand, value);
        }

        [JsonIgnore]
        private CryCommand _openNewCommand;

        [JsonIgnore]
        public CryCommand DeleteNoteWindowCommand
        {
            get => _deleteNoteWindowCommand;
            set => SetProperty(ref _deleteNoteWindowCommand, value);
        }

        [JsonIgnore]
        private CryCommand _deleteNoteWindowCommand;  
        

        [JsonIgnore]
        public CryCommand SettingsCommand
        {
            get => _settingsCommand;
            set => SetProperty(ref _settingsCommand, value);
        }

        [JsonIgnore]
        private CryCommand _settingsCommand;  

        public NoteWindowConfig()
        {
            _addNoteCommand = new CryCommand((obj) => Notes.Add(new Note("New")), () => true);
            _openNewCommand = new CryCommand((obj) => _OpenNewWindow(), () => true);
            _deleteNoteWindowCommand = new CryCommand((obj) => _DeleteNoteWindow(), () => true);
            _settingsCommand = new CryCommand((obj) => _ShowSettingsWindow(), () => true);
            _notes.CollectionChanged += _notes_CollectionChanged;
        }

        public NoteWindowConfig(double x, double y) : this()
        {
            _x = x;
            _y = y;
        }

        public NoteWindowConfig(ObservableCollection<Note> notes) : this(0d, 0d)
        {
            _notes.CollectionChanged -= _notes_CollectionChanged;
            _notes = notes;
            foreach(Note note in Notes)
                note.DeleteNote += Note_DeleteNote;
            _notes.CollectionChanged += _notes_CollectionChanged;
        }

        public NoteWindowConfig(ObservableCollection<Note> notes, double x, double y) : this(notes)
        {
            _x = x;
            _y = y;
        }

        private void _OpenNewWindow()
        {
            App? app = App.Current as CryNotes.App;
            if (app is null)
                return;

            // Give the new window a slight offset to the window that is already open
            app.OpenNew(X * 1.025, Y * 1.1);
        }

        private void _DeleteNoteWindow()
        {
            App? app = App.Current as CryNotes.App;
            if (app is null)
                return;

            app.DeleteNoteWindow(this);
        }

        private void _ShowSettingsWindow()
        {
            App? app = App.Current as CryNotes.App;
            if (app is null)
                return;

            app.ShowSettingsWindow();
        }

        private void _notes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
                foreach (Note note in e.OldItems)
                    note.DeleteNote -= Note_DeleteNote;

            if (e.NewItems is not null)
                foreach (Note note in e.NewItems)
                    note.DeleteNote += Note_DeleteNote;
        }

        private void Note_DeleteNote(object? sender, EventArgs e)
        {
            if(sender is not null && sender is Note note)
                Notes.Remove(note);
        }
    }
}
