using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;

namespace KeenReloaded.UserControls
{
    public partial class KeyCardInventoryBoard : UserControl
    {
        private CommanderKeen _keen;

        public KeyCardInventoryBoard()
        {
            InitializeComponent();
        }

        public CommanderKeen Keen
        {
            get
            {
                return _keen;
            }
            set
            {
                _keen = value;
                if (_keen != null)
                {
                    _keen.KeyCardAcquiredChanged += _keen_KeyCardAcquiredChanged;
                    SetKeyCardStatus();
                }
            }
        }

        private void _keen_KeyCardAcquiredChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            SetKeyCardStatus();
        }

        private void SetKeyCardStatus()
        {
            pbImage.Image = _keen.HasKeyCard
                 ? Properties.Resources.key_card_acquired
                 : Properties.Resources.key_card_not_acquired;
        }
    }
}
