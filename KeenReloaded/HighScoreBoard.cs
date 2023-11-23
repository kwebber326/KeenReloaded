using KeenReloaded.Framework.Enums;
using KeenReloaded.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded
{
    public partial class HighScoreBoard : Form
    {
        private Dictionary<char, Image> _characterImageDict;
        private List<HighScore> _scores;
        private readonly string _mapName;
        private readonly GameModeEnum _gameMode;
        private const int INITIAL_VERTICAL_OFFSET = 106;
        private const int INITIAL_HORIZONTAL_OFFSET = 64;
        private const int COLUMN_OFFSET = 64;
        private const int ROW_OFFSET = 18;
        private const int CHARACTER_SPACE_OFFSET = 4;
        private const int CHARACTER_HEIGHT = 14;
        private const int NAME_CHARACTER_LIMIT = 20;

        public HighScoreBoard(List<HighScore> highScores, string mapName, GameModeEnum gameMode)
        {
            if (highScores == null)
                throw new ArgumentNullException("high score list cannot be null");
            if (string.IsNullOrWhiteSpace(mapName))
                throw new ArgumentException("Invalid map name");

            _scores = highScores;
            _mapName= mapName;
            _gameMode = gameMode;
         

            InitializeComponent();
            InititalizeDictionary();
            InitializeHighScoreBoard();
        }

        private void InititalizeDictionary()
        {
            _characterImageDict = new Dictionary<char, Image>()
            {
                { 'a', Properties.Resources.a },
                { 'b', Properties.Resources.b },
                { 'c', Properties.Resources.c },
                { 'd', Properties.Resources.d },
                { 'e', Properties.Resources.e },
                { 'f', Properties.Resources.f },
                { 'g', Properties.Resources.g },
                { 'h', Properties.Resources.h },
                { 'i', Properties.Resources.i },
                { 'j', Properties.Resources.j },
                { 'k', Properties.Resources.k },
                { 'l', Properties.Resources.l },
                { 'm', Properties.Resources.m },
                { 'n', Properties.Resources.n },
                { 'o', Properties.Resources.o },
                { 'p', Properties.Resources.p },
                { 'q', Properties.Resources.q },
                { 'r', Properties.Resources.r },
                { 's', Properties.Resources.s },
                { 't', Properties.Resources.t },
                { 'u', Properties.Resources.u },
                { 'v', Properties.Resources.v },
                { 'w', Properties.Resources.w },
                { 'x', Properties.Resources.x },
                { 'y', Properties.Resources.y },
                { 'z', Properties.Resources.z },
                { 'A', Properties.Resources.A_ },
                { 'B', Properties.Resources.B_ },
                { 'C', Properties.Resources.C_ },
                { 'D', Properties.Resources.D_ },
                { 'E', Properties.Resources.E_ },
                { 'F', Properties.Resources.F_ },
                { 'G', Properties.Resources.G_ },
                { 'H', Properties.Resources.H_ },
                { 'I', Properties.Resources.I_ },
                { 'J', Properties.Resources.J_ },
                { 'K', Properties.Resources.K_ },
                { 'L', Properties.Resources.L_ },
                { 'M', Properties.Resources.M_ },
                { 'N', Properties.Resources.N_ },
                { 'O', Properties.Resources.O_ },
                { 'P', Properties.Resources.P_ },
                { 'Q', Properties.Resources.Q_ },
                { 'R', Properties.Resources.R_ },
                { 'S', Properties.Resources.S_ },
                { 'T', Properties.Resources.T_ },
                { 'U', Properties.Resources.U_ },
                { 'V', Properties.Resources.V_ },
                { 'W', Properties.Resources.W_ },
                { 'X', Properties.Resources.X_ },
                { 'Y', Properties.Resources.Y_ },
                { 'Z', Properties.Resources.Z_ },
                { '1', Properties.Resources._1 },
                { '2', Properties.Resources._2 },
                { '3', Properties.Resources._3 },
                { '4', Properties.Resources._4 },
                { '5', Properties.Resources._5 },
                { '6', Properties.Resources._6 },
                { '7', Properties.Resources._7 },
                { '8', Properties.Resources._8 },
                { '9', Properties.Resources._9 },
                { '0', Properties.Resources._0 },
                { '!', Properties.Resources.sc_exclamation_point },
                { '@', Properties.Resources.sc_at_symbol },
                { '#', Properties.Resources.sc_hashtag },
                { '$', Properties.Resources.sc_dollar_sign },
                { '%', Properties.Resources.sc_percent_sign },
                { '.', Properties.Resources.sc_period },
                { '&', Properties.Resources.sc_and_sign },
                { '*', Properties.Resources.sc_star },
                { '(', Properties.Resources.sc_left_parenthesis },
                { ')', Properties.Resources.sc_right_parenthesis },
                { '[', Properties.Resources.sc_left_bracket },
                { ']', Properties.Resources.sc_right_bracket },
                { '{', Properties.Resources.sc_left_bracket },
                { '}', Properties.Resources.sc_right_bracket },
                { '-', Properties.Resources.sc_dash },
                { ':', Properties.Resources.sc_colon },
                { ';', Properties.Resources.sc_semicolon },
                { '_', Properties.Resources.sc_underscore },
                { '+', Properties.Resources.sc_plus },
                { '<', Properties.Resources.sc_less_than_caret },
                { '>', Properties.Resources.sc_greater_than_caret },
                { '=', Properties.Resources.sc_equals },
                { '?', Properties.Resources.sc_question_mark },
                { '`',Properties.Resources.sc_left_single_quote },
                { '\'', Properties.Resources.sc_right_single_quote },
                { '~', Properties.Resources.sc_tilde },
                { '/', Properties.Resources.sc_forward_slash }
            };
        }

        private void InitializeHighScoreBoard()
        {
            int yPos = INITIAL_VERTICAL_OFFSET;
            int xPos = INITIAL_HORIZONTAL_OFFSET;
            
            foreach (var highScore in _scores)
            {
                string name = highScore.Name.Length > NAME_CHARACTER_LIMIT ? highScore.Name.Substring(0, NAME_CHARACTER_LIMIT) : highScore.Name;
                string score = _gameMode == GameModeEnum.ZOMBIE || _gameMode == GameModeEnum.KING_OF_THE_HILL || _gameMode == GameModeEnum.CAPTURE_THE_FLAG ? highScore.Score.ToString() : highScore.Time.ToString();
                //write name in first column
                foreach (char c in name)
                {
                    if (c != ' ')
                    {
                        PictureBox p = new PictureBox();
                        p.Location = new Point(xPos, yPos);
                        p.SizeMode = PictureBoxSizeMode.AutoSize;
                        if (_characterImageDict.TryGetValue(c, out Image image))
                        {
                            p.Image = image;
                            this.Controls.Add(p);
                            p.BringToFront();
                        }


                        xPos += p.Width + CHARACTER_SPACE_OFFSET;
                    }
                    else
                    {
                        xPos += CHARACTER_SPACE_OFFSET * 2;
                    }
                }
                xPos += COLUMN_OFFSET;
                foreach (char c in score)
                {
                    if (c != ' ')
                    {
                        PictureBox p = new PictureBox();
                        p.Location = new Point(xPos, yPos);
                        p.SizeMode = PictureBoxSizeMode.AutoSize;
                        if (_characterImageDict.TryGetValue(c, out Image image))
                        {
                            p.Image = image;
                            this.Controls.Add(p);
                            p.BringToFront();
                        }


                        xPos += p.Width + CHARACTER_SPACE_OFFSET;
                    }
                    else
                    {
                        xPos += CHARACTER_SPACE_OFFSET * 2;
                    }
                }

                yPos += CHARACTER_HEIGHT + ROW_OFFSET;
                xPos = INITIAL_HORIZONTAL_OFFSET;
            }
        }

        private void HighScoreBoard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void HighScoreBoard_Load(object sender, EventArgs e)
        {
            this.Text = _mapName;
        }
    }
}
