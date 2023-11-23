using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework
{
    public static class SpriteSheet
    {
        #region Hill
        private static Image[] _hillSprites;
        public static Image[] HillSprites
        {
            get
            {
                if (_hillSprites == null)
                {
                    _hillSprites = new Image[]
                    {
                        Properties.Resources.mirage_hill1,
                        Properties.Resources.mirage_hill2,
                        Properties.Resources.mirage_hill3,
                        Properties.Resources.mirage_hill4
                    };
                }
                return _hillSprites;
            }
        }
        #endregion

        #region Keen Weapons
        private static Image[] _weaponImages;

        public static Image[] WeaponImages
        {
            get
            {
                if (_weaponImages == null)
                {
                    _weaponImages = new Image[]
                    {
                        Properties.Resources.neural_stunner1,
                        Properties.Resources.neural_stunner_shotgun,
                        Properties.Resources.neural_stunner_smg_1,
                        Properties.Resources.neural_stunner_rocket_launcher1,
                        Properties.Resources.keen_dreams_boobus_bomb1,
                        Properties.Resources.neural_stunner_railgun1
                    };
                }
                return _weaponImages;
            }
        }
        #endregion
        #region Keen Weapon Generator
        private static Image[] _weaponGeneratorImages;

        public static Image[] WeaponGeneratorImages
        {
            get
            {
                if (_weaponGeneratorImages == null)
                {
                    _weaponGeneratorImages = new Image[]
                    {
                        Properties.Resources.random_item_generator_closed,
                        Properties.Resources.random_item_generator_open1,
                        Properties.Resources.random_item_generator_open2,
                        Properties.Resources.random_item_generator_open3,
                        Properties.Resources.random_item_generator_open4,
                        Properties.Resources.random_item_generator_open5
                    };
                }
                return _weaponGeneratorImages;
            }
        }
        #endregion

        #region Shield
        public static Image Shield
        {
            get
            {
                return Properties.Resources.Shield;
            }
        }
        #endregion

        #region Keen Biome Changer
        private static Image[] _biomeChangerImages;
        public static Image[] BiomeChangerImages
        {
            get
            {
                if (_biomeChangerImages == null)
                {
                    _biomeChangerImages = new Image[]
                    {
                        Properties.Resources.keen5_quantum_dynamo_sphere1,
                        Properties.Resources.keen5_quantum_dynamo_sphere2,
                        Properties.Resources.keen5_quantum_dynamo_sphere3,
                        Properties.Resources.keen5_quantum_dynamo_sphere4,
                        Properties.Resources.keen5_quantum_dynamo_sphere5,
                        Properties.Resources.keen5_quantum_dynamo_sphere6,
                        Properties.Resources.keen5_quantum_dynamo_sphere7,
                        Properties.Resources.keen5_quantum_dynamo_sphere8,
                        Properties.Resources.keen5_quantum_dynamo_sphere9,
                        Properties.Resources.keen5_quantum_dynamo_sphere10,
                        Properties.Resources.keen5_quantum_dynamo_sphere11,
                        Properties.Resources.keen5_quantum_dynamo_sphere12
                    };
                }
                return _biomeChangerImages;
            }
        }
        #endregion

        #region Keen enemy spawner

        private static Image[] _enemySpawnerImages;

        public static Image[] EnemySpawnerImages
        {
            get
            {
                if (_enemySpawnerImages == null)
                {
                    _enemySpawnerImages = new Image[]{
                        Properties.Resources.enemy_spawner1,
                        Properties.Resources.enemy_spawner2,
                        Properties.Resources.enemy_spawner3
                    };
                }
                return _enemySpawnerImages;
            }
        }

        #endregion

        #region Keen 4 Oracle Door

        private static Image[] _keen4OracleDoorImages;

        public static Image[] Keen4OracleDoorImages
        {
            get
            {
                if (_keen4OracleDoorImages == null)
                {
                    _keen4OracleDoorImages = new Image[]
                    {
                        Properties.Resources.keen4_oracle_door1,
                        Properties.Resources.keen4_oracle_door2
                    };
                }
                return _keen4OracleDoorImages;
            }
        }

        #endregion

        #region Keen 4 Secret Platform
        private static Image[] _keen4SecretPlatformImages;

        public static Image[] Keen4SecretPlatformImages
        {
            get
            {
                if (_keen4SecretPlatformImages == null)
                {
                    _keen4SecretPlatformImages = new Image[]
                    {
                        null,
                        Properties.Resources.keen4_secret_platform1,
                        Properties.Resources.keen4_secret_platform3,
                        Properties.Resources.keen4_secret_platform2

                    };
                }
                return _keen4SecretPlatformImages;
            }
        }
        #endregion

        #region Keen 4 Mirage Platform

        private static Image[] _miragePlatformImages;

        public static Image[] MiragePlatformImages
        {
            get
            {
                if (_miragePlatformImages == null)
                {
                    _miragePlatformImages = new Image[]
                    {
                        Properties.Resources.keen4_mirage_platform1,
                        Properties.Resources.keen4_mirage_platform2,
                        Properties.Resources.keen4_mirage_platform3,
                        Properties.Resources.keen4_mirage_platform4
                    };
                }
                return _miragePlatformImages;
            }
        }

        #endregion

        #region Keen 4 Rocket-Propelled Platform
        private static Image[] _rocketPropelledPlatformImages;

        public static Image[] RocketPropelledPlatformImages
        {
            get
            {
                if (_rocketPropelledPlatformImages == null)
                {
                    _rocketPropelledPlatformImages = new Image[]{
                        Properties.Resources.keen4_rocket_propelled_platform1,
                        Properties.Resources.keen4_rocket_propelled_platform2,
                        Properties.Resources.keen4_rocket_propelled_platform3,
                        Properties.Resources.keen4_rocket_propelled_platform4
                    };
                }
                return _rocketPropelledPlatformImages;
            }
        }
        #endregion

        #region Keen 4 Poison Pool

        private static Image[] _keen4PoisonPoolLeftImages;

        public static Image[] Keen4PoisonPoolLeftImages
        {
            get
            {
                if (_keen4PoisonPoolLeftImages == null)
                {
                    _keen4PoisonPoolLeftImages = new Image[]
                    {
                        Properties.Resources.keen4_poison_pool1_edge_left,
                        Properties.Resources.keen4_poison_pool2_edge_left
                    };
                }
                return _keen4PoisonPoolLeftImages;
            }
        }

        private static Image[] _keen4PoisonPoolRightImages;

        public static Image[] Keen4PoisonPoolRightImages
        {
            get
            {
                if (_keen4PoisonPoolRightImages == null)
                {
                    _keen4PoisonPoolRightImages = new Image[]
                    {
                        Properties.Resources.keen4_poison_pool1_edge_right,
                        Properties.Resources.keen4_poison_pool2_edge_right
                    };
                }
                return _keen4PoisonPoolRightImages;
            }
        }

        private static Image[] _keen4PoisonPoolMiddleImages;

        public static Image[] Keen4PoisonPoolMiddleImages
        {
            get
            {
                if (_keen4PoisonPoolMiddleImages == null)
                {
                    _keen4PoisonPoolMiddleImages = new Image[]
                    {
                        Properties.Resources.keen4_poison_pool1_middle,
                        Properties.Resources.keen4_poison_pool2_middle
                    };
                }
                return _keen4PoisonPoolMiddleImages;
            }
        }
        #endregion

        #region Keen 4 TarPit
        private static Image[] _keen4TarPitImages;

        public static Image[] Keen4TarPitImages
        {
            get
            {
                if (_keen4TarPitImages == null)
                {
                    _keen4TarPitImages = new Image[]
                    {
                         Properties.Resources.keen4_tar1,
                         Properties.Resources.keen4_tar2,
                         Properties.Resources.keen4_tar3,
                         Properties.Resources.keen4_tar4
                    };
                }
                return _keen4TarPitImages;
            }
        }
        #endregion

        #region Keen 5 Control Panel (for little amptons)
        private static Image[] _keen5ControlPanelImages;

        public static Image[] Keen5ControlPanelImages
        {
            get
            {
                if (_keen5ControlPanelImages == null)
                {
                    _keen5ControlPanelImages = new Image[]
                    {
                         Properties.Resources.keen5_control_panel1,
                         Properties.Resources.keen5_control_panel2
                    };
                }
                return _keen5ControlPanelImages;
            }
        }

        private static Dictionary<int, Image> _keen5LittleAmptonCalibrationImages;

        public static Dictionary<int, Image> Keen5LittleAmptonCalibrationImages
        {
            get
            {
                if (_keen5LittleAmptonCalibrationImages == null)
                {
                    _keen5LittleAmptonCalibrationImages = new Dictionary<int, Image>()
                    {
                        { 0, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 1, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 2, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 3, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 4, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 5, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 6, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 7, Properties.Resources.keen5_little_ampton_calibrate2 },

                        { 8, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 9, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 10, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 11, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 12, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 13, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 14, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 15, Properties.Resources.keen5_little_ampton_calibrate2 },


                        { 16, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 17, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 18, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 19, Properties.Resources.keen5_little_ampton_calibrate1 },
                        { 20, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 21, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 22, Properties.Resources.keen5_little_ampton_calibrate2 },
                        { 23, Properties.Resources.keen5_little_ampton_calibrate2 },
                    };
                }
                return _keen5LittleAmptonCalibrationImages;
            }
        }
        #endregion

        #region Keen 5 KeyCard
        private static Image[] _keen5KeyCardImages;
        public static Image[] Keen5KeyCardImages
        {
            get
            {
                if (_keen5KeyCardImages == null)
                {
                    _keen5KeyCardImages = new Image[]
                    {
                        Properties.Resources.keen5_key_card1,
                        Properties.Resources.keen5_key_card2
                    };
                }
                return _keen5KeyCardImages;
            }
        }

        private static Image[] _keen5KeyCardAcquiredImages;
        public static Image[] Keen5KeyCardAcquiredImages
        {
            get
            {
                if (_keen5KeyCardAcquiredImages == null)
                {
                    _keen5KeyCardAcquiredImages = new Image[]
                    {
                        Properties.Resources.keen5_key_card_acquired
                    };
                }
                return _keen5KeyCardAcquiredImages;
            }
        }

        #endregion

        #region Keen 5 Exit Door

        private static Image[] _keen5ExitDoorOpenImages;

        public static Image[] Keen5ExitDoorOpenImages
        {
            get
            {
                if (_keen5ExitDoorOpenImages == null)
                {
                    _keen5ExitDoorOpenImages = new Image[]
                    {
                        Properties.Resources.keen5_exit_door_open1,
                        Properties.Resources.keen5_exit_door_open2,
                        Properties.Resources.keen5_exit_door_open3
                    };
                }
                return _keen5ExitDoorOpenImages;
            }
        }

        #endregion

        #region Spindred
        private static Image[] _spindredImages;
        public static Image[] SpindredImages
        {
            get
            {
                if (_spindredImages == null)
                {
                    _spindredImages = new Image[]
                    {
                        Properties.Resources.keen5_spindred1,
                        Properties.Resources.keen5_spindred2,
                        Properties.Resources.keen5_spindred3,
                        Properties.Resources.keen5_spindred4
                    };
                }
                return _spindredImages;
            }
        }
        #endregion

        #region Lick
        public static Image[] LickFireBreathImagesLeft
        {
            get
            {
                return _fireBreathImagesLeft;
            }
        }

        public static Image[] LickFireBreathImagesRight
        {
            get
            {
                return _fireBreathImagesRight;
            }
        }

        public static Image[] LickStunImages
        {
            get
            {
                return _lickStunImages;
            }
        }

        private static Image[] _fireBreathImagesLeft = new Image[]
        {
            Properties.Resources.keen4_lick_left_fire1,
            Properties.Resources.keen4_lick_left_fire2,
            Properties.Resources.keen4_lick_left_fire3,
        };

        private static Image[] _fireBreathImagesRight = new Image[]
        {
            Properties.Resources.keen4_lick_right_fire1,
            Properties.Resources.keen4_lick_right_fire2,
            Properties.Resources.keen4_lick_right_fire3,
        };

        private static Image[] _lickStunImages = new Image[]
        {
            Properties.Resources.keen4_lick_stun2,
            Properties.Resources.keen4_lick_stun3,
            Properties.Resources.keen4_lick_stun4,
        };
        #endregion

        #region Wormmouth

        private static Image[] _wormmouthStunSprites;

        public static Image[] WormmouthStunSprites
        {
            get
            {
                if (_wormmouthStunSprites == null)
                {
                    _wormmouthStunSprites = new Image[]
                    {
                        Properties.Resources.keen4_wormmouth_stun2,
                        Properties.Resources.keen4_wormmouth_stun3,
                        Properties.Resources.keen4_wormmouth_stun4
                    };

                }
                return _wormmouthStunSprites;
            }
        }

        #endregion

        #region Schoolfish

        private static Image[] _schoolFishLeftImages = new Image[]
        {
            Properties.Resources.keen4_schoolfish_left1,
            Properties.Resources.keen4_schoolfish_left2
        };

        public static Image[] SchoolFishLeftImages
        {
            get
            {
                return _schoolFishLeftImages;
            }
        }

        private static Image[] _schoolFishRightImages = new Image[]
        {
            Properties.Resources.keen4_schoolfish_right1,
            Properties.Resources.keen4_schoolfish_right2
        };

        public static Image[] SchoolFishRightImages
        {
            get
            {
                return _schoolFishRightImages;
            }
        }


        #endregion

        #region Inchworm

        private static Image[] _inchwormLeftImages = new Image[]
        {
            Properties.Resources.keen4_inchworm_left1,
            Properties.Resources.keen4_inchworm_left2
        };

        public static Image[] InchwormLeftImages
        {
            get
            {
                return _inchwormLeftImages;
            }
        }

        private static Image[] _inchwormRightImages = new Image[]
        {
            Properties.Resources.keen4_inchworm_right1,
            Properties.Resources.keen4_inchworm_right2
        };

        public static Image[] InchwormRightImages
        {
            get
            {
                return _inchwormRightImages;
            }
        }

        #endregion

        #region Shikadi Mine
        private static Image[] _shikadiMineSelfDestructImages;
        private static Image[] _shikadiMineExplodeImages;
        public static Image[] ShikadiMineSelfDestructImages
        {
            get
            {
                if (_shikadiMineSelfDestructImages == null)
                {
                    _shikadiMineSelfDestructImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_mine_self_destruct_sequence1,
                        Properties.Resources.keen5_shikadi_mine_self_destruct_sequence2
                    };
                }
                return _shikadiMineSelfDestructImages;
            }
        }

        public static Image[] ShikadiMineExplodeImages
        {
            get
            {
                if (_shikadiMineExplodeImages == null)
                {
                    _shikadiMineExplodeImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_mine_explode1,
                        Properties.Resources.keen5_shikadi_mine_explode2
                    };
                }
                return _shikadiMineExplodeImages;
            }
        }
        #endregion

        #region Shockshund
        private static Image[] _shockShundWalkLeftSprites;

        public static Image[] ShockShundWalkLeftSprites
        {
            get
            {
                if (_shockShundWalkLeftSprites == null)
                {
                    _shockShundWalkLeftSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_left1,
                        Properties.Resources.keen5_shockshund_left2,
                        Properties.Resources.keen5_shockshund_left3,
                        Properties.Resources.keen5_shockshund_left4
                    };
                }
                return _shockShundWalkLeftSprites;
            }
        }

        private static Image[] _shockShundWalkRightSprites;

        public static Image[] ShockShundWalkRightSprites
        {
            get
            {
                if (_shockShundWalkRightSprites == null)
                {
                    _shockShundWalkRightSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_right1,
                        Properties.Resources.keen5_shockshund_right2,
                        Properties.Resources.keen5_shockshund_right3,
                        Properties.Resources.keen5_shockshund_right4
                    };
                }
                return _shockShundWalkRightSprites;
            }
        }

        private static Image[] _shockShundLookSprites;

        public static Image[] ShockShundLookSprites
        {
            get
            {
                if (_shockShundLookSprites == null)
                {
                    _shockShundLookSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_look1,
                        Properties.Resources.keen5_shockshund_look2
                    };
                }
                return _shockShundLookSprites;
            }
        }

        private static Image[] _shockShundShootLeftSprites;

        public static Image[] ShockShundShootLeftSprites
        {
            get
            {
                if (_shockShundShootLeftSprites == null)
                {
                    _shockShundShootLeftSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_shoot_left1,
                        Properties.Resources.keen5_shockshund_shoot_left2
                    };
                }
                return _shockShundShootLeftSprites;
            }
        }

        private static Image[] _shockShundShootRightSprites;

        public static Image[] ShockShundShootRightSprites
        {
            get
            {
                if (_shockShundShootRightSprites == null)
                {
                    _shockShundShootRightSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_shoot_right1,
                        Properties.Resources.keen5_shockshund_shoot_right2
                    };
                }
                return _shockShundShootRightSprites;
            }
        }

        private static Image[] _shockShundStunnedSprites;

        public static Image[] ShockShundStunnedSprites
        {
            get
            {
                if (_shockShundStunnedSprites == null)
                {
                    _shockShundStunnedSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_stunned1,
                        Properties.Resources.keen5_shockshund_stunned2,
                        Properties.Resources.keen5_shockshund_stunned3,
                        Properties.Resources.keen5_shockshund_stunned4
                    };
                }
                return _shockShundStunnedSprites;
            }
        }

        #endregion

        #region Shikadi
        private static Image[] _shikadiLookImages;

        public static Image[] ShikadiLookImages
        {
            get
            {
                if (_shikadiLookImages == null)
                {
                    _shikadiLookImages = new Image[]
                    {
                        Properties.Resources.keen5_standard_shikadi_look1,
                        Properties.Resources.keen5_standard_shikadi_look2,
                        Properties.Resources.keen5_standard_shikadi_look3,
                        Properties.Resources.keen5_standard_shikadi_look4,
                    };
                }
                return _shikadiLookImages;
            }
        }

        private static Image[] _shikadiWalkLeftImages;

        public static Image[] ShikadWalkLeftImages
        {
            get
            {
                if (_shikadiWalkLeftImages == null)
                {
                    _shikadiWalkLeftImages = new Image[]
                    {
                        Properties.Resources.keen5_standard_shikadi_walk_left1,
                        Properties.Resources.keen5_standard_shikadi_walk_left2,
                        Properties.Resources.keen5_standard_shikadi_walk_left3,
                        Properties.Resources.keen5_standard_shikadi_walk_left4
                    };
                }
                return _shikadiWalkLeftImages;
            }
        }

        private static Image[] _shikadiWalkRightImages;

        public static Image[] ShikadWalkRightImages
        {
            get
            {
                if (_shikadiWalkRightImages == null)
                {
                    _shikadiWalkRightImages = new Image[]
                    {
                        Properties.Resources.keen5_standard_shikadi_walk_right1,
                        Properties.Resources.keen5_standard_shikadi_walk_right2,
                        Properties.Resources.keen5_standard_shikadi_walk_right3,
                        Properties.Resources.keen5_standard_shikadi_walk_right4
                    };
                }
                return _shikadiWalkRightImages;
            }
        }

        private static Image[] _shikadiStunnedImages;

        public static Image[] ShikadiStunnedImages
        {
            get
            {
                if (_shikadiStunnedImages == null)
                {
                    _shikadiStunnedImages = new Image[]
                    {
                        Properties.Resources.keen5_standard_shikadi_stunned1,
                        Properties.Resources.keen5_standard_shikadi_stunned2,
                        Properties.Resources.keen5_standard_shikadi_stunned3,
                        Properties.Resources.keen5_standard_shikadi_stunned4
                    };
                }
                return _shikadiStunnedImages;
            }
        }
        #endregion

        #region Shikadi Master
        private static Image[] _shikadiMasterLookImages;

        public static Image[] ShikadiMasterLookImages
        {
            get
            {
                if (_shikadiMasterLookImages == null)
                {
                    _shikadiMasterLookImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_master_look1,
                        Properties.Resources.keen5_shikadi_master_look2,
                        Properties.Resources.keen5_shikadi_master_look3,
                        Properties.Resources.keen5_shikadi_master_look4
                    };
                }
                return _shikadiMasterLookImages;
            }
        }

        private static Image[] _shikadiMasterTeleportImages;

        public static Image[] ShikadiMasterTeleportImages
        {
            get
            {
                if (_shikadiMasterTeleportImages == null)
                {
                    _shikadiMasterTeleportImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_master_teleport1,
                        Properties.Resources.keen5_shikadi_master_teleport2
                    };
                }
                return _shikadiMasterTeleportImages;
            }
        }

        private static Image[] _energyBallImages;

        public static Image[] ShikadiMasterEnergyBallImages
        {
            get
            {
                if (_energyBallImages == null)
                {
                    _energyBallImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_master_energy_ball1,
                        Properties.Resources.keen5_shikadi_master_energy_ball2,
                        Properties.Resources.keen5_shikadi_master_energy_ball3,
                        Properties.Resources.keen5_shikadi_master_energy_ball4
                    };
                }
                return _energyBallImages;
            }
        }

        private static Image[] _shockwaveImages;

        public static Image[] ShikadiMasterShockwaveImages
        {
            get
            {
                if (_shockwaveImages == null)
                {
                    _shockwaveImages = new Image[]
                    {
                        Properties.Resources.keen5_shikadi_master_shockwave1,
                        Properties.Resources.keen5_shikadi_master_shockwave2,
                        Properties.Resources.keen5_shikadi_master_shockwave3,
                        Properties.Resources.keen5_shikadi_master_shockwave4
                    };
                }
                return _shockwaveImages;
            }
        }
        #endregion

        #region Korath Inhabitant
        private static Image[] _korathIWalkLeftImages;
        public static Image[] KorathIWalkLeftImages
        {
            get
            {
                if (_korathIWalkLeftImages == null)
                {
                    _korathIWalkLeftImages = new Image[]
                    {
                        Properties.Resources.keen5_korath_inhabitant_walk_left1,
                        Properties.Resources.keen5_korath_inhabitant_walk_left2,
                        Properties.Resources.keen5_korath_inhabitant_walk_left3,
                        Properties.Resources.keen5_korath_inhabitant_walk_left4
                    };
                }
                return _korathIWalkLeftImages;
            }
        }

        private static Image[] _korathIWalkRightImages;
        public static Image[] KorathIWalkRightImages
        {
            get
            {
                if (_korathIWalkRightImages == null)
                {
                    _korathIWalkRightImages = new Image[]
                    {
                        Properties.Resources.keen5_korath_inhabitant_walk_right1,
                        Properties.Resources.keen5_korath_inhabitant_walk_right2,
                        Properties.Resources.keen5_korath_inhabitant_walk_right3,
                        Properties.Resources.keen5_korath_inhabitant_walk_right4
                    };
                }
                return _korathIWalkRightImages;
            }
        }

        private static Image[] _korathIStunnedImages;
        public static Image[] KorathIStunnedImages
        {
            get
            {
                if (_korathIStunnedImages == null)
                {
                    _korathIStunnedImages = new Image[]
                    {
                        Properties.Resources.keen5_korath_inhabitant_stunned1,
                        Properties.Resources.keen5_korath_inhabitant_stunned2,
                        Properties.Resources.keen5_korath_inhabitant_stunned3,
                        Properties.Resources.keen5_korath_inhabitant_stunned4
                    };
                }
                return _korathIStunnedImages;
            }
        }

        #endregion

        #region Keen 5 Spinning Fire
        private static Image[] _keen5SpinningFireImages;
        public static Image[] Keen5SpinningFireImages
        {
            get
            {
                if (_keen5SpinningFireImages == null)
                {
                    _keen5SpinningFireImages = new Image[]
                    {
                        Properties.Resources.keen5_spinning_fire_hazard1,
                        Properties.Resources.keen5_spinning_fire_hazard2,
                        Properties.Resources.keen5_spinning_fire_hazard3,
                        Properties.Resources.keen5_spinning_fire_hazard4,
                        Properties.Resources.keen5_spinning_fire_hazard5,
                        Properties.Resources.keen5_spinning_fire_hazard6,
                        Properties.Resources.keen5_spinning_fire_hazard7
                    };
                }
                return _keen5SpinningFireImages;
            }
        }
        #endregion

        #region Keen5 Spinning Burn Platform

        private static Image[] _spinningBurnPlatformImages;

        public static Image[] SpinningBurnPlatformImages
        {
            get
            {
                if (_spinningBurnPlatformImages == null)
                {
                    _spinningBurnPlatformImages = new Image[]
                    {
                        Properties.Resources.keen5_spinning_burn_platform1,
                        Properties.Resources.keen5_spinning_burn_platform2,
                        Properties.Resources.keen5_spinning_burn_platform3,
                        Properties.Resources.keen5_spinning_burn_platform4,
                        Properties.Resources.keen5_spinning_burn_platform5,
                        Properties.Resources.keen5_spinning_burn_platform6,
                        Properties.Resources.keen5_spinning_burn_platform7,
                        Properties.Resources.keen5_spinning_burn_platform8
                    };
                }
                return _spinningBurnPlatformImages;
            }
        }

        #endregion

        #region Keen 6 Blooglet Red

        private static Image[] _bloogletRedRightImages;

        public static Image[] BloogletRedRightImages
        {
            get
            {
                if (_bloogletRedRightImages == null)
                {
                    _bloogletRedRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_red_right1,
                        Properties.Resources.keen6_blooglet_red_right2,
                        Properties.Resources.keen6_blooglet_red_right3,
                        Properties.Resources.keen6_blooglet_red_right4
                    };
                }
                return _bloogletRedRightImages;
            }
        }

        private static Image[] _bloogletRedLeftImages;

        public static Image[] BloogletRedLeftImages
        {
            get
            {
                if (_bloogletRedLeftImages == null)
                {
                    _bloogletRedLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_red_left1,
                        Properties.Resources.keen6_blooglet_red_left2,
                        Properties.Resources.keen6_blooglet_red_left3,
                        Properties.Resources.keen6_blooglet_red_left4
                    };
                }
                return _bloogletRedLeftImages;
            }
        }

        private static Image[] _bloogletRedStunnedImages;

        public static Image[] BloogletRedStunnedImages
        {
            get
            {
                if (_bloogletRedStunnedImages == null)
                {
                    _bloogletRedStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_red_stunned1,
                        Properties.Resources.keen6_blooglet_red_stunned2,
                        Properties.Resources.keen6_blooglet_red_stunned3,
                        Properties.Resources.keen6_blooglet_red_stunned4
                    };
                }
                return _bloogletRedStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Blooglet Blue

        private static Image[] _bloogletBlueRightImages;

        public static Image[] BloogletBlueRightImages
        {
            get
            {
                if (_bloogletBlueRightImages == null)
                {
                    _bloogletBlueRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_blue_right1,
                        Properties.Resources.keen6_blooglet_blue_right2,
                        Properties.Resources.keen6_blooglet_blue_right3,
                        Properties.Resources.keen6_blooglet_blue_right4
                    };
                }
                return _bloogletBlueRightImages;
            }
        }

        private static Image[] _bloogletBlueLeftImages;

        public static Image[] BloogletBlueLeftImages
        {
            get
            {
                if (_bloogletBlueLeftImages == null)
                {
                    _bloogletBlueLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_blue_left1,
                        Properties.Resources.keen6_blooglet_blue_left2,
                        Properties.Resources.keen6_blooglet_blue_left3,
                        Properties.Resources.keen6_blooglet_blue_left4
                    };
                }
                return _bloogletBlueLeftImages;
            }
        }

        private static Image[] _bloogletBlueStunnedImages;

        public static Image[] BloogletBlueStunnedImages
        {
            get
            {
                if (_bloogletBlueStunnedImages == null)
                {
                    _bloogletBlueStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_blue_stunned1,
                        Properties.Resources.keen6_blooglet_blue_stunned2,
                        Properties.Resources.keen6_blooglet_blue_stunned3,
                        Properties.Resources.keen6_blooglet_blue_stunned4
                    };
                }
                return _bloogletBlueStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Blooglet Green

        private static Image[] _bloogletGreenRightImages;

        public static Image[] BloogletGreenRightImages
        {
            get
            {
                if (_bloogletGreenRightImages == null)
                {
                    _bloogletGreenRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_green_right1,
                        Properties.Resources.keen6_blooglet_green_right2,
                        Properties.Resources.keen6_blooglet_green_right3,
                        Properties.Resources.keen6_blooglet_green_right4
                    };
                }
                return _bloogletGreenRightImages;
            }
        }

        private static Image[] _bloogletGreenLeftImages;

        public static Image[] BloogletGreenLeftImages
        {
            get
            {
                if (_bloogletGreenLeftImages == null)
                {
                    _bloogletGreenLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_green_left1,
                        Properties.Resources.keen6_blooglet_green_left2,
                        Properties.Resources.keen6_blooglet_green_left3,
                        Properties.Resources.keen6_blooglet_green_left4
                    };
                }
                return _bloogletGreenLeftImages;
            }
        }

        private static Image[] _bloogletGreenStunnedImages;

        public static Image[] BloogletGreenStunnedImages
        {
            get
            {
                if (_bloogletGreenStunnedImages == null)
                {
                    _bloogletGreenStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_green_stunned1,
                        Properties.Resources.keen6_blooglet_green_stunned2,
                        Properties.Resources.keen6_blooglet_green_stunned3,
                        Properties.Resources.keen6_blooglet_green_stunned4
                    };
                }
                return _bloogletGreenStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Blooglet Yellow

        private static Image[] _bloogletYellowRightImages;

        public static Image[] BloogletYellowRightImages
        {
            get
            {
                if (_bloogletYellowRightImages == null)
                {
                    _bloogletYellowRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_yellow_right1,
                        Properties.Resources.keen6_blooglet_yellow_right2,
                        Properties.Resources.keen6_blooglet_yellow_right3,
                        Properties.Resources.keen6_blooglet_yellow_right4
                    };
                }
                return _bloogletYellowRightImages;
            }
        }

        private static Image[] _bloogletYellowLeftImages;

        public static Image[] BloogletYellowLeftImages
        {
            get
            {
                if (_bloogletYellowLeftImages == null)
                {
                    _bloogletYellowLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_yellow_left1,
                        Properties.Resources.keen6_blooglet_yellow_left2,
                        Properties.Resources.keen6_blooglet_yellow_left3,
                        Properties.Resources.keen6_blooglet_yellow_left4
                    };
                }
                return _bloogletYellowLeftImages;
            }
        }

        private static Image[] _bloogletYellowStunnedImages;

        public static Image[] BloogletYellowStunnedImages
        {
            get
            {
                if (_bloogletYellowStunnedImages == null)
                {
                    _bloogletYellowStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_blooglet_yellow_stunned1,
                        Properties.Resources.keen6_blooglet_yellow_stunned2,
                        Properties.Resources.keen6_blooglet_yellow_stunned3,
                        Properties.Resources.keen6_blooglet_yellow_stunned4
                    };
                }
                return _bloogletYellowStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Bloog

        private static Image[] _bloogWalkLeftImages;

        public static Image[] BloogWalkLeftImages
        {
            get
            {
                if (_bloogWalkLeftImages == null)
                {
                    _bloogWalkLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_bloog_left1,
                        Properties.Resources.keen6_bloog_left2,
                        Properties.Resources.keen6_bloog_left3,
                        Properties.Resources.keen6_bloog_left4
                    };
                }
                return _bloogWalkLeftImages;
            }
        }

        private static Image[] _bloogWalkRightImages;

        public static Image[] BloogWalkRightImages
        {
            get
            {
                if (_bloogWalkRightImages == null)
                {
                    _bloogWalkRightImages = new Image[]
                    {
                        Properties.Resources.keen6_bloog_right1,
                        Properties.Resources.keen6_bloog_right2,
                        Properties.Resources.keen6_bloog_right3,
                        Properties.Resources.keen6_bloog_right4
                    };
                }
                return _bloogWalkRightImages;
            }
        }

        private static Image[] _bloogStunnedImages;

        public static Image[] BloogStunnedImages
        {
            get
            {
                if (_bloogStunnedImages == null)
                {
                    _bloogStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_bloog_stunned1,
                        Properties.Resources.keen6_bloog_stunned2,
                        Properties.Resources.keen6_bloog_stunned3,
                        Properties.Resources.keen6_bloog_stunned4
                    };
                }
                return _bloogStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Blooguard

        private static Image[] _blooguardWalkLeftImages;

        public static Image[] BlooguardWalkLeftImages
        {
            get
            {
                if (_blooguardWalkLeftImages == null)
                {
                    _blooguardWalkLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooguard_left1,
                        Properties.Resources.keen6_blooguard_left2,
                        Properties.Resources.keen6_blooguard_left3,
                        Properties.Resources.keen6_blooguard_left4
                    };
                }
                return _blooguardWalkLeftImages;
            }
        }

        private static Image[] _blooguardWalkRightImages;

        public static Image[] BlooguardWalkRightImages
        {
            get
            {
                if (_blooguardWalkRightImages == null)
                {
                    _blooguardWalkRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooguard_right1,
                        Properties.Resources.keen6_blooguard_right2,
                        Properties.Resources.keen6_blooguard_right3,
                        Properties.Resources.keen6_blooguard_right4
                    };
                }
                return _blooguardWalkRightImages;
            }
        }

        private static Image[] _blooguardStunnedImages;

        public static Image[] BlooguardStunnedImages
        {
            get
            {
                if (_blooguardStunnedImages == null)
                {
                    _blooguardStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_blooguard_stunned1,
                        Properties.Resources.keen6_blooguard_stunned2,
                        Properties.Resources.keen6_blooguard_stunned3,
                        Properties.Resources.keen6_blooguard_stunned4
                    };
                }
                return _blooguardStunnedImages;
            }
        }


        private static Image[] _blooguardSmashLeftImages;

        public static Image[] BlooguardSmashLeftImages
        {
            get
            {
                if (_blooguardSmashLeftImages == null)
                {
                    _blooguardSmashLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_blooguard_smash_left1,
                        Properties.Resources.keen6_blooguard_smash_left2,
                        Properties.Resources.keen6_blooguard_smash_left3
                    };
                }
                return _blooguardSmashLeftImages;
            }
        }

        private static Image[] _blooguardSmashRightImages;

        public static Image[] BlooguardSmashRightImages
        {
            get
            {
                if (_blooguardSmashRightImages == null)
                {
                    _blooguardSmashRightImages = new Image[]
                    {
                        Properties.Resources.keen6_blooguard_smash_right1,
                        Properties.Resources.keen6_blooguard_smash_right2,
                        Properties.Resources.keen6_blooguard_smash_right3
                    };
                }
                return _blooguardSmashRightImages;
            }
        }

        #endregion

        #region Keen 6 Babobba
        private static Image[] _babobbaStunnedImages;

        public static Image[] BabobbaStunnedImages
        {
            get
            {
                if (_babobbaStunnedImages == null)
                {
                    _babobbaStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_babobba_stunned1,
                        Properties.Resources.keen6_babobba_stunned2,
                        Properties.Resources.keen6_babobba_stunned3,
                        Properties.Resources.keen6_babobba_stunned4
                    };
                }
                return _babobbaStunnedImages;
            }
        }

        private static Image[] _babobbaSleepImages;

        public static Image[] BabobbaSleepImages
        {
            get
            {
                if (_babobbaSleepImages == null)
                {
                    _babobbaSleepImages = new Image[]
                    {
                        Properties.Resources.keen6_babobba_fall_asleep1,
                        Properties.Resources.keen6_babobba_fall_asleep2,
                        Properties.Resources.keen6_babobba_fall_asleep3,
                        Properties.Resources.keen6_babobba_fall_asleep4
                    };
                }
                return _babobbaSleepImages;
            }
        }
        #endregion

        #region Keen 6 Bobba

        private static Image[] _bobbaFireBallImages;

        public static Image[] BobbaFireBallImages
        {
            get
            {
                if (_bobbaFireBallImages == null)
                {
                    _bobbaFireBallImages = new Image[]
                    {
                        Properties.Resources.keen6_bobba_fireball1,
                        Properties.Resources.keen6_bobba_fireball2,
                        Properties.Resources.keen6_bobba_fireball3,
                        Properties.Resources.keen6_bobba_fireball4
                    };
                }
                return _bobbaFireBallImages;
            }
        }

        #endregion

        #region Keen 6 Gix

        private static Image[] _gixMoveLeftImages;

        public static Image[] GixMoveLeftImages
        {
            get
            {
                if (_gixMoveLeftImages == null)
                {
                    _gixMoveLeftImages = new Image[] {
                        Properties.Resources.keen6_gix_left1,
                        Properties.Resources.keen6_gix_left2,
                        Properties.Resources.keen6_gix_left3
                    };
                }
                return _gixMoveLeftImages;
            }
        }

        private static Image[] _gixMoveRightImages;

        public static Image[] GixMoveRightImages
        {
            get
            {
                if (_gixMoveRightImages == null)
                {
                    _gixMoveRightImages = new Image[] {
                        Properties.Resources.keen6_gix_right1,
                        Properties.Resources.keen6_gix_right2,
                        Properties.Resources.keen6_gix_right3
                    };
                }
                return _gixMoveRightImages;
            }
        }

        private static Image[] _gixSlideLeftImages;

        public static Image[] GixSlideLeftImages
        {
            get
            {
                if (_gixSlideLeftImages == null)
                {
                    _gixSlideLeftImages = new Image[] {
                        Properties.Resources.keen6_gix_slide_left1,
                        Properties.Resources.keen6_gix_slide_left2
                    };
                }
                return _gixSlideLeftImages;
            }
        }

        private static Image[] _gixSlideRightImages;

        public static Image[] GixSlideRightImages
        {
            get
            {
                if (_gixSlideRightImages == null)
                {
                    _gixSlideRightImages = new Image[] {
                        Properties.Resources.keen6_gix_slide_right1,
                        Properties.Resources.keen6_gix_slide_right2
                    };
                }
                return _gixSlideRightImages;
            }
        }

        #endregion

        #region Keen 6 Flect

        private static Image[] _flectWalkLeftImages;

        public static Image[] FlectWalkLeftImages
        {
            get
            {
                if (_flectWalkLeftImages == null)
                {
                    _flectWalkLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_flect_left1,
                        Properties.Resources.keen6_flect_left2,
                        Properties.Resources.keen6_flect_left3,
                        Properties.Resources.keen6_flect_left4,
                        Properties.Resources.keen6_flect_left5
                    };
                }
                return _flectWalkLeftImages;
            }
        }


        private static Image[] _flectWalkRightImages;

        public static Image[] FlectWalkRightImages
        {
            get
            {
                if (_flectWalkRightImages == null)
                {
                    _flectWalkRightImages = new Image[]
                    {
                        Properties.Resources.keen6_flect_right1,
                        Properties.Resources.keen6_flect_right2,
                        Properties.Resources.keen6_flect_right3,
                        Properties.Resources.keen6_flect_right4,
                        Properties.Resources.keen6_flect_right5
                    };
                }
                return _flectWalkRightImages;
            }
        }

        private static Image[] _flectStunnedImages;

        public static Image[] FlectStunnedImages
        {
            get
            {
                if (_flectStunnedImages == null)
                {
                    _flectStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_flect_stunned1,
                        Properties.Resources.keen6_flect_stunned2,
                        Properties.Resources.keen6_flect_stunned3,
                        Properties.Resources.keen6_flect_stunned4
                    };
                }
                return _flectStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Nospike

        private static Image[] _nospikePatrolLeftImages;

        public static Image[] NospikePatrolLeftImages
        {
            get
            {
                if (_nospikePatrolLeftImages == null)
                {
                    _nospikePatrolLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_nospike_patrol_left1,
                        Properties.Resources.keen6_nospike_patrol_left2,
                        Properties.Resources.keen6_nospike_patrol_left3,
                        Properties.Resources.keen6_nospike_patrol_left4
                    };
                }
                return _nospikePatrolLeftImages;
            }
        }

        private static Image[] _nospikePatrolRightImages;

        public static Image[] NospikePatrolRightImages
        {
            get
            {
                if (_nospikePatrolRightImages == null)
                {
                    _nospikePatrolRightImages = new Image[]
                    {
                        Properties.Resources.keen6_nospike_patrol_right1,
                        Properties.Resources.keen6_nospike_patrol_right2,
                        Properties.Resources.keen6_nospike_patrol_right3,
                        Properties.Resources.keen6_nospike_patrol_right4
                    };
                }
                return _nospikePatrolRightImages;
            }
        }


        private static Image[] _nospikeChargeLeftImages;

        public static Image[] NospikeChargeLeftImages
        {
            get
            {
                if (_nospikeChargeLeftImages == null)
                {
                    _nospikeChargeLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_nospike_charge_left1,
                        Properties.Resources.keen6_nospike_charge_left2,
                        Properties.Resources.keen6_nospike_charge_left3,
                        Properties.Resources.keen6_nospike_charge_left4
                    };
                }
                return _nospikeChargeLeftImages;
            }
        }

        private static Image[] _nospikeChargeRightImages;

        public static Image[] NospikeChargeRightImages
        {
            get
            {
                if (_nospikeChargeRightImages == null)
                {
                    _nospikeChargeRightImages = new Image[]
                    {
                        Properties.Resources.keen6_nospike_charge_right1,
                        Properties.Resources.keen6_nospike_charge_right2,
                        Properties.Resources.keen6_nospike_charge_right3,
                        Properties.Resources.keen6_nospike_charge_right4
                    };
                }
                return _nospikeChargeRightImages;
            }
        }

        private static Image[] _nospikeStunnedImages;

        public static Image[] NospikeStunnedImages
        {
            get
            {
                if (_nospikeStunnedImages == null)
                {
                    _nospikeStunnedImages = new Image[]{
                        Properties.Resources.keen6_nospike_stunned1,
                        Properties.Resources.keen6_nospike_stunned2,
                        Properties.Resources.keen6_nospike_stunned3,
                        Properties.Resources.keen6_nospike_stunned4
                    };
                }
                return _nospikeStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Ceilick

        private static Image[] _ceilickAttackImages;

        public static Image[] CeilickAttackImages
        {
            get
            {
                if (_ceilickAttackImages == null)
                {
                    _ceilickAttackImages = new Image[]{
                        Properties.Resources.keen6_ceilick_tongue_attack1,
                        Properties.Resources.keen6_ceilick_tongue_attack2,
                        Properties.Resources.keen6_ceilick_tongue_attack3,
                        Properties.Resources.keen6_ceilick_tongue_attack4
                    };
                }
                return _ceilickAttackImages;
            }
        }


        private static Image[] _ceilickPeakImages;

        public static Image[] CeilickPeakImages
        {
            get
            {
                if (_ceilickPeakImages == null)
                {
                    _ceilickPeakImages = new Image[]{
                        Properties.Resources.keen6_ceilick_peak_head1,
                        Properties.Resources.keen6_ceilick_peak_head2,
                        Properties.Resources.keen6_ceilick_peak_head3,
                        Properties.Resources.keen6_ceilick_peak_head4,
                        Properties.Resources.keen6_ceilick_peak_head5,
                        Properties.Resources.keen6_ceilick_peak_head6,
                        Properties.Resources.keen6_ceilick_peak_head7
                    };
                }
                return _ceilickPeakImages;
            }
        }

        private static Image[] _ceilickStunnedImages;

        public static Image[] CeilickStunnedImages
        {
            get
            {
                if (_ceilickStunnedImages == null)
                {
                    _ceilickStunnedImages = new Image[]{
                        Properties.Resources.keen6_ceilick_stunned1,
                        Properties.Resources.keen6_ceilick_stunned2,
                        Properties.Resources.keen6_ceilick_stunned3,
                        Properties.Resources.keen6_ceilick_stunned4
                    };
                }
                return _ceilickStunnedImages;
            }
        }

        #endregion

        #region Keen 6 Bip Ship
        private static Image[] _bipShipTurnLeftImages;

        public static Image[] BipShipTurnLeftImages
        {
            get
            {
                if (_bipShipTurnLeftImages == null)
                {
                    _bipShipTurnLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_bip_ship_turn_left1,
                        Properties.Resources.keen6_bip_ship_turn_left2,
                        Properties.Resources.keen6_bip_ship_turn_left3
                    };
                }
                return _bipShipTurnLeftImages;
            }
        }

        private static Image[] _bipShipTurnRightImages;

        public static Image[] BipShipTurnRightImages
        {
            get
            {
                if (_bipShipTurnRightImages == null)
                {
                    _bipShipTurnRightImages = new Image[]
                    {
                        Properties.Resources.keen6_bip_ship_turn_right1,
                        Properties.Resources.keen6_bip_ship_turn_right2,
                        Properties.Resources.keen6_bip_ship_turn_right3
                    };
                }
                return _bipShipTurnRightImages;
            }
        }

        private static Image[] _bipShipExplodeImages;

        public static Image[] BipShipExplodeImages
        {
            get
            {
                if (_bipShipExplodeImages == null)
                {
                    _bipShipExplodeImages = new Image[]
                    {
                        Properties.Resources.keen6_bip_ship_explosion1,
                        Properties.Resources.keen6_bip_ship_explosion2
                    };
                }
                return _bipShipExplodeImages;
            }
        }

        #endregion

        #region Keen 6 Bip

        private static Image[] _bipLeftImages;

        public static Image[] BipLeftImages
        {
            get
            {
                if (_bipLeftImages == null)
                {
                    _bipLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_bip_left1,
                        Properties.Resources.keen6_bip_left2,
                        Properties.Resources.keen6_bip_left3,
                        Properties.Resources.keen6_bip_left4
                    };
                }
                return _bipLeftImages;
            }
        }

        private static Image[] _bipRightImages;

        public static Image[] BipRightImages
        {
            get
            {
                if (_bipRightImages == null)
                {
                    _bipRightImages = new Image[]
                    {
                        Properties.Resources.keen6_bip_right1,
                        Properties.Resources.keen6_bip_right2,
                        Properties.Resources.keen6_bip_right3,
                        Properties.Resources.keen6_bip_right4
                    };
                }
                return _bipRightImages;
            }
        }

        #endregion

        #region Keen 6 Blorb
        private static Image[] _blorbImages;

        public static Image[] BlorbImages
        {
            get
            {
                if (_blorbImages == null)
                {
                    _blorbImages = new Image[]
                    {
                        Properties.Resources.keen6_blorb1,
                        Properties.Resources.keen6_blorb2
                    };
                }
                return _blorbImages;
            }
        }
        #endregion

        #region Keen 6 Orbatrix

        private static Image[] _orbatrixLookImages;

        public static Image[] OrbatrixLookImages
        {
            get
            {
                if (_orbatrixLookImages == null)
                {
                    _orbatrixLookImages = new Image[]
                    {
                        Properties.Resources.keen6_orbatrix_look1,
                        Properties.Resources.keen6_orbatrix_look2,
                        Properties.Resources.keen6_orbatrix_look3,
                        Properties.Resources.keen6_orbatrix_look4,
                    };
                }
                return _orbatrixLookImages;
            }
        }

        private static Image[] _orbatrixLookLeftImages;

        public static Image[] OrbatrixLookLeftImages
        {
            get
            {
                if (_orbatrixLookLeftImages == null)
                {
                    _orbatrixLookLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_orbatrix_look_left1,
                        Properties.Resources.keen6_orbatrix_look_left2
                    };
                }
                return _orbatrixLookLeftImages;
            }
        }

        private static Image[] _orbatrixLookRightImages;

        public static Image[] OrbatrixLookRightImages
        {
            get
            {
                if (_orbatrixLookRightImages == null)
                {
                    _orbatrixLookRightImages = new Image[]
                    {
                        Properties.Resources.keen6_orbatrix_look_right1,
                        Properties.Resources.keen6_orbatrix_look_right2
                    };
                }
                return _orbatrixLookRightImages;
            }
        }

        private static Image[] _orbatrixAttackImages;

        public static Image[] OrbatrixAttackImages
        {
            get
            {
                if (_orbatrixAttackImages == null)
                {
                    _orbatrixAttackImages = new Image[]
                    {
                        Properties.Resources.keen6_orbatrix_attack1,
                        Properties.Resources.keen6_orbatrix_attack2,
                        Properties.Resources.keen6_orbatrix_attack3,
                        Properties.Resources.keen6_orbatrix_attack4,
                    };
                }
                return _orbatrixAttackImages;
            }
        }

        #endregion

        #region Keen 6 Fleex

        private static Image[] _fleexLookImages;

        public static Image[] FleexLookImages
        {
            get
            {
                if (_fleexLookImages == null)
                {
                    _fleexLookImages = new Image[]
                    {
                        Properties.Resources.keen6_fleex_look1,
                        Properties.Resources.keen6_fleex_look2
                    };
                }
                return _fleexLookImages;
            }
        }


        private static Image[] _fleexRightImages;

        public static Image[] FleexRightImages
        {
            get
            {
                if (_fleexRightImages == null)
                {
                    _fleexRightImages = new Image[]
                    {
                        Properties.Resources.keen6_fleex_right1,
                        Properties.Resources.keen6_fleex_right2
                    };
                }
                return _fleexRightImages;
            }
        }


        private static Image[] _fleexLeftImages;

        public static Image[] FleexLeftImages
        {
            get
            {
                if (_fleexLeftImages == null)
                {
                    _fleexLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_fleex_left1,
                        Properties.Resources.keen6_fleex_left2
                    };
                }
                return _fleexLeftImages;
            }
        }


        private static Image[] _fleexStunnedImages;

        public static Image[] FleexStunnedImages
        {
            get
            {
                if (_fleexStunnedImages == null)
                {
                    _fleexStunnedImages = new Image[]
                    {
                        Properties.Resources.keen6_fleex_stunned1,
                        Properties.Resources.keen6_fleex_stunned2,
                        Properties.Resources.keen6_fleex_stunned3,
                        Properties.Resources.keen6_fleex_stunned4
                    };
                }
                return _fleexStunnedImages;
            }
        }

        #endregion

        #region Keen 6 LaserField
        private static Image[] _keen6LaserFieldImages;

        public static Image[] Keen6LaserFieldImages
        {
            get
            {
                if (_keen6LaserFieldImages == null)
                {
                    _keen6LaserFieldImages = new Image[]{
                        Properties.Resources.keen6_laser_field_laser1,
                        Properties.Resources.keen6_laser_field_laser2,
                        Properties.Resources.keen6_laser_field_laser3
                    };
                }
                return _keen6LaserFieldImages;
            }
        }
        #endregion

        #region Keen 6 Burn Hazard

        private static Image[] _keen6BurnHazardImages
            ;

        public static Image[] Keen6BurnHazardImages
        {
            get
            {
                if (_keen6BurnHazardImages == null)
                {
                    _keen6BurnHazardImages = new Image[]
                    {
                        Properties.Resources.keen6_burn_hazard1,
                        Properties.Resources.keen6_burn_hazard2,
                        Properties.Resources.keen6_burn_hazard3,
                        Properties.Resources.keen6_burn_hazard4
                    };
                }
                return _keen6BurnHazardImages;
            }
        }

        #endregion

        #region Keen 6 Flame Thrower
        private static Image[] _flameThrowerBurnImages;

        public static Image[] Keen6FlameThrowerBurnImages
        {
            get
            {
                if (_flameThrowerBurnImages == null)
                {
                    _flameThrowerBurnImages = new Image[]
                    {
                        Properties.Resources.keen6_flame_thrower_off,
                        Properties.Resources.keen6_flame_thrower_on1,
                        Properties.Resources.keen6_flame_thrower_on2,
                        Properties.Resources.keen6_flame_thrower_on3,
                        Properties.Resources.keen6_flame_thrower_on2,
                        Properties.Resources.keen6_flame_thrower_on1
                    };
                }
                return _flameThrowerBurnImages;
            }
        }
        #endregion

        #region Keen 6 Smasher

        private static Image[] _keen6SmasherOnImages;

        public static Image[] Keen6SmasherOnImages
        {
            get
            {
                if (_keen6SmasherOnImages == null)
                {
                    _keen6SmasherOnImages = new Image[]
                    {
                        Properties.Resources.keen6_smasher_1,
                        Properties.Resources.keen6_smasher_2,
                        Properties.Resources.keen6_smasher_3,
                        Properties.Resources.keen6_smasher_2,
                        Properties.Resources.keen6_smasher_1
                    };
                }
                return _keen6SmasherOnImages;
            }
        }

        #endregion

        #region Keen 6 Slime Hazard
        private static Image[] _keen6SlimeHazardLeftImages;

        public static Image[] Keen6SlimeHazardLeftImages
        {
            get
            {
                if (_keen6SlimeHazardLeftImages == null)
                {
                    _keen6SlimeHazardLeftImages = new Image[]
                    {
                        Properties.Resources.keen6_slime_hazard_left1,
                        Properties.Resources.keen6_slime_hazard_left2,
                        Properties.Resources.keen6_slime_hazard_left3
                    };
                }
                return _keen6SlimeHazardLeftImages;
            }
        }

        private static Image[] _keen6SlimeHazardRightImages;

        public static Image[] Keen6SlimeHazardRightImages
        {
            get
            {
                if (_keen6SlimeHazardRightImages == null)
                {
                    _keen6SlimeHazardRightImages = new Image[]
                    {
                        Properties.Resources.keen6_slime_hazard_right1,
                        Properties.Resources.keen6_slime_hazard_right2,
                        Properties.Resources.keen6_slime_hazard_right3
                    };
                }
                return _keen6SlimeHazardRightImages;
            }
        }

        private static Image[] _keen6SlimeHazardMiddleImages;

        public static Image[] Keen6SlimeHazardMiddleImages
        {
            get
            {
                if (_keen6SlimeHazardMiddleImages == null)
                {
                    _keen6SlimeHazardMiddleImages = new Image[]
                    {
                        Properties.Resources.keen6_slime_hazard_middle1,
                        Properties.Resources.keen6_slime_hazard_middle2,
                        Properties.Resources.keen6_slime_hazard_middle3
                    };
                }
                return _keen6SlimeHazardMiddleImages;
            }
        }
        #endregion

        #region Keen 6 Drill
        private static Image[] _keen6DrillSprites;

        public static Image[] Keen6DrillSprites
        {
            get
            {
                if (_keen6DrillSprites == null)
                {
                    _keen6DrillSprites = new Image[]
                    {
                         Properties.Resources.keen6_drill1,
                         Properties.Resources.keen6_drill2,
                         Properties.Resources.keen6_drill3,
                         Properties.Resources.keen6_drill4
                    };
                }
                return _keen6DrillSprites;
            }
        }
        #endregion

        #region Keen 6 Electric Rods
        private static Image[] _keen6ElectricRodSprites;

        public static Image[] Keen6ElectricRodSprites
        {
            get
            {
                if (_keen6ElectricRodSprites == null)
                {
                    _keen6ElectricRodSprites = new Image[]
                    {
                        Properties.Resources.keen6_electric_rods1,
                        Properties.Resources.keen6_electric_rods2
                    };
                }
                return _keen6ElectricRodSprites;
            }
        }
        #endregion

        #region Keen 6 Conveyer Belt

        private static Image[] _keen6ConveyerBeltLeftSprites;

        public static Image[] Keen6ConveyerBeltLeftSprites
        {
            get
            {
                if (_keen6ConveyerBeltLeftSprites == null)
                {
                    _keen6ConveyerBeltLeftSprites = new Image[]
                    {
                        Properties.Resources.keen6_conveyer_belt_left1,
                        Properties.Resources.keen6_conveyer_belt_left2,
                        Properties.Resources.keen6_conveyer_belt_left3,
                        Properties.Resources.keen6_conveyer_belt_left4,
                    };
                }
                return _keen6ConveyerBeltLeftSprites;
            }
        }

        private static Image[] _keen6ConveyerBeltRightSprites;

        public static Image[] Keen6ConveyerBeltRightSprites
        {
            get
            {
                if (_keen6ConveyerBeltRightSprites == null)
                {
                    _keen6ConveyerBeltRightSprites = new Image[]
                    {
                        Properties.Resources.keen6_conveyer_belt_right1,
                        Properties.Resources.keen6_conveyer_belt_right2,
                        Properties.Resources.keen6_conveyer_belt_right3,
                        Properties.Resources.keen6_conveyer_belt_right4,
                    };
                }
                return _keen6ConveyerBeltRightSprites;
            }
        }

        private static Image[] _keen6ConveyerBeltMiddleSprites;

        public static Image[] Keen6ConveyerBeltMiddleSprites
        {
            get
            {
                if (_keen6ConveyerBeltMiddleSprites == null)
                {
                    _keen6ConveyerBeltMiddleSprites = new Image[]
                    {
                        Properties.Resources.keen6_conveyer_belt_middle1,
                        Properties.Resources.keen6_conveyer_belt_middle2,
                        Properties.Resources.keen6_conveyer_belt_middle3,
                        Properties.Resources.keen6_conveyer_belt_middle4,
                    };
                }
                return _keen6ConveyerBeltMiddleSprites;
            }
        }

        #endregion

        #region Keen Stand Sprites

        private static Image[] _keenStandImages;

        public static Image[] KeenStandImages
        {
            get
            {
                if (_keenStandImages == null)
                {
                    _keenStandImages = new Image[]
                    {
                        Properties.Resources.keen_stand_left,
                        Properties.Resources.keen_stand_right
                    };
                }
                return _keenStandImages;
            }
        }

        #endregion

        #region Tiles

        #region Keen 4 Cave

        private static Image[] _keen4CaveFloorTiles;

        public static Image[] Keen4CaveFloorTiles
        {
            get
            {
                if (_keen4CaveFloorTiles == null)
                {
                    _keen4CaveFloorTiles = new Image[]
                    {
                        Properties.Resources.keen4_cave_air_floor_edge_left,
                        Properties.Resources.keen4_cave_floor_middle,
                        Properties.Resources.keen4_cave_floor_edge_right
                    };
                }
                return _keen4CaveFloorTiles;
            }
        }

        private static Image[] _keen4CaveTilesBottom;

        public static Image[] Keen4CaveTilesBottom
        {
            get
            {
                if (_keen4CaveTilesBottom == null)
                {
                    _keen4CaveTilesBottom = new Image[]
                    {
                        Properties.Resources.keen4_cave_wall_bottom1,
                        Properties.Resources.keen4_cave_wall_bottom2,
                        Properties.Resources.keen4_cave_wall_bottom3,
                        Properties.Resources.keen4_cave_wall_bottom4,
                        Properties.Resources.keen4_cave_wall_bottom5,
                        Properties.Resources.keen4_cave_wall_bottom6,
                        Properties.Resources.keen4_cave_wall_bottom7
                    };
                }
                return _keen4CaveTilesBottom;
            }
        }

        #endregion

        #region Keen 4 Forest

        private static Image[] _keen4Floors;

        public static Image[] Keen4Floors
        {
            get
            {
                if (_keen4Floors == null)
                {
                    _keen4Floors = new Image[]
                    {
                        Properties.Resources.keen4_forest_floor_edge_left,
                        Properties.Resources.keen4_forest_floor_middle,
                        Properties.Resources.keen4_forest_floor_edge_right
                    };
                }
                return _keen4Floors;
            }
        }

        private static Image[] _keen4ForestTilesBottom;

        public static Image[] Keen4ForestTilesBottom
        {
            get
            {
                if (_keen4ForestTilesBottom == null)
                {
                    _keen4ForestTilesBottom = new Image[]
                    {
                        Properties.Resources.keen4_forest_wall_bottom1,
                        Properties.Resources.keen4_forest_wall_bottom2
                    };
                }
                return _keen4ForestTilesBottom;
            }
        }

        private static Image[] _keen4ForestPlatforms;

        public static Image[] Keen4ForestPlatforms
        {
            get
            {
                if (_keen4ForestPlatforms == null)
                {
                    _keen4ForestPlatforms = new Image[]
                    {
                        Properties.Resources.keen4_forest_platform_left_edge,
                        Properties.Resources.keen4_forest_platform_middle,
                        Properties.Resources.keen4_forest_platform_right_edge,
                        Properties.Resources.keen4_forest_platform_single
                    };
                }
                return _keen4ForestPlatforms;
            }
        }

        #endregion


        private static Image[] _keen4MirageTilesBottom;

        public static Image[] Keen4MirageTilesBottom
        {
            get
            {
                if (_keen4MirageTilesBottom == null)
                {
                    _keen4MirageTilesBottom = new Image[]
                    {
                        Properties.Resources.keen4_mirage_wall_bottom1,
                        Properties.Resources.keen4_mirage_wall_bottom2,
                        Properties.Resources.keen4_mirage_wall_bottom3,
                        Properties.Resources.keen4_mirage_wall_bottom4
                    };
                }
                return _keen4MirageTilesBottom;
            }
        }


        private static Image[] _keen4PyramidTilesBottom;

        public static Image[] Keen4PyramidTilesBottom
        {
            get
            {
                if (_keen4PyramidTilesBottom == null)
                {
                    _keen4PyramidTilesBottom = new Image[]
                    {
                        Properties.Resources.keen4_pyramid_wall_bottom1,
                        Properties.Resources.keen4_pyramid_wall_bottom2
                    };
                }
                return _keen4PyramidTilesBottom;
            }
        }

        private static Image[] _keen5BlackTilesBottom;

        public static Image[] Keen5BlackTilesBottom
        {
            get
            {
                if (_keen5BlackTilesBottom == null)
                {
                    _keen5BlackTilesBottom = new Image[]
                    {
                        Properties.Resources.keen5_wall_black_bottom
                    };
                }
                return _keen5BlackTilesBottom;
            }
        }

        private static Image[] _keen5GreenTilesBottom;

        public static Image[] Keen5GreenTilesBottom
        {
            get
            {
                if (_keen5GreenTilesBottom == null)
                {
                    _keen5GreenTilesBottom = new Image[]
                    {
                        Properties.Resources.keen5_ceiling_green
                    };
                }
                return _keen5GreenTilesBottom;
            }
        }

        private static Image[] _keen5RedTilesBottom;

        public static Image[] Keen5RedTilesBottom
        {
            get
            {
                if (_keen5RedTilesBottom == null)
                {
                    _keen5RedTilesBottom = new Image[]
                    {
                        Properties.Resources.keen5_ceiling_red
                    };
                }
                return _keen5RedTilesBottom;
            }
        }

        private static Image[] _keen6ForestTilesBottom;

        public static Image[] Keen6ForestTilesBottom
        {
            get
            {
                if (_keen6ForestTilesBottom == null)
                {
                    _keen6ForestTilesBottom = new Image[]
                    {
                        Properties.Resources.keen6_forest_ceiling1,
                        Properties.Resources.keen6_forest_ceiling2
                    };
                }
                return _keen6ForestTilesBottom;
            }
        }

        private static Image[] _keen6IndustrialTilesBottom;

        public static Image[] Keen6IndustrialTilesBottom
        {
            get
            {
                if (_keen6IndustrialTilesBottom == null)
                {
                    _keen6IndustrialTilesBottom = new Image[]
                    {
                        Properties.Resources.keen6_industrial_ceiling
                    };
                }
                return _keen6IndustrialTilesBottom;
            }
        }

        private static Image[] _keen6DomeTilesBottom;

        public static Image[] Keen6DomeTilesBottom
        {
            get
            {
                if (_keen6DomeTilesBottom == null)
                {
                    _keen6DomeTilesBottom = new Image[]
                    {
                        Properties.Resources.keen6_dome_ceiling2,
                        Properties.Resources.keen6_dome_ceiling3
                    };
                }
                return _keen6DomeTilesBottom;
            }
        }

        #endregion

        #region Backgrounds

        private static Image[] _keen5DownArrows;

        public static Image[] Keen5DownArrows
        {
            get
            {
                if (_keen5DownArrows == null)
                {
                    _keen5DownArrows = new Image[]
                    {
                        Properties.Resources.keen5_arrow_down_off,
                        Properties.Resources.keen5_arrow_down_on
                    };
                }
                return _keen5DownArrows;
            }
        }

        public static Image Keen5Omegamatic1
        {
            get
            {
                return Properties.Resources.keen5_background_omegamatic_blue1;
            }
        }

        public static Image Keen5Omegamatic2
        {
            get
            {
                return Properties.Resources.keen5_background_omegamatic_blue2;
            }
        }

        public static Image Keen5SecurityFuelTanks
        {
            get
            {
                return Properties.Resources.keen5_background_security_center_fuel_tanks;
            }
        }

        public static Image Keen5SecurityCenterGrayWall
        {
            get
            {
                return Properties.Resources.keen5_background_security_center_gray;
            }
        }


        #endregion


        #region Animations
        #region Keen 4

        private static Image[] _keen4Seaweed1Images;

        public static Image[] Keen4Seaweed1Images
        {
            get
            {
                if (_keen4Seaweed1Images == null)
                {
                    _keen4Seaweed1Images = new Image[]
                    {
                        Properties.Resources.keen4_seaweed1_1,
                        Properties.Resources.keen4_seaweed1_2,
                        Properties.Resources.keen4_seaweed1_3,
                        Properties.Resources.keen4_seaweed1_4
                    };
                }
                return _keen4Seaweed1Images;
            }
        }

        private static Image[] _keen4Seaweed2Images;


        public static Image[] Keen4Seaweed2Images
        {
            get
            {
                if (_keen4Seaweed2Images == null)
                {
                    _keen4Seaweed2Images = new Image[]
                    {
                        Properties.Resources.keen4_seaweed2_1,
                        Properties.Resources.keen4_seaweed2_2,
                        Properties.Resources.keen4_seaweed2_3,
                        Properties.Resources.keen4_seaweed2_4
                    };
                }
                return _keen4Seaweed2Images;
            }
        }

        #endregion

        #endregion

        #region Moving Platforms
        private static Image[] _keen4MovingPlatformLeftSprites;

        public static Image[] Keen4MovingPlatformLeftSprites
        {
            get
            {
                if (_keen4MovingPlatformLeftSprites == null)
                {
                    _keen4MovingPlatformLeftSprites = new Image[]
                    {
                        Properties.Resources.keen4_platform_horizontal_left1,
                        Properties.Resources.keen4_platform_horizontal_left2
                    };
                }
                return _keen4MovingPlatformLeftSprites;
            }
        }

        private static Image[] _keen4MovingPlatformRightSprites;

        public static Image[] Keen4MovingPlatformRightSprites
        {
            get
            {
                if (_keen4MovingPlatformRightSprites == null)
                {
                    _keen4MovingPlatformRightSprites = new Image[]
                    {
                        Properties.Resources.keen4_platform_horizontal_right1,
                        Properties.Resources.keen4_platform_horizontal_right2
                    };
                }
                return _keen4MovingPlatformRightSprites;
            }
        }

        private static Image[] _keen5OrangePlatformSprites;

        public static Image[] Keen5OrangePlatformSprites
        {
            get
            {
                if (_keen5OrangePlatformSprites == null)
                {
                    _keen5OrangePlatformSprites = new Image[]
                    {
                        Properties.Resources.keen5_orange_platform
                    };
                }
                return _keen5OrangePlatformSprites;
            }
        }

        private static Image[] _keen5PinkPlatformSprites;

        public static Image[] Keen5PinkPlatformSprites
        {
            get
            {
                if (_keen5PinkPlatformSprites == null)
                {
                    _keen5PinkPlatformSprites = new Image[]
                    {
                        Properties.Resources.keen5_pink_platform
                    };
                }
                return _keen5PinkPlatformSprites;
            }
        }

        private static Image[] _keen6PlatformSprites;

        public static Image[] Keen6MovingPlatformSprites
        {
            get
            {
                if (_keen6PlatformSprites == null)
                {
                    _keen6PlatformSprites = new Image[]
                    {
                        Properties.Resources.keen6_bip_platform
                    };
                }
                return _keen6PlatformSprites;
            }
        }
        #endregion

        #region CTF Game Mode

        private static Image[] _ctfColors;

        public static Image[] CTFColors
        {
            get
            {
                if (_ctfColors == null)
                {
                    _ctfColors = new Image[]
                    {
                        Properties.Resources.Red_Flag,
                        Properties.Resources.Blue_Flag,
                        Properties.Resources.Green_Flag,
                        Properties.Resources.Yellow_Flag
                    };
                }
                return _ctfColors;
            }
        }

        private static Image[] _ctfDestinations;

        public static Image[] CTFDestinations
        {
            get
            {
                if (_ctfDestinations == null)
                {
                    _ctfDestinations = new Image[]
                    {
                        Properties.Resources.red_flag_destination,
                        Properties.Resources.blue_flag_destination,
                        Properties.Resources.green_flag_destination,
                        Properties.Resources.yellow_flag_destination
                    };
                }
                return _ctfDestinations;
            }
        }

        #endregion

        #region Explosions

        private static Image[] _rpgExplosionSprites;

        public static Image[] RPGExplosionSprites
        {
            get
            {
                if (_rpgExplosionSprites == null)
                {
                    _rpgExplosionSprites = new Image[]
                    {
                         Properties.Resources.keen_stun_shot1,
                         Properties.Resources.keen_stun_shot2,
                         Properties.Resources.keen_stun_shot3,
                         Properties.Resources.keen_stun_shot4,
                         Properties.Resources.keen_stun_shot_hit1,
                         Properties.Resources.keen_stun_shot_hit2
                    };
                }
                return _rpgExplosionSprites;
            }
        }

        #endregion

        #region Snake Gun Sprites
        private static Image[] _snakeGunVerticalSprites;

        public static Image[] SnakeGunVerticalSprites
        {
            get
            {
                if (_snakeGunVerticalSprites == null)
                {
                    _snakeGunVerticalSprites = new Image[]
                    {
                        Properties.Resources.snake_gun_shot_vertical1,
                        Properties.Resources.snake_gun_shot_vertical2,
                        Properties.Resources.snake_gun_shot_vertical3,
                        Properties.Resources.snake_gun_shot_vertical4,
                    };
                }
                return _snakeGunVerticalSprites;
            }
        }

        private static Image[] _snakeGunHorizontalSprites;

        public static Image[] SnakeGunHorizontalSprites
        {
            get
            {
                if (_snakeGunHorizontalSprites == null)
                {
                    _snakeGunHorizontalSprites = new Image[]
                    {
                        Properties.Resources.snake_gun_shot_horizontal1,
                        Properties.Resources.snake_gun_shot_horizontal2,
                        Properties.Resources.snake_gun_shot_horizontal3,
                        Properties.Resources.snake_gun_shot_horizontal4,
                    };
                }
                return _snakeGunHorizontalSprites;
            }
        }
        #endregion
    }
}
