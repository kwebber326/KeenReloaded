using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class ToggleSwitch : CollisionObject, IActivator, ISprite
    {
        private SwitchType _type;
        private bool _canToggle = true;
        Timer _toggleDelayTimer;

        private const int TOGGLE_DELAY_MILLISECONDS = 500;
        public ToggleSwitch(SpaceHashGrid grid, Rectangle hitbox, SwitchType type, List<IActivateable> toggleObjects, bool isActive)
            : base(grid, hitbox)
        {
            _isActive = isActive;
            _toggleObjects = toggleObjects;
            _type = type;
            Initialize();
        }

        private void Initialize()
        {
            if (_toggleObjects == null)
                _toggleObjects = new List<IActivateable>();
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            UpdateSprite();
            if (_sprite != null && this.HitBox != null)
            {
                _sprite.Location = this.HitBox.Location;
                this.HitBox = new Rectangle(this.HitBox.Location, _sprite.Size);
            }
            UpdateToggleObjects();
            _toggleDelayTimer = new Timer();
            _toggleDelayTimer.Interval = TOGGLE_DELAY_MILLISECONDS;
            _toggleDelayTimer.Tick += _toggleDelayTimer_Tick;
        }

        private void _toggleDelayTimer_Tick(object sender, EventArgs e)
        {
            _canToggle = true;
            _toggleDelayTimer.Stop();
        }

        public List<IActivateable> ToggleObjects
        {
            get { return _toggleObjects; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public void Toggle()
        {
            if (_canToggle)
            {
                _isActive = !_isActive;
                UpdateSprite();
                UpdateToggleObjects();
                DelayToggleAbility();
            }
        }

        private void DelayToggleAbility()
        {
            _canToggle = false;
            _toggleDelayTimer.Start();
        }

        private void UpdateSprite()
        {
            switch (_type)
            {
                case SwitchType.KEEN4_1:
                    _sprite.Image = _isActive ? Properties.Resources.keen4_switch1_on : Properties.Resources.keen4_switch1_off;
                    break;
                case SwitchType.KEEN4_2:
                    _sprite.Image = _isActive ? Properties.Resources.keen4_switch2_on : Properties.Resources.keen4_switch2_off;
                    break;
                case SwitchType.KEEN5_1:
                    _sprite.Image = _isActive ? Properties.Resources.keen5_switch1_on : Properties.Resources.keen5_switch1_off;
                    break;
                case SwitchType.KEEN5_2:
                    _sprite.Image = _isActive ? Properties.Resources.keen5_switch2_on : Properties.Resources.keen5_switch2_off;
                    break;
            }  
        }

        private void UpdateToggleObjects()
        {
            if (_isActive)
            {
                foreach (var to in _toggleObjects)
                {
                    to.Activate();
                }
            }
            else
            {
                foreach (var to in _toggleObjects)
                {
                    to.Deactivate();
                }
            }
        }

        public event EventHandler<KeenEventArgs.ToggleEventArgs> Toggled;
        private List<IActivateable> _toggleObjects;
        private bool _isActive;
        private PictureBox _sprite;

        protected override void HandleCollision(CollisionObject obj)
        {
          
        }

        public PictureBox Sprite
        {
            get { return _sprite; }
        }
    }
}
