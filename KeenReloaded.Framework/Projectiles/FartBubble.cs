using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Trajectories
{
    public class FartBubble : CollisionObject, IUpdatable, ICreateRemove, ISprite
    {
        private const int FLOAT_VELOCITY = 15;
        private const int HORIZONTAL_VELOCITY = 5;
        private const int HORIZONTAL_MOVE_TIME = 6;
        private int _horizontalMoveTimeTick;

        public FartBubble(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Image = Properties.Resources.keen4_dopefish_fart_bubble;
            _direction = Direction.RIGHT;
        }

        public override Rectangle HitBox
        {
            get
            {
                return base.HitBox;
            }
            protected set
            {
                base.HitBox = value;
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(Enums.Direction.UP);
                    this.UpdateCollisionNodes(_direction);
                }
            }
        }

        public void Update()
        {
            int xOffset = _direction == Direction.RIGHT ? HORIZONTAL_VELOCITY : HORIZONTAL_VELOCITY * -1;
            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y - FLOAT_VELOCITY, this.HitBox.Width, this.HitBox.Height);
            if (++_horizontalMoveTimeTick == HORIZONTAL_MOVE_TIME)
            {
                _horizontalMoveTimeTick = 0;
                _direction = ChangeHorizontalDirection(_direction);
            }

            if (!_collidingNodes.Any())
            {
                ObjectEventArgs e = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                OnRemove(e);
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs e)
        {
            if (this.Create != null)
            {
                this.Create(this, e);
            }
        }

        protected void OnRemove(ObjectEventArgs e)
        {
            if (this.Remove != null)
            {
                this.Remove(this, e);
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        private PictureBox _sprite;
        private Direction _direction;

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
