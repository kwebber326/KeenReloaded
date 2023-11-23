using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.UserControls
{
    public partial class FlagColorScoreKeeper : UserControl
    {
        private readonly GemColor _color;
        private int _score;
        private int _flagCount;

        public FlagColorScoreKeeper(GemColor color, int initialScore, int flagCount = 1)
        {
            InitializeComponent();
            _color = color;
            _score = initialScore;
            _flagCount = flagCount;
            InitializeBoard();
        }

        public GemColor Color
        {
            get
            {
                return _color;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                UpdateScoreLabel();
            }
        }

        public int FlagCount
        {
            get
            {
                return _flagCount;
            }
            set
            {
                _flagCount = value;
                UpdateScoreLabel();
            }
        }

        private void InitializeBoard()
        {
            pbFlagImage.Image = SpriteSheet.UserControlSpriteSheet.FlagImages[(int)_color];
            UpdateScoreLabel();
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = $"x{this.FlagCount}: {this.Score}";
        }
    }
}
