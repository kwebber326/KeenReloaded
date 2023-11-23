using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Hazards;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class PoisonSlug : DestructibleObject, IUpdatable, IGravityObject, ISprite, IMoveable, IEnemy, ICreateRemove, IZombieBountyEnemy
    {
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private SlugMoveState _state;

        private const int FALL_VELOCITY = 20;
        private const int MOVE_VELOCITY = 2;
        private int _currentMoveSprite;
        private int _currentStunnedSprite;
        private int _currentStunnedSpriteSet;

        private const int REEVALUATE_DIRECTION_DELAY = 60;
        private int _currentReevaluationDelayTick;

        private const int MOVE_SPRITE_UPDATE_DELAY = 1;
        private int _currentMoveSpriteUpdateDelaytick;

        private bool _stunSpritesChosen = false;

        private const int POOP_PROBABILITY = 2;//set this number higher for lower poop probability
        private const int TRY_POOP_DELAY = 40;
        private const int EDGE_OFFSET = 4;
        private int _tryPoopDelayTick = 0;
        private const int POOP_DELAY = 10;
        private int _currentPoopDelayTick = 0;

        private const int MAX_JUMP_HEIGHT = 20;
        private const int JUMP_VELOCITY = 5;
        private int _currentJumpHeight = 0;

        private Image[] _moveLeftImages = new Image[]
        {
            Properties.Resources.keen4_slug_move_left1,
            Properties.Resources.keen4_slug_move_left2
        };

        private Image[] _moveRightImages = new Image[]
        {
            Properties.Resources.keen4_slug_move_right1,
            Properties.Resources.keen4_slug_move_right2
        };

        private Image[] _stunImages1 = new Image[]
        {
            Properties.Resources.keen4_slug_stunned1_1,
            Properties.Resources.keen4_slug_stunned1_2,
            Properties.Resources.keen4_slug_stunned1_3,
            Properties.Resources.keen4_slug_stunned1_4
        };

        private Image[] _stunImages2 = new Image[]
        {
            Properties.Resources.keen4_slug_stunned2_1,
            Properties.Resources.keen4_slug_stunned2_2,
            Properties.Resources.keen4_slug_stunned2_3,
            Properties.Resources.keen4_slug_stunned2_4
        };

        public PoisonSlug(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Health = 1;
            this.Direction = GetRandomDirection();
            _sprite.Location = this.HitBox.Location;
            this.State = SlugMoveState.FALLING;
            this.Fall();
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        internal SlugMoveState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateSprite();
            }
        }

        public override void Die()
        {
            this.State = SlugMoveState.STUNNED;
            Random random = new Random();
            _currentStunnedSpriteSet = random.Next(1, 3);
            _stunSpritesChosen = true;
            OnKilled();
        }

        private Direction GetRandomDirection()
        {
            int i = _random.Next(0, 2);
            var direction = i == 0 ? Direction.LEFT : Direction.RIGHT;
            return direction;
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
                    if (this.State == SlugMoveState.FALLING)
                    {
                        this.UpdateCollisionNodes(Direction.DOWN);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(this.Direction);
                    }
                }
            }
        }

        public void Jump()
        {
            if (_currentJumpHeight < MAX_JUMP_HEIGHT)
            {
                var xOffset = this.Direction == Enums.Direction.LEFT ? MOVE_VELOCITY * -1 : MOVE_VELOCITY;
                Rectangle areaToCheck = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y - JUMP_VELOCITY, this.HitBox.Width, this.HitBox.Height);
                var collisions = this.CheckCollision(areaToCheck);
                var tiles = collisions.OfType<DebugTile>();
                if (tiles.Any())
                {
                    HandleAllTileCollisionsFromJumpState(xOffset, collisions, tiles);
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y - JUMP_VELOCITY, this.HitBox.Width, this.HitBox.Height);
                    _currentJumpHeight += JUMP_VELOCITY;
                }
            }
            else
            {
                InitiateFallFromJumpingState();
            }
        }

        private void HandleAllTileCollisionsFromJumpState(int xOffset, List<CollisionObject> collisions, IEnumerable<DebugTile> tiles)
        {
            if (!tiles.Any(t1 => t1.HitBox.Bottom < this.HitBox.Top))
                return;

            //find vertical collisions if any
            var maxY = tiles.Where(t1 => t1.HitBox.Bottom < this.HitBox.Top).ToList().Select(t => t.HitBox.Bottom).Max();
            var yPos = maxY > 0 ? maxY + 1 : this.HitBox.Y - JUMP_VELOCITY;
            //find horizontal Collisionts if any
            var xPos = this.HitBox.X + xOffset;
            if (this.Direction == Enums.Direction.LEFT)
            {
                //if direction is left, find right most left tile
                CollisionObject t = GetRightMostLeftTile(collisions);
                if (t != null)
                {
                    xPos = t.HitBox.Right + 1;
                }
            }
            else
            {
                //else, find the left most right tile
                CollisionObject t = GetLeftMostRightTile(collisions);
                if (t != null)
                {
                    xPos = t.HitBox.Left - 1;
                }
            }
            //update location
            this.HitBox = new Rectangle(xPos, yPos, this.HitBox.Width, this.HitBox.Height);
            //if there was a Y collision, fall
            if (maxY > 0)
            {
                InitiateFallFromJumpingState();
            }
        }

        private void InitiateFallFromJumpingState()
        {
            this.State = SlugMoveState.FALLING;
            _currentJumpHeight = 0;
            _wasMovingWhenShot = false;
        }

        public bool CanJump
        {
            get { return _wasMovingWhenShot; }
        }

        public void Fall()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + FALL_VELOCITY);
            var collisions = this.CheckCollision(areaToCheck);

            CollisionObject tile = this.GetTopMostLandingTile(collisions);

            if (tile != null)
            {        
                if (IsDead())
                {
                    this.State = SlugMoveState.STUNNED;
                }
                else
                {
                    this.State = SlugMoveState.MOVING;
                }
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                if (!this.IsDead() && collisions.OfType<CommanderKeen>().Any(k => k.HitBox.Top < tile.HitBox.Top))
                {
                    var keens = collisions.OfType<CommanderKeen>().Where(k => k.HitBox.Top < tile.HitBox.Top);
                    foreach (var keen in keens)
                    {
                        keen.Die();
                    }
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + FALL_VELOCITY, this.HitBox.Width, this.HitBox.Height);
                if (!this.IsDead())
                    KillCollidingKeens(collisions);
            }
        }

        protected override CollisionObject GetTopMostLandingTile(List<CollisionObject> collisions)
        {
            CollisionObject topMostTile;
            var landingTiles = collisions.Where(h => (h is DebugTile || h is PlatformTile)
                && h.HitBox.Top >= this.HitBox.Top);

            if (!landingTiles.Any())
                return null;

            int minY = landingTiles.Select(c => c.HitBox.Top).Min();
            topMostTile = landingTiles.FirstOrDefault(t => t.HitBox.Top == minY);

            return topMostTile;
        }

        private DebugTile GetHighestLandingTile(List<CollisionObject> collisions)
        {
            var tiles = collisions.OfType<DebugTile>();
            if (tiles.Any())
            {
                return tiles.OrderBy(c => c.HitBox.Top).FirstOrDefault();
            }
            return null;
        }

        private PlatformTile GetHighestLandingPlatform(List<CollisionObject> collisions)
        {
            var tiles = collisions.OfType<PlatformTile>();
            if (tiles.Any())
            {
                return tiles.OrderBy(c => c.HitBox.Top).FirstOrDefault();
            }
            return null;
        }

        public void Update()
        {
            if (this.State == SlugMoveState.FALLING)
            {
                this.Fall();
            }
            else if (this.State == SlugMoveState.MOVING)
            {
                this.Move();
            }
            else if (this.State == SlugMoveState.POOPING)
            {
                this.Poop();
            }
            else if (this.State == SlugMoveState.STUNNED)
            {
                if (_wasMovingWhenShot)
                {
                    this.Jump();
                }
                else
                {
                    this.BasicFall(FALL_VELOCITY);
                }
                UpdateStunSprite();
            }
        }

        private void UpdateStunSprite()
        {
            if (!_stunSpritesChosen)
            {
                Random random = new Random();
                _currentStunnedSpriteSet = random.Next(1, 3);
                _stunSpritesChosen = true;
                this.Sprite.Location = new Point(this.Sprite.Location.X, this.Sprite.Location.Y - 10);
            }

            var currentSprites = _currentStunnedSpriteSet == 1 ? _stunImages1 : _stunImages2;
            if (++_currentStunnedSprite < currentSprites.Length)
            {
                this.Sprite.Image = currentSprites[_currentStunnedSprite];
            }
            else
            {
                _currentStunnedSprite = 1;
            }
        }

        private void Poop()
        {
            if (_currentPoopDelayTick++ == POOP_DELAY)
            {
                _currentPoopDelayTick = 0;

                PoisonSlugPoop poop = new PoisonSlugPoop(_collisionGrid, new Rectangle(this.HitBox.X + this.HitBox.Width / 2, this.HitBox.Bottom - PoisonSlugPoop.POOP_HEIGHT, PoisonSlugPoop.POOP_WIDTH, PoisonSlugPoop.POOP_HEIGHT));
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = poop
                };
                poop.Remove += new EventHandler<ObjectEventArgs>(poop_Remove);
                OnCreate(args);
                this.State = SlugMoveState.MOVING;
            }
        }

        void poop_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Move()
        {
            int xOffset = this.Direction == Enums.Direction.LEFT ? MOVE_VELOCITY * -1 : MOVE_VELOCITY;
            int collisionStartPoint = this.Direction == Enums.Direction.LEFT ? this.HitBox.X : this.HitBox.Right;

            Rectangle areaToCheck = new Rectangle(collisionStartPoint + xOffset, this.HitBox.Y, xOffset, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck);
            CollisionObject tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            KillCollidingKeens(collisions);

            if (TryPoop())
            {
                return;
            }

            if (_currentMoveSpriteUpdateDelaytick++ == MOVE_SPRITE_UPDATE_DELAY)
            {
                _currentMoveSpriteUpdateDelaytick = 0;
                UpdateMoveSprite();
            }

            if (tile != null)
            {
                this.HitBox = this.Direction == Enums.Direction.LEFT ?
                    new Rectangle(tile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height) :
                    new Rectangle(tile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                SwitchDirection();
                return;
            }

            List<CollisionObject> tilesBelow = GetTilesBelow(xOffset);
            if (!tilesBelow.Any())
            {
                this.State = SlugMoveState.FALLING;
                return;
            }
            else if (IsOnEdge(this.Direction, EDGE_OFFSET))
            {
                SwitchDirection();
            }

            if (_currentReevaluationDelayTick++ == REEVALUATE_DIRECTION_DELAY)
            {
                _currentReevaluationDelayTick = 0;
                this.Direction = GetRandomDirection();
            }

            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
        }

        protected override bool IsOnEdge(Direction directionToCheck, int edgeOffset = 0)
        {
            if (directionToCheck == Direction.LEFT)
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.Left - this.HitBox.Width + edgeOffset, this.HitBox.Bottom, this.HitBox.Width, 10);
                var tiles = this.CheckCollision(areaToCheck, true);
                return !tiles.Any();
            }
            else if (directionToCheck == Direction.RIGHT)
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.Right - edgeOffset, this.HitBox.Bottom, this.HitBox.Width, 10);
                var tiles = this.CheckCollision(areaToCheck, true);
                return !tiles.Any();
            }
            return false;
        }


        private bool TryPoop()
        {
            if (_tryPoopDelayTick++ == TRY_POOP_DELAY)
            {
                _tryPoopDelayTick = 0;
                int poopValue = _random.Next(0, POOP_PROBABILITY);
                if (poopValue == 0)
                {
                    this.State = SlugMoveState.POOPING;
                    return true;
                }
            }
            return false;
        }

        private void UpdateMoveSprite()
        {
            if (_currentMoveSprite < _moveLeftImages.Length)
            {
                this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? _moveLeftImages[_currentMoveSprite] : _moveRightImages[_currentMoveSprite];
                _currentMoveSprite++;
            }
            else
            {
                _currentMoveSprite = 0;
            }
        }

        private void KillCollidingKeens(List<CollisionObject> collisions)
        {
            var collidingKeens = collisions.OfType<CommanderKeen>();
            if (collidingKeens.Any())
            {
                foreach (var keen in collidingKeens)
                {
                    keen.Die();
                }
            }
        }

        private void SwitchDirection()
        {
            _currentReevaluationDelayTick = 0;
            this.Direction = this.Direction == Enums.Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }

        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            var tiles = collisions.OfType<DebugTile>();
            if (tiles.Any())
            {
                return tiles.Where(t => t.HitBox.Top < this.HitBox.Bottom).OrderByDescending(c => c.HitBox.Right).FirstOrDefault();
            }
            return null;
        }

        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            var tiles = collisions.OfType<DebugTile>();
            if (tiles.Any())
            {
                return tiles.Where(t => t.HitBox.Top < this.HitBox.Bottom).OrderBy(c => c.HitBox.Left).FirstOrDefault();
            }
            return null;
        }

        private List<CollisionObject> GetTilesBelow(int xOffset)
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Bottom, this.HitBox.Width, 10);
            var collisions = this.CheckCollision(areaToCheck);
            var tiles = collisions.Where(c => c is DebugTile || c is PlatformTile || c is PoleTile).ToList();
            return tiles;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public Enums.MoveState MoveState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Enums.Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            switch (State)
            {
                case SlugMoveState.FALLING:
                case SlugMoveState.MOVING:
                    if (!IsDead())
                    {
                        var sprites = this.Direction == Direction.LEFT ? _moveLeftImages : _moveRightImages;
                        if (_currentMoveSprite >= sprites.Length)
                        {
                            _currentMoveSprite = 0;
                        }
                        this.Sprite.Image = sprites[_currentMoveSprite];
                    }
                    break;
                case SlugMoveState.POOPING:
                    this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? Properties.Resources.keen4_slug_poop_left : Properties.Resources.keen4_slug_poop_right;
                    break;
                case SlugMoveState.STUNNED:
                    this.Sprite.Image = _currentStunnedSpriteSet == 1 ? _stunImages1[_currentStunnedSprite] : _stunImages2[_currentStunnedSprite];
                    break;
            }
            this.HitBox = new Rectangle(new Point(this.HitBox.X, this.HitBox.Y), this.HitBox.Size);
        }

        public bool DeadlyTouch
        {
            get { return !IsDead(); }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            if (trajectory != null)
            {
                this.Health -= trajectory.Damage;
                if (this.Health <= 0)
                {
                    if (this.State == SlugMoveState.MOVING)
                    {
                        _wasMovingWhenShot = true;
                    }
                    this.Die();
                }
            }
        }

        public bool IsActive
        {
            get { return !IsDead(); }
        }

        public PointItemType PointItem => PointItemType.KEEN4_THREE_TOOTH_GUM;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (Remove != null)
            {
                Remove(this, args);
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private bool _wasMovingWhenShot;


        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum SlugMoveState
    {
        MOVING,
        FALLING,
        POOPING,
        STUNNED
    }
}
