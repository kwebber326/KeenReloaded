using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Assets
{
    public class KeyGate : CollisionObject, ICreateRemove, IActivateable, IUpdatable
    {
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private PictureBox _sprite;
        private int _currentDeactivateSprite = 1;
        private bool _IsOpened;
        private Image[] _images = new Image[]
        {
            Properties.Resources.key_gate1,
            Properties.Resources.key_gate2,
            Properties.Resources.key_gate3,
            Properties.Resources.key_gate4
        };
        private bool _isActive;
        private readonly Guid _activationId;

        public KeyGate(SpaceHashGrid grid, Rectangle hitbox, Guid activationId) : base (grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = _images[0];
            _sprite.Size = this.HitBox.Size;
            _isActive = true;
            this.CollisionTile = new DebugTile(_collisionGrid, this.HitBox);
            InitializeSpriteMap(_sprite.Image.Size);
        }

        private void InitializeSpriteMap(Size imageDimensions)
        {
            //image width and height
            int width = imageDimensions.Width;
            int height = imageDimensions.Height;
            //location to write image (0,0 is the top left corner of the picture box, not the form)
            int currentX = 0;
            int currentY = 0;
            //declare a bitmap for the image
            var bitmap = new Bitmap(this.HitBox.Width, this.HitBox.Height);
            //update the size of the picture box to accomodate background size
            _sprite.Size = this.HitBox.Size;
            //for each subsequent length and height, draw the image onto the bitmap
            //to fill out the background rectangle
            for (int i = 0; i < this.HitBox.Width; i += width)
            {
                for (int j = 0; j < this.HitBox.Height; j += height)
                {
                    Graphics.FromImage(bitmap).DrawImage(_sprite.Image, new Rectangle(currentX, currentY, _sprite.Image.Width, _sprite.Image.Height));
                    currentY += height;
                }
                currentY = 0;
                currentX += width;
            }
            //assign the resulting bitmap to the picture box's image property so it loads as 
            //one image onto the form
            _sprite.Image = bitmap;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }

        protected void OnCreate(ObjectEventArgs e)
        {
            if (Create != null)
                Create(this, e);
        }

        protected void OnRemove(ObjectEventArgs e)
        {
            if (Remove != null)
                Remove(this, e);
        }

        public DebugTile CollisionTile
        {
            get;
            private set;
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        public Guid ActivationID => _activationId;

        public void Update()
        {
            if (!this.IsActive)
            {
                if (_currentDeactivateSprite == _images.Length && !_IsOpened)
                {
                    _IsOpened = true;
                    ObjectEventArgs e = new ObjectEventArgs()
                    {
                        ObjectSprite = this.CollisionTile
                    };
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                        node.Objects.Remove(this.CollisionTile);
                        node.Tiles.Remove(this.CollisionTile);
                    }
                    OnRemove(e);
                }
                else if (!_IsOpened)
                {
                    _sprite.Image = _images[_currentDeactivateSprite++];
                    InitializeSpriteMap(_sprite.Image.Size);
                }
            }
        }
    }
}
