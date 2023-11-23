using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System.Drawing;

namespace KeenReloaded.Framework.Trajectories
{
    public class ShelleyExplosion : CollisionObject, IUpdatable, ISprite, ICreateRemove, IExplodable
    {
        private int _currentSprite;
        private const int SPRITE_CHANGE_DELAY = 2;
        private int _currentSpriteChangeDelayTick;
        private Image[] _explosionSprites = new Image[]
        {
            Properties.Resources.keen5_shelley_explosion1,
            Properties.Resources.keen5_shelley_explosion2,
            Properties.Resources.keen5_shelley_explosion3,
            Properties.Resources.keen5_shelley_explosion4
        };
        public ShelleyExplosion(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = _explosionSprites[_currentSprite];
            _state = Enums.ExplosionState.EXPLODING;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (_state == Enums.ExplosionState.EXPLODING)
            {
                this.Explode();
            }
            else if (_state == Enums.ExplosionState.DONE)
            {
                OnRemove(new ObjectEventArgs() { ObjectSprite = this });
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private Enums.ExplosionState _state;

        public void Explode()
        {
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                int nextSprite = _currentSprite + 1;
                if (nextSprite >= _explosionSprites.Length)
                {
                    _state = Enums.ExplosionState.DONE;
                    return;
                }
                else 
                {
                    int verticalOffset = 0, horizontalOffset = 0;
                    var currentImg = _sprite.Image;
                    var nextImg = _explosionSprites[nextSprite];
                    //center explosion
                    if (currentImg.Height < nextImg.Height)
                    {
                        verticalOffset = currentImg.Height - nextImg.Height;
                    }
                    if (currentImg.Width < nextImg.Width)
                    {
                        horizontalOffset = currentImg.Width - nextImg.Width;
                    }
                    //set next image;
                    _sprite.Image = nextImg;
                    _currentSprite = nextSprite;
                    //align hitbox and recheck collision
                    this.HitBox = new Rectangle(this.HitBox.X + horizontalOffset, this.HitBox.Y + verticalOffset, _sprite.Image.Width, _sprite.Image.Height);
                    _sprite.Location = this.HitBox.Location;
                }
            }
            //check for collision with keen
            if (this.HitBox.IntersectsWith(_keen.HitBox))
            {
                _keen.Die();
            }
        }

        public Enums.ExplosionState ExplosionState
        {
            get { return _state; }
        }

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
                if (args.ObjectSprite == this)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }
    }
}
