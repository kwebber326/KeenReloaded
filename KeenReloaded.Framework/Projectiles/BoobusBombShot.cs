﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Enemies;

namespace KeenReloaded.Framework.Trajectories
{
    public class BoobusBombShot : KeenStunShot, IExplodable, ICreateRemove
    {
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;

        private Image[] _collidedImages = new Image[] 
        {
            Properties.Resources.keen_dreams_boobus_bomb1
        };
        private bool _collided;
        private int _currentCollidedSprite = 0;
        private bool _reverse;
        private int _velocity = INITIAL_MOVE_VELOCITY;
        private const int INITIAL_MOVE_VELOCITY = 100;
        private const int INITIAL_FALL_VELOCITY = 5;
        private int _fallVelocity = INITIAL_FALL_VELOCITY;
        private const int FALL_ACCELERATION = 5;
        private const int VELOCITY_DECREASE = 2;
        private const int KINETIC_IMPACT_DENOMINATOR = 5;
        private const int MAX_FALL_VELOCITY = 100;
        private bool _readyToExplode;
        private bool _suppressExplosion;
        private int _deflectionCount;
        private const int MAX_DEFLECTIONS = 20;

        private const int EXPLODE_DELAY = 5;
        private int _explodeDelayTick;
        private const int EXPLOSION_SPRITE_CHANGE_DELAY = 1;
        private int _explosionSpriteChangeDelayTick;

        private int GetImpactVelocityHorizontal(int velocity, bool switchDirection)
        {
            double kineticLoss = velocity / KINETIC_IMPACT_DENOMINATOR; /*(INITIAL_MOVE_VELOCITY - Math.Abs(velocity)) / INITIAL_MOVE_VELOCITY;*/ // velocity / VELOCITY_DECREASE;  //((double)velocity) / ((double)INITIAL_MOVE_VELOCITY) * 100.0;
            int kLossInt = Convert.ToInt32(kineticLoss);
            // if (kLossInt < velocity)
            velocity -= kLossInt;
            //else 
            //  velocity = 0;
            if (switchDirection)
                velocity *= -1;
            return velocity;
        }

        private int GetImpactVelocityVertical(int velocity)
        {
            double kineticLoss = velocity / VELOCITY_DECREASE; //(((double)velocity + VELOCITY_DECREASE) - (double)velocity) / ((double)velocity) * 100.0;
            int kLossInt = Convert.ToInt32(kineticLoss);
            if (kLossInt < velocity)
            {
                velocity -= kLossInt;
            }
            else
            {
                velocity = 0;
            }
            velocity *= -1;
            return velocity;
        }

