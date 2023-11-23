using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;
using KeenReloaded.Framework.Items;

namespace KeenReloaded.UserControls
{
    public partial class ShieldInventoryItem : WeaponInventoryItem
    {

        private Shield _shield;

        public ShieldInventoryItem()
        {
            InitializeComponent();
           
        }

        protected override void InitializeWeaponImages()
        {
            _weaponImages.Add("Shield", Properties.Resources.Shield_small);
        }

        private void ShieldInventoryItem_Load(object sender, EventArgs e)
        {
            int currentX = label1.Right + 1;
            for (int i = 0; i < _ammoCountLEDImages.Count; i++)
            {
                PictureBox p = new PictureBox();
                p.Image = _digitLEDs['x'];
                p.Location = new Point(currentX, AMMO_VERTICAL_OFFSET);
                currentX += DIGIT_HORIZONTAL_OFFSET;
                _ammoCountLEDImages[i].Location = p.Location;
            }
            UpdateWeapon();
        }

        public override void UpdateWeapon()
        {
            //update image
            weaponPicBox.Image = _weaponImages["Shield"];
            weaponPicBox.Location = new Point(weaponPicBox.Location.X, AMMO_VERTICAL_OFFSET * 2);
            label1.Location = new Point(weaponPicBox.Right + 1, label1.Location.Y);
            //update ammo
            UpdateAmmo();

            SetSelectedState();
        }

        public void ResetShield(Shield shield)
        {
            _shield.ShieldDurationChanged -= _shield_ShieldDurationChanged;
            _shield = null;
            _shield = shield;
            if (_shield != null)
            {
                _shield.ShieldDurationChanged += _shield_ShieldDurationChanged;
            }
            UpdateAmmo();
        }

        public override CommanderKeen Keen
        {
            get => base.Keen;
            set
            {
                _keen = value;
                if (_keen != null)
                {
                    _keen.ShieldAcquired += _keen_ShieldAcquired;
                    _keen.ItemLost += _keen_ItemLost;
                }
            }
        }

        public Shield Shield { get { return _shield; } }

        private void _keen_ItemLost(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
        {
            var shield = e.Item as Shield;
            if (shield != null)
            {
                _shield.ShieldDurationChanged -= _shield_ShieldDurationChanged;
                _shield = null;
            
                UpdateAmmo();
            }
        }

        private void _keen_ShieldAcquired(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            var shield = e.ObjectSprite as Shield;
            if (shield != null)
            {
                _shield = shield;
                _shield.ShieldDurationChanged += _shield_ShieldDurationChanged;

                UpdateAmmo();
            }
        }

        private void _shield_ShieldDurationChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            UpdateAmmo();
        }

        protected override void UpdateAmmo()
        {
            int ammo = _shield?.Duration ?? 0;
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
    }
}
