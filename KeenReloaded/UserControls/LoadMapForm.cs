using KeenReloaded.Framework.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.UserControls
{
    public partial class LoadMapForm : Form
    {
        private readonly string _selectedCharacter;
        private readonly GameModeEnum _gameMode;

        public LoadMapForm(GameModeEnum gameMode, string selectedCharacter)
        {
            _selectedCharacter = selectedCharacter;
            _gameMode = gameMode;
            InitializeComponent();
        }

        public string LoadedMapName
        {
            get
            {
                return lblMapName.Text;
            }
        }

        private void LoadMapForm_Load(object sender, EventArgs e)
        {
            string folder = GetFolder();
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + $@"\{folder}";
            SetFormTitle();
        }

        private string GetMapName()
        {
            return openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('\\') + 1);
        }

        private string GetFolder()
        {
            switch (_gameMode)
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

        private void SetFormTitle()
        {
            string rawGameModeText = _gameMode.ToString();
            string firstLetter = rawGameModeText.Substring(0, 1);
            string rest = rawGameModeText.Substring(1);
            firstLetter = firstLetter.ToUpper();
            rest = rest.ToLower();
            rest = rest.Replace("_", " ");
            string gameModeText = firstLetter + rest;
            label1.Text = $"Choose { gameModeText } Map: ";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                btnPlayMap.Enabled = true;
              
                lblMapName.Text = GetMapName().Replace(".txt", "");
            }
        }

        private void BtnPlayMap_Click(object sender, EventArgs e)
        {
            string mapName = GetMapName();
            Form1 frm = new Form1(mapName, _gameMode, _selectedCharacter);
            frm.ShowDialog();
            this.DialogResult = DialogResult.OK;
            this.Close();
           
        }
    }
}
