using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class BipShipExplosion : IUpdatable, ISprite, ICreateRemove
    {

        public const int HEIGHT = 84, WIDTH = 64;
        private const int SPRITE_CHANGE_DELAY = 2;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private System.Windows.Forms.PictureBox _sprite;

        Image[] _sprites = new Image[]
        {
            Properties.Resources.keen6_bip_ship_explosion1,
            Properties.Resources.keen6_bip_ship_explosion2
        };

        public BipShipExplosion(Point location)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = location;
            _sprite.Image = _sprites[_currentSprite];
            _sprite.BringToFront();
        }

        public void Update()
        {
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                _currentSprite++;
                if (_currentSprite < _sprites.Length)
                {
                    _sprite.Image = _sprites[_currentSprite];
                }
                else
                {
                    OnRemove(new ObjectEventArgs() { ObjectSprite = this });
                }
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (this.Remove != null)
            {
                this.Remove(this, args);
            }
        }
    }
}
