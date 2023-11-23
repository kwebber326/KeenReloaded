using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Assets
{
    public class RemovablePlatform : CollisionObject, IActivateable, ICreateRemove, IBiomeTile
    {
        private int _length;
        private BiomeType _type;
        private bool _isActive;
        private List<RemovablePlatformMiddle> _removeableSection;
        private DebugTile _middleCollisionTile;
        private DebugTile _leftCollisionTile;
        private DebugTile _rightCollisionTile;

        public RemovablePlatform(SpaceHashGrid grid, Rectangle hitbox, BiomeType type, bool isActive, Guid activationId, int length = 1)
            : base(grid, hitbox)
        {
            _isActive = isActive;
            _length = length;
            _type = type;
            _activationId = activationId;
            Initialize();
        }

        private void Initialize()
        {
            if (_length < 1)
                _length = 1;

            //start with right
            _rightEdge = new RemoveablePlatformEdge(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Y, 42, 64), _type, Direction.LEFT, _isActive);
            _rightCollisionTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Y + 32, _rightEdge.HitBox.Width - 20, _rightEdge.HitBox.Height - 32));
            //spawn the middle if active
            int platformSpan = 32 * _length;
            _middleCollisionTile = new DebugTile(_collisionGrid, new Rectangle(_rightEdge.HitBox.Right, this.HitBox.Y + 32, platformSpan, 32));
            _removeableSection = new List<RemovablePlatformMiddle>();
            for (int i = _rightEdge.HitBox.Right; i < _rightEdge.HitBox.Right + platformSpan; i += 32)
            {
                RemovablePlatformMiddle m = new RemovablePlatformMiddle(_collisionGrid, new Rectangle(i, this.HitBox.Y, 32, 64), this.IsActive);
                _removeableSection.Add(m);
            }
            //end with left
            _leftEdge = new RemoveablePlatformEdge(_collisionGrid, new Rectangle(_rightEdge.HitBox.Right + platformSpan, this.HitBox.Y, 38, 64), _type, Direction.RIGHT, this.IsActive);
            _leftCollisionTile = new DebugTile(_collisionGrid, new Rectangle(_leftEdge.HitBox.X, _leftEdge.HitBox.Y + 32, _leftEdge.HitBox.Width, _leftEdge.HitBox.Height - 32));
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, _rightEdge.HitBox.Width + platformSpan + _leftEdge.HitBox.Width, this.HitBox.Height);
            
        }

        public override Rectangle HitBox { get => base.HitBox; protected set
            {
                base.HitBox = value;
                if (value != null && _collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }

        }

        public DebugTile LeftCollisionTile
        {
            get
            {
                return _leftCollisionTile;
            }
        }

        public DebugTile MiddleCollisiontTile
        {
            get
            {
                return _middleCollisionTile;
            }
        }

        public DebugTile RightCollisionTile
        {
            get
            {
                return _rightCollisionTile;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
        }

        public void Activate()
        {
            _leftEdge.Activate();
            foreach (var m in _removeableSection)
            {
                m.Activate();
            }
            _rightEdge.Activate();

            this.UpdateCollisionNodes(Direction.UP_RIGHT);
            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            foreach (var node in _collidingNodes)
            {
                if (node.HashBox.IntersectsWith(_middleCollisionTile.HitBox))
                {
                        node.Objects.Add(_middleCollisionTile);
                   
                        node.Tiles.Add(_middleCollisionTile);
                }
            }
        }

        public void Deactivate()
        {
            _leftEdge.Deactivate();
            foreach (var m in _removeableSection)
            {
                m.Deactivate();
            }
            _rightEdge.Deactivate();
            OnRemove();
        }

        public List<RemovablePlatformMiddle> RemoveableSection
        {
            get
            {
                return _removeableSection;
            }
        }

        public RemoveablePlatformEdge LeftEdge
        {
            get
            {
                return _leftEdge;
            }
        }

        public RemoveablePlatformEdge RightEdge
        {
            get
            {
                return _rightEdge;
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        protected void OnRemove()
        {
            if (Remove != null)
            {
                var collisionNodes = _collisionGrid._nodes.Where(n => n.HashBox.IntersectsWith(_middleCollisionTile.HitBox));
                foreach (var node in collisionNodes)
                {
                    node.Objects.Remove(this);
                    node.Objects.Remove(_middleCollisionTile);
                    node.Tiles.Remove(_middleCollisionTile);
                }
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = _middleCollisionTile
                };
                Remove(this, args);
            }
        }

        protected void OnCreate()
        {
            if (Create != null)
            {
                int platformSpan = 32 * _length;
                _middleCollisionTile = new DebugTile(_collisionGrid, new Rectangle(_rightEdge.HitBox.Right, this.HitBox.Y + 32, platformSpan, 32));
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = _middleCollisionTile
                };
                Create(this, args);
                Create(this, new ObjectEventArgs() { ObjectSprite = _rightCollisionTile });
                Create(this, new ObjectEventArgs() { ObjectSprite = _leftCollisionTile });
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private RemoveablePlatformEdge _leftEdge;
        private RemoveablePlatformEdge _rightEdge;
        private readonly Guid _activationId;

        public BiomeType Biome
        {
            get { return _type; }
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            if (_leftEdge != null)
                _leftEdge.ChangeBiome(newBiome);
            if (_rightEdge != null)
                _rightEdge.ChangeBiome(newBiome);
        }

        public Point SpriteLocation
        {
            get { return _leftEdge != null ? _leftEdge.SpriteLocation : this.HitBox.Location; }
        }

        public Guid ActivationID => _activationId;
    }

    public class RemoveablePlatformEdge : CollisionObject, ISprite, IActivateable, IBiomeTile
    {

        public RemoveablePlatformEdge(SpaceHashGrid grid, Rectangle hitbox, BiomeType type, Direction direction, bool isActive)
            : base(grid, hitbox)
        {
            _type = type;
            _direction = direction;
            _isActive = isActive;
            _activationId = Guid.NewGuid();
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            UpdateSprite();
        }

        private BiomeType _type;
        private PictureBox _sprite;
        private Direction _direction;
        private bool _isActive;
        private readonly Guid _activationId;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            private set
            {
                _isActive = value;
                UpdateSprite();
            }
        }

        public PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Activate()
        {
            this.IsActive = true;

        }

        public void Deactivate()
        {
            this.IsActive = false;
        }

        private void UpdateSprite()
        {
            //TODO: fill out all the sprites for every biome type
            switch (_type)
            {
                case BiomeType.KEEN4_GREEN:
                case BiomeType.KEEN4_MIRAGE:  
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen4_removable_platform_left_edge_filled : Properties.Resources.keen4_removable_platform_left_edge;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen4_removable_platform_right_edge_filled : Properties.Resources.keen4_removable_platform_right_edge;
                    }
                    break;
                case BiomeType.KEEN4_PYRAMID:
                case BiomeType.KEEN4_CAVE:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen4_removable_platform_left_edge_filled_pyramid : Properties.Resources.keen4_removable_platform_left_edge_pyramid;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen4_removable_platform_right_edge_filled_pyramid : Properties.Resources.keen4_removable_platform_right_edge_pyramid;
                    }
                    break;
                case BiomeType.KEEN5_BLACK:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_left_edge_filled_black : Properties.Resources.keen5_removable_platform_left_edge_black;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_right_edge_filled_black : Properties.Resources.keen5_removable_platform_right_edge_black;
                    }
                    break;
                case BiomeType.KEEN5_GREEN:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_left_edge_filled_green : Properties.Resources.keen5_removable_platform_left_edge_green;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_right_edge_filled_green : Properties.Resources.keen5_removable_platform_right_edge_green;
                    }
                    break;
                case BiomeType.KEEN5_RED:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_left_edge_filled_red : Properties.Resources.keen5_removable_platform_left_edge_red;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen5_removable_platform_right_edge_filled_red : Properties.Resources.keen5_removable_platform_right_edge_red;
                    }
                    break;
                case BiomeType.KEEN6_DOME:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen6_removable_platform_right_edge_filled_dome : Properties.Resources.keen6_removable_platform_right_edge_dome;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen6_removable_platform_left_edge_filled_dome : Properties.Resources.keen6_removable_platform_left_edge_dome;
                    }
                    break;
                case BiomeType.KEEN6_FOREST:
                    if (_direction == Direction.LEFT)
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen6_removable_platform_forest_right_edge_filled : Properties.Resources.keen6_removable_platform_forest_right_edge;
                    }
                    else
                    {
                        _sprite.Image = this.IsActive ? Properties.Resources.keen6_removable_platform_forest_left_edge_filled : Properties.Resources.keen6_removable_platform_forest_left_edge;
                    }
                    break;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public BiomeType Biome
        {
            get { return _type; }
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            _type = newBiome;
            UpdateSprite();
        }

        public Point SpriteLocation
        {
            get { return _sprite.Location; }
        }

        public Guid ActivationID => _activationId;
    }

    public class RemovablePlatformMiddle : RemovableTile, ISprite, IActivateable
    {
        public RemovablePlatformMiddle(SpaceHashGrid grid, Rectangle hitbox, bool isActive)
            : base(grid, hitbox)
        {
            _isActive = isActive;
            _activationId = Guid.NewGuid();
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            UpdateSprite();
        }

        private bool _isActive;
        private readonly Guid _activationId;

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public void Activate()
        {
            this.IsActive = true;
            OnCreate();
        }

        public void Deactivate()
        {
            this.IsActive = false;
            OnRemove();
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                UpdateSprite();
            }
        }

        public Guid ActivationID => _activationId;

        private void UpdateSprite()
        {
            _sprite.Image = this.IsActive ? Properties.Resources.keen4_removable_platform : null;
            _sprite.Location = this.HitBox.Location;
        }

    }
}
