﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class Ceilick : DestructibleObject, IUpdatable, ISprite, IEnemy, IZombieBountyEnemy
    {
        private CeilickState _state;
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private Rectangle _originalLocation;

        private Image[] _attackSprites, _peakSprites, _tauntSprites, _stunnedSprites;
        private int _currentAttackSprite, _currentPeakSprite, _currentTauntSprite, _currentStunnedSprite;

        private const int ATTACK_SPRITE_CHANGE_DELAY = 1, PEAK_SPRITE_CHANGE_DELAY = 0, TAUNT_SPRITE_CHANGE_DELAY = 2, STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentAttackSpriteChangeDelayTick, _currentPeakSpriteChangeDelayTick, _currentTauntSpriteChangeDelayTick, _currentStunnedSpriteChangeDelayTick;

        private const int ATTACK_HEIGHT_RANGE = 64, ATTACK_WIDTH_RANGE = 48;
        private int _attackState;

        private bool _retracting;
        private bool _shouldTaunt;
        private const int TAUNTS = 3;
        private int _tauntCount;

        private const int HIDE_TIME = 10;
        private int _hideTimeTick;

        public Ceilick(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _originalLocation = new Rectangle(hitbox.Location, hitbox.Size);
            Inititalize();
        }

        private void Inititalize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            _attackSprites = SpriteSheet.CeilickAttackImages;
            _peakSprites = SpriteSheet.CeilickPeakImages.Take(6).ToArray();
            _tauntSprites = SpriteSheet.CeilickPeakImages.Skip(5).Take(2).ToArray();
            _stunnedSprites = SpriteSheet.CeilickStunnedImages;

            this.Wait();
            this.UpdateSprite();
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
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public override void Die()
        {
            this.UpdateStunnedState();
        }

        public void Update()
        {
            switch (_state)
            {
                case CeilickState.WAITING:
                    this.Wait();
                    break;
                case CeilickState.ATTACKING:
                    this.Attack();
                    break;
                case CeilickState.PEAKING:
                    this.Peak();
                    break;
                case CeilickState.TAUNTING:
                    this.Taunt();
                    break;
                case CeilickState.HIDING:
                    this.Hide();
                    break;
                case CeilickState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void Hide()
        {
            if (this.State != CeilickState.HIDING)
            {
                this.State = CeilickState.HIDING;
                _hideTimeTick = 0;
            }

            if (_hideTimeTick++ == HIDE_TIME)
            {
                this.Wait();
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != CeilickState.STUNNED)
            {
                this.State = CeilickState.STUNNED;
            }

            this.UpdateSpriteByDelayBase(ref _currentStunnedSpriteChangeDelayTick, ref _currentStunnedSprite, STUNNED_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void Taunt()
        {
            if (this.State != CeilickState.TAUNTING)
            {
                this.State = CeilickState.TAUNTING;
                _tauntCount = 0;
            }

            this.UpdateSpriteByDelayBase(ref _currentTauntSpriteChangeDelayTick, ref _currentTauntSprite, TAUNT_SPRITE_CHANGE_DELAY, UpdateSprite);
            if (_tauntCount == TAUNTS)
            {
                _retracting = true;
                this.Peak();
            }
        }

        private void Peak()
        {
            if (this.State != CeilickState.PEAKING)
            {
                this.State = CeilickState.PEAKING;
                _shouldTaunt = false;
                if (!_retracting)
                {
                    _currentPeakSprite = 0;
                    int widthDiff = ATTACK_WIDTH_RANGE - this.HitBox.Width;
                    this.HitBox = new Rectangle(this.HitBox.X - widthDiff / 2 + 7, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
                else
                {
                    _currentPeakSprite = _peakSprites.Length - 1;
                }
            }
            if (!_retracting)
            {
                this.UpdateSpriteByDelayBase(ref _currentPeakSpriteChangeDelayTick, ref _currentPeakSprite, PEAK_SPRITE_CHANGE_DELAY, UpdateSprite);
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                if (_shouldTaunt)
                {
                    this.Taunt();
                }
            }
            else
            {
                if (_currentPeakSpriteChangeDelayTick++ == PEAK_SPRITE_CHANGE_DELAY)
                {
                    _currentPeakSpriteChangeDelayTick = 0;
                    if (_currentPeakSprite > 0)
                    {
                        _currentPeakSprite--;
                        UpdateSprite();
                    }
                    else
                    {
                        this.HitBox = new Rectangle(_originalLocation.Location, _originalLocation.Size);
                        this.Hide();
                    }
                }
            }
        }

        private void Attack()
        {
            if (this.State != CeilickState.ATTACKING)
            {
                this.State = CeilickState.ATTACKING;
                _attackState = 0;
                _currentAttackSprite = 0;
            }

            if (_currentAttackSpriteChangeDelayTick++ == ATTACK_SPRITE_CHANGE_DELAY)
            {
                _currentAttackSpriteChangeDelayTick = 0;
                switch (_attackState)
                {
                    case 0:
                        _currentAttackSprite = 1;//wag left
                        this.HitBox = new Rectangle(this.HitBox.X - 10, this.HitBox.Y, this.HitBox.Width + 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 1:
                        _currentAttackSprite = 2; //straight
                        this.HitBox = new Rectangle(this.HitBox.X + 10, this.HitBox.Y, this.HitBox.Width - 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 2:
                        _currentAttackSprite = 3;//wag right
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width + 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 3:
                        _currentAttackSprite = 2;
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width - 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 4:
                        _currentAttackSprite = 1;
                        this.HitBox = new Rectangle(this.HitBox.X - 10, this.HitBox.Y, this.HitBox.Width + 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 5:
                        _currentAttackSprite = 2;
                        this.HitBox = new Rectangle(this.HitBox.X + 10, this.HitBox.Y, this.HitBox.Width - 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 6:
                        _currentAttackSprite = 3;
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width + 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 7:
                        _currentAttackSprite = 2;
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width - 10, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 8:
                        _currentAttackSprite = 0;//retracting
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 9:
                        _sprite.Image = Properties.Resources.keen6_ceilick_tongue_waiting;//retracting further
                        this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, _attackSprites[_currentAttackSprite].Height);
                        break;
                    case 10://ready to taunt
                        _sprite.Image = null;
                        break;
                }
                if (_attackState < 9)
                {
                    UpdateSprite();
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
                    }
                }
                else if (_attackState == 10)
                {
                    this.Peak();
                    return;
                }

                _attackState++;
            }
        }

        private bool IsKeenInRangeForAttack()
        {
            int widthDiff = ATTACK_WIDTH_RANGE - this.HitBox.Width;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X - widthDiff / 2, this.HitBox.Y, ATTACK_WIDTH_RANGE, ATTACK_HEIGHT_RANGE + this.HitBox.Height - 1);
            return _keen.HitBox.IntersectsWith(areaToCheck);
        }

        private void Wait()
        {
            if (this.State != CeilickState.WAITING)
            {
                this.State = CeilickState.WAITING;
                _retracting = false;
            }

            if (IsKeenInRangeForAttack())
            {
                this.Attack();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return _state == CeilickState.ATTACKING; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return _state == CeilickState.TAUNTING || _state == CeilickState.PEAKING; }
        }

        CeilickState State
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

        public PointItemType PointItem => PointItemType.KEEN6_PUDDING;

        private void UpdateSprite()
        {
            switch (_state)
            {
                case CeilickState.HIDING:
                    _sprite.Image = null;
                    break;
                case CeilickState.WAITING:
                    _sprite.Image = Properties.Resources.keen6_ceilick_tongue_waiting;
                    break;
                case CeilickState.ATTACKING:
                    if (_currentAttackSprite >= _attackSprites.Length)
                    {
                        _currentAttackSprite = 0;
                    }
                    _sprite.Image = _attackSprites[_currentAttackSprite];
                    break;
                case CeilickState.PEAKING:
                    if (_currentPeakSprite >= _peakSprites.Length)
                    {
                        _currentPeakSprite = 0;
                        _shouldTaunt = true;
                    }
                    _sprite.Image = _peakSprites[_currentPeakSprite];
                    break;
                case CeilickState.TAUNTING:
                    if (_currentTauntSprite >= _tauntSprites.Length)
                    {
                        _currentTauntSprite = 0;
                        _tauntCount++;
                    }
                    _sprite.Image = _tauntSprites[_currentTauntSprite];
                    break;
                case CeilickState.STUNNED:
                    if (_currentStunnedSprite >= _stunnedSprites.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = _stunnedSprites[_currentStunnedSprite];
                    break;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum CeilickState
    {
        WAITING,
        ATTACKING,
        PEAKING,
        TAUNTING,
        HIDING,
        STUNNED
    }
}
