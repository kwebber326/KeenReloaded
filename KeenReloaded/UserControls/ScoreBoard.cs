using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework.Weapons;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.UserControls
{
    public partial class ScoreBoard : UserControl
    {
        private CommanderKeen _keen;

        List<PictureBox> _scoreLEDImages = new List<PictureBox>();
        List<PictureBox> _lifeCountLEDImages = new List<PictureBox>();
        List<PictureBox> _ammoCountLEDImages = new List<PictureBox>();

        Dictionary<char, Image> _digitLEDs = new Dictionary<char, Image>();

        private const int INITIAL_SCORE_HORIZONTAL_OFFSET = 12;
        private const int SCORE_VERITICAL_OFFSET = 11;
        private const int DISTANCE_BETWEEN_LED_DIGITS = 4;
        private const int SCORE_BOARD_DIGIT_COUNT = 9;

        private const int INITIAL_LIFE_HORIZONTAL_OFFSET = 44;
        private const int LIFE_VERTICAL_OFFSET = 42;
        private const int LIFE_DIGIT_COUNT = 2;

        private const int INITIAL_AMMO_HORIZONTAL_OFFSET = 124;
        private const int AMMO_VERTICAL_OFFSET = 42;
        private const int AMMO_DIGIT_COUNT = 2;

        public CommanderKeen Keen
        {
            get
            {
                return _keen;
            }
            set
            {
                _keen = value;
                if (_keen != null)
                {
                    _keen.KeenAcquiredItem += new EventHandler<Framework.KeenEventArgs.ItemAcquiredEventArgs>(_keen_KeenAcquiredItem);
                    _keen.KeenAcquiredWeapon += new EventHandler<Framework.KeenEventArgs.WeaponAcquiredEventArgs>(_keen_KeenAcquiredWeapon);
                    _keen.ScoreChanged += new EventHandler<Framework.KeenEventArgs.ObjectEventArgs>(_keen_ScoreChanged);
                    _keen.WeaponChanged += new EventHandler<Framework.KeenEventArgs.ObjectEventArgs>(_keen_WeaponChanged);
                    _keen.LivesChanged += new EventHandler<Framework.KeenEventArgs.ObjectEventArgs>(_keen_LivesChanged);
                    weaponInventoryBoard1.Keen = _keen;
                    keyInventoryBoard1.Keen = _keen;
                    lifeletCounter1.Keen = _keen;
                    keyCardInventoryBoard1.Keen = _keen;
                    shieldInventoryItem1.Keen = _keen;
                    flagInventoryBoard1.Keen = _keen;
                    UpdateLives();
                    UpdateScore();
                    UpdateAmmo();
                }
            }
        }

        public Shield Shield
        {
            get
            {
                return shieldInventoryItem1.Shield;
            }
        }

        public void DetachKeenEvents()
        {
            _keen.KeenAcquiredItem -= _keen_KeenAcquiredItem;
            _keen.KeenAcquiredWeapon -= _keen_KeenAcquiredWeapon;
            _keen.ScoreChanged -= _keen_ScoreChanged;
            _keen.WeaponChanged -= _keen_WeaponChanged;
            _keen.LivesChanged -= _keen_LivesChanged;
        }

        void _keen_LivesChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            UpdateLives();
        }

      

        public ScoreBoard()
        {
            InitializeComponent();
        }

        private void ScoreBoard_Load(object sender, EventArgs e)
        {
            InitializeLEDDictionary();
            InitializeScoreboard();
            InitializeLifeBoard();
            InitializeAmmoBoard();
        }

        private void InitializeLEDDictionary()
        {
            _digitLEDs.Add('x', Properties.Resources.scoreboard_default_LED);
            _digitLEDs.Add('0', Properties.Resources.scoreboard_LED_zero);
            _digitLEDs.Add('1', Properties.Resources.scoreboard_LED_one);
            _digitLEDs.Add('2', Properties.Resources.scoreboard_LED_two);
            _digitLEDs.Add('3', Properties.Resources.scoreboard_LED_three);
            _digitLEDs.Add('4', Properties.Resources.scoreboard_LED_four);
            _digitLEDs.Add('5', Properties.Resources.scoreboard_LED_five);
            _digitLEDs.Add('6', Properties.Resources.scoreboard_LED_six);
            _digitLEDs.Add('7', Properties.Resources.scoreboard_LED_seven);
            _digitLEDs.Add('8', Properties.Resources.scoreboard_LED_eight);
            _digitLEDs.Add('9', Properties.Resources.scoreboard_LED_nine);
        }

        void _keen_WeaponChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            UpdateAmmo();
        }

        void _keen_ScoreChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            long score = _keen.Points;
            if (score >= 999999999)
            {
                foreach (var digit in _scoreLEDImages)
                {
                    digit.Image = _digitLEDs['9'];
                }
            }
            else
            {
                string scoreString = score.ToString();
                int currentDigit = 0;
                for (int i = 0; i < SCORE_BOARD_DIGIT_COUNT; i++)
                {
                    if (SCORE_BOARD_DIGIT_COUNT - i > scoreString.Length)
                    {
                        _scoreLEDImages[i].Image = _digitLEDs['x'];
                    }
                    else
                    {
                        _scoreLEDImages[i].Image = _digitLEDs[scoreString[currentDigit++]];
                    }
                }
            }
        }

        public void SetBoardForGameMode(GameModeEnum gameMode)
        {
            flagInventoryBoard1.Visible = gameMode == GameModeEnum.CAPTURE_THE_FLAG;
        }

        public void ResetGems()
        {
            keyInventoryBoard1.Reset();
        }

        public void ResetFlags()
        {
            flagInventoryBoard1.Reset();
        }

        public void ResetShield(Shield shield)
        {
            shieldInventoryItem1.ResetShield(shield);
        }

        private void UpdateLives()
        {
            int  lives = _keen.Lives;
            if (lives < 0)
                lives = 0;

            if (lives >= 99)
            {
                foreach (var digit in _lifeCountLEDImages)
                {
                    digit.Image = _digitLEDs['9'];
                }
            }
            else
            {
                string livesString = lives.ToString();
                int currentDigit = 0;
                for (int i = 0; i < LIFE_DIGIT_COUNT; i++)
                {
                    if (LIFE_DIGIT_COUNT - i > livesString.Length)
                    {
                        _lifeCountLEDImages[i].Image = _digitLEDs['x'];
                    }
                    else
                    {
                        _lifeCountLEDImages[i].Image = _digitLEDs[livesString[currentDigit++]];
                    }
                }
            }
        }

        private void UpdateAmmo()
        {
            int ammo = _keen.Ammo;
            if (ammo >= 99)
            {
                foreach (var digit in _ammoCountLEDImages)
                {
                    digit.Image = _digitLEDs['9'];
                }
            }
            else
            {
                string ammoString = ammo.ToString();
                int currentDigit = 0;
                for (int i = 0; i < AMMO_DIGIT_COUNT; i++)
                {
                    if (AMMO_DIGIT_COUNT - i > ammoString.Length)
                    {
                        _ammoCountLEDImages[i].Image = _digitLEDs['x'];
                    }
                    else
                    {
                        _ammoCountLEDImages[i].Image = _digitLEDs[ammoString[currentDigit++]];
                    }
                }
            }
        }

        void _keen_KeenAcquiredWeapon(object sender, Framework.KeenEventArgs.WeaponAcquiredEventArgs e)
        {
            UpdateAmmo();
        }

        void _keen_KeenAcquiredItem(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
        {
            if (e.Item is NeuralStunnerAmmo)
            {
                UpdateAmmo();
            }
        }

        private void InitializeScoreboard()
        {
            int currentXPos = pictureBoxScoreBoard.Location.X + INITIAL_SCORE_HORIZONTAL_OFFSET;
            for (int i = 0; i < SCORE_BOARD_DIGIT_COUNT; i++)
            {
                Image image = Properties.Resources.scoreboard_default_LED;
                PictureBox p = new PictureBox();
                p.Image = image;
                p.Location = new Point(currentXPos, SCORE_VERITICAL_OFFSET);
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = image;
                _scoreLEDImages.Add(p);
                this.Controls.Add(p);
                currentXPos += p.Width + DISTANCE_BETWEEN_LED_DIGITS;
                p.BringToFront();
            }
        }

        private void InitializeLifeBoard()
        {
            int currentXPos = pictureBoxScoreBoard.Location.X + INITIAL_LIFE_HORIZONTAL_OFFSET;
            for (int i = 0; i < LIFE_DIGIT_COUNT; i++)
            {
                Image image = Properties.Resources.scoreboard_default_LED;
                PictureBox p = new PictureBox();
                p.Image = image;
                p.Location = new Point(currentXPos, LIFE_VERTICAL_OFFSET);
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = image;
                _lifeCountLEDImages.Add(p);
                this.Controls.Add(p);
                currentXPos += p.Width + DISTANCE_BETWEEN_LED_DIGITS;
                p.BringToFront();
            }
        }

        private void InitializeAmmoBoard()
        {
            int currentXPos = pictureBoxScoreBoard.Location.X + INITIAL_AMMO_HORIZONTAL_OFFSET;
            for (int i = 0; i < AMMO_DIGIT_COUNT; i++)
            {
                Image image = Properties.Resources.scoreboard_default_LED;
                PictureBox p = new PictureBox();
                p.Image = image;
                p.Location = new Point(currentXPos, AMMO_VERTICAL_OFFSET);
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = image;
                _ammoCountLEDImages.Add(p);
                this.Controls.Add(p);
                currentXPos += p.Width + DISTANCE_BETWEEN_LED_DIGITS;
                p.BringToFront();
            }
        }

        private void ShieldInventoryItem1_Load(object sender, EventArgs e)
        {

        }
    }
}