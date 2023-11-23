using KeenReloaded.Framework;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Enemies;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Hazards;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.HelperClasses
{
    public class ImageToObjectCreationFactory
    {
        private Dictionary<string, TypeConstructorValues> _typeDict;
        private static ImageToObjectCreationFactory _instance;
        private ImageToObjectCreationFactory()
        {
            _typeDict = new Dictionary<string, TypeConstructorValues>()
            {
                #region backgrounds
                {  "keen5_background_omegamatic_blue1"
                      , new TypeConstructorValues(typeof(BackgroundSprite),
                        new object[] { "area", "sprite", "zIndex", "stretchImage" },
                        new object[] { new Rectangle(0,0,64,64), SpriteSheet.Keen5Omegamatic1,  0, false })
                },
                {  "keen5_background_omegamatic_blue2"
                      , new TypeConstructorValues(typeof(BackgroundSprite),
                        new object[] { "area", "sprite", "zIndex", "stretchImage" },
                        new object[] { new Rectangle(0,0,64,64), SpriteSheet.Keen5Omegamatic2,  0, false })
                },
                {  "keen5_background_security_center_fuel_tanks"
                      , new TypeConstructorValues(typeof(BackgroundSprite),
                        new object[] { "area", "sprite", "zIndex", "stretchImage" },
                        new object[] { new Rectangle(0,0,126,58), SpriteSheet.Keen5SecurityFuelTanks,  0, false })
                },
                {  "keen5_background_security_center_gray"
                      , new TypeConstructorValues(typeof(BackgroundSprite),
                        new object[] { "area", "sprite", "zIndex", "stretchImage" },
                        new object[] { new Rectangle(0,0,130,53), SpriteSheet.Keen5SecurityCenterGrayWall,  0, false })
                },
                {
                    "keen_stand_right"
                     , new TypeConstructorValues(typeof(CommanderKeen),
                        new object[] { "grid", "hitbox", "direction" },
                        new object[] { new SpaceHashGrid(1500,1500,150,150), new Rectangle(0,0,26,64), Direction.RIGHT })
                },
                {
                    "keen_stand_left"
                     , new TypeConstructorValues(typeof(CommanderKeen),
                        new object[] { "grid", "hitbox", "direction" },
                        new object[] { new SpaceHashGrid(1500,1500,150,150), new Rectangle(0,0,26,64), Direction.LEFT })
                },
                #region keen4 forest
                {
                    "keen4_forest_platform_middle"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 38), BiomeType.KEEN4_GREEN, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_forest_platform_right_edge"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN4_GREEN, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_forest_platform_left_edge"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN4_GREEN, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_forest_floor_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN4_GREEN, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_forest_floor_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_GREEN, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_forest_floor_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN4_GREEN, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_forest_wall_bottom2"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_GREEN, TileType.CEILING })
                },
                {
                    "keen4_forest_wall_bottom1"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_GREEN, TileType.CEILING })
                },
                {
                    "keen4_forest_platform_single"
                     , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN4_GREEN })
                },
                {
                    "keen4_forest_wall_right_edge"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_GREEN, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen4_forest_wall_left_edge"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_GREEN, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen4_forest_wall_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_GREEN, TileType.WALL_MIDDLE })
                },
