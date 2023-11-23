using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class BackgroundSpriteCanvas : IBackground, ISprite
    {
        private int _zIndex;
        private PictureBox _sprite;
        private readonly Color _backColor;

        public BackgroundSpriteCanvas(Point location, int zIndex, string[] files, int columnCount, Color backgroundColor)
        {
            _zIndex = zIndex;
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = location;
            _backColor = backgroundColor;
            _sprite.Image = BitMapTool.CombineBitmap(files, columnCount, _backColor);
            _sprite.BackColor = backgroundColor;

        }

        public static string[] ParseFilesFromText(string text)
        {

            if (string.IsNullOrWhiteSpace(text))
                return new string[] { };

            text = text.Replace("[", "").Replace("]", "");

            string[] canvasItemStrings = text.Split(',');
            for (int i = 0; i < canvasItemStrings.Length; i++)
            {
                try
                {
                    
                    canvasItemStrings[i] = FileIOUtilities.GetResourcesPath() + canvasItemStrings[i];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error parsing image item for background canvas: {ex.Message}");
                }
            }
            return canvasItemStrings;
        }

    


        public int ZIndex => _zIndex;

        public PictureBox Sprite => _sprite;
    }

    public class BackgroundCanvasItem
    {
        public BackgroundCanvasItem(Rectangle area, Image img)
        {
            this.ImageArea = area;
            this.Image = img;
        }

        public Rectangle ImageArea { get; private set; }

        public Image Image { get; private set; }
    }
}
