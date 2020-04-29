using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Core.Sprites
{
    public class TextureMapper
    {
        private Dictionary<int, Rectangle> _mapping;

        private Texture2D texture;
        public ContentManager Content { get; set; }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public TextureMapper()
        {
            _mapping = new Dictionary<int, Rectangle>();
        }

        public void LoadContent(ContentManager content, string path)
        {
            this.Content = content;
            if ( !string.IsNullOrEmpty(path))
                this.texture = content.Load<Texture2D>(path);

            // player
            //AddMap(0, new Rectangle(0, 200,  96, 24));   // small
            //AddMap(1, new Rectangle(0, 240, 112, 24));   // medium
            //AddMap(2, new Rectangle(0, 280, 128, 24));   // large
        }

        public void AddMap( int index, Rectangle rectangle )
        {
            _mapping.Add(index, rectangle);
        }

        public Rectangle this[int index]
        {
            get
            {
                return _mapping[index];
            }
        }

        public void UnloadContent()
        {
            this.texture.Dispose();
        }
    }
}
