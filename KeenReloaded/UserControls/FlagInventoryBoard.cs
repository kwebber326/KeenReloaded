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

namespace KeenReloaded.UserControls
{
    public partial class FlagInventoryBoard : UserControl
    {
        private const int VERTICAL_OFFSET = 20;
        private const int HORIZONAL_MARGIN = 2;
        private const int MARGIN_BETWEEN_TITLE_AND_FLAGS = 4;

        private CommanderKeen _keen;
        private List<Flag> _flags;
        private List<FlagColorScoreKeeper> _flagImages;

        public FlagInventoryBoard()
        {
            InitializeComponent();
            _flagImages = new List<FlagColorScoreKeeper>();
            _flags = new List<Flag>();
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
                    _keen.KeenAcquiredItem += new EventHandler<Framework.KeenEventArgs.ItemAcquiredEventArgs>(_keen_KeenAcquiredItem);
                    _keen.ItemLost += new EventHandler<Framework.KeenEventArgs.ItemAcquiredEventArgs>(_keen_ItemLost);
                }
            }
        }

        public void Reset()
        {
            foreach (var item in _flagImages)
            {
                this.Controls.Remove(item);
            }
            _flagImages.Clear();
            _flags.Clear();
        }

         private void _keen_ItemLost(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
         {
            var flag = e.Item as Flag;
            if (flag != null)
            {
                var scoreKeeper = _flagImages.FirstOrDefault(f => f.Color == flag.Color);
                if (scoreKeeper != null)
                {
                    int indexOfScoreBoard = _flagImages.IndexOf(scoreKeeper);
                    UpdateFlagScoreKeeperPositionsOnBoard(indexOfScoreBoard);
                    _flagImages.Remove(scoreKeeper);
                    _flags.RemoveAll(f => f.Color == flag.Color);
                    this.Controls.Remove(scoreKeeper);
                }
            }
        }

        private void _keen_KeenAcquiredItem(object sender, Framework.KeenEventArgs.ItemAcquiredEventArgs e)
        {
            var flag = e.Item as Flag;
            if (flag != null)
            {
                var flagColor = flag.Color;
                if (_flags.Any(f => f.Color == flagColor))
                {
                    _flags.Add(flag);
                    UpdateColorFlagScore(flagColor);
                }
                else
                {
                    FlagColorScoreKeeper scoreKeeper = new FlagColorScoreKeeper(flagColor, flag.CurrentPointValue);                 
                    AddNewFlagScoreToBoard(scoreKeeper);
                    _flags.Add(flag);
                }
                
            }
        }

        private void UpdateFlagScoreKeeperPositionsOnBoard(int indexOfScoreBoard)
        {
            for (int i = indexOfScoreBoard + 1; i < _flagImages.Count; i++)
            {
                var scoreKeeper = _flagImages[i];
                int xPos = scoreKeeper.Location.X;
                int yPos = scoreKeeper.Location.Y - scoreKeeper.Height;
                scoreKeeper.Location = new Point(xPos, yPos);
            }
        }


        private void UpdateColorFlagScore(Framework.Enums.GemColor flagColor)
        {
            var flagBoard = _flagImages.FirstOrDefault(f => f.Color == flagColor);
            var flags = _flags.Where(f => f.Color == flagColor).ToList();
            int score = flags
                .Select(f1 => f1.CurrentPointValue)
                .Sum();
            int count = flags.Count;
            flagBoard.Score = score;
            flagBoard.FlagCount = count;
        }

        private void AddNewFlagScoreToBoard(FlagColorScoreKeeper scoreKeeper)
        {
            this.Controls.Add(scoreKeeper);
            int currentFlagCount = _flagImages.Count;
            int xPos = HORIZONAL_MARGIN;
            int yPos = VERTICAL_OFFSET + MARGIN_BETWEEN_TITLE_AND_FLAGS + (currentFlagCount * scoreKeeper.Height); ;
            scoreKeeper.Location = new Point(xPos, yPos);
            this.Controls.Add(scoreKeeper);
            _flagImages.Add(scoreKeeper);
        }
    }
}
