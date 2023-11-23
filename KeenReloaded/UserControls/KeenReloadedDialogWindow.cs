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
    public partial class KeenReloadedDialogWindow : Form
    {
        public KeenReloadedDialogWindow(string text)
        {
            InitializeComponent();
            lblMessageText.Text = text;
        }

        private void KeenReloadedDialogWindow_Load(object sender, System.EventArgs e)
        {
            
        }

        protected void KeenReloadedDialogWindow_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void KeenReloadedDialogWindow_KeyUp_1(object sender, KeyEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
