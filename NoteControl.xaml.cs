using System.Windows.Controls;

namespace CryNotes
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }

        private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.OnContextMenuOpening(e);
        }
    }
}
