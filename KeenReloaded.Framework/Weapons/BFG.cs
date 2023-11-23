﻿using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Projectiles;
using KeenReloaded.Framework.Trajectories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Weapons
{
    public class BFG : NeuralStunner
    {
        public BFG(SpaceHashGrid grid, Rectangle hitbox, int ammo = 1) : base(grid, hitbox, ammo)
        {
            REFIRE_DELAY = 50;
            DAMAGE = 999;
            VELOCITY = 5;
            BLAST_RADIUS = 300;
        }

        public override void Fire()
        {
            if (!_isFiring)
            {   //TODO: make all these values driven by constants
                for (int i = 0; i < SHOTS_PER_FIRE; i++)
                {
                    if (i <= _ammo)
                    {
                        BFGProjectile s = null;
                        switch (this.Direction)
                        {
                            case Enums.Direction.RIGHT:
                                s = new BFGProjectile(_grid, new Rectangle(this.HitBox.Right + 1, this.HitBox.Top + 10, 16, 16), DAMAGE, VELOCITY, PIERCE, SPREAD, BLAST_RADIUS, REFIRE_DELAY, this.Direction);
                                break;
                            case Enums.Direction.LEFT:
                                s = new BFGProjectile(_grid, new Rectangle(this.HitBox.Left - 16, this.HitBox.Top + 10, 16, 16), DAMAGE, VELOCITY, PIERCE, SPREAD, BLAST_RADIUS, REFIRE_DELAY, this.Direction);
                                break;
                            case Enums.Direction.UP:
                                s = new BFGProjectile(_grid, new Rectangle(this.HitBox.Right - 30, this.HitBox.Top - 16, 16, 16), DAMAGE, VELOCITY, PIERCE, SPREAD, BLAST_RADIUS, REFIRE_DELAY, this.Direction);
                                break;
                            case Enums.Direction.DOWN:
                                s = new BFGProjectile(_grid, new Rectangle(this.HitBox.Right - 30, this.HitBox.Bottom + 1, 16, 16), DAMAGE, VELOCITY, PIERCE, SPREAD, BLAST_RADIUS, REFIRE_DELAY, this.Direction);
                                break;
                            default:
                                s = new BFGProjectile(_grid, new Rectangle(this.HitBox.Right + 1, this.HitBox.Top + 10, 16, 16), DAMAGE, VELOCITY, PIERCE, SPREAD, BLAST_RADIUS, REFIRE_DELAY, this.Direction);
                                break;
                        }
                        s.ObjectComplete += new EventHandler(s_ObjectComplete);
                        ObjectEventArgs e = new ObjectEventArgs()
                        {
                            ObjectSprite = s
                        };
                        OnCreatedObject(e);
                        _ammo--;
                    }
                }
                _currentRefireDelayTick = REFIRE_DELAY;
            }
        }

        protected override void s_ObjectComplete(object sender, EventArgs e)
        {
            var projectile = sender as BFGProjectile;
            ObjectEventArgs args = new ObjectEventArgs()
            {
                ObjectSprite = projectile
            };
            OnRemovedObject(args);
            if (projectile != null)
            {
                projectile.Create += new EventHandler<ObjectEventArgs>(trajectory_Create);
                projectile.Explode();
            }
        }

        void trajectory_Create(object sender, ObjectEventArgs e)
        {
            OnCreatedObject(e);
            var explosion = e.ObjectSprite as BFGExplosion;
            if (explosion != null)
            {
                explosion.Remove += new EventHandler<ObjectEventArgs>(explosion_Remove);
            }
        }

        void explosion_Remove(object sender, ObjectEventArgs e)
        {
            OnRemovedObject(e);
        }
    }
}
