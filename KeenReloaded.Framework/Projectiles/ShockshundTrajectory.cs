using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Enemies;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Trajectories
{
    public class ShockshundTrajectory : StraightShotTrajectory, ICancellableTrajectory
    {
        public ShockshundTrajectory(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox, direction, EnemyTrajectoryType.KEEN5_SHOCKSHUND_SHOT)
        {

        }

        public override void Update()
        {
            if (!_shotComplete)
            {
                GetSpreadOffset();
                var areaToCheck = GetAreaToCheckForCollision();
                var collisionObjects = this.CheckCollision(areaToCheck);
                var debugTiles = collisionObjects.OfType<DebugTile>();
                var keens = collisionObjects.OfType<CommanderKeen>().ToList();
                var itemsToCheck = new List<CollisionObject>();
                var explodables = collisionObjects.OfType<Shelley>().ToList();
                var keenStunShots = collisionObjects.OfType<KeenStunShot>().ToList();
                itemsToCheck.AddRange(debugTiles);
                foreach (var keen in keens)
                {
                    if (keen != null)
                    {
                        itemsToCheck.Add(keen);
                    }
                }
                itemsToCheck.AddRange(explodables);
                itemsToCheck.AddRange(keenStunShots);
                if (itemsToCheck.Any())
                {
                    HandleCollisionByDirection(collisionObjects);
                }
                else
                {
                    this.Move();
                }
            }
            else if (_shotCompleteSprites != null && _shotCompleteSprites.Any())
            {
                UpdateSprite();
            }
            else
            {
                EndShot();
            }
        }

        private void EndShot()
        {
            this.UpdateCollisionNodes(this.Direction);
            CleanUpCollisionNodes();
            OnObjectComplete();
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is DebugTile)
            {
                StopAtCollisionObject(obj);
            }
            else if (obj is CommanderKeen)
            {
                var keen = (CommanderKeen)obj;
                keen.Die();
                if (--_pierce < 0)
                {
                    StopAtCollisionObject(obj);
                }
            }
            else if (obj is Shelley)
            {
                var shelley = (Shelley)obj;
                shelley.Explode();
                if (--_pierce < 0)
                {
                    StopAtCollisionObject(obj);
                }
            }
            else if (obj is KeenStunShot && !(obj is IExplodable))
            {
                if (--_pierce < 0)
                {
                    StopAtCollisionObject(obj);
                }
            }
            else if (obj is IExplodable)
            {
                if (--_pierce < 0)
                {
                    StopAtCollisionObject(obj);
                }
                var boobusBomb = obj as BoobusBombShot;
                if (boobusBomb != null)
                {
                    boobusBomb.ForceExplosion();
                }
            }
        }

        public void Cancel()
        {
            _shotComplete = true;
            UpdateSprite();
        }
    }
}
