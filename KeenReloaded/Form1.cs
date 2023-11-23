using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;
using KeenReloaded.Framework.Enums;
using System.Threading.Tasks;
using System.Diagnostics;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Weapons;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework.Hazards;
using KeenReloaded.Framework.Enemies;
using KeenReloaded.UserControls;
using KeenReloaded.Framework.Singletons;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.HelperClasses;
using System.IO;

namespace KeenReloaded
{
    public partial class Form1 : Form
    {
        public Form1(string mapName, GameModeEnum gameMode, string selectedCharacter)
        {
            _mapName = mapName;
            _gameMode = gameMode;
            _selectedCharacter = selectedCharacter;
            InitializeComponent();
        }

        void _moveTimer_Tick(object sender, EventArgs e)
        {
            if (!_pauseGame && !_levelCompleted)
            {
                _game.UpdateGame();
                if (_keen.IsLookingUp)
                {
                    if (_currentVisionOffset * -1 < MAX_VISION_OFFSET)
                    {
                        _currentVisionOffset--;
                        int x = _keen.HitBox.X - VIEW_RADIUS;
                        int y = _keen.HitBox.Y - VIEW_RADIUS + (_currentVisionOffset * VISION_OFFSET_COEFFICIENT);
                        _gamePanel.AutoScrollPosition = new Point(x, y);
                    }
                }
                else if (_keen.IsLookingDown)
                {
                    if (_currentVisionOffset < MAX_VISION_OFFSET)
                    {
                        _currentVisionOffset++;
                        int x = _keen.HitBox.X - VIEW_RADIUS;
                        int y = (_keen.HitBox.Y - VIEW_RADIUS < 0 ? 0 : _keen.HitBox.Y - VIEW_RADIUS) + (_currentVisionOffset * VISION_OFFSET_COEFFICIENT);
                        if (Math.Abs(y) > _maxVisionY)
                            y = y < 0 ? _maxVisionY * -1 : _maxVisionY;
                        _gamePanel.AutoScrollPosition = new Point(x, y);
                    }
                }
                else if (_currentVisionOffset != 0)
                {
                    _currentVisionOffset = _currentVisionOffset < 0 ? _currentVisionOffset + 1 : _currentVisionOffset - 1;
                    int x = _keen.HitBox.X - VIEW_RADIUS;
                    int y = (_keen.HitBox.Y - VIEW_RADIUS < 0 ? 0 : _keen.HitBox.Y - VIEW_RADIUS) + (_currentVisionOffset * VISION_OFFSET_COEFFICIENT);

                    _gamePanel.AutoScrollPosition = new Point(x, y);
                }
            }
        }

        private void _deathCheckTimer_Tick(object sender, EventArgs e)
        {
            var mapBounds = new Rectangle(new Point(0, 0), _map.Size);
            if (_keen.HitBox.Right < _deathPoint.X - VIEW_RADIUS
             || _keen.HitBox.Left > _deathPoint.X + VIEW_RADIUS
             || _keen.HitBox.Top > _deathPoint.Y + (VIEW_RADIUS * 2)
             || !mapBounds.IntersectsWith(_keen.HitBox)
             || !_keenGame.Controls.Contains(_keen.Sprite)
             || !_keen.HasCollisionNodes)
            {
                _deathCheckTimer.Stop();
                if (_keen.Lives >= 0)
                {
                    ShowRetryDialog();
                }
                else
                {
                    ShowGameOverDialog();
                    this.Close();
                }
            }
        }

        private void ShowGameOverDialog()
        {
            _pauseGame = true;
            _gameTime.Stop();
            KeenReloadedDialogWindow window = new KeenReloadedDialogWindow("Game Over");
            window.ShowDialog();

            if (IsHighScore())
            {
                ShowHighScoreDialog();
                WriteHighScores();
            }
        }

