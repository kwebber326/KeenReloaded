using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework
{
    public abstract class DestructibleObject : CollisionObject
    {
        public DestructibleObject(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            this.Health = 1;
        }

        protected bool _killedEventFired;

        public int Health { get; protected set; }

        public virtual void TakeDamage(int damage)
        {
            this.Health -= damage;
            if (this.Health <= 0)
            {
                this.Die();
                if (!_killedEventFired)
                {
                    OnKilled();
                    _killedEventFired = true;
                }
            }
        }

        public virtual void TakeDamage(ITrajectory trajectory)
        {
            this.Health -= trajectory.Damage;
            if (this.Health <= 0)
            {
                this.Die();
                if (!_killedEventFired)
                {
                    OnKilled();
                    _killedEventFired = true;
                }
            }
        }

        public bool IsDead()
        {
            return this.Health <= 0;
        }

        public abstract void Die();

        public event EventHandler<ObjectEventArgs> Killed;

        protected void OnKilled()
        {
            if (Killed != null)
                this.Killed(this, new ObjectEventArgs() { ObjectSprite = this as ISprite });
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is ITrajectory)
            {
                var trajectory = (ITrajectory)obj;
                this.TakeDamage(trajectory);
            }
        }
    }
}
