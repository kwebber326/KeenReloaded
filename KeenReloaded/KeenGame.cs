using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded
{
    public partial class KeenGame : Form
    {
        private Form1 _game = new Form1();
        public KeenGame()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            _game.MdiParent = this;
            _game.Location = new Point(0, 300);
            _game.Show();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
    }
}
