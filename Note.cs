using Newtonsoft.Json;

using System;

namespace CryNotes
{
    public class Note : CryBaseModelNDepObj
    {
        public event EventHandler? DeleteNote;

        public string Description 
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _description = "";

        [JsonIgnore]
        public CryCommand DeleteNoteCommand
        {
            get => _deleteNoteCommand;
            set => SetProperty(ref _deleteNoteCommand, value);
        }

        [JsonIgnore]
        private CryCommand _deleteNoteCommand;

        public Note()
        {
            _deleteNoteCommand = new CryCommand((obj) => DeleteNote?.Invoke(this, EventArgs.Empty), () => true);
        }

        public Note(string description) : this()
        {
            _description = description;
        }
    }
}
