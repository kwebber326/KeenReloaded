using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Assets
{
    public class Keen5ControlPanel : CollisionObject, IUpdatable, ISprite
    {
        private PictureBox _sprite;
        private const int SPRITE_CHANGE_DELAY = 12;
        private const int CALIBRATE_HITBOX_LEFT_OFFSET = 20, CALIBRATE_HITBOX_RIGHT_OFFSET = 20;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private Image[] _sprites = SpriteSheet.Keen5ControlPanelImages;

        public Keen5ControlPanel(SpaceHashGrid grid, Rectangle hitbox): base(grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Image = _sprites[_currentSprite];
            _sprite.Location = this.HitBox.Location;
            this.HitBox = new Rectangle(this.HitBox.X + CALIBRATE_HITBOX_LEFT_OFFSET//x
                , this.HitBox.Y//y
                , this.HitBox.Width - CALIBRATE_HITBOX_LEFT_OFFSET - CALIBRATE_HITBOX_RIGHT_OFFSET//width
                , this.HitBox.Height);//height

            this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
           
        }

        public PictureBox Sprite => _sprite;

        public void Update()
        {
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }
    }
}
