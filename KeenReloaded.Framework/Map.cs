using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System.IO;
using System.Diagnostics;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Enemies;
using System.Drawing;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework.Hazards;
using System.Reflection;
using System.Windows.Forms;
using KeenReloaded.Framework.AltCharacters;
using KeenReloaded.Framework.Utilities;

namespace KeenReloaded.Framework
{
    public class Map
    {
        private const int SPACE_HASH_WIDTH = 150, SPACE_HASH_HEIGHT = 150;
        private const char PROPERTY_SEPARATOR = '|';
        private const string KEEN_NOT_FOUND_TEXT = "Commander keen must be declared before any enemies can be declared.  Consider initializing keen at the beginning of the map file.";
        private const char LIST_BEGIN_CHAR = '[', LIST_END_CHAR = ']';
        private const char LIST_ITEM_SEPARATOR = ',';

        public Map(SpaceHashGrid grid, List<CollisionObject> tiles, List<IUpdatable> movingObjects, List<ISprite> masks, GameModeEnum gameMode)
        {
            if (grid == null)
                throw new ArgumentNullException("no collision detection grid found");


            _grid = grid;
            this.Size = grid.Size;
            this.Tiles = tiles;
            this.Objects = movingObjects;
            this.Masks = masks;
        }

        public SpaceHashGrid Grid
        {
            get
            {
                return _grid;
            }
        }

        public Size Size { get; private set; }

        public List<CollisionObject> Tiles { get; private set; }

        public List<IUpdatable> Objects { get; private set; }
        public List<ISprite> Masks { get; private set; }
        public GameModeEnum GameMode { get; private set; }
        private static CommanderKeen _keen;
        private static string _selectedCharacter;
        private static Dictionary<long, Door> _doors;
        private static Dictionary<int, int> _missedDoorMappings = new Dictionary<int, int>();
        private static Dictionary<string, IActivator> _missedActivateables = new Dictionary<string, IActivator>();
        private static Dictionary<string, IActivateable> _activateables;
        SpaceHashGrid _grid;

        public void SetKeen(CommanderKeen keen)
        {
            _keen = keen;
        }