#endregion
               
                #region keen4 pyramid
                {
                    "keen4_pyramid_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN4_PYRAMID, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_pyramid_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_PYRAMID, TileType.FLOOR_RIGHT_EDGE })
                },
                 {
                    "keen4_pyramid_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN4_PYRAMID, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_pyramid_wall_bottom1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_PYRAMID, TileType.CEILING })
                },
                 {
                    "keen4_pyramid_wall_bottom2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_PYRAMID, TileType.CEILING })
                },
                 {
                    "keen4_pyramid_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 70, 64), BiomeType.KEEN4_PYRAMID, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen4_pyramid_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 70, 64), BiomeType.KEEN4_PYRAMID, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen4_pyramid_wall_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_PYRAMID, TileType.WALL_MIDDLE })
                },
                {
                    "keen4_secret_platform1"
                      , new TypeConstructorValues(typeof(SecretPlatformTile),
                        new object[] { "grid", "hitbox", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 6, 2), SecretPlatformState.HIDDEN })
                },
                #endregion
                #region keen 4 mirage

                {
                    "keen4_mirage_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_mirage_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_RIGHT_EDGE })
                },
                 {
                    "keen4_mirage_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_mirage_wall_bottom1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_MIRAGE, TileType.CEILING })
                },
                 {
                    "keen4_mirage_wall_bottom2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_MIRAGE, TileType.CEILING })
                },
                  {
                    "keen4_mirage_wall_bottom3"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_MIRAGE, TileType.CEILING })
                },
                 {
                    "keen4_mirage_wall_bottom4"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_MIRAGE, TileType.CEILING })
                },
                 {
                    "keen4_mirage_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_MIRAGE, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen4_mirage_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_MIRAGE, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen4_mirage_wall_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_MIRAGE, TileType.WALL_MIDDLE })
                },
                 {
                    "keen4_secret_platform_mirage"
                      , new TypeConstructorValues(typeof(SecretPlatformTile),
                        new object[] { "grid", "hitbox", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 6, 2), SecretPlatformState.HIDDEN })
                },
                {
                    "keen4_mirage_floor_to_platform_left"
                      , new TypeConstructorValues(typeof(FloorToPlatformTile),
                        new object[] { "grid", "hitbox", "biome"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_MIRAGE })
                },
                  {
                    "keen4_mirage_platform_left_edge"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 48), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_mirage_platform_right_edge"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 48), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_mirage_platform_middle"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 48), BiomeType.KEEN4_MIRAGE, TileType.FLOOR_MIDDLE })
                },
                 {
                    "keen4_mirage_platform_single"
                      , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 48), BiomeType.KEEN4_MIRAGE })
                },
                 {
                    "keen4_mirage_wall_to_platform_left"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN4_MIRAGE, Direction.LEFT })
                },
                {
                    "keen4_mirage_wall_to_platform_right"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN4_MIRAGE, Direction.RIGHT })
                },
                {
                    "keen4_mirage_platform1"
                      , new TypeConstructorValues(typeof(MiragePlatform),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), MiragePlatformState.PHASE1 })
                },
                {
                    "keen4_mirage_platform2"
                      , new TypeConstructorValues(typeof(MiragePlatform),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), MiragePlatformState.PHASE2 })
                },
                {
                    "keen4_mirage_platform3"
                      , new TypeConstructorValues(typeof(MiragePlatform),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), MiragePlatformState.PHASE3 })
                },
                {
                    "keen4_mirage_platform4"
                      , new TypeConstructorValues(typeof(MiragePlatform),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), MiragePlatformState.PHASE4 })
                },