        private void ShowHighScoreDialog()
        {
            KeenReloadedTextInputDialog textInputDialog = new KeenReloadedTextInputDialog("High Score! Enter your username:");
            var dialogResult = textInputDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (_gameMode == GameModeEnum.NORMAL)
                {
                    _highScores.Add(new HighScore() { Name = textInputDialog.UserNameText, Score = _keen.Points, Time = _gameTime.Elapsed });
                    _highScores = _highScores.OrderBy(s => s.Time).ThenByDescending(s => s.Time).Take(HIGH_SCORE_LENGTH).ToList();
                }
                else if (_gameMode == GameModeEnum.ZOMBIE || _gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG)
                {
                    _highScores.Add(new HighScore() { Name = textInputDialog.UserNameText, Score = _keen.Points, Time = _gameTime.Elapsed });
                    _highScores = _highScores.OrderByDescending(s => s.Score).ThenBy(s => s.Time).Take(HIGH_SCORE_LENGTH).ToList();
                }
            }
        }

        private bool IsHighScore()
        {
            List<HighScore> highScores;
            _highScores = new List<HighScore>();
            bool isHighScore = false;

            if (_gameMode == GameModeEnum.NORMAL && !_levelCompleted)
                return false;

            if (ReadHighScores(out highScores))
            {
                if (highScores.Count < HIGH_SCORE_LENGTH)
                {
                    isHighScore = true;
                }

                foreach (var score in highScores)
                {
                    //if we beat just one of these, it is a high score
                    if (_gameMode == GameModeEnum.NORMAL)
                    {
                        //compare current time to time on the score.  If tie, compare points
                        if (!isHighScore && _gameTime.Elapsed <= score.Time)
                        {
                            isHighScore = true;
                        }
                        _highScores.Add(score);
                    }
                    else if (_gameMode == GameModeEnum.ZOMBIE || _gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG)
                    {
                        //compare current points to points on the score.  If tie, compare time
                        if (!isHighScore && _keen.Points >= score.Score)
                        {
                            isHighScore = true;
                        }
                        _highScores.Add(score);
                    }
                }
            }
            if (_keen.Points == 0 && (_gameMode == GameModeEnum.ZOMBIE || _gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG))
                isHighScore = false;

            return isHighScore;
        }

