using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace KeenReloaded.Framework.Animations
{
    public class Animation
    {
        public Animation(Image[] images, int startAt = 0, int imageChangeDelayMilliseconds = 0, bool repeating = false)
        {
            if (images == null)
                throw new ArgumentNullException("images array cannot be null if an animation is to be constructed");

            if (startAt < 0 || startAt >= images.Length)
                throw new ArgumentOutOfRangeException("the starting point of the image animation must be greater than zero and less than the number of images in the animation");

            this.AnimationImages = images;
            this.CurrentIndex = startAt;
            this.ImageChangeDelayMilliseconds = imageChangeDelayMilliseconds;
            this.Repeating = repeating;
            if (imageChangeDelayMilliseconds > 0)
            {
                _runTimer = new Timer();
                _runTimer.Interval = this.ImageChangeDelayMilliseconds;
                _runTimer.Tick += new EventHandler(_runTimer_Tick);
            }
        }

        public event EventHandler AnimationImageChanged;
        public event EventHandler AnimationCompleted;
        public event EventHandler AnimationStarted;

        protected void OnAnimationImageChanged(object sender, EventArgs e)
        {
            if (this.AnimationImageChanged != null)
                this.AnimationImageChanged(sender, e);
        }

        protected void OnAnimationCompleted(object sender, EventArgs e)
        {
            if (this.AnimationCompleted != null)
                this.AnimationCompleted(sender, e);
        }

        protected void OnAnimationStarted(object sender, EventArgs e)
        {
            if (this.AnimationStarted != null)
                this.AnimationStarted(sender, e);
        }

        void _runTimer_Tick(object sender, EventArgs e)
        {
            this.MoveNext();
        }

        private Timer _runTimer;
        public Image[] AnimationImages { get; set; }

        public int ImageChangeDelayMilliseconds
        {
            get;
            private set;
        }

        public bool Repeating
        {
            get;
            private set;
        }

        public int CurrentIndex
        {
            get;
            private set;
        }

        public Image CurrentImage
        {
            get
            {
                try
                {
                    Image img = AnimationImages[this.CurrentIndex];
                    return img;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void MoveNext()
        {
            if (this.CurrentIndex < this.AnimationImages.Length - 1)
            {
                this.CurrentIndex++;
                OnAnimationImageChanged(this, null);
            }
            else if (this.Repeating)
            {
                this.CurrentIndex = 0;
                OnAnimationCompleted(this, null);
            }
            else
            {
                StopAnimation();
            }
        }

        public void StartAnimation()
        {
            if (!_runTimer.Enabled)
            {
                _runTimer.Start();
                OnAnimationStarted(this, null);
            }
        }

        public void StopAnimation()
        {
            if (_runTimer.Enabled)
                _runTimer.Stop();
            this.CurrentIndex = 0;
            OnAnimationCompleted(this, null);
        }
    }
}
