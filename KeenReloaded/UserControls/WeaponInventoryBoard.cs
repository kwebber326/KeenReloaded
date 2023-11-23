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

namespace KeenReloaded.UserControls
{
    public partial class WeaponInventoryBoard : UserControl
    {
        private CommanderKeen _keen;
        private int _currentY;
        private WeaponInventoryItem _selectedWeapon;
        public WeaponInventoryBoard()
        {
            InitializeComponent();
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
                if (_keen != null)
                {
                    SetWeaponsList();
                    WriteCurrentWeapon();
                    _keen.KeenAcquiredWeapon += new EventHandler<Framework.KeenEventArgs.WeaponAcquiredEventArgs>(_keen_KeenAcquiredWeapon);
                    _keen.WeaponChanged += new EventHandler<Framework.KeenEventArgs.ObjectEventArgs>(_keen_WeaponChanged);
                }
            }
        }

        public WeaponInventoryItem SelectedWeapon
        {
            get
            {
                return _selectedWeapon;
            }
            set
            {
                _selectedWeapon = value;
            }
        }

        private void WriteCurrentWeapon()
        {
            currentWeaponLabel.Text = "Current Weapon: \n" + SeparateWords(_keen.CurrentWeapon.GetType().Name);
        }

        private string SeparateWords(string str)
        {
            List<string> words = new List<string>();

            int index = 0;
            while (str.Length > 0)
            {
                if ((str[index].ToString().ToUpper() == str[index].ToString()) || index == str.Length - 1)
                {
                    if (index > 0)
                    {
                        string word = index == str.Length - 1 ? str : str.Substring(0, index);
                        str = index == str.Length - 1 ? "" : str.Substring(index);
                        index = 0;
                        words.Add(word);
                    }
                    else
                    {
                        index++;
                    }
                }
                else
                {
                    index++;
                }
            }
            string retVal = string.Join(" ", words);
            return retVal;
        }

        private void SetWeaponsList()
        {
            weaponsPanel.Controls.Clear();
            _currentY = 0;
            for (int i = 0; i < _keen.Weapons.Count; i++)
            {
                var weapon = _keen.Weapons[i];
                WeaponInventoryItem item = new WeaponInventoryItem();
                item.Keen = _keen;
                item.WeaponIndex = i + 1;
                item.Weapon = weapon;
                item.Location = new Point(0, _currentY);
                _currentY += 50;
                weaponsPanel.Controls.Add(item);
                if (_keen.CurrentWeapon == weapon)
                {
                    this.SelectedWeapon = item;
                }
            }
          
        }

        void _keen_WeaponChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            if (_keen == null)
                return;
            if (this.SelectedWeapon == null)
                return;
            if (this.SelectedWeapon.Weapon == null)
                return;
            if (_keen.CurrentWeapon != this.SelectedWeapon.Weapon)
            {
                foreach (var control in weaponsPanel.Controls)
                {
                    WeaponInventoryItem item = control as WeaponInventoryItem;
                    if (item != null)
                    {
                        item.SetSelectedState();
                        item.UpdateWeapon();
                        if (item.IsSelected)
                        {
                            this.SelectedWeapon = item;
                        }
                    }
                }
            }
            else
            {
                foreach (var control in weaponsPanel.Controls)
                {
                    WeaponInventoryItem item = control as WeaponInventoryItem;
                    if (item != null)
                    {
                        item.UpdateWeapon();
                    }
                }
            }
            WriteCurrentWeapon();
        }

        void _keen_KeenAcquiredWeapon(object sender, Framework.KeenEventArgs.WeaponAcquiredEventArgs e)
        {
            SetWeaponsList();
        }

        private void WeaponInventoryBoard_Load(object sender, EventArgs e)
        {
           
        }
    }
}
