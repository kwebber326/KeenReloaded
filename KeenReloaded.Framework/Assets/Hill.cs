using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Assets
{
    public class Hill : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        protected readonly CommanderKeen _keen;
        protected readonly List<Point> _points;
        protected readonly int _holdTime;
        protected readonly int _spawnDelay;
        protected readonly int _pointsPerSecond;
        protected readonly int _additionalPointsPerMonster;
        protected PictureBox _sprite;

        protected int _currentHoldTimeTick;
        protected int _currentSpawnDelayTick;
        protected HillState _state;

        protected const int SPRITE_CHANGE_DELAY = 5;
        protected int _currentSpriteChangeDelayTick;

        protected int _currentPoint = -1;

        protected const int POINT_EVALUATION_DELAY = 20;
        protected int _currentPointEvaluationDelayTick;

        protected bool _fading;
        protected bool _firstAppearance = true;

        public event EventHandler<ObjectEventArgs> Create;
        public event EventHandler<ObjectEventArgs> Remove;

        public Hill(SpaceHashGrid grid, Rectangle hitbox, List<Point> points, int holdTimeSeconds, int spawnDelaySeconds, int pointsPerSecond, int additionPointsPerMonster, CommanderKeen keen) : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("keen was not properly set");

            _keen = keen;
            _points = points;
            _holdTime = holdTimeSeconds * 20;
            _spawnDelay = spawnDelaySeconds * 20;
            _pointsPerSecond = pointsPerSecond;
            _additionalPointsPerMonster = additionPointsPerMonster;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Size = this.HitBox.Size;
            this.HillState = HillState.INACTIVE;
            _sprite.Image = null;
        }
        public override Rectangle HitBox
        {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (_sprite != null && value != null)
                {
                    _sprite.Location = value.Location;
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public PictureBox Sprite => _sprite;

        public bool IsActive
        {
            get
            {
                return _state != HillState.INACTIVE;
            }
        }

        public HillState HillState
        {
            get
            {
                return _state;
            }
            protected set
            {
                _state = value;
                if (this.IsActive)
                {
                    _sprite.Image = SpriteSheet.HillSprites[(int)_state];
                }
                else
                {
                    _sprite.Image = null;
                    OnRemove();
                }
            }
        }

        public void Update()
        {
            if (!this.IsActive)
            {
                _currentPointEvaluationDelayTick = 0;
                if (_currentSpawnDelayTick++ == _spawnDelay)
                {
                    _currentSpawnDelayTick = 0;
                    OnCreate();
                    if (_firstAppearance)
                    {
                        this.HillState = HillState.STRENGTH1;
                        _firstAppearance = false;
                    }
                    else
                    {
                        SpawnAtNextLocation();
                    }
                }
            }
            else if (this.HillState != HillState.STRENGTH4 && !_fading)
            {
                if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                {
                    _currentSpriteChangeDelayTick = 0;
                    UpdateStrength();
                }
                EvaluateScoreOnDelay();
            }
            else
            {
                EvaluateScoreOnDelay();
                if (!_fading)
                {
                    if (_currentHoldTimeTick++ == _holdTime)
                    {
                        _currentHoldTimeTick = 0;
                        _fading = true;
                    }
                }
                else if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                {
                    _currentSpriteChangeDelayTick = 0;
                    int strength = (int)this.HillState;
                    if (strength > 0)
                    {
                        UpdateStrength();
                    }
                    else
                    {
                        ResetSequence();
                    }
                }
            }
        }

        private void ResetSequence()
        {
            _fading = false;
            _currentHoldTimeTick = 0;
            _currentSpawnDelayTick = 0;
            _currentSpriteChangeDelayTick = 0;
            this.HillState = HillState.INACTIVE;
        }

        private void EvaluateScoreOnDelay()
        {
            if (_currentPointEvaluationDelayTick++ == POINT_EVALUATION_DELAY)
            {
                _currentPointEvaluationDelayTick = 0;
                EvaluatePointsScored();
            }
        }

        private void EvaluatePointsScored()
        {
            int totalPointsScored = 0;
            if (!_keen.IsDead() && _keen.HitBox.IntersectsWith(this.HitBox))
            {
                totalPointsScored += _pointsPerSecond;


                var collisions = this.CheckCollision(this.HitBox);
                var enemies = collisions.OfType<IEnemy>();
                if (enemies.Any(e => e.IsActive))
                {
                    int additionalPoints = enemies.Count(e => e.IsActive) * _additionalPointsPerMonster;
                    totalPointsScored += additionalPoints;
                }
            }

            if (totalPointsScored > 0)
            {
                _keen.GivePoints(totalPointsScored);
            }
        }

        protected virtual void SpawnAtNextLocation()
        {
            this.HillState = HillState.STRENGTH1;
            if (++_currentPoint >= _points.Count)
            {
                _currentPoint = 0;

            }
            this.HitBox = new Rectangle(_points[_currentPoint], this.HitBox.Size);
        }

        private void UpdateStrength()
        {
            int currentStrength = (int)this.HillState;
            if (!_fading)
                currentStrength++;
            else if (currentStrength > 0)
                currentStrength--;
            this.HillState = (HillState)Enum.Parse(typeof(HillState), "STRENGTH" + (currentStrength + 1));
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        protected void OnCreate()
        {
            this.Create?.Invoke(this, new ObjectEventArgs() { ObjectSprite = this });
        }

        protected void OnRemove()
        {
            this.Remove?.Invoke(this, new ObjectEventArgs() { ObjectSprite = this });
        }
    }

    public enum HillState
    {
        STRENGTH1 = 0,
        STRENGTH2 = 1,
        STRENGTH3 = 2,
        STRENGTH4 = 3,
        INACTIVE = -1
    }
}
