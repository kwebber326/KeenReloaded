using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.UserControls
{
    public partial class KeenReloadedTextInputDialog : KeenReloadedDialogWindow
    {
        private TextBox _txtUsername;
        public KeenReloadedTextInputDialog(string text) : base (text)
        {
            InitializeComponent();
            _txtUsername = new TextBox();
            _txtUsername.Location = new Point(180, 100);
            _txtUsername.Size = new Size(150, 32);
            _txtUsername.BorderStyle = BorderStyle.Fixed3D;
            _txtUsername.KeyDown += KeenReloadedTextInputDialog_KeyDown;

            this.Controls.Add(_txtUsername);
            _txtUsername.BringToFront();
          
            this.KeyUp -= KeenReloadedDialogWindow_KeyUp_1;
        }


        private void KeenReloadedTextInputDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(UserNameText))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public string UserNameText
        {
            get
            {
                return _txtUsername?.Text;
            }
        }
    }
}
