using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class GnosticeneItemDestroyer : CollisionObject, IUpdatable, ICreateRemove, ISprite
    {
        private Item _itemToDestroy;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;

        private Image[] _images = new Image[]
        {
            Properties.Resources.keen4_gnosticene_ancient_dust1,
            Properties.Resources.keen4_gnosticene_ancient_dust2,
            Properties.Resources.keen4_gnosticene_ancient_dust3,
            Properties.Resources.keen4_gnosticene_ancient_dust4
        };

        public GnosticeneItemDestroyer(SpaceHashGrid grid, Rectangle hitbox, Item itemToDestroy)
            : base(grid, hitbox)
        {
            if (itemToDestroy == null)
                throw new ArgumentNullException("there needs to be an item for the destroyer to destroy");

            _itemToDestroy = itemToDestroy;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _itemToDestroy.Destroy();
        }

        public void Update()
        {
            if (_currentSprite >= _images.Length)
            {
                OnRemove();
            }
            else if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                this.Sprite.Image = _images[_currentSprite++];
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private PictureBox _sprite;

        protected void OnRemove()
        {
            if (Remove != null)
            {
                foreach (var node in _collidingNodes)
                {
                    node.Objects.Remove(this);
                    node.NonEnemies.Remove(this);
                }
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                Remove(this, args);
            }
        }

        protected void OnCreate()
        {
            if (Create != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                Create(this, args);
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }
    }
}
