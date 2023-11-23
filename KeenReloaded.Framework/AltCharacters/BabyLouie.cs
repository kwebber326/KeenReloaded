﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.AltCharacters
{
    public class BabyLouie : CommanderKeen
    {
        public BabyLouie(SpaceHashGrid grid, Rectangle hitbox, Direction direction) : base(grid, hitbox, direction)
        {
            Initialize();
        }

        public BabyLouie(SpaceHashGrid grid, Rectangle hitbox, Direction direction, int lives, long points) :
            base (grid, hitbox, direction, lives, points)
        {
            Initialize();
        }

        private void Initialize()
        {
            _keenRunRightSprites = new Image[]
            {
                Properties.Resources.baby_louie_run_right1,
                Properties.Resources.baby_louie_run_right2,
                Properties.Resources.baby_louie_run_right3,
                Properties.Resources.baby_louie_run_right4
            };

            _keenRunLeftSprites = new Image[]
            {
                Properties.Resources.baby_louie_run_left1,
                Properties.Resources.baby_louie_run_left2,
                Properties.Resources.baby_louie_run_left3,
                Properties.Resources.baby_louie_run_left4
            };

            _keenClimbRightSprites = new Image[]
            {
                Properties.Resources.baby_louie_wall_climb_right1,
                Properties.Resources.baby_louie_wall_climb_right2,
                Properties.Resources.baby_louie_wall_climb_right3,
                Properties.Resources.baby_louie_wall_climb_right4
            };

            _keenClimbLeftSprites = new Image[]
            {
                Properties.Resources.baby_louie_wall_climb_left1,
                Properties.Resources.baby_louie_wall_climb_left2,
                Properties.Resources.baby_louie_wall_climb_left3,
                Properties.Resources.baby_louie_wall_climb_left4
            };

            _keenClimbUpPoleRightSprites = new Image[]
            {
                Properties.Resources.baby_louie_pole_left1,
                Properties.Resources.baby_louie_pole_left2,
                Properties.Resources.baby_louie_pole_left3
            };

            _keenClimbUpPoleLeftSprites = new Image[]
            {
                Properties.Resources.baby_louie_pole_right1,
                Properties.Resources.baby_louie_pole_right2,
                Properties.Resources.baby_louie_pole_right3
            };

            _keenClimbDownPoleSprites = new Image[]
            {
                Properties.Resources.baby_louie_shimmy_down1,
                Properties.Resources.baby_louie_shimmy_down2,
                Properties.Resources.baby_louie_shimmy_down3,
                Properties.Resources.baby_louie_shimmy_down4,
            };

            _keenLookDownSprites = new Image[]
            {
                Properties.Resources.baby_louie_duck,
                Properties.Resources.baby_louie_duck
            };

            _keenDoorEnterSprites = new Image[]
            {
                Properties.Resources.baby_louie_walk_through_door1,
                Properties.Resources.baby_louie_walk_through_door2,
                Properties.Resources.baby_louie_walk_through_door3,
                Properties.Resources.baby_louie_walk_through_door4,
                Properties.Resources.baby_louie_walk_through_door5
            };

            _keenShootUpAerial = Properties.Resources.baby_louie_shoot_up_aerial;
            _keenShootDownAerial = Properties.Resources.baby_louie_shoot_down_aerial;
            _keenShootRightAerial = Properties.Resources.baby_louie_shoot_right_aerial;
            _keenShootLeftAerial = Properties.Resources.baby_louie_shoot_left_aerial;

            _keenShootUp = Properties.Resources.baby_louie_shoot_up_standing;
            _keenShootLeft = Properties.Resources.baby_louie_shoot_left_standing;
            _keenShootRight = Properties.Resources.baby_louie_shoot_right_standing;

            _keenShootDownRightPole = Properties.Resources.baby_louie_shoot_down_pole_right;
            _keenShootUpRightPole = Properties.Resources.baby_louie_shoot_up_pole_right;
            _keenShootDownLeftPole = Properties.Resources.baby_louie_shoot_down_pole_left;
            _keenShootUpLeftPole = Properties.Resources.baby_louie_shoot_up_pole_left;
            _keenShootPoleLeft = Properties.Resources.baby_louie_shoot_left_pole;
            _keenShootPoleRight = Properties.Resources.baby_louie_shoot_right_pole;

            _keenLookUp = Properties.Resources.baby_louie_look_up;
            _keenEnterDoor1 = Properties.Resources.baby_louie_walk_through_door1;
            _keenStunned = Properties.Resources.baby_louie_dead1;
            _keenStandright = Properties.Resources.baby_louie_stand_right;
            _keenStandLeft = Properties.Resources.baby_louie_stand_left;
            _keenFallRight = Properties.Resources.baby_louie_fall_right;
            _keenFallLeft = Properties.Resources.baby_louie_fall_left;
            _keenJumpLeft1 = Properties.Resources.baby_louie_jump_left1;
            _keenJumpRight1 = Properties.Resources.baby_louie_jump_right1;
            _keenHangLeft = Properties.Resources.baby_louie_wall_hang_left;
            _keenHangRight = Properties.Resources.baby_louie_wall_hang_right;

            _keenDead1 = Properties.Resources.baby_louie_dead1;
            _keenDead2 = Properties.Resources.baby_louie_dead2;

            this.UpdateSprite();
        }
    }
}
