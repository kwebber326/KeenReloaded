using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;

namespace KeenReloaded.Framework.Tiles
{
    public class SecretPlatformTile : PlatformTile, IUpdatable
    {
        private SecretPlatformState _state;
        private const int SHOW_STATE_CHANGE_DELAY = 0;
        private int _currentShowStateChangeDelayTick;
        private int _currentSprite;

        private const int HIDE_TIME = 60;
        private int _hideTimeTick;

        private Image[] _sprites = SpriteSheet.Keen4SecretPlatformImages;

        public SecretPlatformTile(SpaceHashGrid grid, Rectangle hitbox, SecretPlatformState initialState)
            : base(grid, hitbox)
        {
            Inititalize(initialState);
        }

        private void Inititalize(SecretPlatformState initialState)
        {
            _state = initialState;
            this.Sprite.BackColor = Color.Transparent;
            this.Sprite.Location = this.HitBox.Location;
            this.Sprite.Image = _sprites[_currentSprite];
        }

        public SecretPlatformState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                _currentSprite = (int)_state;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            this.Sprite.Image = _sprites[_currentSprite];
            if (this.Sprite.Image != null)
            {
                this.Sprite.BringToFront();
            }
        }

        public void Update()
        {
            switch (_state)
            {
                case SecretPlatformState.HIDDEN:
                    this.UpdateHideState();
                    break;
                default:
                    this.Show();
                    break;
            }
        }

        private void UpdateHideState()
        {
            if (this.State != SecretPlatformState.HIDDEN)
            {
                this.State = SecretPlatformState.HIDDEN;
                _hideTimeTick = 0;
            }

            if (_hideTimeTick++ == HIDE_TIME)
            {
                this.Show();
            }
        }

        private void Show()
        {
            switch (_state)
            {
                case SecretPlatformState.HIDDEN:
                    this.State = SecretPlatformState.SHOW1;                
                    break;
                case SecretPlatformState.SHOW1:
                    if (_currentShowStateChangeDelayTick++ == SHOW_STATE_CHANGE_DELAY)
                    {
                        _currentShowStateChangeDelayTick = 0;
                        this.State = SecretPlatformState.SHOW2;
                       
                    }
                    break;
                case SecretPlatformState.SHOW2:
                    if (_currentShowStateChangeDelayTick++ == SHOW_STATE_CHANGE_DELAY)
                    {
                        _currentShowStateChangeDelayTick = 0;
                        this.State = SecretPlatformState.SHOW3;
                    }
                    break;
                case SecretPlatformState.SHOW3:
                    if (_currentShowStateChangeDelayTick++ == SHOW_STATE_CHANGE_DELAY)
                    {
                        _currentShowStateChangeDelayTick = 0;
                        this.UpdateHideState();
                    }
                    break;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.State}";
        }
    }

    public enum SecretPlatformState
    {
        HIDDEN,
        SHOW1,
        SHOW2,
        SHOW3
    }
}
