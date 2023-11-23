using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;
using KeenReloaded.Framework.Weapons;
using KeenReloaded.Framework.Items;

namespace KeenReloaded.UserControls
{
    public partial class WeaponInventoryItem : UserControl
    {
        protected CommanderKeen _keen;
        private int _weaponIndex;
        private NeuralStunner _weapon;

        protected int DIGIT_HORIZONTAL_OFFSET = 16;
        protected const int AMMO_VERTICAL_OFFSET = 6;
        protected const int AMMO_DIGIT_COUNT = 3;

        protected Dictionary<string, Image> _weaponImages = new Dictionary<string, Image>();
        protected Dictionary<char, Image> _digitLEDs = new Dictionary<char, Image>();

        protected List<PictureBox> _ammoCountLEDImages = new List<PictureBox>();

        public WeaponInventoryItem()
        {
            InitializeComponent();
            InitializeLEDDictionary();
            InitializeWeaponImages();
        }

        protected virtual void InitializeWeaponImages()
        {
            _weaponImages.Add("NeuralStunner", Properties.Resources.neural_stunner1);
            _weaponImages.Add("ShotgunNeuralStunner", Properties.Resources.neural_stunner_shotgun);
            _weaponImages.Add("SMGNeuralStunner", Properties.Resources.neural_stunner_smg_1);
            _weaponImages.Add("RailgunNeuralStunner", Properties.Resources.neural_stunner_railgun1);
            _weaponImages.Add("RPGNeuralStunner", Properties.Resources.neural_stunner_rocket_launcher1);
            _weaponImages.Add("BoobusBombLauncher", Properties.Resources.keen_dreams_boobus_bomb2);
            _weaponImages.Add("BFG", Properties.Resources.BFG1);
            _weaponImages.Add("SnakeGun", Properties.Resources.snake_gun1);
        }

        protected virtual void InitializeLEDDictionary()
        {
            _digitLEDs.Add('x', Properties.Resources.scoreboard_default_LED);
            _digitLEDs.Add('0', Properties.Resources.scoreboard_LED_zero);
            _digitLEDs.Add('1', Properties.Resources.scoreboard_LED_one);
            _digitLEDs.Add('2', Properties.Resources.scoreboard_LED_two);
            _digitLEDs.Add('3', Properties.Resources.scoreboard_LED_three);
            _digitLEDs.Add('4', Properties.Resources.scoreboard_LED_four);
            _digitLEDs.Add('5', Properties.Resources.scoreboard_LED_five);
            _digitLEDs.Add('6', Properties.Resources.scoreboard_LED_six);
            _digitLEDs.Add('7', Properties.Resources.scoreboard_LED_seven);
            _digitLEDs.Add('8', Properties.Resources.scoreboard_LED_eight);
            _digitLEDs.Add('9', Properties.Resources.scoreboard_LED_nine);

            AddDefaultLEDs();
        }

        protected void AddDefaultLEDs()
        {
            int currentX = label1.Right + 1;
            for (int i = 0; i < AMMO_DIGIT_COUNT; i++)
            {
                PictureBox p = new PictureBox();
                p.Image = _digitLEDs['x'];
                p.Location = new Point(currentX, AMMO_VERTICAL_OFFSET);
                currentX += DIGIT_HORIZONTAL_OFFSET;
                _ammoCountLEDImages.Add(p);
                this.Controls.Add(p);
                p.BringToFront();
            }
        }

        public virtual CommanderKeen Keen
        {
            get
            {
                return _keen;
            }
            set
            {
                _keen = value;
            }
        }

        public NeuralStunner Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;
                UpdateWeapon();
            }
        }

        public virtual void UpdateWeapon()
        {
            //update index
            string digit = Convert.ToString(this.WeaponIndex);
            indexPicBox.Image = _digitLEDs[digit[0]];
            //update image
            weaponPicBox.Image = _weaponImages[this.Weapon.GetType().Name];
            weaponPicBox.Location = new Point(weaponPicBox.Location.X, AMMO_VERTICAL_OFFSET * 2);
            label1.Location = new Point(weaponPicBox.Right + 1, label1.Location.Y);
            //update ammo
            UpdateAmmo();

            SetSelectedState();
        }

        public bool IsSelected
        {
            get;
            private set;
        }

        public void SetSelectedState()
        {
            if (_keen == null)
                return;
            if (this.Weapon == null)
                return;

            if (_keen.CurrentWeapon.GetType().Name  == this.Weapon.GetType().Name)
            {
                this.BackColor = Color.Gray;
                this.IsSelected = true;
            }
            else
            {
                this.BackColor = Color.Black;
                this.IsSelected = false;
            }
        }

        protected virtual void UpdateAmmo()
        {
            int ammo = this.Weapon.Ammo;
            if (ammo >= 999)
            {
                foreach (var digit in _ammoCountLEDImages)
                {
                    digit.Image = _digitLEDs['9'];
                }
            }
            else
            {
                string ammoString = ammo.ToString();
                int currentDigit = 0;
                for (int i = 0; i < AMMO_DIGIT_COUNT; i++)
                {
                    if (AMMO_DIGIT_COUNT - i > ammoString.Length)
                    {
                        _ammoCountLEDImages[i].Image = _digitLEDs['x'];
                    }
                    else
                    {
                        _ammoCountLEDImages[i].Image = _digitLEDs[ammoString[currentDigit++]];
                    }
                }
            }
        }

        public int WeaponIndex
        {
            get
            {
                return _weaponIndex;
            }
            set
            {
                _weaponIndex = value;
            }
        }

        private void WeaponInventoryItem_Load(object sender, EventArgs e)
        {
            
        }
    }
}