        public static Map LoadMap(string mapName, GameModeEnum gameMode, string selectedCharacter)
        {
            try
            {
                string fullPath = string.Empty;
                _selectedCharacter = selectedCharacter;
                List<CollisionObject> tiles = new List<CollisionObject>();
                List<IUpdatable> objects = new List<IUpdatable>();
                _activateables = new Dictionary<string, IActivateable>();
                _doors = new Dictionary<long, Door>();
                List<ISprite> masks = new List<ISprite>();

                switch (gameMode)
                {
                    case GameModeEnum.NORMAL:
                        fullPath = Environment.CurrentDirectory + @"\SavedNormalMaps\";
                        break;
                    case GameModeEnum.ZOMBIE:
                        fullPath = Environment.CurrentDirectory + @"\SavedZombieMaps\";
                        break;
                    case GameModeEnum.KING_OF_THE_HILL:
                        fullPath = Environment.CurrentDirectory + @"\SavedKingOfTheHillMaps\";
                        break;
                    case GameModeEnum.CAPTURE_THE_FLAG:
                        fullPath = Environment.CurrentDirectory + @"\SavedCTFMaps\";
                        break;
                }

                fullPath += mapName;
                SpaceHashGrid grid;
                using (FileStream fs = File.OpenRead(fullPath))
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    string widthHeightText = streamReader.ReadLine();
                    string[] dimensionValues = widthHeightText.Split(PROPERTY_SEPARATOR);
                    int width = Convert.ToInt32(dimensionValues[0]);
                    int height = Convert.ToInt32(dimensionValues[1]);
                    grid = new SpaceHashGrid(width, height, SPACE_HASH_WIDTH, SPACE_HASH_HEIGHT);
                    while (!streamReader.EndOfStream)
                    {
                        string objText = streamReader.ReadLine();
                        object obj = LoadObjectByType(objText, grid);
                        if (obj is IUpdatable)
                        {
                            objects.Add((IUpdatable)obj);
                        }
                        else if (obj is CollisionObject)
                        {
                            tiles.Add((CollisionObject)obj);
                        }
                        else if (obj is ISprite)
                        {
                            masks.Add((ISprite)obj);
                        }
                    }
                    HandleMissedDoorMappings(tiles);
                    List<IActivateable> activateables = new List<IActivateable>();
                    if (tiles.OfType<IActivateable>().Any())
                    {
                        activateables.AddRange(tiles.OfType<IActivateable>());
                    }
                    if (objects.OfType<IActivateable>().Any())
                    {
                        activateables.AddRange(objects.OfType<IActivateable>());
                    }
                    HandleMissedActivateables(activateables);
                }

                Map m = new Map(grid, tiles, objects, masks, gameMode);
                return m;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        private static void HandleMissedDoorMappings(List<CollisionObject> tiles)
        {
            foreach (var mapping in _missedDoorMappings)
            {
                var door = tiles.OfType<Door>().FirstOrDefault(d => d.Id == mapping.Key);
                if (door != null)
                {
                    var destinationDoor = tiles.OfType<Door>().FirstOrDefault(d => d.Id == mapping.Value);
                    door.DestinationDoor = destinationDoor;
                }
            }
            _missedDoorMappings.Clear();
        }

        private static void HandleMissedActivateables(IEnumerable<IActivateable> activateables)
        {
            if (activateables != null && activateables.Any() && _missedActivateables.Any())
            {
                foreach (var activateable in _missedActivateables)
                {
                    var item = activateables.FirstOrDefault(a => a.ActivationID.ToString() == activateable.Key);
                    if (item != null)
                    {
                        activateable.Value.ToggleObjects.Add(item);
                    }
                }
            }
            _missedActivateables.Clear();
        }

        public void Clear(CommanderKeenGame game)
        {
            foreach (var obj in this.Objects)
            {
                game.DetachEventsForObject(obj);
            }
            foreach (var node in _grid._nodes)
            {
                node.Objects.Clear();
                node.NonEnemies.Clear();
                node.Tiles.Clear();
            }
            this.Tiles.Clear();
            this.Objects.Clear();
            this.Masks.Clear();
        }

        private static object LoadObjectByType(string rawText, SpaceHashGrid grid)
        {
            string[] objProperties = rawText.Split(PROPERTY_SEPARATOR);
            string objType = objProperties[0];
            int x = Convert.ToInt32(objProperties[1]);
            int y = Convert.ToInt32(objProperties[2]);
            switch (objType)
            {
                case nameof(CommanderKeen):
                    Direction direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                  
                    string fullyQualifiedNamespace = Assembly.GetExecutingAssembly().GetName().Name;
                    if (_selectedCharacter != nameof(CommanderKeen))
                    {
                        fullyQualifiedNamespace += ".AltCharacters";
                    }
                    fullyQualifiedNamespace += ("." + _selectedCharacter);

                    _keen = (CommanderKeen)Activator.CreateInstance(Type.GetType(fullyQualifiedNamespace), grid, new Rectangle(x, y, 26, 64), direction);
                    return _keen;
                case nameof(HorizontalExtendedBiomeTile):

                    BiomeType bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[3]);
                    TileType tType = (TileType)Enum.Parse(typeof(TileType), objProperties[4]);
                    int lengths = Convert.ToInt32(objProperties[5]);
                    int width = HorizontalExtendedBiomeTile.GetWidthByTileTile(tType);
                    int height = tType == TileType.CEILING ? 32 : 64;
                    HorizontalExtendedBiomeTile tile = new HorizontalExtendedBiomeTile(grid, new System.Drawing.Rectangle(x, y, width, height), bType, tType, lengths);
                    return tile;
                case nameof(VerticalExtendedBiomeTile):

                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[3]);
                    tType = (TileType)Enum.Parse(typeof(TileType), objProperties[4]);
                    lengths = Convert.ToInt32(objProperties[5]);
                    width = HorizontalExtendedBiomeTile.GetWidthByTileTile(tType);
                    var vTile = new VerticalExtendedBiomeTile(grid, new System.Drawing.Rectangle(x, y, width, 64), bType, tType, lengths);
                    return vTile;
                case nameof(MiragePlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    MiragePlatformState initState = (MiragePlatformState)Enum.Parse(typeof(MiragePlatformState), objProperties[3]);
                    MiragePlatform miragePlatform = new MiragePlatform(grid, new Rectangle(x, y, 120, 64), _keen, initState);
                    return miragePlatform;
                case nameof(Pole):

                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[4]);
                    PoleType pType = (PoleType)Enum.Parse(typeof(PoleType), objProperties[3]);
                    lengths = Convert.ToInt32(objProperties[5]);
                    Pole p = new Pole(grid, new System.Drawing.Rectangle(x, y, 10, 31), pType, bType, lengths);
                    return p;
                case nameof(Keen6EyeBallPole):
                    lengths = Convert.ToInt32(objProperties[3]);
                    Keen6EyeBallPole eyeBallPole = new Keen6EyeBallPole(grid, new Rectangle(x, y, 10, 31), lengths);
                    return eyeBallPole;
                case nameof(Keen6ClawTile):
                    lengths = Convert.ToInt32(objProperties[3]);
                    Keen6ClawTile keen6ClawTile = new Keen6ClawTile(grid, new Rectangle(x, y, 124, 96), lengths);
                    return keen6ClawTile;
                case nameof(Arachnut):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    Arachnut arachnut = new Arachnut(grid, new System.Drawing.Rectangle(x, y, Arachnut.STAND_WIDTH, Arachnut.STAND_HEIGHT), _keen);
                    return arachnut;
                case nameof(Keen4Sprite):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var sprite = new Keen4Sprite(grid, new Rectangle(x, y, 46, 48), _keen);
                    return sprite;
                case nameof(MadMushroom):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var mushroom = new MadMushroom(grid, new Rectangle(x, y, 58, 62), _keen);
                    return mushroom;
                case nameof(PoisonSlug):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var slug = new PoisonSlug(grid, new Rectangle(x, y, 32, 48));
                    return slug;
                case nameof(Berkeloid):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var berkeloid = new Berkeloid(grid, new Rectangle(x, y, 62, 88), _keen);
                    return berkeloid;
                case nameof(Lick):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var lick = new Lick(grid, new Rectangle(x, y, 36, 32), _keen);
                    return lick;
                case nameof(Wormmouth):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var worm = new Wormmouth(grid, new Rectangle(x, y, 16, 10), _keen);
                    return worm;
                case nameof(SkyPest):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var pest = new SkyPest(grid, new Rectangle(x, y, 34, 14), _keen);
                    return pest;
                case nameof(Schoolfish):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var fish = new Schoolfish(grid, new Rectangle(x, y, 26, 16), _keen);
                    return fish;
                case nameof(Inchworm):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var iworm = new Inchworm(grid, new Rectangle(x, y, 48, 16), _keen);
                    return iworm;
                case nameof(BlueEagleEgg):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var egg = new BlueEagleEgg(grid, new Rectangle(x, y, 38, 44), _keen);
                    return egg;
                case nameof(Mimrock):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var mimrock = new Mimrock(grid, new Rectangle(x, y, 46, 40), _keen);
                    return mimrock;
                case nameof(GnosticeneAncient):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var ancient = new GnosticeneAncient(grid, new Rectangle(x, y, 80, 60), _keen);
                    return ancient;
                case nameof(Dopefish):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var dopefish = new Dopefish(grid, new Rectangle(x, y, 152, 128), _keen);
                    return dopefish;
                case nameof(Bounder):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var bounder = new Bounder(grid, new Rectangle(x, y, 46, 46), _keen);
                    return bounder;
                case nameof(Sparky):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var sparky = new Sparky(grid, new Rectangle(x, y, 58, 60), _keen);
                    return sparky;
                case nameof(LittleAmpton):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var littleAmpton = new LittleAmpton(grid, new Rectangle(x, y, 42, 46), _keen);
                    return littleAmpton;
                case nameof(HorizontalSlicestar):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    int minX = Convert.ToInt32(objProperties[4]);
                    int minY = Convert.ToInt32(objProperties[5]);
                    int maxX = Convert.ToInt32(objProperties[6]);
                    int maxY = Convert.ToInt32(objProperties[7]);


                    var horizontalSlicestar = new HorizontalSlicestar(grid, new Rectangle(x, y, 58, 60), direction, new Point(minX, minY), new Point(maxX, maxY), _keen);
                    return horizontalSlicestar;
                case nameof(VerticalSlicestar):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    minX = Convert.ToInt32(objProperties[4]);
                    minY = Convert.ToInt32(objProperties[5]);
                    maxX = Convert.ToInt32(objProperties[6]);
                    maxY = Convert.ToInt32(objProperties[7]);

                    var verticalSlicestar = new VerticalSlicestar(grid, new Rectangle(x, y, 58, 60), direction, new Point(minX, minY), new Point(maxX, maxY), _keen);
                    return verticalSlicestar;
                case nameof(DiagonalSlicestar):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var diagonalSlicestar = new DiagonalSlicestar(grid, new Rectangle(x, y, 58, 60), _keen);
                    return diagonalSlicestar;
                case nameof(VolteFace):

                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var locationPoints = GetListElements(rawText);
                    List<Point> locations = new List<Point>();

                    for (int i = 0; i < locationPoints.Count; i += 2)
                    {
                        int xNode = Convert.ToInt32(locationPoints[i]);
                        int yNode = Convert.ToInt32(locationPoints[i + 1]);
                        Point pNode = new Point(xNode, yNode);
                        locations.Add(pNode);
                    }

                    var volteFace = new VolteFace(grid, new Rectangle(x, y, 62, 48), locations, _keen);
                    return volteFace;
                case nameof(Sphereful):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var sphereful = new Sphereful(grid, new Rectangle(x, y, 62, 62), _keen);
                    return sphereful;
                case nameof(RoboRed):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var roboRed = new RoboRed(grid, new Rectangle(x, y, 122, 128), _keen);
                    return roboRed;
                case nameof(Shelley):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var shelley = new Shelley(grid, new Rectangle(x, y, 24, 30), _keen);
                    return shelley;
                case nameof(Spirogrip):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var spirogrip = new Spirogrip(grid, new Rectangle(x, y, 32, 62), _keen);
                    return spirogrip;
                case nameof(Spindred):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);

