using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.UserControls.SpriteSheet;
using System.Threading.Tasks;

namespace KeenReloaded.UserControls
{
    public partial class LoadingAnimation : UserControl
    {
        private Timer _timer;
        private Image[] _images;
        private int _currentImage;
        public LoadingAnimation()
        {
            InitializeComponent();
            _images = UserControlSpriteSheet.LoadingAnimationImages;
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Tick += _timer_Tick;

            this.Visible = false;
        }

        public void ShowAnimation()
        {
            this.Visible = true;
            _currentImage = 0;
            pbLoadingIcon.Image = _images[_currentImage];
            _timer.Start();
        }

        public void HideAnimation()
        {
            this.Visible = false;
            _currentImage = 0;
            pbLoadingIcon.Image = _images[_currentImage];
            _timer.Stop();
        }

        private void LoadingAnimation_Load(object sender, EventArgs e)
        {

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_currentImage++ >= _images.Length)
            {
                _currentImage = 0;
            }
            pbLoadingIcon.Image = _images[_currentImage];
        }
    }
}
