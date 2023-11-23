using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Assets
{
    public class Keen6Switch : CollisionObject, IActivator, ISprite
    {
        private List<IActivateable> _toggleObjects;
        private PictureBox _sprite;
        private bool _isActive;
        private List<PictureBox> _poleSprites;
        private readonly int _addedLengths;

        private const int SWITCH_X_OFFSET = 10;
        private const int SWITCH_Y_OFFSET_OFF = 10;
        private const int SWITCH_Y_OFFSET_ON = 26;
        private const int SWITCH_WIDTH = 32;
        private const int SWITCH_HEIGHT = 16;

        private const int POLE_X_OFFSET = 30;
        private const int POLE_HEIGHT = 20;

        public Keen6Switch(SpaceHashGrid grid, Rectangle hitbox, List<IActivateable> toggleObjects, bool isActive, int addedLengths = 0) : base(grid, hitbox)
        {
            _toggleObjects = toggleObjects ?? new List<IActivateable>();
            _isActive = isActive;
            _addedLengths = addedLengths;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            this.UpdateSprite();
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;

            UpdateHitbox();

            _poleSprites = new List<PictureBox>();

            int currentY = _sprite.Bottom;

            for (int i = 0; i < _addedLengths; i++)
            {
                PictureBox p = new PictureBox();
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = Properties.Resources.keen6_Switch_pole;
                p.Location = new Point(_sprite.Location.X + POLE_X_OFFSET, currentY);
                _poleSprites.Add(p);

                currentY += POLE_HEIGHT;
            }
        }

        private void UpdateHitbox()
        {
            int xOffset = SWITCH_X_OFFSET;
            int yOffset = _isActive ? SWITCH_Y_OFFSET_ON : SWITCH_Y_OFFSET_OFF;

            this.HitBox = new Rectangle(this.Sprite.Location.X + xOffset, this.Sprite.Location.Y + yOffset, SWITCH_WIDTH, SWITCH_HEIGHT);
        }

        public override Rectangle HitBox
        {
            get => base.HitBox;

            protected set
            {
                base.HitBox = value;
                if (_collidingNodes != null && _collidingNodes.Any())
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }

        public PictureBox Sprite => _sprite;

        public List<PictureBox> PoleSprites
        {
            get
            {
                return _poleSprites;
            }
        }

        public List<IActivateable> ToggleObjects => _toggleObjects;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                UpdateSprite();
                UpdateHitbox();
            }
        }

        public event EventHandler<ToggleEventArgs> Toggled;

        public void Toggle()
        {
            foreach (var obj in _toggleObjects)
            {
                if (!this.IsActive)
                {
                    obj.Deactivate();
                }
                else
                {
                    obj.Activate();
                }
            }

            ToggleEventArgs e = new ToggleEventArgs()
            {
                IsActive = _isActive
            };
        }

        protected void OnToggled(ToggleEventArgs e)
        {
            this.Toggled?.Invoke(this, e);
        }

        public void OnCollide(CollisionObject obj)
        {
            this.HandleCollision(obj);
            this.Toggle();
        }

        private void UpdateSprite()
        {
            this.Sprite.Image = _isActive ? Properties.Resources.keen6_Switch_On : Properties.Resources.keen6_Switch_Off;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is CommanderKeen)
            {
                if (obj.HitBox.Top > this.HitBox.Bottom)
                {
                    this.IsActive = false;
                }
                else if (obj.HitBox.Bottom < this.HitBox.Top)
                {
                    this.IsActive = true;
                }
            }
        }
    }
}
