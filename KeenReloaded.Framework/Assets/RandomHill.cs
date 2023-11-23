using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Assets
{
    public class RandomHill : Hill
    {
        public RandomHill(SpaceHashGrid grid, Rectangle hitbox, List<Point> points, int holdTimeSeconds, int spawnDelaySeconds, int pointsPerSecond, int additionPointsPerMonster, CommanderKeen keen) 
            : base(grid, hitbox, points, holdTimeSeconds, spawnDelaySeconds, pointsPerSecond, additionPointsPerMonster, keen)
        {
        }

        protected override void SpawnAtNextLocation()
        {
            this.HillState = HillState.STRENGTH1;
            _random = new Random();
            _currentPoint = _random.Next(0, _points.Count);
            var newPoint = _points[_currentPoint];
            this.HitBox = new Rectangle(newPoint, this.HitBox.Size);
        }
    }
}
