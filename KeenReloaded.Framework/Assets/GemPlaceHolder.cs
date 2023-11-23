using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Assets
{
    public class GemPlaceHolder : CollisionObject, ISprite, IActivator
    {
        private bool _isActive;
        private GemColor _gemColor;
        private PictureBox _sprite;

        public GemPlaceHolder(SpaceHashGrid grid, Rectangle hitbox, GemColor color, List<IActivateable> objectsToActivate)
            : base(grid, hitbox)
        {
            this.ToggleObjects = objectsToActivate;
            _gemColor = color;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            switch (_gemColor)
            {
                case GemColor.BLUE:
                    _sprite.Image = Properties.Resources.gem_placeholder_blue_empty;
                    break;
                case GemColor.GREEN:
                    _sprite.Image = Properties.Resources.gem_placeholder_green_empty;
                    break;
                case GemColor.RED:
                    _sprite.Image = Properties.Resources.gem_placeholder_red_empty;
                    break;
                case GemColor.YELLOW:
                    _sprite.Image = Properties.Resources.gem_placeholder_yellow_empty;
                    break;
            }
            if (this.HitBox != null)
                _sprite.Location = this.HitBox.Location;

            this.Toggled += new EventHandler<ToggleEventArgs>(GemPlaceHolder_Toggled);
        }

        void GemPlaceHolder_Toggled(object sender, ToggleEventArgs e)
        {
            if (this.ToggleObjects != null && this.ToggleObjects.Any())
            {
                foreach (var obj in this.ToggleObjects)
                {
                    if (_isActive)
                    {
                        obj.Deactivate();
                    }
                }
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public List<IActivateable> ToggleObjects
        {
            get;
            private set;
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public PictureBox Sprite
        {
            get { return _sprite; }
        }

        public GemColor Color
        {
            get
            {
                return _gemColor;
            }
        }

        public void Toggle()
        {
            _isActive = !_isActive;

            ToggleEventArgs e = new ToggleEventArgs()
            {
                IsActive = this.IsActive
            };
            if (this.IsActive)
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - 10, this.HitBox.Width, this.HitBox.Height + 10);
                _sprite.Location = this.HitBox.Location;
                _sprite.Size = this.HitBox.Size;
                SetActivatedSprite();
            }
            OnToggled(e);
        }

        private void SetActivatedSprite()
        {
            switch (_gemColor)
            {
                case GemColor.BLUE:
                    _sprite.Image = Properties.Resources.gem_placeholder_blue_filled;
                    break;
                case GemColor.GREEN:
                    _sprite.Image = Properties.Resources.gem_placeholder_green_filled;
                    break;
                case GemColor.RED:
                    _sprite.Image = Properties.Resources.gem_placeholder_red_filled;
                    break;
                case GemColor.YELLOW:
                    _sprite.Image = Properties.Resources.gem_placeholder_yellow_filled;
                    break;
            }
        }

       

        protected void OnToggled(ToggleEventArgs e)
        {
            if (Toggled != null)
            {
                Toggled(this, e);
            }
        }

        public event EventHandler<KeenEventArgs.ToggleEventArgs> Toggled;
    }
}
