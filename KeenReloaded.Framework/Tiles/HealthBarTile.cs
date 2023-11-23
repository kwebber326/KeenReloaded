using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class HealthBarTile : DestructibleCollisionTile, IHealthBar
    {
        private ProgressBar _healthBar;

        public HealthBarTile(SpaceHashGrid grid, Rectangle hitbox, int health, bool initallyVisible = false) : base(grid, hitbox)
        {
            _healthBar = new ProgressBar();
            _healthBar.Maximum = health;
            _healthBar.Value = health;
            _healthBar.Visible = initallyVisible;
            _healthBar.Location = new Point(hitbox.X - 100, hitbox.Y + (hitbox.Height / 2) + (hitbox.Height / 4));
        }

        public void SetHealthBarVisiblity(bool visible)
        {
            _healthBar.Visible = visible;
        }

        public void SetHealthBarValue(int val)
        {
            _healthBar.Value = val < 0 ? 0 : val;
        }

        public ProgressBar HealthBar => _healthBar;
    }
}