                    var spindred = new Spindred(grid, new Rectangle(x, y, 46, 38), _keen, direction);
                    return spindred;
                case nameof(ShikadiMine):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var shikadiMine = new ShikadiMine(grid, new Rectangle(x, y, 68, 52), _keen);
                    return shikadiMine;
                case nameof(Shockshund):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var shockshund = new Shockshund(grid, new Rectangle(x, y, 48, 34), _keen);
                    return shockshund;
                case nameof(Shikadi):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var shikadi = new Shikadi(grid, new Rectangle(x, y, 48, 62), _keen);
                    return shikadi;
                case nameof(ShikadiMaster):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var tBoundsDimensionValues = GetListElements(rawText);

                    var xBound = Convert.ToInt32(tBoundsDimensionValues[0]);
                    var yBound = Convert.ToInt32(tBoundsDimensionValues[1]);
                    var widthBound = Convert.ToInt32(tBoundsDimensionValues[2]);
                    var heightBound = Convert.ToInt32(tBoundsDimensionValues[3]);

                    Rectangle teleportBounds1 = new Rectangle(xBound, yBound, widthBound, heightBound);

                    var shikadiMaster = new ShikadiMaster(grid, new Rectangle(x, y, 94, 80), _keen, teleportBounds1);
                    return shikadiMaster;
                case nameof(KorathInhabitant):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var korathInhabitant = new KorathInhabitant(grid, new Rectangle(x, y, 44, 44), _keen);
                    return korathInhabitant;
                case nameof(Blooglet):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    var pi = typeof(Color).GetProperty(objProperties[3]);
                    var color = (Color)pi.GetValue(new Color(), null);
                    bool hasKey = Convert.ToBoolean(objProperties[4]);
                    var blooglet = new Blooglet(grid, new Rectangle(x, y, 44, 44), _keen, color, hasKey);
                    return blooglet;
                case nameof(Bloog):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var bloog = new Bloog(grid, new Rectangle(x, y, 80, 84), _keen);
                    return bloog;
                case nameof(Blooguard):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var blooguard = new Blooguard(grid, new Rectangle(x, y, 92, 100), _keen);
                    return blooguard;
                case nameof(Babobba):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var babobba = new Babobba(grid, new Rectangle(x, y, 36, 44), _keen);
                    return babobba;
                case nameof(Bobba):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var bobba = new Bobba(grid, new Rectangle(x, y, 65, 88), _keen);
                    return bobba;
                case nameof(Gik):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var gik = new Gik(grid, new Rectangle(x, y, 48, 30), _keen);
                    return gik;
                case nameof(Flect):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var flect = new Flect(grid, new Rectangle(x, y, 42, 60), _keen);
                    return flect;
                case nameof(Nospike):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var nospike = new Nospike(grid, new Rectangle(x, y, 66, 74), _keen);
                    return nospike;
                case nameof(Ceilick):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var ceilick = new Ceilick(grid, new Rectangle(x, y, 10, 10), _keen);
                    return ceilick;
                case nameof(Bip):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var bip = new Bip(grid, new Rectangle(x, y, Bip.WIDTH, Bip.HEIGHT), _keen);
                    return bip;
                case nameof(BipShip):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var bipShip = new BipShip(grid, new Rectangle(x, y, 64, 32), _keen);
                    return bipShip;
                case nameof(Blorb):

                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var blorb = new Blorb(grid, new Rectangle(x, y, 63, 58), _keen);
                    return blorb;
                case nameof(Orbatrix):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var orbatrix = new Orbatrix(grid, new Rectangle(x, y, 72, 32), _keen);

