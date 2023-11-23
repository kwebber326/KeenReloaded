using KeenReloaded.Framework;
using KeenReloaded.Framework.AltCharacters;
using KeenReloaded.Framework.Enums;
using KeenReloaded.HelperClasses;
using KeenReloaded.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KeenReloaded
{
    public partial class MainMenu : Form
    {
        private List<MenuOption> _options;
        private int _currentSelectedOptionIndex;
        private bool _delayed;
        private const int OPTION_MOVE_DELAY_MILLISECONDS = 100;
        private const int BUTTON_COLOR_CHANGE_TIME = 100;
        private Timer _delayTimer;
        private Timer _buttonColorChangeTimer;
        private List<string> _availableCharacters;
        private string _selectedCharacter;
        private Dictionary<string, Image> _selectionImages = new Dictionary<string, Image>()
        {
            { nameof(CommanderKeen), Properties.Resources.keen_shoot_right_standing },
            { nameof(PrincessIndi), Properties.Resources.princess_indi_shoot_right_standing },
            { nameof(BabyLouie), Properties.Resources.baby_louie_shoot_right_standing },
            { nameof(Locoyorp), Properties.Resources.yorp_shoot_right_standing },
            { nameof(KChirps), Properties.Resources.keen_dreams_shoot_right_standing },
            { nameof(MortimerMcMire), Properties.Resources.mm_shoot_right_standing },
            { nameof(OracleElder), Properties.Resources.oracle_elder_shoot_right_standing },
            { nameof(CouncilPage), Properties.Resources.council_page_shoot_right_standing }
        };
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            _delayTimer = new Timer();
            _delayTimer.Interval = OPTION_MOVE_DELAY_MILLISECONDS;
            _delayTimer.Tick += _delayTimer_Tick;

            _buttonColorChangeTimer = new Timer();
            _buttonColorChangeTimer.Interval = BUTTON_COLOR_CHANGE_TIME;
            _buttonColorChangeTimer.Tick += _buttonColorChangeTimer_Tick;
            LoadCharacterSelection();
            _options = new List<MenuOption>()
            {
                new MenuOption("Play Normal Mode", OpenNormalMode, _selectionImages, _selectedCharacter),
                new MenuOption("Play Zombie Mode", OpenZombieMode, _selectionImages, _selectedCharacter),
                new MenuOption("Play King of the Hill", OpenKingOfTheHillMode, _selectionImages, _selectedCharacter),
                new MenuOption("Play MapMaker Mode", OpenMapMakerMode, _selectionImages, _selectedCharacter),
                new MenuOption("Play Capture The Flag", OpenCTFMode, _selectionImages, _selectedCharacter)
            };

            int yPos = 0;
            for (int i = 0; i < _options.Count; i++)
            {
                _options[i].Location = new Point(0, yPos);
                yPos += _options[i].Height;
                _options[i].Selected += MenuOption_Selected;
            }
            _options[0].SelectOption();
            foreach (var option in _options)
            {
                pnlMenuOptions.Controls.Add(option);
            }

           
        }

        private void _buttonColorChangeTimer_Tick(object sender, EventArgs e)
        {
            btnRandomCharacter.ForeColor = Color.Lime;
            _buttonColorChangeTimer.Stop();
        }

        private void LoadCharacterSelection()
        {

            PopulateAvailableCharacters();
            InitializeSelection();
        }

        private void InitializeSelection()
        {
            try
            {
                string fullPath = Environment.CurrentDirectory + @"\Characters\SelectedCharacter.txt";
                using (FileStream fs = File.OpenRead(fullPath))
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    if (!streamReader.EndOfStream)
                    {
                        _selectedCharacter = streamReader.ReadLine();
                    }
                }

                if (_availableCharacters.Any())
                {
                    int selectedIndex = cmbCharacters.Items.IndexOf(_selectedCharacter);
                    if (selectedIndex != -1)
                    {
                        cmbCharacters.SelectedIndex = selectedIndex;
                        cmbCharacters.Text = _selectedCharacter;
                    }
                    else
                    {
                        cmbCharacters.SelectedIndex = 0;
                        cmbCharacters.Text = cmbCharacters.Items[0]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing selected character: ", ex.Message);
            }
        }

        private void PopulateAvailableCharacters()
        {
            try
            {
                string fullPath = Environment.CurrentDirectory + @"\Characters\AvailableCharacters.txt";
                _availableCharacters = new List<string>();
                using (FileStream fs = File.OpenRead(fullPath))
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string character = streamReader.ReadLine();
                        _availableCharacters.Add(character);
                    }
                }
                cmbCharacters.Items.AddRange(_availableCharacters.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading characters: " + ex.Message);
            }
        }

        private void UpdateSelectedCharacter()
        {
            if (_options != null && _options.Any())
            {
                var selectedOption = _options.FirstOrDefault(o => o.IsSelected);
                foreach (var option in _options)
                {
                    option.SelectedCharacter = _selectedCharacter;
                }
                if (selectedOption != null)
                {
                    selectedOption.UpdateSelectionImage();
                }
            }
        }

        private void SaveCharacterSelectionToFile()
        {
            try
            {
                string fullPath = Environment.CurrentDirectory + @"\Characters\SelectedCharacter.txt";
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine(_selectedCharacter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving character selection: " + ex.Message);
            }
        }

        private void _delayTimer_Tick(object sender, EventArgs e)
        {
            _delayed = false;
            _delayTimer.Stop();
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
                case Keys.Enter:
                    KeyEventArgs args = new KeyEventArgs(keyData);
                    base.OnKeyDown(args);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MenuOption_Selected(object sender, EventArgs e)
        {
            var option = sender as MenuOption;
            if (option != null)
            {
                var otherItems = _options.Where(i => i != option).ToList();
                foreach (var item in otherItems)
                {
                    item.DeselectOption();
                }
            }
        }

        private void OpenZombieMode()
        {
            LoadMapForm form = new LoadMapForm(GameModeEnum.ZOMBIE, _selectedCharacter);
            if (form.ShowDialog() == DialogResult.OK && ReadHighScores(out List<HighScore> scores, GameModeEnum.ZOMBIE, form.LoadedMapName))
            {
                ShowHighScoreBoard(scores, form.LoadedMapName, GameModeEnum.ZOMBIE);
            }
        }

        private void OpenNormalMode()
        {
            LoadMapForm form = new LoadMapForm(GameModeEnum.NORMAL, _selectedCharacter);
            if (form.ShowDialog() == DialogResult.OK && ReadHighScores(out List<HighScore> scores, GameModeEnum.NORMAL, form.LoadedMapName))
            {
                ShowHighScoreBoard(scores, form.LoadedMapName, GameModeEnum.NORMAL);
            }
        }
        private void ShowHighScoreBoard(List<HighScore> highScores, string mapName, GameModeEnum gameMode)
        {
            HighScoreBoard hsb = new HighScoreBoard(highScores, mapName, gameMode);
            hsb.BringToFront();
            hsb.Show();
        }
        private bool ReadHighScores(out List<HighScore> highScores, GameModeEnum gameMode, string mapName)
        {
            highScores = new List<HighScore>();
            try
            {
                string fullPath = Environment.CurrentDirectory;
                if (gameMode == GameModeEnum.NORMAL)
                {
                    fullPath += $@"\NormalModeTimes\{mapName}.txt";
                }
                else if (gameMode == GameModeEnum.ZOMBIE)
                {
                    fullPath += $@"\ZombieModeScores\{mapName}.txt";
                }
                else if (gameMode == GameModeEnum.KING_OF_THE_HILL)
                {
                    fullPath += $@"\KingOfTheHillScores\{mapName}.txt";
                }
                else if (gameMode == GameModeEnum.CAPTURE_THE_FLAG)
                {
                    fullPath += $@"\CTFScores\{mapName}.txt";
                }

                if (!File.Exists(fullPath))
                {
                    File.Create(fullPath);
                }

                using (var stream = File.OpenRead(fullPath))
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string[] rawData = streamReader.ReadLine().Split('|');
                        HighScore score = new HighScore()
                        {
                            Name = rawData[0],
                            Score = Convert.ToInt64(rawData[1]),
                            Time = TimeSpan.Parse(rawData[2])
                        };

                        highScores.Add(score);
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

        private void OpenMapMakerMode()
        {
            MapMaker mapMaker = new MapMaker(_selectedCharacter);
            mapMaker.ShowDialog();
        }

        private void OpenKingOfTheHillMode()
        {
            LoadMapForm form = new LoadMapForm(GameModeEnum.KING_OF_THE_HILL, _selectedCharacter);
            if (form.ShowDialog() == DialogResult.OK && ReadHighScores(out List<HighScore> scores, GameModeEnum.KING_OF_THE_HILL, form.LoadedMapName))
            {
                ShowHighScoreBoard(scores, form.LoadedMapName, GameModeEnum.KING_OF_THE_HILL);
            }
        }

        private void OpenCTFMode()
        {
            var gameMode = GameModeEnum.CAPTURE_THE_FLAG;
            LoadMapForm form = new LoadMapForm(gameMode, _selectedCharacter);
            if (form.ShowDialog() == DialogResult.OK && ReadHighScores(out List<HighScore> scores, gameMode, form.LoadedMapName))
            {
                ShowHighScoreBoard(scores, form.LoadedMapName, gameMode);
            }
        }

        private void MoveDown()
        {
            if (_currentSelectedOptionIndex >= _options.Count - 1)
            {
                _currentSelectedOptionIndex = 0;
            }
            else
            {
                _currentSelectedOptionIndex++;
            }
            _options[_currentSelectedOptionIndex].SelectOption();
            _options[_currentSelectedOptionIndex].Focus();
        }

        private void MoveUp()
        {
            if (_currentSelectedOptionIndex <= 0)
            {
                _currentSelectedOptionIndex = _options.Count - 1;
            }
            else
            {
                _currentSelectedOptionIndex--;
            }
            _options[_currentSelectedOptionIndex].SelectOption();
            _options[_currentSelectedOptionIndex].Focus();
        }

        private void MainMenu_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!_delayed)
                    {
                        _delayed = true;
                        _delayTimer.Start();
                        MoveUp();
                    }
                    break;
                case Keys.Down:
                    if (!_delayed)
                    {
                        _delayed = true;
                        _delayTimer.Start();
                        MoveDown();
                    }
                    break;
                case Keys.Enter:
                    _options[_currentSelectedOptionIndex].InvokeAction();
                    break;
            }
        }

        private void CmbCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedCharacter = cmbCharacters.SelectedItem?.ToString();

            UpdateSelectedCharacter();
            SaveCharacterSelectionToFile();
            pnlMenuOptions.Focus();
        }

        private void BtnRandomCharacter_Click(object sender, EventArgs e)
        {
            SelectRandomCharacter();
        }

        private void SelectRandomCharacter()
        {
            int count = cmbCharacters.Items?.Count ?? 0;
            btnRandomCharacter.ForeColor = Color.Red;
            _buttonColorChangeTimer.Start();
            if (count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, count);
                cmbCharacters.SelectedIndex = randomIndex;
            }
        }
    }
}
