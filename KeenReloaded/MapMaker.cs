using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using System.Reflection;
using System.IO;
using KeenReloaded.Framework;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Hazards;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enemies;
using KeenReloaded.UserControls;
using KeenReloaded.HelperClasses;

namespace KeenReloaded
{

    public partial class MapMaker : Form
    {
        private const int MAP_SIZE_MIN = 500;
        private const int MAP_SIZE_MAX = 5000;
        private const int MAP_SIZE_INCREMENT = 100;
        private const int MAP_SIZE_DEFAULT = 1500;
        private const char SEPARATOR = '|';
        private readonly string _selectedCharacter;
        private Timer _cursorUpdateTimer;
        public MapMaker(string selectedCharacter)
        {
            _selectedCharacter = selectedCharacter;
            InitializeComponent();
        }

        public Control.ControlCollection CanvasObjects
        {
            get
            {
                return pnlCanvas.Controls;
            }
        }

        private void MapMaker_Load(object sender, EventArgs e)
        {
            var gameModeValues = Enum.GetValues(typeof(GameModeEnum));
            object[] items = gameModeValues.Cast<object>().ToArray();
            cmbGameMode.Items.AddRange(items);
            if (items.Any())
                cmbGameMode.SelectedIndex = 0;

            var listOfNumbers = new List<int>();
            for (int i = MAP_SIZE_MIN; i <= MAP_SIZE_MAX; i += MAP_SIZE_INCREMENT)
            {
                listOfNumbers.Add(i);
            }
            var dimensionItems = listOfNumbers.Cast<object>().ToArray();
            cmbWidth.Items.AddRange(dimensionItems);
            cmbHeight.Items.AddRange(dimensionItems);
            int selectedIndex = (MAP_SIZE_DEFAULT / MAP_SIZE_INCREMENT) - (MAP_SIZE_MIN / 100);
            cmbWidth.SelectedIndex = selectedIndex;
            cmbHeight.SelectedIndex = selectedIndex;
            this.WindowState = FormWindowState.Maximized;
            objectMenu1.Parent = this;

            _cursorUpdateTimer = new Timer();
            _cursorUpdateTimer.Interval = 100;
            _cursorUpdateTimer.Tick += _cursorUpdateTimer_Tick;
            _cursorUpdateTimer.Start();

            pnlCanvas.MouseClick += MapMaker_Click;

        }

        public void AddCursorItemToCanvas()
        {
            MapMaker_Click(objectMenu1, null);
        }

        public void AddItemToCanvas(KeenReloadedItemPic item)
        {
            if (item.SpriteObject == null || item.SpriteObject.Sprite == null)
                return;

            pnlCanvas.Controls.Add(item);
            SetRawObjectDataLocationOnCanvas(item.RawObjectCreationData, item.Location);
            item.SpriteObject = item.RawObjectCreationData.ConstructObject() as ISprite;
            objectMenu1.AttachObjectToSelectedEvent(item);
            item.BackColor = Color.Transparent;
            item.BringToFront();
        }

        public void RemoveItemFromCanvas(KeenReloadedItemPic item)
        {
            pnlCanvas.Controls.Remove(item);
            objectMenu1.DetachObjectFromSelectedEvent(item);
        }

        private void MapMaker_Click(object sender, EventArgs e)
        {
            if (objectMenu1.CursorItem != null && pnlCanvas.ClientRectangle.Contains(objectMenu1.CursorItem.ClientRectangle))
            {
                var rectangleCursor = new Rectangle(objectMenu1.CursorItem.Location, objectMenu1.CursorItem.Size);
                var offsetX = pnlCanvas.Location.X;
                var offsetY = pnlCanvas.Location.Y;
                rectangleCursor = new Rectangle(rectangleCursor.Location.X, rectangleCursor.Location.Y, rectangleCursor.Width, rectangleCursor.Height);
                var pnlRect = new Rectangle(pnlCanvas.Location, pnlCanvas.Size);
                if (pnlRect.Contains(rectangleCursor))
                {
                    KeenReloadedItemPic pic = new KeenReloadedItemPic(objectMenu1.CursorItem.BackgroundImage, objectMenu1.CursorItem.FileName);

                    pnlCanvas.Controls.Add(pic);
                    pic.BackColor = Color.Transparent;
                    pic.RawObjectCreationData = objectMenu1.CursorItem.RawObjectCreationData;
                    pic.Image = objectMenu1.CursorItem.Image;
                    pic.Location = new Point(rectangleCursor.Location.X - offsetX - 2, rectangleCursor.Location.Y - offsetY - 2);

                    SetRawObjectDataLocationOnCanvas(pic.RawObjectCreationData, pic.Location);
                    pic.SpriteObject = pic.RawObjectCreationData.ConstructObject() as ISprite;
                    objectMenu1.AttachObjectToSelectedEvent(pic);
                    pic.BringToFront();
                }
            }
        }