                    return orbatrix;
                case nameof(Fleex):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var fleex = new Fleex(grid, new Rectangle(x, y, 94, 106), _keen);
                    return fleex;
                case nameof(NeuralStunnerAmmo):
                    int ammo = Convert.ToInt32(objProperties[3]);
                    var neuralStunnerAmmo = new NeuralStunnerAmmo(grid, new Rectangle(x, y, 30, 22), ammo);
                    return neuralStunnerAmmo;
                case nameof(ShotgunNeuralStunnerAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var shotgunNeuralStunnerAmmo = new ShotgunNeuralStunnerAmmo(grid, new Rectangle(x, y, 53, 22), ammo);
                    return shotgunNeuralStunnerAmmo;
                case nameof(RailgunNeuralStunnerAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var railgunNeuralStunnerAmmo = new RailgunNeuralStunnerAmmo(grid, new Rectangle(x, y, 55, 22), ammo);
                    return railgunNeuralStunnerAmmo;
                case nameof(RPGNeuralStunnerAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var rPGNeuralStunnerAmmo = new RPGNeuralStunnerAmmo(grid, new Rectangle(x, y, 55, 26), ammo);
                    return rPGNeuralStunnerAmmo;
                case nameof(BoobusBombLauncherAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var boobusBombLauncherAmmo = new BoobusBombLauncherAmmo(grid, new Rectangle(x, y, 30, 30), ammo);
                    return boobusBombLauncherAmmo;
                case nameof(SMGNeuralStunnerAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var sMGNeuralStunnerAmmo = new SMGNeuralStunnerAmmo(grid, new Rectangle(x, y, 46, 22), ammo);
                    return sMGNeuralStunnerAmmo;
                case nameof(BFGAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var bfgAmmo = new BFGAmmo(grid, new Rectangle(x, y, 61, 24), ammo);
                    return bfgAmmo;
                case nameof(SnakeGunAmmo):
                    ammo = Convert.ToInt32(objProperties[3]);
                    var snakeGunAmmo = new SnakeGunAmmo(grid, new Rectangle(x, y, 57, 20), ammo);
                    return snakeGunAmmo;
                case nameof(CTFDestination):
                    var destinationColor = (GemColor)Enum.Parse(typeof(GemColor), objProperties[3]);
                    var dest = new CTFDestination(grid, new Rectangle(x, y, 130, 12), destinationColor);
                    return dest;
                case nameof(Flag):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var flagColor = (GemColor)Enum.Parse(typeof(GemColor), objProperties[3]);
                    int maxPoints = Convert.ToInt32(objProperties[4]);
                    int minPoints = Convert.ToInt32(objProperties[5]);
                    int pointsDegradation = Convert.ToInt32(objProperties[6]);
                    var flag = new Flag(grid, new Rectangle(x, y, 51, 64), flagColor, maxPoints, minPoints, pointsDegradation, _keen);
                    return flag;
                case nameof(EnemyFlag):
                    pointsDegradation = Convert.ToInt32(objProperties[3]);
                    var enemyFlag = new EnemyFlag(grid, new Rectangle(x, y, 51, 64), _keen, pointsDegradation);
                    return enemyFlag;
                case nameof(Gem):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var gemColor = (GemColor)Enum.Parse(typeof(GemColor), objProperties[3]);

                    var gem = new Gem(grid, new Rectangle(x, y, 30, 24), gemColor);
                    return gem;
                case nameof(Door):

                    DoorType doorType = (DoorType)Enum.Parse(typeof(DoorType), objProperties[3]);
                    int doorId = Convert.ToInt32(objProperties[4]);
                    Door destinationDoor = null;
                    if (int.TryParse(objProperties[5], out int destinationDoorId))
                    {
                        if (_doors.ContainsKey(destinationDoorId))
                        {
                            destinationDoor = _doors[destinationDoorId];
                        }
                        else
                        {
                            _missedDoorMappings.Add(doorId, destinationDoorId);
                        }
                    }

                    Door door = null;
                    switch (doorType)
                    {
                        case DoorType.KEEN4_ORACLE:
                            door = new Keen4OracleDoor(grid, new Rectangle(x, y, 126, 160), doorId, destinationDoor);
                            break;
                        case DoorType.KEEN4_BLUE:
                        case DoorType.KEEN4_GRAY:
                            door = new Door(grid, new Rectangle(x, y, 60, 92), doorType, doorId, destinationDoor);
                            break;
                        case DoorType.KEEN5_REGULAR:
                            door = new Door(grid, new Rectangle(x, y, 92, 94), doorType, doorId, destinationDoor);
                            break;
                        case DoorType.CHUTE:
                            door = new Door(grid, new Rectangle(x, y, 64, 64), doorType, doorId, null);
                            break;
                        case DoorType.KEEN6:
                            door = new Door(grid, new Rectangle(x, y, 124, 126), doorType, doorId, destinationDoor);
                            break;
                    }

                    if (door != null && !_doors.ContainsKey(door.Id))
                        _doors.Add(door.Id, door);

                    return door;
                case nameof(PointItem):
                    PointItemType pointItemType = (PointItemType)Enum.Parse(typeof(PointItemType), objProperties[3]);

                    var pointItem = new PointItem(grid, new Rectangle(x, y, 20, 32), pointItemType);
                    return pointItem;
                case nameof(Shield):
                    var duration = Convert.ToInt32(objProperties[3]);
                    Shield shield = new Shield(grid, new Rectangle(x, y, 62, 97), duration, _keen);
                    return shield;
                case nameof(Hill):
                case nameof(RandomHill):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    //points
                    locationPoints = GetListElements(objProperties[3]);
                    locations = new List<Point>();

                    for (int i = 0; i < locationPoints.Count; i += 2)
                    {
                        int xNode = Convert.ToInt32(locationPoints[i]);
                        int yNode = Convert.ToInt32(locationPoints[i + 1]);
                        Point pNode = new Point(xNode, yNode);
                        locations.Add(pNode);
                    }
                    //hold time in seconds
                    int holdTime = Convert.ToInt32(objProperties[4]);
                    //spawnDelay in seconds
                    int spawnDelay = Convert.ToInt32(objProperties[5]);
                    //points per second
                    int pointsPerSecond = Convert.ToInt32(objProperties[6]);
                    //additional per monster
                    int addPerMonster = Convert.ToInt32(objProperties[7]);
                    if (objType == nameof(Hill))
                    {
                        Hill hill = new Hill(grid, new Rectangle(x, y, 324, 256), locations, holdTime, spawnDelay, pointsPerSecond, addPerMonster, _keen);
                        return hill;
                    }
                    else
                    {
                        RandomHill randomHill = new RandomHill(grid, new Rectangle(x, y, 324, 256), locations, holdTime, spawnDelay, pointsPerSecond, addPerMonster, _keen);
                        return randomHill;
                    }
                case nameof(ExtraLife):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    ExtraLifeType extraLifeType = (ExtraLifeType)Enum.Parse(typeof(ExtraLifeType), objProperties[3]);

                    var extraLife = new ExtraLife(grid, new Rectangle(x, y, 30, 26), extraLifeType);
                    return extraLife;
                case nameof(KeyCard):
                    var keyCard = new KeyCard(grid, new Rectangle(x, y, 24, 28));
                    return keyCard;
                case nameof(Hazard):

                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var hitboxWidth = Convert.ToInt32(objProperties[3]);
                    var hitboxHeight = Convert.ToInt32(objProperties[4]);

                    var hazardType = (HazardType)Enum.Parse(typeof(HazardType), objProperties[5]);

                    var hazard = new Hazard(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), hazardType);
                    return hazard;
                case nameof(Mine):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    xBound = Convert.ToInt32(objProperties[4]);
                    yBound = Convert.ToInt32(objProperties[5]);
                    widthBound = Convert.ToInt32(objProperties[6]);
                    heightBound = Convert.ToInt32(objProperties[7]);

                    var mine = new Mine(grid, new Rectangle(x, y, 46, 46), direction, new Rectangle(xBound, yBound, widthBound, heightBound));
                    return mine;
                case nameof(Fire):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);

                    var fire = new Fire(grid, new Rectangle(x, y, 32, 42), direction);
                    return fire;
                case nameof(DartGun):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    var dartGun = new DartGun(grid, new Rectangle(x, y, 32, 36), direction);
                    return dartGun;
                case nameof(Spear):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    var spear = new Spear(grid, new Rectangle(x, y, 8, 16), direction);
                    return spear;
                case nameof(PoisonPool):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    lengths = Convert.ToInt32(objProperties[3]);

                    var poisonPool = new PoisonPool(grid, new Rectangle(x, y, 174, 86), lengths);
                    return poisonPool;
                case nameof(Keen6SlimeHazard):

                    lengths = Convert.ToInt32(objProperties[3]);                   //these height and width values will be overridden
                    var slimeTile = new Keen6SlimeHazard(grid, new Rectangle(x, y, 64, 64), lengths);
                    return slimeTile;
                case nameof(ConveyerBelt):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    int conveyerBeltExtraLengths = Convert.ToInt32(objProperties[4]);

                    ConveyerBelt belt = new ConveyerBelt(grid, new Rectangle(x, y, 126, 96), _keen, direction, conveyerBeltExtraLengths);
                    return belt;
                case nameof(TarPit):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    lengths = Convert.ToInt32(objProperties[3]);
                    int depths = Convert.ToInt32(objProperties[4]);
                    var tarPit = new TarPit(grid, new Rectangle(x, y, 64, 62), lengths, depths);
                    return tarPit;
                case nameof(LaserTurret):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[3]);
                    bool isActive = Convert.ToBoolean(objProperties[4]);
                    TurretType turretType = (TurretType)Enum.Parse(typeof(TurretType), objProperties[5]);

                    var laserTurret = new LaserTurret(grid, new Rectangle(x, y, 64, 32), direction, isActive, turretType);
                    return laserTurret;
                case nameof(Keen6BurnHazard):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var keen6BurnHazard = new Keen6BurnHazard(grid, new Rectangle(x, y, 32, 32));
                    return keen6BurnHazard;
                case nameof(Keen6Drill):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var keen6Drill = new Keen6Drill(grid, new Rectangle(x, y, 32, 32));
                    return keen6Drill;
                case nameof(Keen6ElectricRodHazard):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var keen6Rods = new Keen6ElectricRodHazard(grid, new Rectangle(x, y, 128, 96));
                    return keen6Rods;
                case nameof(Keen5SpinningFire):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var keen5SpinningFire = new Keen5SpinningFire(grid, new Rectangle(x, y, 32, 32));
                    return keen5SpinningFire;
                case nameof(Keen5LaserField):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    LaserFieldState laserFieldState = (LaserFieldState)Enum.Parse(typeof(LaserFieldState), objProperties[5]);

                    var keen5LaserField = new Keen5LaserField(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, laserFieldState);
                    return keen5LaserField;
                case nameof(Keen6LaserField):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    laserFieldState = (LaserFieldState)Enum.Parse(typeof(LaserFieldState), objProperties[5]);
                    var keen6LaserField = new Keen6LaserField(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, laserFieldState);
                    return keen6LaserField;
                case nameof(ForceField):
                    int forceFieldHeight = Convert.ToInt32(objProperties[3]);
                    int forceFieldhealth = Convert.ToInt32(objProperties[4]);
                    int totalHeight = (ForceField.GENERATOR_HEIGHT * 2) + forceFieldHeight;
                    var forceField = new ForceField(grid, new Rectangle(x, y, ForceField.GENERATOR_WIDTH, totalHeight), forceFieldhealth);
                    return forceField;
                case nameof(RocketPropelledPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var rocketPropelledPlatform = new RocketPropelledPlatform(grid, new Rectangle(x, y, 32, 32), _keen);
                    return rocketPropelledPlatform;
                case nameof(FlameThrower):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    FlameThrowerState flameThrowerState = (FlameThrowerState)Enum.Parse(typeof(FlameThrowerState), objProperties[3]);
                    var flameThrower = new FlameThrower(grid, new Rectangle(x, y, 26, 48), _keen, flameThrowerState);
                    return flameThrower;
                case nameof(Smasher):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    SmasherState smasherState = (SmasherState)Enum.Parse(typeof(SmasherState), objProperties[3]);
                    var smasher = new Smasher(grid, new Rectangle(x, y, 32, 32), _keen, smasherState);
                    return smasher;
                case nameof(KeyGate):

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    Guid guid = new Guid(objProperties[5]);

                    var keyGate = new KeyGate(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), guid);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), keyGate);
                    }
                    return keyGate;
                case nameof(GemPlaceHolder):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);
                    gemColor = (GemColor)Enum.Parse(typeof(GemColor), objProperties[3]);

