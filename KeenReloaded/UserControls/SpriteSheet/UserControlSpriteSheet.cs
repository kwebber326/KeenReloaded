using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.UserControls.SpriteSheet
{
    public static class UserControlSpriteSheet
    {
        #region Option Button
        private static Image[] _keenOptionButtonImages;

        public static Image[] KeenOptionButtonImages
        {
            get
            {
                if (_keenOptionButtonImages == null)
                {
                    _keenOptionButtonImages = new Image[]
                    {
                        Properties.Resources.keen_message_window_option_selected1,
                        Properties.Resources.keen_message_window_option_selected2
                    };
                }
                return _keenOptionButtonImages;
            }
        }
        #endregion

        #region Loading Animation
        private static Image[] _loadingAnimationImages;

        public static Image[] LoadingAnimationImages
        {
            get
            {
                if (_loadingAnimationImages == null)
                {
                    _loadingAnimationImages = new Image[]
                    {
                        Properties.Resources.loading1,
                        Properties.Resources.loading2,
                        Properties.Resources.loading3,
                        Properties.Resources.loading4,
                        Properties.Resources.loading5,
                        Properties.Resources.loading6,
                        Properties.Resources.loading7,
                        Properties.Resources.loading8
                    };
                }
                return _loadingAnimationImages;
            }
        }
        #endregion

        #region Flag Inventory
        private static Image[] _flagImages;
        
        public static Image[] FlagImages
        {
            get
            {
                if (_flagImages == null)
                {
                    _flagImages = new Image[]
                    {
                        Properties.Resources.Red_Flag,
                        Properties.Resources.Blue_Flag,
                        Properties.Resources.Green_Flag,
                        Properties.Resources.Yellow_Flag
                    };
                }
                return _flagImages;
            }
        }
        #endregion
    }
}