#endregion
                #region keen4 cave
                {
                    "keen4_cave_air_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN4_CAVE, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_cave_air_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_CAVE, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_cave_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_CAVE, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_cave_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN4_CAVE, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_cave_platform_left_edge"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 38), BiomeType.KEEN4_CAVE, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen4_cave_platform_right_edge"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 38), BiomeType.KEEN4_CAVE, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen4_cave_platform_middle"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 36), BiomeType.KEEN4_CAVE, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen4_cave_wall_bottom1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_bottom2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_bottom3"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_bottom4"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                 {
                    "keen4_cave_wall_bottom5"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_bottom6"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_bottom7"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN4_CAVE, TileType.CEILING })
                },
                {
                    "keen4_cave_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN4_CAVE, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen4_cave_wall_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_CAVE, TileType.WALL_MIDDLE })
                },
                {
                    "keen4_cave_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN4_CAVE, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen4_secret_platform1_cave"
                      , new TypeConstructorValues(typeof(SecretPlatformTile),
                        new object[] { "grid", "hitbox", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 6, 2), SecretPlatformState.SHOW1 })
                },
                #endregion
                #region keen 5 black
                {
                    "keen5_black_wall_to_platform_left"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_BLACK, Direction.LEFT })
                },
                {
                    "keen5_black_floor_to_platform_left"
                      , new TypeConstructorValues(typeof(FloorToPlatformTile),
                        new object[] { "grid", "hitbox", "biome"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_BLACK  })
                },
                {
                    "keen5_floor_black_floor_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN5_BLACK, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_floor_black_floor_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_BLACK, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_floor_black_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN5_BLACK, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_wall_black_bottom"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_BLACK, TileType.CEILING })
                },
                {
                    "keen5_wall_black_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_BLACK, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen5_wall_black_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_BLACK, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen5_wall_black_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_BLACK, TileType.WALL_MIDDLE })
                },
                {
                    "keen5_pipe_platform"
                     , new TypeConstructorValues(typeof(Keen5LargePipePlatform),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 346, 80)})
                },
                {
                    "keen5_platform_blue_edge_left"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_BLACK, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_platform_blue_edge_right"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_BLACK, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_platform_blue_middle"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 34), BiomeType.KEEN5_BLACK, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_single_platform_blue"
                     , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 36), BiomeType.KEEN5_BLACK })
                },
                #endregion
                #region keen 5 green
                {
                    "keen5_green_wall_to_platform_left"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_GREEN, Direction.LEFT })
                },
                {
                    "keen5_floor_green_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN5_GREEN, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_floor_green_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_GREEN, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_floor_green_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN5_GREEN, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_wall_green_bottom"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_GREEN, TileType.CEILING })
                },
                {
                    "keen5_wall_green_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_GREEN, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen5_wall_green_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_GREEN, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen5_wall_green_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_GREEN, TileType.WALL_MIDDLE })
                },
                {
                    "keen5_ceiling_green"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN5_GREEN, TileType.CEILING })
                },
                 {
                    "keen5_platform_green_edge_left"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_GREEN, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_platform_green_edge_right"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_GREEN, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_platform_green_middle"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 34), BiomeType.KEEN5_GREEN, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_single_platform_green"
                     , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 36), BiomeType.KEEN5_GREEN })
                },
                #endregion

                #region keen5 red
                   {
                    "keen5_red_wall_to_platform_left"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction"},
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_RED, Direction.LEFT })
                },
                {
                    "keen5_floor_red_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN5_RED, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_floor_red_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_RED, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_floor_red_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN5_RED, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_wall_red_bottom"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN5_RED, TileType.CEILING })
                },
                {
                    "keen5_wall_red_edge_left"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_RED, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen5_wall_red_edge_right"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN5_RED, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen5_wall_red_middle"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN5_RED, TileType.WALL_MIDDLE })
                },
                {
                    "keen5_ceiling_red"
                     , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN5_RED, TileType.CEILING })
                },
                 {
                    "keen5_platform_red_edge_left"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_RED, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen5_platform_red_edge_right"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 34), BiomeType.KEEN5_RED, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen5_platform_red_middle"
                     , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 34), BiomeType.KEEN5_RED, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen5_single_platform_red"
                     , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 36), BiomeType.KEEN5_RED })
                },
                #endregion

                #region weapons
                {
                    "neural_stunner1"
                     , new TypeConstructorValues(typeof(NeuralStunnerAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 22), 5 })
                },
                 {
                    "neural_stunner_smg_1"
                     , new TypeConstructorValues(typeof(SMGNeuralStunnerAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 22), 30 })
                },
                   {
                    "neural_stunner_shotgun"
                     , new TypeConstructorValues(typeof(ShotgunNeuralStunnerAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 53, 22), 50 })
                },
                 {
                    "neural_stunner_rocket_launcher1"
                     , new TypeConstructorValues(typeof(RPGNeuralStunnerAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 56, 26), 10 })
                },
                   {
                    "neural_stunner_railgun1"
                     , new TypeConstructorValues(typeof(RailgunNeuralStunnerAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 22), 5 })
                },
                 {
                    "keen_dreams_boobus_bomb2"
                     , new TypeConstructorValues(typeof(BoobusBombLauncherAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 30), 10 })
                },
                {
                    "BFG1"
                   , new TypeConstructorValues(typeof(BFGAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 61, 24), 1 })
                },
                {
                    "snake_gun1"
                   , new TypeConstructorValues(typeof(SnakeGunAmmo),
                        new object[] { "grid", "hitbox", "ammo" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 57, 20), 10 })
                },
                #endregion

                #region keen4 points
                {
                    "keen4_candy_bar1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 32), PointItemType.KEEN4_SHIKKERS_CANDY_BAR })
                },
                {
                    "keen4_doughnut1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 30), PointItemType.KEEN4_DOUGHNUT })
                },
                  {
                    "keen4_icecream_cone1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 20, 32), PointItemType.KEEN4_ICECREAM_CONE })
                },
                {
                    "keen4_jawbreaker1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 30), PointItemType.KEEN4_JAWBREAKER })
                },
                  {
                    "keen4_shikadi_soda1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 18, 32), PointItemType.KEEN4_SHIKADI_SODA })
                },
                {
                    "keen4_three_tooth_gum1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32), PointItemType.KEEN4_THREE_TOOTH_GUM })
                },
                #endregion

                #region keen 5 points
                {
                    "keen5_bag_o_sugar1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 32), PointItemType.KEEN5_BAG_O_SUGAR })
                },
                {
                    "keen5_chocolate_milk1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 24, 32), PointItemType.KEEN5_CHOCOLATE_MILK })
                },
                  {
                    "keen5_marshmallow1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 24, 24), PointItemType.KEEN5_MARSHMALLOW })
                },
                {
                    "keen5_shikadi_gum1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32), PointItemType.KEEN5_SHIKADI_GUM })
                },
                  {
                    "keen5_sugar_stoopies_cereal1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 22), PointItemType.KEEN5_SUGAR_STOOPIES_CEREAL })
                },
                {
                    "keen5_tart_stix1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 30), PointItemType.KEEN5_TART_STIX })
                },
                #endregion

                #region keen 6 points
                {
                    "keen6_banana_split1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 26), PointItemType.KEEN6_BANANA_SPLIT })
                },
                {
                    "keen6_bloog_soda1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 18, 32), PointItemType.KEEN6_BLOOG_SODA })
                },
                {
                    "keen6_ice_cream_bar1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 18, 32), PointItemType.KEEN6_ICE_CREAM_BAR })
                },
                {
                    "keen6_pizza_slice1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32), PointItemType.KEEN6_PIZZA_SLICE })
                },
                {
                    "keen6_pudding1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 32), PointItemType.KEEN6_PUDDING })
                },
                {
                    "keen6_root_beer_float1"
                     , new TypeConstructorValues(typeof(PointItem),
                        new object[] { "grid", "hitbox", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 24, 32), PointItemType.KEEN6_ROOT_BEER_FLOAT })
                },
                #endregion
                //enemies
                //keen 4
                {
                    "keen4_arachnut1"
                     , new TypeConstructorValues(typeof(Arachnut),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 76, 80), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_berkeloid_move_left4"
                     , new TypeConstructorValues(typeof(Berkeloid),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 84), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_berkeloid_move_right4"
                     , new TypeConstructorValues(typeof(Berkeloid),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 84), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_blue_eagle_egg"
                     , new TypeConstructorValues(typeof(BlueEagleEgg),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 38, 44), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_bounder1"
                     , new TypeConstructorValues(typeof(Bounder),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 46), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                 {
                    "keen4_dopefish_move_left1"
                     , new TypeConstructorValues(typeof(Dopefish),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 172, 128), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_dopefish_move_right1"
                     , new TypeConstructorValues(typeof(Dopefish),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 172, 126), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_gnosticene_ancient_jump_left1"
                     , new TypeConstructorValues(typeof(GnosticeneAncient),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 90, 72), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_gnosticene_ancient_jump_right2"
                     , new TypeConstructorValues(typeof(GnosticeneAncient),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 94, 76), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT) })
                },
                {
                    "keen4_slug_move_left2"
                     , new TypeConstructorValues(typeof(PoisonSlug),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 44) })
                },
                {
                    "keen4_slug_move_right2"
                     , new TypeConstructorValues(typeof(PoisonSlug),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 44) })
                },
                {
                    "keen4_lick_left_fall"
                     , new TypeConstructorValues(typeof(Lick),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 36, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_lick_right_jump2"
                     , new TypeConstructorValues(typeof(Lick),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 34, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_mad_mushroom_left1"
                     , new TypeConstructorValues(typeof(MadMushroom),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 62), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_mad_mushroom_right1"
                     , new TypeConstructorValues(typeof(MadMushroom),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 62), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_wormmouth_move"
                     , new TypeConstructorValues(typeof(Wormmouth),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 16, 10), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_schoolfish_left1"
                     , new TypeConstructorValues(typeof(Schoolfish),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 16), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                 {
                    "keen4_inchworm_right1"
                     , new TypeConstructorValues(typeof(Inchworm),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 16), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                 {
                    "keen4_skypest_fly_left1"
                     , new TypeConstructorValues(typeof(SkyPest),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                 {
                    "keen4_skypest_fly_right1"
                     , new TypeConstructorValues(typeof(SkyPest),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_sprite_look_right"
                     , new TypeConstructorValues(typeof(Keen4Sprite),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 48), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_mimrock_wait"
                     , new TypeConstructorValues(typeof(Mimrock),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 40), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen4_thundercloud_dormant"
                     , new TypeConstructorValues(typeof(ThunderCloud),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 128, 48) })
                },
                //keen 5 enemies
                {
                    "keen5_korath_inhabitant_walk_left3"
                     , new TypeConstructorValues(typeof(KorathInhabitant),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 46), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_korath_inhabitant_walk_right1"
                     , new TypeConstructorValues(typeof(KorathInhabitant),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 44), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_little_ampton_look"
                     , new TypeConstructorValues(typeof(LittleAmpton),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 48), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_robo_red_left"
                     , new TypeConstructorValues(typeof(RoboRed),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 118, 128), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_robo_red_right"
                     , new TypeConstructorValues(typeof(RoboRed),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 128, 128), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_shelley_left2"
                     , new TypeConstructorValues(typeof(Shelley),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 22, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_shikadi_master_look1"
                     , new TypeConstructorValues(typeof(ShikadiMaster),
                        new object[] { "grid", "hitbox", "keen", "teleportBounds" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 62, 80), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), new Rectangle(0,0,1500,1500)  })
                },
                {
                    "keen5_shikadi_mine"
                     , new TypeConstructorValues(typeof(ShikadiMine),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 68, 52), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_shockshund_look1"
                     , new TypeConstructorValues(typeof(Shockshund),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 36), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_sparky_left4"
                     , new TypeConstructorValues(typeof(Sparky),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 60), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_sparky_right2"
                     , new TypeConstructorValues(typeof(Sparky),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 60), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_sphereful1"
                     , new TypeConstructorValues(typeof(Sphereful),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 62, 62), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_spindred1"
                     , new TypeConstructorValues(typeof(Spindred),
                        new object[] { "grid", "hitbox", "keen",  "direction" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 48), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Direction.DOWN  })
                },
                {
                    "keen5_spirogrip_rotate5"
                     , new TypeConstructorValues(typeof(Spirogrip),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 62), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_standard_shikadi_look4"
                     , new TypeConstructorValues(typeof(Shikadi),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 60, 64), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                 {
                    "keen5_volte-face1"
                     , new TypeConstructorValues(typeof(VolteFace),
                        new object[] { "grid", "hitbox", "locationNodes", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 62, 48), new List<Point>() { new Point(0,0)  }, new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_slicestar"
                     , new TypeConstructorValues(typeof(DiagonalSlicestar),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 60), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen5_horizontal_slicestar"
                     , new TypeConstructorValues(typeof(HorizontalSlicestar),
                        new object[] { "grid", "hitbox", "direction", "startPoint", "endPoint", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 60), Direction.LEFT, new Point(0,0), new Point(0,0), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                 {
                    "keen5_vertical_slicestar"
                     , new TypeConstructorValues(typeof(VerticalSlicestar),
                        new object[] { "grid", "hitbox", "direction", "startPoint", "endPoint", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 58, 60), Direction.DOWN, new Point(0,0), new Point(0,0), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                #region keen 5 hazards
                {
                    "keen5_spinning_fire_hazard1"
                     , new TypeConstructorValues(typeof(Keen5SpinningFire),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32) })
                },
                {
                    "keen5_laser_field_down"
                     , new TypeConstructorValues(typeof(Keen5LaserField),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32),new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), LaserFieldState.OFF })
                },
                {
                    "keen5_laser_field_up"
                     , new TypeConstructorValues(typeof(Keen5LaserField),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32),new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), LaserFieldState.OFF })
                },
                {
                    "keen5_laser_turret_down"
                     , new TypeConstructorValues(typeof(LaserTurret),
                        new object[] { "grid", "hitbox", "direction", "IsActive", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 66), Direction.DOWN, true, TurretType.KEEN5 })
                },
                {
                    "keen5_laser_turret_up"
                     , new TypeConstructorValues(typeof(LaserTurret),
                        new object[] { "grid", "hitbox", "direction", "IsActive", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), Direction.UP, true, TurretType.KEEN5 })
                },
                {
                    "keen5_laser_turret_left"
                     , new TypeConstructorValues(typeof(LaserTurret),
                        new object[] { "grid", "hitbox", "direction", "IsActive", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 56, 32), Direction.LEFT, true, TurretType.KEEN5 })
                },
                {
                    "keen5_laser_turret_right"
                     , new TypeConstructorValues(typeof(LaserTurret),
                        new object[] { "grid", "hitbox", "direction", "IsActive", "type" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), Direction.RIGHT, true, TurretType.KEEN5 })
                },
                {
                    "keen5_spinning_burn_platform1"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 58), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 1 })
                },
                {
                    "keen5_spinning_burn_platform2"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 52), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 2 })
                },
                {
                    "keen5_spinning_burn_platform3"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 3 })
                },
                {
                    "keen5_spinning_burn_platform4"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 52), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 4 })
                },
                {
                    "keen5_spinning_burn_platform5"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 58), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 5 })
                },
                {
                    "keen5_spinning_burn_platform6"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 47), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 6 })
                },
                {
                    "keen5_spinning_burn_platform7"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 7 })
                },
                {
                    "keen5_spinning_burn_platform8"
                     , new TypeConstructorValues(typeof(Keen5SpinningBurnPlatform),
                        new object[] { "grid", "hitbox", "keen", "hasBurner", "startIndex" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 28, 48), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), true, 8 })
                },
                #endregion

                #region keen 6 hazards
                {
                    "keen6_burn_hazard3"
                     , new TypeConstructorValues(typeof(Keen6BurnHazard),
                        new object[] { "grid", "hitbox" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32) })
                },
                {
                    "keen6_dome_spikes"
                     , new TypeConstructorValues(typeof(Hazard),
                        new object[] { "grid", "hitbox", "hazardType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 56, 54), HazardType.KEEN6_SPIKE })
                },
                {
                    "keen6_flame_thrower_off"
                     , new TypeConstructorValues(typeof(FlameThrower),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 48),  new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), FlameThrowerState.OFF })
                },
                {
                    "keen6_flame_thrower_on1"
                     , new TypeConstructorValues(typeof(FlameThrower),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 82),  new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), FlameThrowerState.ON_PHASE1 })
                },
                {
                    "keen6_flame_thrower_on2"
                     , new TypeConstructorValues(typeof(FlameThrower),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 30, 96),  new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), FlameThrowerState.ON_PHASE2 })
                },
                 {
                    "keen6_flame_thrower_on3"
                     , new TypeConstructorValues(typeof(FlameThrower),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 26, 128),  new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), FlameThrowerState.ON_PHASE3 })
                },
                {
                    "keen6_laser_field_bottom"
                     , new TypeConstructorValues(typeof(Keen6LaserField),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32),new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), LaserFieldState.OFF })
                },
                {
                    "keen6_laser_field_top"
                     , new TypeConstructorValues(typeof(Keen6LaserField),
                        new object[] { "grid", "hitbox", "keen", "initialState" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 32),new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), LaserFieldState.OFF })
                },
                #endregion
                //keen 6 enemies
                {
                    "keen6_babobba_land_left"
                     , new TypeConstructorValues(typeof(Babobba),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 38, 44), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_babobba_land_right"
                     , new TypeConstructorValues(typeof(Babobba),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 40, 44), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bip_ship_left"
                     , new TypeConstructorValues(typeof(BipShip),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 62, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bip_ship_right"
                     , new TypeConstructorValues(typeof(BipShip),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bloog_left2"
                     , new TypeConstructorValues(typeof(Bloog),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 92, 88), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bloog_right2"
                     , new TypeConstructorValues(typeof(Bloog),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 92, 88), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_blooglet_blue_left1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Blue, false  })
                },
                {
                    "keen6_blooglet_blue_right1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Blue, false  })
                },
                {
                    "keen6_blooglet_green_left1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Green, false  })
                },
                {
                    "keen6_blooglet_green_right1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Green, false  })
                },
                {
                    "keen6_blooglet_red_left1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Red, false  })
                },
                {
                    "keen6_blooglet_red_right1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Red, false  })
                },
                {
                    "keen6_blooglet_yellow_left1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Yellow, false  })
                },
                {
                    "keen6_blooglet_yellow_right1"
                     , new TypeConstructorValues(typeof(Blooglet),
                        new object[] { "grid", "hitbox", "keen", "color", "holdsItem" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 46, 42), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT), Color.Yellow, false  })
                },
                {
                    "keen6_blooguard_left1"
                     , new TypeConstructorValues(typeof(Blooguard),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 94, 90), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_blooguard_right4"
                     , new TypeConstructorValues(typeof(Blooguard),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 96, 98), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_blorb1"
                     , new TypeConstructorValues(typeof(Blorb),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 62, 60), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bobba_land_left"
                     , new TypeConstructorValues(typeof(Bobba),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 78, 86), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_bobba_land_right"
                     , new TypeConstructorValues(typeof(Bobba),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 78, 86), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_ceilick_tongue_waiting"
                     , new TypeConstructorValues(typeof(Ceilick),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 10, 10), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_flect_look"
                     , new TypeConstructorValues(typeof(Flect),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 44, 60), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_fleex_look1"
                     , new TypeConstructorValues(typeof(Fleex),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 86, 108), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_gix_left1"
                     , new TypeConstructorValues(typeof(Gik),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 30), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_gix_right3"
                     , new TypeConstructorValues(typeof(Gik),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 28), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_nospike_look"
                     , new TypeConstructorValues(typeof(Nospike),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 66, 74), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_orbatrix_look_left2"
                     , new TypeConstructorValues(typeof(Orbatrix),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },
                {
                    "keen6_orbatrix_look_right1"
                     , new TypeConstructorValues(typeof(Orbatrix),
                        new object[] { "grid", "hitbox", "keen" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 32), new CommanderKeen(new SpaceHashGrid(1500, 1500, 150,150), new Rectangle(0,0,28,64), Direction.RIGHT)  })
                },

                //keen 6 tiles
                #region Keen 6 Dome
                {
                    "keen6_dome_ceiling1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_DOME, TileType.CEILING })
                },
                 {
                    "keen6_dome_ceiling2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_DOME, TileType.CEILING })
                },
                {
                    "keen6_dome_ceiling3"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_DOME, TileType.CEILING })
                },
                {
                    "keen6_dome_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN6_DOME, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen6_dome_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_DOME, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen6_dome_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN6_DOME, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen6_dome_platform_edge_left"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN6_DOME, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen6_dome_platform_edge_right"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN6_DOME, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen6_dome_platform_middle"
                      , new TypeConstructorValues(typeof(MaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN6_DOME, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen6_dome_platform_single"
                      , new TypeConstructorValues(typeof(SingleMaskedPlatformTile),
                        new object[] { "grid", "hitbox", "biome" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 40), BiomeType.KEEN6_DOME })
                },
                {
                    "keen6_dome_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_DOME, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen6_dome_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_DOME, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen6_dome_wall_middle1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_DOME, TileType.WALL_MIDDLE })
                },
                {
                    "keen6_dome_wall_middle2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_DOME, TileType.WALL_MIDDLE })
                },
                {
                    "keen6_dome_wall_to_platform"
                      , new TypeConstructorValues(typeof(WallToPlatformTile),
                        new object[] { "grid", "hitbox", "biome", "direction" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 32, 64), BiomeType.KEEN6_DOME, Direction.LEFT })
                },
                #endregion

                #region Keen 6 Forest
                {
                    "keen6_forest_ceiling1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_FOREST, TileType.CEILING })
                },
                {
                    "keen6_forest_ceiling2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_FOREST, TileType.CEILING })
                },
                {
                    "keen6_forest_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN6_FOREST, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen6_forest_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_FOREST, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen6_forest_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN6_FOREST, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen6_forest_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_FOREST, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen6_forest_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_FOREST, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen6_forest_wall_middle1"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_FOREST, TileType.WALL_MIDDLE })
                },
                {
                    "keen6_forest_wall_middle2"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_FOREST, TileType.WALL_MIDDLE })
                },
                #endregion

                #region Keen 6 Industrial
                 {
                    "keen6_industrial_ceiling"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 32), BiomeType.KEEN6_INDUSTRIAL, TileType.CEILING })
                },
                {
                    "keen6_industrial_floor_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 72, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.FLOOR_LEFT_EDGE })
                },
                {
                    "keen6_industrial_floor_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.FLOOR_RIGHT_EDGE })
                },
                {
                    "keen6_industrial_floor_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 48, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.FLOOR_MIDDLE })
                },
                {
                    "keen6_industrial_wall_edge_left"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.WALL_LEFT_EDGE })
                },
                {
                    "keen6_industrial_wall_edge_right"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 52, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.WALL_RIGHT_EDGE })
                },
                {
                    "keen6_industrial_wall_middle"
                      , new TypeConstructorValues(typeof(BiomeTile),
                        new object[] { "grid", "hitbox", "biome", "floorType" },
                        new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0, 0, 64, 64), BiomeType.KEEN6_INDUSTRIAL, TileType.WALL_MIDDLE })
                },
                #endregion

                //keen 4 extra life items
                {
                    "keen4_lifewater_flask1"
                    , new TypeConstructorValues(typeof(ExtraLife)
                    , new object[] { "grid", "hitbox", "type" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 22,32), ExtraLifeType.KEEN4_LIFEWATER_FLASK })
                },
                {
                    "keen4_drop1"
                    , new TypeConstructorValues(typeof(RainDrop)
                    , new object[] { "grid", "hitbox" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 20,32) })
                },

                //keen 5 extra life items
                {
                    "keen5_keg_o_vitalin1"
                    , new TypeConstructorValues(typeof(ExtraLife)
                    , new object[] { "grid", "hitbox", "type" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 64,50), ExtraLifeType.KEEN5_KEG_O_VITALIN })
                },
                {
                    "keen5_vitalin1"
                    , new TypeConstructorValues(typeof(Vitalin)
                    , new object[] { "grid", "hitbox" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 18,32) })
                },

                //keen 6 extra life items
                {
                    "keen6_viva_queen2"
                    , new TypeConstructorValues(typeof(ExtraLife)
                    , new object[] { "grid", "hitbox", "type" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 30,30), ExtraLifeType.KEEN6_VIVA_QUEEN })
                },
                {
                    "keen6_viva_flying1"
                    , new TypeConstructorValues(typeof(Viva)
                    , new object[] { "grid", "hitbox", "perch" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 30,30), false })
                },
                {
                    "keen6_viva_perched1"
                    , new TypeConstructorValues(typeof(Viva)
                    , new object[] { "grid", "hitbox", "perch" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 30,30), true })
                },
                #endregion

                #region Gems
                {
                    "gem_blue1"
                    , new TypeConstructorValues(typeof(Gem)
                    , new object[] { "grid", "hitbox", "gemColor", "hasGravity" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 26,20), GemColor.BLUE, false })
                },
                {
                    "gem_red1"
                    , new TypeConstructorValues(typeof(Gem)
                    , new object[] { "grid", "hitbox", "gemColor", "hasGravity" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 26,20), GemColor.RED, false })
                },
                {
                    "gem_yellow1"
                    , new TypeConstructorValues(typeof(Gem)
                    , new object[] { "grid", "hitbox", "gemColor", "hasGravity" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 26,20), GemColor.YELLOW, false })
                },
                {
                    "gem_green1"
                    , new TypeConstructorValues(typeof(Gem)
                    , new object[] { "grid", "hitbox", "gemColor", "hasGravity" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 26,20), GemColor.GREEN, false })
                },

                #endregion

                #region Keen 4 Hazards

                {
                    "keen 4 spikes"
                    , new TypeConstructorValues(typeof(Hazard)
                    , new object[] { "grid", "hitbox", "hazardType" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 63,70), HazardType.KEEN4_SPIKE })
                },
                {
                    "keen4_dart_gun_down"
                    , new TypeConstructorValues(typeof(DartGun)
                    , new object[] { "grid", "hitbox", "direction", "isFiring" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 30,24), Direction.DOWN, false })
                },
                {
                    "keen4_dart_gun_left"
                    , new TypeConstructorValues(typeof(DartGun)
                    , new object[] { "grid", "hitbox", "direction", "isFiring" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 26,32), Direction.LEFT, false })
                },
                {
                    "keen4_dart_gun_right"
                    , new TypeConstructorValues(typeof(DartGun)
                    , new object[] { "grid", "hitbox", "direction", "isFiring" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 36,32), Direction.RIGHT, false })
                },
                {
                    "keen4_dart_gun_up"
                    , new TypeConstructorValues(typeof(DartGun)
                    , new object[] { "grid", "hitbox", "direction", "isFiring" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 32,36), Direction.UP, false })
                },
                {
                    "keen4_fire_left1"
                    , new TypeConstructorValues(typeof(Fire)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 32,42), Direction.LEFT })
                },
                {
                    "keen4_fire_right1"
                    , new TypeConstructorValues(typeof(Fire)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 31,42), Direction.RIGHT })
                },
                {
                    "keen4_mine"
                    , new TypeConstructorValues(typeof(Mine)
                    , new object[] { "grid", "hitbox", "initialDirection", "bounds" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 46,46), Direction.RIGHT, new Rectangle(0,0,0,0) })
                },
                {
                    "keen4_poison_pool1"
                    , new TypeConstructorValues(typeof(PoisonPool)
                    , new object[] { "grid", "hitbox", "lengths" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 174,86), 1 })
                },
                {
                    "keen4_rocket_propelled_platform1"
                    , new TypeConstructorValues(typeof(RocketPropelledPlatform)
                    , new object[] { "grid", "hitbox", "keen" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 32,32), new CommanderKeen(new SpaceHashGrid(1500,1500,150,150), new Rectangle(0,0,32,64), Direction.RIGHT) })
                },
                {
                    "keen4_spear_wait_down"
                    , new TypeConstructorValues(typeof(Spear)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 20,10), Direction.DOWN })
                },
                {
                    "keen4_spear_wait_left"
                    , new TypeConstructorValues(typeof(Spear)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 8,20), Direction.LEFT })
                },
                {
                    "keen4_spear_wait_right"
                    , new TypeConstructorValues(typeof(Spear)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 8,16), Direction.RIGHT })
                },
                {
                    "keen4_spear_wait_up"
                    , new TypeConstructorValues(typeof(Spear)
                    , new object[] { "grid", "hitbox", "direction" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 16,10), Direction.UP })
                },
                {
                    "keen4_tar1"
                    , new TypeConstructorValues(typeof(TarPit)
                    , new object[] { "grid", "hitbox", "lengths", "depths" }
                    , new object[] { new SpaceHashGrid(1500, 1500, 150, 150), new Rectangle(0,0, 64,62), 1, 1 })
                },
                #endregion
            };
        }

        public static ImageToObjectCreationFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImageToObjectCreationFactory();
                }
                return _instance;
            }
        }

        public bool TryGetTypeItem(string key, out TypeConstructorValues val)
        {
            if (_typeDict == null)
            {
                val = null;
                return false;
            }
            return _typeDict.TryGetValue(key, out val);
        }
    }
}