                    List<string> activationIds = GetListElements(objProperties[4]);
                    List<IActivateable> activationItems = new List<IActivateable>();
                    HashSet<string> missedIds = new HashSet<string>();
                    foreach (var id in activationIds)
                    {
                        if (_activateables.TryGetValue(id, out IActivateable item))
                        {
                            activationItems.Add(item);
                        }
                        else
                        {
                            missedIds.Add(id);
                        }
                    }
                    var gemPlaceHolder = new GemPlaceHolder(grid, new Rectangle(x, y, 26, 22), gemColor, activationItems);
                    foreach (var id in missedIds)
                    {
                        _missedActivateables.Add(id, gemPlaceHolder);
                    }
                    return gemPlaceHolder;
                case nameof(RemovablePlatform):

                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[3]);
                    bool initiallyActive = Convert.ToBoolean(objProperties[4]);
                    lengths = Convert.ToInt32(objProperties[5]);
                    guid = new Guid(objProperties[6]);

                    var removablePlatform = new RemovablePlatform(grid, new Rectangle(x, y, 32, 32), bType, initiallyActive, guid, lengths);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), removablePlatform);
                    }

                    return removablePlatform;
                case nameof(ToggleSwitch):

                    SwitchType switchType = (SwitchType)Enum.Parse(typeof(SwitchType), objProperties[3]);
                    initiallyActive = Convert.ToBoolean(objProperties[4]);
                    activationItems = new List<IActivateable>();
                    activationIds = GetListElements(objProperties[5]);
                    missedIds = new HashSet<string>();
                    foreach (var id in activationIds)
                    {
                        if (_activateables.TryGetValue(id, out IActivateable item))
                        {
                            activationItems.Add(item);
                        }
                        else
                        {
                            missedIds.Add(id);
                        }
                    }
                    var toggleSwitch = new ToggleSwitch(grid, new Rectangle(x, y, 32, 32), switchType, activationItems, initiallyActive);
                    foreach (var id in missedIds)
                    {
                        _missedActivateables.Add(id, toggleSwitch);
                    }
                    return toggleSwitch;
                case nameof(Keen6Switch):
                    activationItems = new List<IActivateable>();
                    activationIds = GetListElements(objProperties[3]);
                    foreach (var id in activationIds)
                    {
                        if (_activateables.TryGetValue(id, out IActivateable item))
                        {
                            activationItems.Add(item);
                        }
                    }
                    isActive = Convert.ToBoolean(objProperties[4]);
                    int addedPoleLengths = Convert.ToInt32(objProperties[5]);
                    Keen6Switch keen6Switch = new Keen6Switch(grid, new Rectangle(x, y, 90, 96), activationItems, isActive, addedPoleLengths);
                    return keen6Switch;
                case nameof(FlippingPlatform):

                    FlippingPlatformState flippingPlatformState = (FlippingPlatformState)Enum.Parse(typeof(FlippingPlatformState), objProperties[3]);
                    var flippingPlatform = new FlippingPlatform(grid, new Rectangle(x, y, 64, 26), FlippingPlatformState.STILL);
                    return flippingPlatform;
                case nameof(HorizonalMovingPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    PlatformType platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[6]);
                    int maxMoveDistance = Convert.ToInt32(objProperties[7]);
                    guid = new Guid(objProperties[8]);
                    initiallyActive = Convert.ToBoolean(objProperties[9]);

                    var horizonalMovingPlatform = new HorizonalMovingPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), platformType, _keen, direction, maxMoveDistance, guid, initiallyActive);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), horizonalMovingPlatform);
                    }
                    return horizonalMovingPlatform;
                case nameof(VerticalMovingPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[6]);
                    maxMoveDistance = Convert.ToInt32(objProperties[7]);
                    guid = new Guid(objProperties[8]);
                    initiallyActive = Convert.ToBoolean(objProperties[9]);

                    var verticalMovingPlatform = new VerticalMovingPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, platformType, direction, maxMoveDistance, initiallyActive, guid);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), verticalMovingPlatform);
                    }
                    return verticalMovingPlatform;
                case nameof(BottomOutPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    int bottomOutDistance = Convert.ToInt32(objProperties[6]);

                    var bottomOutPlatform = new BottomOutPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, platformType, bottomOutDistance);
                    return bottomOutPlatform;
                case nameof(StationaryPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    StationaryPlatform stationaryPlatform = new StationaryPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), platformType, _keen);
                    return stationaryPlatform;
                case nameof(DropPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    int maxDropDistance = Convert.ToInt32(objProperties[6]);
                    var activationGuid = new Guid(objProperties[7]);
                    DropPlatform drop = new DropPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), platformType, _keen, maxDropDistance, activationGuid);
                    return drop;
                case nameof(HorizontalTrickPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    HorizontalTrickPlatform horizontalTrickPlatform = new HorizontalTrickPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, platformType);
                    return horizontalTrickPlatform;
                case nameof(SetPathPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), objProperties[5]);
                    guid = new Guid(objProperties[6]);
                    List<Point> pathwayPoints = GetPathPoints(objProperties[7]);
                    initiallyActive = Convert.ToBoolean(objProperties[8]);
                    SetPathPlatform setPathPlatform = new SetPathPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), platformType, _keen, pathwayPoints, guid, initiallyActive);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), setPathPlatform);
                    }
                    return setPathPlatform;
                case nameof(Keen6SetPathPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    guid = new Guid(objProperties[5]);
                    pathwayPoints = GetPathPoints(objProperties[6]);
                    initiallyActive = Convert.ToBoolean(objProperties[7]);
                    Keen6SetPathPlatform keen6SetPathPlatform = new Keen6SetPathPlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), _keen, pathwayPoints, guid, initiallyActive);
                    if (!_activateables.ContainsKey(guid.ToString()))
                    {
                        _activateables.Add(guid.ToString(), keen6SetPathPlatform);
                    }
                    return keen6SetPathPlatform;
                case nameof(BiomeTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[5]);
                    tType = (TileType)Enum.Parse(typeof(TileType), objProperties[6]);

                    var biomeTile = new BiomeTile(grid, new System.Drawing.Rectangle(x, y, hitboxWidth, hitboxHeight), bType, tType);
                    return biomeTile;
                case nameof(MapEdgeTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    MapEdgeBehavior behavior = (MapEdgeBehavior)Enum.Parse(typeof(MapEdgeBehavior), objProperties[5]);
                    var mapEdgeTile = new MapEdgeTile(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), behavior);
                    return mapEdgeTile;
                case nameof(EnemySpawner):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    int maxConcurrentSpawn = Convert.ToInt32(objProperties[3]);
                    int spawnDelayTicks = Convert.ToInt32(objProperties[4]);
                    int health = Convert.ToInt32(objProperties[5]);
                    List<string> enemySpawnList = GetListElements(objProperties[6]);
                    var enemySpawner = new EnemySpawner(grid, new Rectangle(x, y, 92, 94), enemySpawnList, maxConcurrentSpawn, spawnDelayTicks, health, _keen);
                    return enemySpawner;
                case nameof(BiomeChanger):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    int minDelay = Convert.ToInt32(objProperties[3]);
                    int maxDelay = Convert.ToInt32(objProperties[4]);
                    int maxVelocity = Convert.ToInt32(objProperties[5]);
                    health = Convert.ToInt32(objProperties[6]);
                    var biomeChanger = new BiomeChanger(grid, new Rectangle(x, y, 87, 83), minDelay, maxDelay, maxVelocity, health);
                    return biomeChanger;
                case nameof(ThunderCloud):
                    var thunderCloud = new ThunderCloud(grid, new Rectangle(x, y, 128, 48));
                    return thunderCloud;
                case nameof(Keen5SpinningBurnPlatform):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    bool hasBurner = Convert.ToBoolean(objProperties[3]);
                    int startIndex = Convert.ToInt32(objProperties[4]);
                    var keen5SpinningBurnPlatform = new Keen5SpinningBurnPlatform(grid, new Rectangle(x, y, 28, 32), _keen, hasBurner, startIndex);
                    return keen5SpinningBurnPlatform;
                case nameof(SingleMaskedPlatformTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[5]);
                    var singleMaskedPlatformTile = new SingleMaskedPlatformTile(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), bType);
                    return singleMaskedPlatformTile;
                case nameof(Keen5LargePipePlatform):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    var keen5PipeMaskedPlatformTile = new Keen5LargePipePlatform(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight));
                    return keen5PipeMaskedPlatformTile;
                case nameof(MaskedPlatformTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[5]);
                    tType = (TileType)Enum.Parse(typeof(TileType), objProperties[6]);
                    var maskedPlatformTile = new MaskedPlatformTile(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), bType, tType);
                    return maskedPlatformTile;
                case nameof(Keen5ControlPanel):
                    var keen5ControlPanel = new Keen5ControlPanel(grid, new Rectangle(x, y, 56, 50));
                    return keen5ControlPanel;
                case nameof(FloorToPlatformTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[5]);
                    var floorToPlatformTile = new FloorToPlatformTile(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), bType);
                    return floorToPlatformTile;
                case nameof(WallToPlatformTile):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    bType = (BiomeType)Enum.Parse(typeof(BiomeType), objProperties[5]);
                    direction = (Direction)Enum.Parse(typeof(Direction), objProperties[6]);
                    var wallToPlatformTile = new WallToPlatformTile(grid, new Rectangle(x, y, hitboxWidth, hitboxHeight), bType, direction);
                    return wallToPlatformTile;
                case nameof(ForegroundSprite):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    string fileName = FileIOUtilities.GetResourcesPath() + objProperties[5];
                    Image img = Image.FromFile(fileName);
                    var fSprite = new ForegroundSprite(new Rectangle(x, y, hitboxWidth, hitboxHeight), img);
                    return fSprite;
                case nameof(AnimatedForegroundSprite):
                    var animationSprites = GetListElements(objProperties[3]);
                    animationSprites = animationSprites.Select(i => FileIOUtilities.GetResourcesPath() + i).ToList();
                    var anchor = (AnimationAnchor)Enum.Parse(typeof(AnimationAnchor), objProperties[4]);

                    fSprite = new AnimatedForegroundSprite(new Rectangle(x, y, 0, 0), animationSprites, anchor);
                    return fSprite;
                case nameof(BackgroundSprite):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    fileName = FileIOUtilities.GetResourcesPath() + objProperties[5];
                    img = Image.FromFile(fileName);
                    int zIndex = Convert.ToInt32(objProperties[6]);
                    bool stretchImage = Convert.ToBoolean(objProperties[7]);
                    var bSprite = new BackgroundSprite(new Rectangle(x, y, hitboxWidth, hitboxHeight), img, zIndex, stretchImage);
                    return bSprite;
                case nameof(BackgroundSpriteCanvas):
                    zIndex = Convert.ToInt32(objProperties[3]);
                    string itemListText = objProperties[4];
                    int columnCount = Convert.ToInt32(objProperties[5]);
                    Color backColor = Color.FromName(objProperties[6]);
                    string[] files = BackgroundSpriteCanvas.ParseFilesFromText(itemListText);
                    BackgroundSpriteCanvas canvas = new BackgroundSpriteCanvas(new Point(x, y), zIndex, files, columnCount, backColor);
                    return canvas;
                case nameof(AnimatedBackgroundSprite):
                    zIndex = Convert.ToInt32(objProperties[3]);
                    int delayTicks = Convert.ToInt32(objProperties[4]);
                    var filenames = GetListElements(objProperties[5]);
                    List<Image> images = new List<Image>();
                    foreach (var filename in filenames)
                    {
                        string fullname = FileIOUtilities.GetResourcesPath() + filename;
                        Image i = Image.FromFile(fullname);
                        images.Add(i);
                    }
                    AnimatedBackgroundSprite abSprite = new AnimatedBackgroundSprite(new Point(x, y), images.ToArray(), delayTicks, zIndex);
                    return abSprite;
                case nameof(ForegroundWall):
                    hitboxWidth = Convert.ToInt32(objProperties[3]);
                    hitboxHeight = Convert.ToInt32(objProperties[4]);
                    fileName = FileIOUtilities.GetResourcesPath() + objProperties[5];
                    img = Image.FromFile(fileName);
                    zIndex = Convert.ToInt32(objProperties[6]);
                    stretchImage = Convert.ToBoolean(objProperties[7]);
                    var fgSprite = new ForegroundWall(new Rectangle(x, y, hitboxWidth, hitboxHeight), img, zIndex, stretchImage);
                    return fgSprite;
                case nameof(ExitDoor):
                    var exitDoor = new ExitDoor(grid, new Rectangle(x, y, 124, 114));
                    return exitDoor;
                case nameof(RandomWeaponGenerator):
                    if (_keen == null)
                        throw new InvalidDataException(KEEN_NOT_FOUND_TEXT);

                    var cost = Convert.ToInt32(objProperties[3]);
                    var randomWeaponGenerator = new RandomWeaponGenerator(grid, new Rectangle(x, y, 124, 92), cost, _keen);
                    return randomWeaponGenerator;
                case nameof(SecretPlatformTile):
                    SecretPlatformState initialState = (SecretPlatformState)Enum.Parse(typeof(SecretPlatformState), objProperties[3]);
                    SecretPlatformTile secretPlatformTile = new SecretPlatformTile(grid, new Rectangle(x, y, 6, 2), initialState);
                    return secretPlatformTile;
                case nameof(Vitalin):
                    Vitalin vitalin = new Vitalin(grid, new Rectangle(x, y, 18, 32));
                    return vitalin;
                case nameof(RainDrop):
                    RainDrop rainDrop = new RainDrop(grid, new Rectangle(x, y, 20, 32));
                    return rainDrop;
                case nameof(Viva):
                    bool perch = Convert.ToBoolean(objProperties[3]);
                    Point pViva = new Point(x, y);
                    Size sViva = perch ? new Size(14, 16) : new Size(26, 18);
                    Viva viva = new Viva(grid, new Rectangle(x, y, 18, 32), perch);
                    return viva;

            }
            return null;
        }

        public static bool SaveMap(string mapName, Size dimensions, GameModeEnum gameMode, List<CollisionObject> gameObjects)
        {
            try
            {
                string fullPath = string.Empty;

                switch (gameMode)
                {
                    case GameModeEnum.NORMAL:
                        fullPath = Environment.CurrentDirectory + @"\SavedNormalMaps\";
                        break;
                    case GameModeEnum.ZOMBIE:
                        fullPath = Environment.CurrentDirectory + @"\SavedZombieMaps\";
                        break;
                    case GameModeEnum.KING_OF_THE_HILL:
                        fullPath = Environment.CurrentDirectory + @"\SavedKingOfTheHillMaps\";
                        break;
                    case GameModeEnum.CAPTURE_THE_FLAG:
                        fullPath = Environment.CurrentDirectory + @"\SavedCTFMaps\";
                        break;
                }

                fullPath += mapName + ".txt";

                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    string dimensionsLine = $"{dimensions.Width}{PROPERTY_SEPARATOR}{dimensions.Height}";
                    writer.WriteLine(dimensionsLine);

                    //Set keen as the first item we write since many game objects depend on commander keen's existence 
                    CommanderKeen keenObj = gameObjects.FirstOrDefault(i => i is CommanderKeen) as CommanderKeen;
                    if (keenObj != null)
                    {
                        gameObjects.Remove(keenObj);
                        gameObjects.Insert(0, keenObj);
                    }

                    foreach (var item in gameObjects)
                    {
                        string line = item.ToString();
                        writer.WriteLine(line);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

    

        private static List<Point> GetPathPoints(string rawText)
        {
            List<string> items = GetListElements(rawText);
            List<Point> points = new List<Point>();
            for (int i = 0; i < items.Count; i += 2)
            {
                int x = Convert.ToInt32(items[i]);
                int y = Convert.ToInt32(items[i + 1]);
                Point p = new Point(x, y);
                points.Add(p);
            }
            return points;
        }

        private static List<string> GetListElements(string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return new List<string>();

            var indexOfBegin = rawText.IndexOf(LIST_BEGIN_CHAR);
            var indexOfEnd = rawText.IndexOf(LIST_END_CHAR);

            if (indexOfBegin == -1 || indexOfEnd == 1 || indexOfEnd == -1)
                return new List<string>();

            var substring = rawText.Substring(indexOfBegin, indexOfEnd - indexOfBegin);

            substring = substring.Replace(LIST_BEGIN_CHAR.ToString(), "").Replace(LIST_END_CHAR.ToString(), "");
            string[] returnArray = substring.Split(LIST_ITEM_SEPARATOR);
            return returnArray.ToList();
        }
    }
}