        public BoobusBombShot(SpaceHashGrid grid, Rectangle hitbox,
            int damage, int velocity, int pierce, int spread, int blastRadius, int refireDelay, Direction direction, Direction keenStandingDirection)
            : base(grid, hitbox, damage, velocity, pierce, spread, blastRadius, refireDelay, direction)
        {
            this.KeenStandDirection = keenStandingDirection;
           // this.ObjectComplete += new EventHandler(BoobubBombShot_ObjectComplete);
            this.InitializeSprites();
            this.InitializeVelocities();
            //check initial collisions
            var collisions = this.CheckCollision(this.HitBox, true);
            var tile = this.KeenStandDirection == Enums.Direction.LEFT ? GetLeftMostRightTile(collisions) : GetRightMostLeftTile(collisions);
            if (tile != null && !(tile is DestructibleCollisionTile))
            {
                this.HitBox = new Rectangle(
                    this.KeenStandDirection == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1,
                   this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = ChangeHorizontalDirection(this.Direction);
                _velocity *= -1;
            }
            if (IsVerticalDirection(direction))
            {
                var verticalTile = direction == Enums.Direction.UP ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
                if (verticalTile != null)
                {
                    this.HitBox = this.Direction == Enums.Direction.UP
                        ? new Rectangle(this.HitBox.X, verticalTile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height)
                        : new Rectangle(this.HitBox.X, verticalTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                }
            }
        }

        private void InitializeVelocities()
        {
            if (this.Direction == Enums.Direction.UP)
            {
                _fallVelocity *= -10;
                _velocity = INITIAL_MOVE_VELOCITY / 5;
                if (KeenStandDirection == Enums.Direction.LEFT)
                {
                    _velocity *= -1;
                }
            }
            else if (this.Direction == Enums.Direction.DOWN)
            {
                _velocity = 0;
                _fallVelocity *= 10;
            }

            if (this.Direction == Enums.Direction.LEFT)
            {
                _velocity *= -1;
            }
        }

        public void ForceExplosion()
        {
            OnObjectComplete();
            RemoveThisFromCollidingNodes();
        }

        void BoobubBombShot_ObjectComplete(object sender, EventArgs e)
        {
            this.Explode();
        }

        public Direction KeenStandDirection { get; set; }

        public override Rectangle HitBox
        {
            get
            {
                return base.HitBox;
            }
            protected set
            {
                base.HitBox = value;
                if (this.Sprite != null)
                {
                    this.Sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(this.Direction);
                    if (_fallVelocity > 0)
                    {
                        this.UpdateCollisionNodes(Direction.DOWN);
                    }
                    else if (_fallVelocity < 0)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                }
            }
        }

        protected void SetHorizontalDirection()
        {
            this.Direction = _velocity < 0 ? Direction.LEFT : Direction.RIGHT;
        }

        public override void Update()
        {
            if (!_readyToExplode)
                this.Move();
            else
            {
                UpdateExplosiveState();
            }
        }

        private void UpdateExplosiveState()
        {
            if (_explodeDelayTick < EXPLODE_DELAY)
            {
                if (_explosionSpriteChangeDelayTick++ == EXPLOSION_SPRITE_CHANGE_DELAY)
                {
                    _explosionSpriteChangeDelayTick = 0;
                    _explodeDelayTick++;
                    if (_explodeDelayTick % 2 == 0)
                    {
                        this.Sprite.Image = Properties.Resources.keen_dreams_boobus_bomb2;
                    }
                    else
                    {
                        this.Sprite.Image = Properties.Resources.keen_dreams_boobus_bomb1;
                    }
                }
            }
            else
            {
                _state = Enums.ExplosionState.DONE;
                //OnRemove(new ObjectEventArgs() { ObjectSprite = this });
                RemoveThisFromCollidingNodes();
                OnObjectComplete();
            }
        }


        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            if (collisions.Any() && collisions.OfType<DebugTile>().Any())
            {
                var leftTiles = collisions.OfType<DebugTile>().Where(c => c.HitBox.Left <= this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (leftTiles.Any())
                {
                    int maxX = leftTiles.Select(t => t.HitBox.Right).Max();
                    CollisionObject obj = leftTiles.FirstOrDefault(x => x.HitBox.Right == maxX);
                    return obj;
                }
            }
            return null;
        }

        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            if (collisions.Any() && collisions.OfType<DebugTile>().Any())
            {
                var rightTiles = collisions.OfType<DebugTile>().Where(c => c.HitBox.Left >= this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (rightTiles.Any())
                {
                    int minX = rightTiles.Select(t => t.HitBox.Left).Min();
                    CollisionObject obj = rightTiles.FirstOrDefault(x => x.HitBox.Left == minX);
                    return obj;
                }
            }
            return null;
        }

        public override void Move()
        {

            int velocity = this.Direction == Enums.Direction.LEFT ? _velocity * -1 : _velocity;
            Rectangle areaToCheck = this.Direction == Enums.Direction.LEFT ?
                new Rectangle(this.HitBox.Left + _velocity, _fallVelocity < 0 ? this.HitBox.Y + _fallVelocity : this.HitBox.Y, this.HitBox.Width + velocity, this.HitBox.Height + Math.Abs(_fallVelocity)) :
                new Rectangle(this.HitBox.X, _fallVelocity < 0 ? this.HitBox.Y + _fallVelocity : this.HitBox.Y, this.HitBox.Width + _velocity, this.HitBox.Height + Math.Abs(_fallVelocity));
            var collisions = this.CheckCollision(areaToCheck);
            var tiles = collisions.OfType<DebugTile>();
            var enemies = collisions.OfType<IEnemy>();
            if (enemies.Any())
            {
                enemies = enemies.Where(e => e.IsActive && !(e is ISquashable)).ToList();
            }
           
            CollisionObject tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            CollisionObject groundTile = GetTopMostLandingTile(_fallVelocity);
            CollisionObject ceilingTile = GetCeilingTile(collisions);

            if (enemies.Any())
            {
                if (tile != null && !(tile is DestructibleCollisionTile))
                {
                    if (Direction == Enums.Direction.LEFT)
                    {
                        enemies = enemies.Where(e => ((CollisionObject)e).HitBox.Right > tile.HitBox.Right);
                    }
                    else
                    {
                        enemies = enemies.Where(e => ((CollisionObject)e).HitBox.Left < tile.HitBox.Left);
                    }
                }
                HandleEnemyCollision(enemies.ToList());

                if (!_suppressExplosion)
                {
                    OnObjectComplete();
                }
                else
                {
                    _suppressExplosion = false;
                }
                return;
            }

            if (tile != null || groundTile != null || ceilingTile != null)
            {
                if (tile != null)
                {
                    if (this.Direction == Enums.Direction.LEFT)
                    {
                        this.HitBox = new Rectangle(tile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                    else
                    {
                        this.HitBox = new Rectangle(tile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                }
                _collided = true;
                bool switchDirection = tile != null && tile.HitBox.Top < this.HitBox.Bottom && tile.HitBox.Bottom > this.HitBox.Top;
                _velocity = GetImpactVelocityHorizontal(_velocity, switchDirection);
                SetHorizontalDirection();
                if (ceilingTile != null || groundTile != null)
                {
                    _fallVelocity = GetImpactVelocityVertical(_fallVelocity);
                    if (ceilingTile != null)
                    {
                        this.HitBox = new Rectangle(this.HitBox.X, ceilingTile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
                    }
                    else if (groundTile != null)
                    {
                        this.HitBox = new Rectangle(this.HitBox.X, groundTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                        CheckVelocities();
                        if (_fallVelocity == 0 && _velocity == 0)
                        {
                            _readyToExplode = true;
                            return;
                        }
                    }
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + _velocity, this.HitBox.Y + _fallVelocity, this.HitBox.Width, this.HitBox.Height);
                if (this.Direction == Enums.Direction.RIGHT)
                {
                    if (_velocity >= VELOCITY_DECREASE)
                    {
                        _velocity -= VELOCITY_DECREASE;
                    }
                }
                else if (this.Direction == Enums.Direction.LEFT)
                {
                    if (_velocity * -1 >= VELOCITY_DECREASE)
                    {
                        _velocity += VELOCITY_DECREASE;
                    }
                }
                if (_fallVelocity + FALL_ACCELERATION <= MAX_FALL_VELOCITY)
                {
                    _fallVelocity += FALL_ACCELERATION;
                }
                else
                {
                    _fallVelocity = MAX_FALL_VELOCITY;
                }
            }
            SetHorizontalDirection();
        }

        private void HandleEnemyCollision(List<IEnemy> enemies)
        {
            CollisionObject enemyToMoveTo = null;
            IEnumerable<IStunnable> stunEnemies = enemies.OfType<IStunnable>();
            IEnumerable<DestructibleObject> destructibles = enemies.OfType<DestructibleObject>();
            if (this.Direction == Enums.Direction.LEFT)
            {
                int maxX = int.MinValue;

                if (stunEnemies.Any())
                {
                    foreach (var enemy in stunEnemies)
                    {
                        var eCollision = (CollisionObject)enemy;
                        if (eCollision.HitBox.X > maxX)
                        {
                            maxX = eCollision.HitBox.X;
                            enemyToMoveTo = eCollision;
                        }
                    }
                }
                if (destructibles.Any())
                {
                    foreach (var destructible in destructibles)
                    {
                        var eCollision = (CollisionObject)destructible;
                        if (eCollision.HitBox.X > maxX)
                        {
                            maxX = eCollision.HitBox.X;
                            enemyToMoveTo = eCollision;
                        }
                    }
                }
                else
                {
                    enemyToMoveTo = enemies.FirstOrDefault(e => e.IsActive) as CollisionObject;
                    if (enemyToMoveTo != null && enemyToMoveTo is RoboRed)
                    {
                        ((RoboRed)enemyToMoveTo).Alert();
                    }
                }
                if (enemyToMoveTo != null)
                {
                    if (_velocity == 0)
                    {
                        var center = enemyToMoveTo.HitBox.Left + enemyToMoveTo.HitBox.Width / 2;
                        if (this.HitBox.X >= center)
                        {
                            this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        }
                        else
                        {
                            this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        }
                    }
                    else
                    {
                        this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                    if (enemyToMoveTo is IStunnable)
                    {
                        ((IStunnable)enemyToMoveTo).Stun();
                    }
                    else if (enemyToMoveTo is IDeflector)
                    {
                        var deflector = (IDeflector)enemyToMoveTo;
                        if (deflector.DeflectsHorizontally)
                        {
                            _velocity = GetImpactVelocityHorizontal(_velocity, true);
                            SetHorizontalDirection();
                            _suppressExplosion = true;
                            var collisions = this.CheckCollision(this.HitBox, true);
                            if (++_deflectionCount >= MAX_DEFLECTIONS || collisions.Any())
                            {
                                _readyToExplode = true;
                            }
                            else
                            {
                                this.Update();
                            }
                        }
                        else if (enemyToMoveTo is DestructibleObject)
                        {
                            ((DestructibleObject)enemyToMoveTo).TakeDamage(this.Damage);
                        }
                    }
                    else if (enemyToMoveTo is DestructibleObject)
                    {
                        ((DestructibleObject)enemyToMoveTo).TakeDamage(this.Damage);
                    }
                }
            }
            else
            {
                int minX = int.MaxValue;

                if (stunEnemies.Any())
                {
                    foreach (var enemy in stunEnemies)
                    {
                        var eCollision = (CollisionObject)enemy;
                        if (eCollision.HitBox.X < minX)
                        {
                            minX = eCollision.HitBox.X;
                            enemyToMoveTo = eCollision;
                        }
                    }
                }
                if (destructibles.Any())
                {
                    foreach (var destructible in destructibles)
                    {
                        var eCollision = (CollisionObject)destructible;
                        if (eCollision.HitBox.X < minX)
                        {
                            minX = eCollision.HitBox.X;
                            enemyToMoveTo = eCollision;
                        }
                    }
                }
                else
                {
                    enemyToMoveTo = enemies.FirstOrDefault(e => e.IsActive) as CollisionObject;
                    if (enemyToMoveTo != null && enemyToMoveTo is RoboRed)
                    {
                        ((RoboRed)enemyToMoveTo).Alert();
                    }
                }
                if (enemyToMoveTo != null)
                {
                    if (_velocity == 0)
                    {
                        var center = enemyToMoveTo.HitBox.Left + enemyToMoveTo.HitBox.Width / 2;
                        if (this.HitBox.X >= center)
                        {
                            this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        }
                        else
                        {
                            this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        }
                    }
                    else
                    {
                        this.HitBox = new Rectangle(enemyToMoveTo.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                    if (enemyToMoveTo is IStunnable)
                    {
                        ((IStunnable)enemyToMoveTo).Stun();
                    }
                    else if (enemyToMoveTo is IDeflector)
                    {
                        var deflector = (IDeflector)enemyToMoveTo;
                        if (deflector.DeflectsHorizontally)
                        {
                            _velocity = GetImpactVelocityHorizontal(_velocity, true);
                            SetHorizontalDirection();
                            _suppressExplosion = true;
                            var collisions = this.CheckCollision(this.HitBox, true);
                            if (++_deflectionCount >= MAX_DEFLECTIONS || collisions.Any())
                            {
                                _readyToExplode = true;
                            }
                            else
                            {
                                this.Update();
                            }
                        }
                        else if (enemyToMoveTo is DestructibleObject)
                        {
                            ((DestructibleObject)enemyToMoveTo).TakeDamage(this.Damage);
                        }
                    }
                    else if (enemyToMoveTo is DestructibleObject)
                    {
                        ((DestructibleObject)enemyToMoveTo).TakeDamage(this.Damage);
                    }
                }
            }
        }

        private void CheckVelocities()
        {
            if (Math.Abs(_fallVelocity) <= FALL_ACCELERATION && Math.Abs(_velocity) <= VELOCITY_DECREASE)
            {
                _fallVelocity = 0;
                _velocity = 0;
            }
        }

        protected override void InitializeSprites()
        {
            base.InitializeSprites();

            _shotCompleteSprites = new Image[]{

            };
            _shotSprites = new Image[] 
            {
                Properties.Resources.keen_dreams_boobus_bomb1
            };
            this.Sprite.Image = _shotSprites[0];
        }

        public void Explode()
        {
            if (!_exploded)
            {
                _readyToExplode = true;
                BoobusBombExplosion explosion = new BoobusBombExplosion(_collisionGrid, this.HitBox, this.BlastRadius, 0);//Damage on the explosion for this makes this weapon overpowered. save that for the rocket launcher
                explosion.Remove += new EventHandler<ObjectEventArgs>(explosion_Remove);
                explosion.Create += new EventHandler<ObjectEventArgs>(explosion_Create);
                ObjectEventArgs e = new ObjectEventArgs()
                {
                    ObjectSprite = explosion
                };
                OnCreate(e);
                _exploded = true;
            }
        }

        void explosion_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        void explosion_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        public Enums.ExplosionState ExplosionState
        {
            get { return _state; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private ExplosionState _state;
        private bool _exploded;

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
                if (e.ObjectSprite == this)
                {
                    RemoveThisFromCollidingNodes();
                }
                this.Remove(this, e);
            }
        }

        private void RemoveThisFromCollidingNodes()
        {
            foreach (var node in _collidingNodes)
            {
                node.Objects.Remove(this);
                node.NonEnemies.Remove(this);
            }
        }
    }
}
