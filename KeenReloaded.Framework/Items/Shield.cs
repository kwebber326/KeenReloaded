using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Items
{
    public class Shield : Item, IUpdatable
    {
        private CommanderKeen _keen;
        private int _duration;
        private bool _acquiredEventRegistered;
        public bool _isActive;

        private const int SHIELD_DEPLETION_DELAY = 10;
        private int _currentShieldDepletionDelayTick;
        public event EventHandler<ObjectEventArgs> ShieldDurationChanged;

        private const int SHIELD_WARNING_DELAY = 5;
        private const int SHIELD_WARNING_THRESHOLD = 10;
        private int _currentShieldWarningDelayTick;
        private bool _isRed;

        public event EventHandler Depleted;

        public Shield(SpaceHashGrid grid, Rectangle hitbox, int duration, CommanderKeen keen) : base(grid, hitbox)
        {
            this.SpriteList = new Image[] { SpriteSheet.Shield };
            _keen = keen;
            _duration = duration;
            _keen.KeenDied += _keen_KeenDied;
            this.Sprite.Image = this.SpriteList[0];
        }

        private void _keen_KeenDied(object sender, ObjectEventArgs e)
        {
            _keen.KeenMoved -= _keen_KeenMoved;
            _keen.KeenDied -= _keen_KeenDied;
        }

        private void _keen_KeenMoved(object sender, EventArgs e)
        {
            if (_keen.HasShield)
                this.UpdateSpriteToSurroundKeen();
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
                if (value != null && this.Sprite != null)
                {
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }
        public int Duration
        {
            get
            {
                return _duration;
            }
        }

        public bool TryActivate()
        {
            if (_duration > 0)
            {
                if (!_isActive)
                {
                    OnCreate();
                    this.Sprite.BringToFront();
                }
                _isActive = true;
                UpdateSpriteToSurroundKeen();
            }
            return _isActive;
        }

        public void Deactivate()
        {
            _isActive = false;
            OnRemoved();
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        public override void Update()
        {
            if (this.IsAcquired && _isActive)
            {
                this.Sprite.Image = Properties.Resources.Shield;
                if (!_acquiredEventRegistered)
                {
                    _keen.KeenMoved += _keen_KeenMoved;
                    _acquiredEventRegistered = true;
                }
                if (++_currentShieldDepletionDelayTick == SHIELD_DEPLETION_DELAY)
                {
                    _currentShieldDepletionDelayTick = 0;
                    if (_duration > 0)
                    {
                        _duration--;
                        if (_duration == 0)
                        {
                            this.Deactivate();
                            OnDepleted();
                        }
                        
                        OnShieldDurationChanged();
                    }
                }
                if (_duration > 0 && _duration <= SHIELD_WARNING_THRESHOLD)
                {
                    this.ToggleWarning();
                }
                else
                {
                    _isRed = false;
                    this.Sprite.BackColor = Color.Transparent;
                }
            }
        }

        protected void OnShieldDurationChanged()
        {
            this.ShieldDurationChanged?.Invoke(this, new ObjectEventArgs() { ObjectSprite = this });
        }

        protected void OnDepleted()
        {
            this.Depleted?.Invoke(this, EventArgs.Empty);
        }

        public void AddShieldToCurrent(Shield shield)
        {
            _duration += shield?.Duration ?? 0;
            OnShieldDurationChanged();
        }

        private void UpdateSpriteToSurroundKeen()
        {
            int widthDifference = Math.Abs(this.HitBox.Width - _keen.HitBox.Width);
            int heightDifference = Math.Abs(this.HitBox.Height - _keen.HitBox.Height);

            int centerX = _keen.HitBox.X - (widthDifference / 2);
            int centerY = _keen.HitBox.Y - (heightDifference / 2);

            Point newLocation = new Point(centerX, centerY);
            this.HitBox = new Rectangle(newLocation, this.HitBox.Size);
        }

        private void ToggleWarning()
        {
            if (++_currentShieldWarningDelayTick == SHIELD_WARNING_DELAY)
            {
                _currentShieldWarningDelayTick = 0;
                _isRed = !_isRed;
                this.Sprite.BackColor = _isRed ? Color.Red : Color.Transparent;
            }
        }

        public void SetKeen(CommanderKeen keen)
        {
            _keen = keen;
            if (_keen != null)
            {
                _keen.KeenDied += _keen_KeenDied;
                _keen.KeenMoved += _keen_KeenMoved;
            }
        }
    }
}
