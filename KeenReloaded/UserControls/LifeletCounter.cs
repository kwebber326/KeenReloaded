using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework;

namespace KeenReloaded.UserControls
{
    public partial class LifeletCounter : UserControl
    {
        private CommanderKeen _keen;
        private const int INITIAL_LIFE_HORIZONTAL_OFFSET = 52;
        private const int LIFE_DIGIT_COUNT = 2;
        private const int DISTANCE_BETWEEN_LED_DIGITS = 4;
        private const int LIFE_VERTICAL_OFFSET = 23;
        List<PictureBox> _lifeCountLEDImages = new List<PictureBox>();

        Dictionary<char, Image> _digitLEDs = new Dictionary<char, Image>();

        private void UpdateDrops()
        {
            int drops = _keen.Drops;
            if (drops >= 99)
            {
                foreach (var digit in _lifeCountLEDImages)
                {
                    digit.Image = _digitLEDs['9'];
                }
            }
            else
            {
                string livesString = drops.ToString();
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

        public LifeletCounter()
        {
            InitializeComponent();
        }

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
                    _keen.LifeDropsChanged += new EventHandler<Framework.KeenEventArgs.ObjectEventArgs>(_keen_LifeDropsChanged);
                }
            }
        }

        void _keen_LifeDropsChanged(object sender, Framework.KeenEventArgs.ObjectEventArgs e)
        {
            UpdateDrops();   
        }

        private void LifeletCounter_Load(object sender, EventArgs e)
        {
            InitializeLEDDictionary();
            InitializeLifeBoard();
        }

        private void InitializeLifeBoard()
        {
            int currentXPos = pBoxBoard.Location.X + INITIAL_LIFE_HORIZONTAL_OFFSET;
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
    }
}
