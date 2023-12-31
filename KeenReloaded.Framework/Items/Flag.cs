﻿using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Items
{
    public class Flag : Item, IFlag
    {
        #region constants
        private const int POINT_DEGRADATION_DELAY = 15;
        #endregion

        #region readonly initialization values
        private readonly int _maxPoints;
        private readonly int _minPoints;
        private readonly int _pointsDegradedPerSecond;
        #endregion

        #region private fields
        private int _currentPointValue;
        private int _currentPointDegradationDelayValue;

        private bool _isCaptured;

        private GemColor _color;

        private CommanderKeen _keen;
        private Point _originalLocation;
        #endregion

        #region events
        public event EventHandler<FlagCapturedEventArgs> FlagCaptured;
        public event EventHandler<FlagCapturedEventArgs> FlagPointsChanged;
        #endregion

        public Flag(SpaceHashGrid grid, Rectangle hitbox, GemColor color, int maxPoints, int minPoints, int pointsDegradedPerSecond, CommanderKeen keen) : base(grid, hitbox)
        {
            if (maxPoints <= 0)
                throw new ArgumentException("Max points must be greater than zero");
            if (minPoints < 0)
                throw new ArgumentException("Min points must be greater than or equal to zero");
            if (maxPoints < minPoints)
                throw new ArgumentException("Max points must be greater than or equal to min points");
            if (keen == null)
                throw new ArgumentException("Commander Keen object cannot be null");
            _color = color;
            _maxPoints = maxPoints;
            _minPoints = minPoints;
            _pointsDegradedPerSecond = pointsDegradedPerSecond;
            _currentPointValue = maxPoints;
            _keen = keen;
            InitializeSprite();
        }

        #region properties

        public int MaxPoints
        {
            get
            {
                return _maxPoints;
            }
        }

        public int MinPoints
        {
            get
            {
                return _minPoints;
            }
        }

        public int PointsDegradedPerSecond
        {
            get
            {
                return _pointsDegradedPerSecond;
            }
        }

        public int CurrentPointValue
        {
            get
            {
                return _currentPointValue;
            }
        }

        public bool IsCaptured
        {
            get
            {
                return _isCaptured;
            }
        }

        public GemColor Color
        {
            get
            {
                return _color;
            }
        }

        public Point LocationOfOrigin
        {
            get
            {
                return _originalLocation;
            }
        }
        #endregion


        #region public methods

        public override void Update()
        {
            base.Update();

            if (this.IsAcquired || _currentPointValue == _minPoints)
                return;

            if (_currentPointDegradationDelayValue++ >= POINT_DEGRADATION_DELAY)
            {
                _currentPointDegradationDelayValue = 0;
                if (_currentPointValue - _pointsDegradedPerSecond >= _minPoints)
                {
                    _currentPointValue -= _pointsDegradedPerSecond;
                }
                else
                {
                    _currentPointValue = _minPoints;
                }
                OnPointsChanged();
            }
        }

        public override string ToString()
        {
            return base.ToString() + "|" + _color.ToString() + "|" + _maxPoints + "|" + _minPoints + "|" + _pointsDegradedPerSecond;
        }

        public void Capture()
        {
            _isCaptured = true;
            OnCaptured();
        }
        #endregion

        #region protected methods
        protected void OnCaptured()
        {
            FlagCapturedEventArgs e = ConstructEventArgs();
            this.FlagCaptured?.Invoke(this, e);
        }

        protected void OnPointsChanged()
        {
            FlagCapturedEventArgs e = ConstructEventArgs();
            this.FlagPointsChanged?.Invoke(this, e);
        }
        #endregion

        #region private helper methods
        private FlagCapturedEventArgs ConstructEventArgs()
        {
            return new FlagCapturedEventArgs()
            {
                Flag = this
            };
        }
        private void InitializeSprite()
        {
            var colorImages = SpriteSheet.CTFColors;
            var image = colorImages[(int)_color];
            this.SpriteList = new Image[] { image };
            this.AcquiredSpriteList = new Image[] { Properties.Resources.Flag_Acquired };
            this.Sprite.Image = image;
            _originalLocation = new Point(this.HitBox.X, this.HitBox.Y);
        }
        #endregion
    }
}
