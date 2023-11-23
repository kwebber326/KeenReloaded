using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework;

namespace KeenReloaded.UserControls
{
    public partial class KeyInventoryBoard : UserControl
    {
        private Gem _redGem;
        private CommanderKeen _keen;
        private Dictionary<Color, PictureBox> _gemsAdded = new Dictionary<Color, PictureBox>();

        private const int RED_GEM_X_OFFSET = 90;
        private const int DISTANCE_BETWEEN_GEMS = 15;
        private const int Y_OFFSET = 13;
        private Gem _yellowGem;
        private Gem _blueGem;
        private Gem _greenGem;

        public KeyInventoryBoard()
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
                    _keen.KeenAcquiredItem += new EventHandler<Framework.KeenEventArgs.ItemAcquiredEventArgs>(_keen_KeenAcquiredItem);
                    _keen.ItemLost += new EventHandler<Framework.KeenEventArgs.ItemAcquiredEventArgs>(_keen_ItemLost);
                }
            }
        }

        void _keen_ItemLost(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
        {
            Gem gem = e.Item as Gem;
            if (gem != null)
            {
                switch (gem.Color)
                {
                    case Framework.Enums.GemColor.RED:
                        this.RedGem = null;
                        break;
                    case Framework.Enums.GemColor.YELLOW:
                        this.YellowGem = null;
                        break;
                    case Framework.Enums.GemColor.BLUE:
                        this.BlueGem = null;
                        break;
                    case Framework.Enums.GemColor.GREEN:
                        this.GreenGem = null;
                        break;
                }
            }
        }

        void _keen_KeenAcquiredItem(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
        {
            Gem gem = e.Item as Gem;
            if (gem != null)
            {
                switch (gem.Color)
                {
                    case Framework.Enums.GemColor.RED:
                        this.RedGem = gem;
                        break;
                    case Framework.Enums.GemColor.YELLOW:
                        this.YellowGem = gem;
                        break;
                    case Framework.Enums.GemColor.BLUE:
                        this.BlueGem = gem;
                        break;
                    case Framework.Enums.GemColor.GREEN:
                        this.GreenGem = gem;
                        break;
                }
            }
        }

        public Gem RedGem
        {
            get
            {
                return _redGem;
            }
            set
            {
                if (value == null && _redGem != null)
                {
                    var gemsAdded = _gemsAdded[Color.Red];
                    this.Controls.Remove(gemsAdded);
                    _redGem = value;
                }
                else if (_redGem == null && value != null && value.Color == Framework.Enums.GemColor.RED)
                {
                    _redGem = value;
                    PictureBox p = new PictureBox();
                    p.Location = new Point(RED_GEM_X_OFFSET, Y_OFFSET);
                    p.SizeMode = PictureBoxSizeMode.AutoSize;
                    p.Image = Properties.Resources.inventory_red_gem;
                    this.Controls.Add(p);
                    p.BringToFront();
                    _gemsAdded.Add(Color.Red, p);
                }
                

            }
        }

        public Gem YellowGem
        {
            get
            {
                return _yellowGem;
            }
            set
            {
                if (value == null && _yellowGem != null)
                {
                    var gemsAdded = _gemsAdded[Color.Yellow];
                    this.Controls.Remove(gemsAdded);
                    _yellowGem = value;
                }
                else if (_yellowGem == null && value != null && value.Color == Framework.Enums.GemColor.YELLOW)
                {
                    _yellowGem = value;
                    PictureBox p = new PictureBox();
                    p.Location = new Point(RED_GEM_X_OFFSET + DISTANCE_BETWEEN_GEMS, Y_OFFSET);
                    p.SizeMode = PictureBoxSizeMode.AutoSize;
                    p.Image = Properties.Resources.inventory_yellow_gem;
                    this.Controls.Add(p);
                    p.BringToFront();
                    _gemsAdded.Add(Color.Yellow, p);
                }


            }
        }

        public Gem BlueGem
        {
            get
            {
                return _blueGem;
            }
            set
            {
                if (value == null && _blueGem != null)
                {
                    var gemsAdded = _gemsAdded[Color.Blue];
                    this.Controls.Remove(gemsAdded);
                    _blueGem = value;
                }
                else if (_blueGem == null && value != null && value.Color == Framework.Enums.GemColor.BLUE)
                {
                    _blueGem = value;
                    PictureBox p = new PictureBox();
                    p.Location = new Point(RED_GEM_X_OFFSET + DISTANCE_BETWEEN_GEMS * 2, Y_OFFSET);
                    p.SizeMode = PictureBoxSizeMode.AutoSize;
                    p.Image = Properties.Resources.inventory_blue_gem;
                    this.Controls.Add(p);
                    p.BringToFront();
                    _gemsAdded.Add(Color.Blue, p);
                }


            }
        }

        public Gem GreenGem
        {
            get
            {
                return _greenGem;
            }
            set
            {
                if (value == null && _greenGem != null)
                {
                    var gemsAdded = _gemsAdded[Color.Green];
                    this.Controls.Remove(gemsAdded);
                    _greenGem = value;
                }
                else if (_greenGem == null && value != null && value.Color == Framework.Enums.GemColor.GREEN)
                {
                    _greenGem = value;
                    PictureBox p = new PictureBox();
                    p.Location = new Point(RED_GEM_X_OFFSET + DISTANCE_BETWEEN_GEMS * 3, Y_OFFSET);
                    p.SizeMode = PictureBoxSizeMode.AutoSize;
                    p.Image = Properties.Resources.inventory_green_gem;
                    this.Controls.Add(p);
                    p.BringToFront();
                    _gemsAdded.Add(Color.Green, p);
                }
            }
        }

        protected void KeyInventoryBoard_Load(object sender, System.EventArgs e)
        {
            
        }

        public void Reset()
        {
            this.RedGem = null;
            this.GreenGem = null;
            this.BlueGem = null;
            this.YellowGem = null;
            foreach (var gem in _gemsAdded)
            {
                this.Controls.Remove(gem.Value);
            }
            _gemsAdded.Clear();
         
        }
    }
}