        private bool WriteHighScores()
        {
            try
            {
                string fullPath = Environment.CurrentDirectory;
                if (_gameMode == GameModeEnum.NORMAL)
                {
                    fullPath += $@"\NormalModeTimes\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.ZOMBIE)
                {
                    fullPath += $@"\ZombieModeScores\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.KING_OF_THE_HILL)
                {
                    fullPath += $@"\KingOfTheHillScores\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.CAPTURE_THE_FLAG)
                {
                    fullPath += $@"\CTFScores\{_mapName}";
                }

                if (!File.Exists(fullPath))
                {
                    File.Create(fullPath);
                }

                StringBuilder builder = new StringBuilder();
                foreach (var score in _highScores)
                {
                    builder.AppendLine($"{score.Name}|{score.Score}|{score.Time}");
                }
                File.WriteAllText(fullPath, builder.ToString());
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private bool ReadHighScores(out List<HighScore> highScores)
        {
            highScores = new List<HighScore>();
            try
            {
                string fullPath = Environment.CurrentDirectory;
                if (_gameMode == GameModeEnum.NORMAL)
                {
                    fullPath += $@"\NormalModeTimes\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.ZOMBIE)
                {
                    fullPath += $@"\ZombieModeScores\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.KING_OF_THE_HILL)
                {
                    fullPath += $@"\KingOfTheHillScores\{_mapName}";
                }
                else if (_gameMode == GameModeEnum.CAPTURE_THE_FLAG)
                {
                    fullPath += $@"\CTFScores\{_mapName}";
                }

                if (!File.Exists(fullPath))
                {
                    using (var file = File.Create(fullPath))
                    {
                    };
                }

                using (var stream = File.OpenRead(fullPath))
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string[] rawData = streamReader.ReadLine().Split('|');
                        if (rawData != null && rawData.Length >= 3)
                        {
                            HighScore score = new HighScore()
                            {
                                Name = rawData[0],
                                Score = Convert.ToInt64(rawData[1]),
                                Time = TimeSpan.Parse(rawData[2])
                            };

                            highScores.Add(score);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                highScores = new List<HighScore>();
            }
            return false;
        }

        private void ShowRetryDialog()
        {
            KeenReloadedYesNoDialogWindow window = new KeenReloadedYesNoDialogWindow($"You failed. Try again?", true);
            if (window.ShowDialog() == DialogResult.Yes)
            {
                window.Close();
                DetachScoreBoardEvents();
                ResetMapGameObjects();
            }
            else
            {
                window.Close();
                ShowGameOverDialog();
                this.Close();
            }
        }

        private void DetachScoreBoardEvents()
        {
            scoreBoard1.DetachKeenEvents();
        }

        private void ResetMapGameObjects()
        {
            _deathCheckTimer.Stop();
            _keenGame.Controls.Clear();
            _map.Clear(_game);

            LoadMapFromFile(_mapName, _gameMode, true);
            UpdateViewRectangle();
        }

        private const int LEVEL_MAX_WIDTH = 10000; //load test to ensure fast map loading on larger maps;
        private const int LEVEL_MAX_HEIGHT = 10000;
        private const int SPACE_HASH_WIDTH = 150;
        private const int SPACE_HASH_HEIGHT = 150;
        private const int VIEW_RADIUS = 400;
        private const int MAX_VISION_OFFSET = 10;
        private const int VISION_OFFSET_COEFFICIENT = 10;
        private const int HIGH_SCORE_LENGTH = 8;
        private readonly string _mapName;
        private readonly GameModeEnum _gameMode;
        private readonly string _selectedCharacter;
        private int _currentVisionOffset;
        private SpaceHashGrid _spaceHashGrid;
        private CommanderKeenGame _game;
        private Map _map;
        private bool _debug = false;
        private bool _pauseGame;
        private Timer _moveTimer;
        private Timer _deathCheckTimer;
        private CommanderKeen _keen;
        private int _maxVisionX, _maxVisionY;
        private bool _levelCompleted;
        private Point _deathPoint;
        private int _currentLives;
        private LoadingAnimation _loadingAnimation;
        private List<HighScore> _highScores;
        private Stopwatch _gameTime;

        public CommanderKeen Keen
        {
            get
            {
                return _keen;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _gameTime = new Stopwatch();
            _loadingAnimation = new LoadingAnimation();
            //center loading animation
            _loadingAnimation.Location = new Point(
                (this.Location.X + this.Width / 2) - (_loadingAnimation.Width / 2),//x
                (this.Location.Y + this.Height / 2) - (_loadingAnimation.Height / 2)//y
            );
            this.Controls.Add(_loadingAnimation);
            LoadMapFromFile(_mapName, _gameMode);
            _moveTimer = new Timer();
            _moveTimer.Interval = 50;
            _moveTimer.Start();
            _moveTimer.Tick += new EventHandler(_moveTimer_Tick);
            _deathCheckTimer = new Timer();
            _deathCheckTimer.Interval = 100;
            _deathCheckTimer.Tick += _deathCheckTimer_Tick;
            _keenGame.Width = LEVEL_MAX_WIDTH;
            _keenGame.Height = LEVEL_MAX_HEIGHT;
            UpdateViewRectangle();
            _gamePanel.AutoScroll = false;

            scoreBoard1.Parent = this;


        }



        private void LoadMapFromFile(string mapName, GameModeEnum gameMode, bool isReset = false)
        {

            //_loadingAnimation.ShowAnimation();
            //_loadingAnimation.BringToFront();
            List<NeuralStunner> weapons = new List<NeuralStunner>();
            _map = Map.LoadMap(mapName, gameMode, _selectedCharacter);
            _game = new CommanderKeenGame(_map);
            var points = _gameMode == GameModeEnum.ZOMBIE || _gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG ? _keen?.Points ?? 0 : 0;
            var drops = _keen?.Drops ?? 0;

            if (isReset)
            {
                _currentLives = _keen.Lives;
                weapons = gameMode == GameModeEnum.ZOMBIE || gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG ? _keen.Weapons : null;
            }

            _keen = _map.Objects.OfType<CommanderKeen>().FirstOrDefault();
            _keen.KeenMoved += new EventHandler(_keen_KeenMoved);
            _keen.KeenAcquiredWeapon += new EventHandler<WeaponAcquiredEventArgs>(_keen_KeenAcquiredWeapon);
            _keen.KeenLevelCompleted += _keen_KeenLevelCompleted;
            _keen.KeenDied += _keen_KeenDied;
            _game.ObjectRemoved += new EventHandler<ObjectEventArgs>(_game_ObjectRemoved);
            _game.ObjectCreated += new EventHandler<ObjectEventArgs>(_game_ObjectCreated);
            _game.RegisterItemEventsForObject(_keen);
            scoreBoard1.SetBoardForGameMode(_gameMode);

            if (isReset)
            {
                if (_gameMode == GameModeEnum.NORMAL)
                {
                    _keen.ResetKeenAfterDeath(_currentLives, drops, points);
                }
                else
                {
                    if (scoreBoard1.Shield != null)
                    {
                        _game.Map.Objects.Add(scoreBoard1.Shield);
                        _game.RegisterItemEventsForObject(scoreBoard1.Shield);
                        _keenGame.Controls.Add(scoreBoard1.Shield.Sprite);
                        scoreBoard1.Shield.Sprite.BringToFront();
                        scoreBoard1.Shield.Deactivate();
                        scoreBoard1.Shield.SetKeen(_keen);
                    }
                    _keen.ResetKeenAfterDeath(_currentLives, drops, points, weapons, scoreBoard1.Shield);
                }
                scoreBoard1.ResetGems();
                scoreBoard1.ResetFlags();
            }


            scoreBoard1.Keen = _keen;

            _maxVisionY = _map.Size.Height - VIEW_RADIUS;
            _maxVisionX = _map.Size.Width - VIEW_RADIUS;

            foreach (var weapon in _keen.Weapons)
            {
                _game.RegisterItemEventsForObject(weapon);
            }

            //here add range of types with higher z index value last
            var hETiles = _map.Tiles.OfType<HorizontalExtendedBiomeTile>();
            if (hETiles.Any())
            {
                foreach (var tile in hETiles)
                {
                    _keenGame.Controls.Add(tile.Sprite);
                    tile.Sprite.BringToFront();
                }
            }
            var vTiles = _map.Tiles.OfType<VerticalExtendedBiomeTile>();
            if (vTiles.Any())
            {
                foreach (var tile in vTiles)
                {
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

            var hills = _map.Objects.OfType<Hill>();
            if (hills.Any())
            {
                foreach (var hill in hills)
                {
                    _game.RegisterItemEventsForObject(hill);
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
                    if (tile is Keen6ClawTile)
                    {
                        var clawTile = (Keen6ClawTile)tile;
                        foreach (var t in clawTile.ArmLengths)
                        {
                            _keenGame.Controls.Add(t);
                            t.BringToFront();
                        }
                    }
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

            var miragePlatforms = _map.Objects.OfType<MiragePlatform>();
            if (miragePlatforms.Any())
            {
                foreach (var tile in miragePlatforms)
                {
                    var sprite = tile.Sprite;
                    _keenGame.Controls.Add(sprite);
                    tile.Create += _game_ObjectCreated;
                    tile.Remove += _game_ObjectRemoved;
                    sprite.BringToFront();
                }
            }

            var removableTiles = _map.Tiles.OfType<RemovablePlatform>();
            List<CollisionObject> perpherialTiles = new List<CollisionObject>();
            if (removableTiles.Any())
            {
                foreach (var rp1 in removableTiles)
                {
                    _game.RegisterItemEventsForObject(rp1);
                    perpherialTiles.Add(rp1.LeftCollisionTile);
                    perpherialTiles.Add(rp1.RightCollisionTile);
                    perpherialTiles.Add(rp1.MiddleCollisiontTile);
                    _keenGame.Controls.Add(rp1.LeftEdge.Sprite);
                    foreach (var item in rp1.RemoveableSection)
                    {
                        if (rp1.IsActive)
                            _keenGame.Controls.Add(item.Sprite);
                        _game.RegisterItemEventsForObject(item);
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
                    _game.RegisterItemEventsForObject(generator);
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
                    _game.RegisterItemEventsForObject(biomeChanger);
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

            var conveyerBelts = _map.Objects.OfType<ConveyerBelt>();
            if (conveyerBelts.Any())
            {
                foreach (var belt in conveyerBelts)
                {
                    foreach (var sprite in belt.Sprites)
                    {
                        _keenGame.Controls.Add(sprite.Sprite);
                        sprite.Sprite?.BringToFront();
                    }
                }
            }

            var keen6SlimeHazards = _map.Objects.OfType<Keen6SlimeHazard>();
            if (keen6SlimeHazards.Any())
            {
                foreach (var slimeHazard in keen6SlimeHazards)
                {
                    foreach (var sprite in slimeHazard.Sprites)
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
                    if (pole is Keen6EyeBallPole)
                    {
                        var eyeBallPole = (Keen6EyeBallPole)pole;
                        var eyeBallSprite = eyeBallPole.EyeBallTileSprite.Sprite;
                        _keenGame.Controls.Add(eyeBallSprite);
                        eyeBallSprite.BringToFront();
                    }

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
                    _game.RegisterItemEventsForObject(item);
                }
            }

            var ctfDestinations = _map.Tiles.OfType<CTFDestination>();
            if (ctfDestinations.Any())
            {
                foreach (var dest in ctfDestinations)
                {
                    _keenGame.Controls.Add(dest.Sprite);
                    dest.Sprite.BringToFront();
                }
            }

            var gates = _map.Objects.OfType<KeyGate>();
            if (gates.Any())
            {
                foreach (var gate in gates)
                {
                    _keenGame.Controls.Add(gate.Sprite);
                    _game.RegisterItemEventsForObject(gate);
                    gate.Sprite.BringToFront();
                }
            }
            var gemHolders = _map.Tiles.OfType<GemPlaceHolder>();
            if (gemHolders.Any())
            {
                foreach (var holder in gemHolders)
                {
                    _keenGame.Controls.Add(holder.Sprite);
                    _game.RegisterItemEventsForObject(holder);
                    holder.Sprite.BringToFront();
                }
            }

            var switches = _map.Tiles.OfType<ToggleSwitch>();
            if (switches.Any())
            {
                foreach (var s in switches)
                {
                    _game.RegisterItemEventsForObject(s);
                    _keenGame.Controls.Add(s.Sprite);
                    s.Sprite.BringToFront();
                }
            }

            var keen6Switches = _map.Tiles.OfType<Keen6Switch>();
            if (keen6Switches.Any())
            {
                foreach (var s in keen6Switches)
                {
                    _game.RegisterItemEventsForObject(s);
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
                    _game.RegisterItemEventsForObject(platform);
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
                    _game.RegisterItemEventsForObject(platform);
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
                    if (!(hazard is PoisonPool) && !(hazard is TarPit) && !(hazard is ConveyerBelt))
                    {
                        _game.RegisterItemEventsForObject(hazard);
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

            var forceFields = _map.Tiles.OfType<ForceField>();
            if (forceFields.Any())
            {
                foreach (var forceField in forceFields)
                {
                    _keenGame.Controls.Add(forceField.Top.Sprite);
                    _keenGame.Controls.Add(forceField.Bottom.Sprite);
                    _keenGame.Controls.Add(forceField.Barrier.Sprite);
                    _keenGame.Controls.Add(forceField.HealthBar);
                    forceField.Top.Sprite.BringToFront();
                    forceField.Bottom.Sprite.BringToFront();
                    forceField.Barrier.Sprite.BringToFront();
                    forceField.HealthBar.BringToFront();
                    _game.RegisterItemEventsForObject(forceField.Barrier);
                }
            }

            var tarpits = _map.Objects.OfType<TarPit>();
            if (tarpits.Any())
            {
                foreach (var pit in tarpits)
                {

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
                    pit.Sprite.SendToBack();
                }
            }

            var enemies = _map.Objects.OfType<IEnemy>();
            if (enemies.Any())
            {
                foreach (var enemy in enemies)
                {
                    _game.RegisterItemEventsForObject(enemy);
                    if (_gameMode == GameModeEnum.ZOMBIE && enemy is IZombieBountyEnemy)
                    {
                        var bountyEnemy = (IZombieBountyEnemy)enemy;
                        bountyEnemy.Killed += BountyEnemy_Killed;

                    }
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
            _keenGame.Controls.Add(_keen.Sprite);
            _keen.Sprite.BringToFront();

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
                    _game.RegisterItemEventsForObject(cloud);
                }
            }

            var schoolfish = _map.Objects.OfType<Schoolfish>();
            if (schoolfish.Any())
            {
                foreach (var sf in schoolfish)
                {
                    _keenGame.Controls.Add(sf.Sprite);
                    sf.Sprite.BringToFront();
                }
            }

            var inchworms = _map.Objects.OfType<Inchworm>();
            if (inchworms.Any())
            {
                foreach (var im in inchworms)
                {
                    _keenGame.Controls.Add(im.Sprite);
                    im.Sprite.BringToFront();
                }
            }


            var masks = _map.Masks;
            var backGrounds = masks.OfType<BackgroundSprite>();
            var canvases = masks.OfType<BackgroundSpriteCanvas>();
            var animatedBackgrounds = _map.Objects.OfType<AnimatedBackgroundSprite>();

            var allBackgrounds = new List<IBackground>();

            var foreGrounds = masks.OfType<ForegroundSprite>();
            var foreGroundWalls = masks.OfType<ForegroundWall>();
            var animatedForegrounds = _map.Objects.OfType<AnimatedForegroundSprite>();
            if (animatedForegrounds.Any())
            {
                foreach (var mask in animatedForegrounds)
                {
                    _keenGame.Controls.Add(mask.Sprite);
                    mask.Sprite.BringToFront();
                }
            }
            if (foreGrounds.Any())
            {
                foreach (var mask in foreGrounds)
                {
                    _keenGame.Controls.Add(mask.Sprite);
                    mask.Sprite.BringToFront();
                }
            }

            if (foreGroundWalls.Any())
            {
                foreGroundWalls = foreGroundWalls.OrderByDescending(f => f.ZIndex);
                foreach (var mask in foreGroundWalls)
                {
                    _keenGame.Controls.Add(mask.Sprite);
                    mask.Sprite.BringToFront();
                }
            }

            if (backGrounds.Any())
            {
                allBackgrounds.AddRange(backGrounds);
            }

            if (canvases.Any())
            {
                allBackgrounds.AddRange(canvases);
            }

            if (animatedBackgrounds.Any())
            {
                allBackgrounds.AddRange(animatedBackgrounds);
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
            _gameTime.Start();
            //_loadingAnimation.HideAnimation();
        }


        private void BountyEnemy_Killed(object sender, ObjectEventArgs e)
        {
            if (e.ObjectSprite is IZombieBountyEnemy)
            {
                var bountyEnemy = (IZombieBountyEnemy)e.ObjectSprite;
                var spriteLoc = e.ObjectSprite.Sprite.Location;
                var spriteWidth = e.ObjectSprite.Sprite.Width;
                var spriteHeight = e.ObjectSprite.Sprite.Height;

                PointItem item = new PointItem(_map.Grid,
                    new Rectangle(
                        new Point(spriteLoc.X + (spriteWidth / 2) - 10, spriteLoc.Y + (spriteHeight / 2) - 16),
                        new Size(20, 32)),
                    bountyEnemy.PointItem);
                _game.RegisterItemEventsForObject(item);
                _map.Objects.Add(item);
                _keenGame.Controls.Add(item.Sprite);
                //remove event for killed after enemy is already killed
                //save memory by disposing this unused object and fix multiple event firing
                bountyEnemy.Killed -= BountyEnemy_Killed;
                item.Sprite.BringToFront();
                if (_keen.HitBox.IntersectsWith(item.HitBox))
                {
                    _keen.GivePoints(item.PointValue);
                    item.IsAcquired = true;
                }
            }
        }

        private void _keen_KeenDied(object sender, ObjectEventArgs e)
        {
            _deathPoint = _keen.HitBox.Location;
            _deathCheckTimer.Start();
        }

        private void _keen_KeenLevelCompleted(object sender, ObjectEventArgs e)
        {
            _levelCompleted = true;
            _keenGame.Controls.Remove(_keen.Sprite);
            _gameTime.Stop();
            KeenReloadedDialogWindow completedMessageWindow = new KeenReloadedDialogWindow("Level Complete!");
            completedMessageWindow.ShowDialog();
            if (IsHighScore())
            {
                ShowHighScoreDialog();
                WriteHighScores();
            }
            this.Close();
        }

        void _keen_KeenMoved(object sender, EventArgs e)
        {
            UpdateViewRectangle();
        }



        private void UpdateViewRectangle()
        {
            if (!_keen.IsDead())
            {
                _gamePanel.AutoScroll = false;
                int x = _keen.HitBox.X - VIEW_RADIUS;
                int y = (_keen.HitBox.Y - VIEW_RADIUS < 0 ? 0 : _keen.HitBox.Y - VIEW_RADIUS) + (_currentVisionOffset * VISION_OFFSET_COEFFICIENT);

                if (Math.Abs(x) > _maxVisionX)
                    x = x < 0 ? _maxVisionX * -1 : _maxVisionX;
                if (Math.Abs(y) > _maxVisionY)
                    y = y < 0 ? _maxVisionY * -1 : _maxVisionY;

                _gamePanel.AutoScrollPosition = new Point(x, y);
            }
        }

        private void LoadDebugTileSquare(Point seed, int length, int width)
        {
            for (int i = seed.X; i <= seed.X + (width * 32); i += 32)
            {
                //ceiling
                DebugTile ct = new DebugTile(_spaceHashGrid, new Rectangle(i, seed.Y, 32, 32));
                _map.Tiles.Add(ct);
                _keenGame.Controls.Add(ct.Sprite);

                //floor
                DebugTile ft = new DebugTile(_spaceHashGrid, new Rectangle(i, seed.Y + (length * 32), 32, 32));
                _map.Tiles.Add(ft);
                _keenGame.Controls.Add(ft.Sprite);
            }

            for (int i = seed.Y; i <= seed.Y + (length * 32); i += 32)
            {
                //leftWall
                DebugTile t = new DebugTile(_spaceHashGrid, new Rectangle(seed.X, i, 32, 32));
                _map.Tiles.Add(t);
                _keenGame.Controls.Add(t.Sprite);

                //rightwall
                DebugTile tRight = new DebugTile(_spaceHashGrid, new Rectangle(seed.X + (width * 32), i, 32, 32));
                _map.Tiles.Add(tRight);
                _keenGame.Controls.Add(tRight.Sprite);
            }
        }

        void _keen_KeenAcquiredWeapon(object sender, WeaponAcquiredEventArgs e)
        {
            var weapon = e.Weapon as NeuralStunner;
            if (weapon != null)
            {
                _game.RegisterItemEventsForObject(weapon);
            }
        }

        void _game_ObjectCreated(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            if (e != null && e.ObjectSprite != null && e.ObjectSprite.Sprite != null)
            {
                _keenGame.Controls.Add(e.ObjectSprite.Sprite);
                if (!(e.ObjectSprite is Hill))
                {
                    e.ObjectSprite.Sprite.BringToFront();
                }
                else
                {
                    MoveObjectsBehindHill(e);
                }

                if (e.ObjectSprite is RemovableTile)
                {
                    SendBackgroundsBehindObject(e);
                }

                if (_gameMode == GameModeEnum.ZOMBIE && e.ObjectSprite is IZombieBountyEnemy)
                {
                    var bountyEnemy = (IZombieBountyEnemy)e.ObjectSprite;
                    bountyEnemy.Killed += BountyEnemy_Killed;
                }
            }
        }

        private void SendBackgroundsBehindObject(ObjectEventArgs e)
        {
            e.ObjectSprite.Sprite?.SendToBack();
            var allBackgrounds = new List<IBackground>();
            var masks = _map.Masks;
            var backGrounds = masks.OfType<BackgroundSprite>();
            var canvases = masks.OfType<BackgroundSpriteCanvas>();
            var animatedBackgrounds = _map.Objects.OfType<AnimatedBackgroundSprite>();

            if (backGrounds.Any())
                allBackgrounds.AddRange(backGrounds);
            if (canvases.Any())
                allBackgrounds.AddRange(canvases);
            if (animatedBackgrounds.Any())
                allBackgrounds.AddRange(animatedBackgrounds);

            foreach (var background in allBackgrounds)
            {
                background.Sprite?.SendToBack();
            }
        }

        private void MoveObjectsBehindHill(ObjectEventArgs e)
        {
            var hill = (Hill)e.ObjectSprite;
            var collisions = hill.CheckCollision(hill.HitBox, true);
            var tiles = collisions.Where(c => c is BiomeTile || c is VerticalExtendedBiomeTile
                || c is HorizontalExtendedBiomeTile).ToList();

            foreach (var tile in tiles)
            {
                ISprite sprite = tile as ISprite;
                if (sprite != null)
                {
                    sprite.Sprite?.SendToBack();
                }
            }

            var backgroundSprites = _map.Masks.OfType<IBackground>();
            if (backgroundSprites != null && backgroundSprites.Any())
            {
                foreach (var mask in backgroundSprites)
                {
                    mask.Sprite.SendToBack();
                }
            }
        }

        void _game_ObjectRemoved(object sender, ObjectEventArgs e)
        {
            if (e != null && e.ObjectSprite != null && e.ObjectSprite.Sprite != null)
            {
                _keenGame.Controls.Remove(e.ObjectSprite.Sprite);

                if (e.ObjectSprite is IHealthBar)
                {
                    var healthBar = ((IHealthBar)e.ObjectSprite).HealthBar;
                    _keenGame.Controls.Remove(healthBar);
                }

                var obj = e.ObjectSprite as IUpdatable;
                if (obj != null && !(obj is Hill))
                {
                    _map.Objects.Remove(obj);
                    if (obj is DestructibleCollisionTile)
                    {
                        var tile = (DestructibleCollisionTile)obj;
                        _map.Tiles.Remove(tile);
                    }
                }

                if (e.ObjectSprite is RemovableTile)
                {
                    _keenGame.Controls.Remove(e.ObjectSprite.Sprite);
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    KeyEventArgs args = new KeyEventArgs(keyData);
                    base.OnKeyDown(args);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            _game.SetKeyPressed(e.KeyCode.ToString(), true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                OpenDialog(e.KeyCode);
            }
            else
            {
                _game.SetKeyPressed(e.KeyCode.ToString(), false);
            }
        }

        private void OpenDialog(Keys key)
        {

            switch (key)
            {
                case Keys.Escape:
                    _pauseGame = true;
                    var messageWindow = new KeenReloadedYesNoDialogWindow("Are you sure you want to quit?", false);

                    var dialogResult = messageWindow.ShowDialog();
                    if (dialogResult == DialogResult.No)
                    {
                        _pauseGame = false;
                    }
                    else
                    {
                        this.Close();
                    }
                    break;
            }
        }
    }
}