        private void SetRawObjectDataLocationOnCanvas(TypeConstructorValues values, Point location)
        {
            int locIndex = values.ConstructorParamNames.ToList().IndexOf("hitbox");
            if (locIndex != -1)
            {
                var locationRect = (System.Drawing.Rectangle)values.Values[locIndex];
                var newlocation = new Rectangle(location, locationRect.Size);
                values.Values[locIndex] = newlocation;
            }
        }
        private void _cursorUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (objectMenu1.CursorItem != null)
            {
                var obj = objectMenu1.CursorItem;
                obj.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                var rawObjData = objectMenu1.CursorItem.RawObjectCreationData;
                this.SetRawObjectDataLocationOnCanvas(rawObjData, obj.Location);
            }
        }

        private void ObjectMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (objectMenu1.CursorItem != null)
            {
                objectMenu1.CursorItem.Location = new Point(e.Location.X, e.Location.Y);
            }
        }


        private string GetFolderNameFromGameMode(GameModeEnum gameMode)
        {
            switch (gameMode)
            {
                case GameModeEnum.NORMAL:
                    return "SavedNormalMaps";
                case GameModeEnum.ZOMBIE:
                    return "SavedZombieMaps";
                case GameModeEnum.KING_OF_THE_HILL:
                    return "SavedKingOfTheHillMaps";
                case GameModeEnum.CAPTURE_THE_FLAG:
                    return "SavedCTFMaps";
            }
            return string.Empty;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        private void SaveMap()
        {
            string mapName = txtMapName.Text;
            if (string.IsNullOrWhiteSpace(mapName))
            {
                MessageBox.Show("Please enter a valid map name.", "Invalid Map Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GameModeEnum gameMode = (GameModeEnum)Enum.Parse(typeof(GameModeEnum), cmbGameMode.SelectedItem?.ToString());
            string folder = GetFolderNameFromGameMode(gameMode);
            string path = Environment.CurrentDirectory + $@"\{folder}\{ mapName }.txt";

            if (File.Exists(path))
            {
                DialogResult result = MessageBox.Show($"Map {mapName} already exists. Overwrite it?", "Map Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            var gameObjects = GetGameObjectsFromCanvas();
            int width = (int)cmbWidth.SelectedItem;
            int height = (int)cmbHeight.SelectedItem;
            Size dimensions = new Size(width, height);
            if (Map.SaveMap(mapName, dimensions, gameMode, gameObjects))
            {
                MessageBox.Show($"Map {mapName} saved successfully!");
            }
            else
            {
                MessageBox.Show($"Map {mapName} failed to save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }

        private List<CollisionObject> GetGameObjectsFromCanvas()
        {
            List<CollisionObject> gameObjects = new List<CollisionObject>();
            var itemPics = pnlCanvas.Controls.OfType<KeenReloadedItemPic>();
            if (itemPics.Any())
            {
                foreach (var obj in itemPics)
                {
                    if (obj is KeenReloadedItemPic)
                    {
                        var gameObj = (KeenReloadedItemPic)obj;

                        var gameConstructObj = gameObj.SpriteObject as CollisionObject;

                        if (gameConstructObj != null)
                        {
                            gameObjects.Add(gameConstructObj);
                        }
                    }
                }
            }
            return gameObjects;
        }

        private void CmbWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int width = (int)cmbWidth.SelectedItem;
            pnlCanvas.Width = width;
        }

        private void CmbHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            int height = (int)cmbHeight.SelectedItem;
            pnlCanvas.Height = height;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private string getInitialDirectoryFromGameMode(GameModeEnum gameMode)
        {
            string folderName = GetFolderNameFromGameMode(gameMode);

            return Environment.CurrentDirectory + $@"\{folderName}\";
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            var gameMode = (GameModeEnum)cmbGameMode.SelectedItem;
            openFileDialog1.InitialDirectory = getInitialDirectoryFromGameMode(gameMode);
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                pnlCanvas.Controls.Clear();
                txtMapName.Text = openFileDialog1.SafeFileName;

                var mapName = txtMapName.Text;
                Map map = Map.LoadMap(mapName, gameMode, _selectedCharacter);
                cmbWidth.SelectedItem = map.Size.Width;
                cmbHeight.SelectedItem = map.Size.Height;
                PopulateMapObjects(map, pnlCanvas);
            }
        }

        private void PopulateMapObjects(Map _map, Panel _keenGame)
        {
            //here add range of types with higher z index value last
            var hETiles = _map.Tiles.OfType<HorizontalExtendedBiomeTile>();
            if (hETiles.Any())
            {
                foreach (var tile in hETiles)
                {
                    int episode = KeenReloadedUtility.GetEpisodeFromBiome(tile.Biome);
                    _keenGame.Controls.Add(tile.Sprite);
                    tile.Sprite.BringToFront();
                }
            }
            var vTiles = _map.Tiles.OfType<VerticalExtendedBiomeTile>();
            if (vTiles.Any())
            {
                foreach (var tile in vTiles)
                {
                    int episode = KeenReloadedUtility.GetEpisodeFromBiome(tile.Biome);
                    _keenGame.Controls.Add(tile.Sprite);
                    tile.Sprite.BringToFront();
                }
            }
            var biomeTiles = _map.Tiles.OfType<BiomeTile>();
            if (biomeTiles.Any())
            {
                foreach (var tile in biomeTiles)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    sprite.BringToFront();
                }
            }

            var singleMaskedTiles = _map.Tiles.OfType<SingleMaskedPlatformTile>();
            if (singleMaskedTiles.Any())
            {
                foreach (var tile in singleMaskedTiles)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    sprite.BringToFront();
                }
            }

            var maskedTiles = _map.Tiles.OfType<MaskedPlatformTile>();
            if (maskedTiles.Any())
            {
                foreach (var tile in maskedTiles)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    sprite.BringToFront();
                }
            }

            var floorToPlatformTiles = _map.Tiles.OfType<FloorToPlatformTile>();
            if (floorToPlatformTiles.Any())
            {
                foreach (var tile in floorToPlatformTiles)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    sprite.BringToFront();
                }
            }

            var wallToPlatformTiles = _map.Tiles.OfType<WallToPlatformTile>();
            if (wallToPlatformTiles.Any())
            {
                foreach (var tile in wallToPlatformTiles)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    sprite.BringToFront();
                }
            }

            var removableTiles = _map.Tiles.OfType<RemovablePlatform>();
            List<CollisionObject> perpherialTiles = new List<CollisionObject>();
            if (removableTiles.Any())
            {
                foreach (var rp1 in removableTiles)
                {
                    perpherialTiles.Add(rp1.LeftCollisionTile);
                    perpherialTiles.Add(rp1.RightCollisionTile);
                    perpherialTiles.Add(rp1.MiddleCollisiontTile);
                    _keenGame.Controls.Add(rp1.LeftEdge.Sprite);
                    foreach (var item in rp1.RemoveableSection)
                    {
                        _keenGame.Controls.Add(item.Sprite);
                    }
                    _keenGame.Controls.Add(rp1.RightEdge.Sprite);
                }
                _map.Tiles.AddRange(perpherialTiles);
            }

            var secretPlatformTiles = _map.Objects.OfType<SecretPlatformTile>();
            if (secretPlatformTiles.Any())
            {
                foreach (var tile in secretPlatformTiles)
                {
                    _keenGame.Controls.Add(tile.Sprite);
                }
            }

            var randomWeaponGenerators = _map.Objects.OfType<RandomWeaponGenerator>();

            if (randomWeaponGenerators.Any())
            {
                foreach (var generator in randomWeaponGenerators)
                {
                    _keenGame.Controls.Add(generator.Sprite);
                    _keenGame.Controls.Add(generator.WeaponSprite);
                    _keenGame.Controls.Add(generator.CostLabel);
                    generator.Sprite.BringToFront();
                    generator.WeaponSprite.BringToFront();
                    generator.CostLabel.BringToFront();
                }
            }

            var doors = new List<Door>(_map.Tiles.OfType<Door>());
            doors.AddRange(_map.Objects.OfType<Door>());

            if (doors.Any())
            {
                foreach (var door in doors)
                {
                    _keenGame.Controls.Add(door.Sprite);
                    door.Sprite.BringToFront();
                }
            }

            var enemySpawners = _map.Objects.OfType<EnemySpawner>();
            if (enemySpawners.Any())
            {
                foreach (var spawner in enemySpawners)
                {
                    spawner.Initialize(_map);
                    //_game.RegisterItemEventsForObject(spawner);
                    _keenGame.Controls.Add(spawner.Sprite);
                    spawner.Sprite.BringToFront();

                }
            }

            var biomeChangers = _map.Objects.OfType<BiomeChanger>();
            if (biomeChangers.Any())
            {
                foreach (var biomeChanger in biomeChangers)
                {
                    _keenGame.Controls.Add(biomeChanger.Sprite);
                    biomeChanger.Sprite.BringToFront();
                }
            }

            var poisonPools = _map.Objects.OfType<PoisonPool>();

            if (poisonPools.Any())
            {
                foreach (var pool in poisonPools)
                {
                    foreach (var sprite in pool.Sprites)
                    {
                        _keenGame.Controls.Add(sprite);
                        sprite.BringToFront();
                    }
                }
            }

            var poles = _map.Tiles.OfType<Pole>();
            if (poles.Any())
            {
                foreach (var pole in poles)
                {
                    foreach (var sprite in pole.Sprites)
                    {
                        _keenGame.Controls.Add(sprite.Sprite);
                        sprite.Sprite.BringToFront();
                    }
                    if (pole.Manhole != null)
                        pole.Manhole.Sprite.BringToFront();
                }
            }

            var controlPanels = _map.Objects.OfType<Keen5ControlPanel>();
            if (controlPanels.Any())
            {
                foreach (var panel in controlPanels)
                {
                    _keenGame.Controls.Add(panel.Sprite);
                    panel.Sprite.BringToFront();
                }
            }

            var items = _map.Objects.OfType<Item>();
            if (items.Any())
            {
                foreach (var item in items)
                {
                    _keenGame.Controls.Add(item.Sprite);
                    item.Sprite.BringToFront();
                }
            }

            var gates = _map.Objects.OfType<KeyGate>();
            if (gates.Any())
            {
                foreach (var gate in gates)
                {
                    _keenGame.Controls.Add(gate.Sprite);
                    gate.Sprite.BringToFront();
                }
            }
            var gemHolders = _map.Tiles.OfType<GemPlaceHolder>();
            if (gemHolders.Any())
            {
                foreach (var holder in gemHolders)
                {
                    _keenGame.Controls.Add(holder.Sprite);
                    holder.Sprite.BringToFront();
                }
            }

            var switches = _map.Tiles.OfType<ToggleSwitch>();
            if (switches.Any())
            {
                foreach (var s in switches)
                {
                    _keenGame.Controls.Add(s.Sprite);
                    s.Sprite.BringToFront();
                }
            }

            var keen6Switches = _map.Tiles.OfType<Keen6Switch>();
            if (keen6Switches.Any())
            {
                foreach (var s in keen6Switches)
                {
                    _keenGame.Controls.Add(s.Sprite);
                    s.Sprite.BringToFront();
                    foreach (var poleSprite in s.PoleSprites)
                    {
                        _keenGame.Controls.Add(poleSprite);
                        poleSprite.BringToFront();
                    }
                }
            }

            var platforms = _map.Objects.OfType<Platform>();
            if (platforms.Any())
            {
                foreach (var platform in platforms)
                {
                    _keenGame.Controls.Add(platform.Sprite);
                    platform.Sprite.BringToFront();
                    if (platform is Keen6SetPathPlatform)
                    {
                        var plat = (Keen6SetPathPlatform)platform;
                        _keenGame.Controls.Add(plat.BipOperator);
                        plat.BipOperator.BringToFront();
                    }
                }
            }

            var flippingPlatforms = _map.Objects.OfType<FlippingPlatform>();
            if (flippingPlatforms.Any())
            {
                foreach (var platform in flippingPlatforms)
                {
                    _keenGame.Controls.Add(platform.Sprite);
                    platform.Sprite.BringToFront();
                }
            }

            var hazards = _map.Objects.OfType<Hazard>().ToList();
            hazards.AddRange(_map.Tiles.OfType<Hazard>());

            if (hazards.Any())
            {
                foreach (var hazard in hazards)
                {
                    if (!(hazard is PoisonPool) && !(hazard is TarPit))
                    {
                        _keenGame.Controls.Add(hazard.Sprite);
                        hazard.Sprite.BringToFront();
                    }

                    if (hazard is Keen5LaserField)
                    {
                        var laserField = (Keen5LaserField)hazard;
                        _keenGame.Controls.Add(laserField.DownTile.Sprite);
                        laserField.DownTile.Sprite.BringToFront();
                        _keenGame.Controls.Add(laserField.UpTile.Sprite);
                        laserField.UpTile.Sprite.BringToFront();
                    }
                    else if (hazard is Keen6LaserField)
                    {
                        var laserField = (Keen6LaserField)hazard;
                        _keenGame.Controls.Add(laserField.DownTile.Sprite);
                        laserField.DownTile.Sprite.BringToFront();
                        _keenGame.Controls.Add(laserField.UpTile.Sprite);
                        laserField.UpTile.Sprite.BringToFront();
                    }
                }
            }

            var tarpits = _map.Objects.OfType<TarPit>();
            if (tarpits.Any())
            {
                foreach (var pit in tarpits)
                {
                    pit.Sprite.SendToBack();
                    foreach (var sprite in pit.Sprites)
                    {
                        _keenGame.Controls.Add(sprite);
                        sprite.BringToFront();
                    }

                    foreach (var sprite in pit.DepthSprites)
                    {
                        _keenGame.Controls.Add(sprite);
                        sprite.BringToFront();
                    }
                }
            }

            var enemies = _map.Objects.OfType<IEnemy>();
            if (enemies.Any())
            {
                foreach (var enemy in enemies)
                {

                    var iSprite = enemy as ISprite;
                    if (iSprite != null)
                    {
                        _keenGame.Controls.Add(iSprite.Sprite);
                        iSprite.Sprite.BringToFront();
                        if (iSprite is Sphereful)
                        {
                            var sphereful = (Sphereful)iSprite;
                            foreach (var orb in sphereful.Orbs)
                            {
                                _keenGame.Controls.Add(orb);
                                orb.BringToFront();
                            }
                        }
                        else if (iSprite is ShikadiMine)
                        {
                            var shikadiMine = (ShikadiMine)iSprite;
                            _keenGame.Controls.Add(shikadiMine.Eye.Sprite);
                            shikadiMine.Eye.Sprite.BringToFront();
                        }
                    }
                }
            }

            //add keen with certain zIndexPriority

            var keen = _map.Objects.OfType<CommanderKeen>().FirstOrDefault();
            if (keen != null)
            {
                _keenGame.Controls.Add(keen.Sprite);
                keen.Sprite.BringToFront();
            }

            if (poles.Any())
            {
                foreach (var pole in poles)
                {
                    if (pole.ManholeFloor != null)
                        pole.ManholeFloor.Sprite.BringToFront();
                }
            }

            var clouds = _map.Objects.OfType<ThunderCloud>();
            if (clouds.Any())
            {
                foreach (var cloud in clouds)
                {
                    _keenGame.Controls.Add(cloud.Sprite);
                    cloud.Sprite.BringToFront();
                }
            }

            var masks = _map.Masks;
            var backGrounds = masks.OfType<BackgroundSprite>();
            var animatedBackgrounds = _map.Objects.OfType<AnimatedBackgroundSprite>();
            var foreGrounds = masks.OfType<ForegroundSprite>();

            var allBackgrounds = new List<IBackground>();
            if (backGrounds.Any())
            {
                allBackgrounds.AddRange(backGrounds);
            }
            if (animatedBackgrounds.Any())
            {
                allBackgrounds.AddRange(animatedBackgrounds);
            }
           
            if (foreGrounds.Any())
            {
                foreach (var mask in foreGrounds)
                {
                    _keenGame.Controls.Add(mask.Sprite);
                    mask.Sprite.BringToFront();
                }
            }
            if (allBackgrounds.Any())
            {
                allBackgrounds = allBackgrounds.OrderByDescending(b => b.ZIndex).ToList();
                foreach (var mask in allBackgrounds)
                {
                    _keenGame.Controls.Add(mask.Sprite);
                    mask.Sprite.SendToBack();
                }
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            SaveMap();
            if (!string.IsNullOrWhiteSpace(txtMapName.Text))
            {
                GameModeEnum gameMode = (GameModeEnum)Enum.Parse(typeof(GameModeEnum), cmbGameMode.SelectedItem?.ToString());
                string mapName = txtMapName.Text + ".txt";
                Form1 frm = new Form1(mapName, gameMode, _selectedCharacter);
                frm.ShowDialog();
            }
        }

        private void MapMaker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && objectMenu1.Mode == Mode.EDIT)
            {
                this.pnlCanvas.Controls.Remove(objectMenu1.SelectedItem);
                objectMenu1.DeselectItem();
            }
        }
    }
}
