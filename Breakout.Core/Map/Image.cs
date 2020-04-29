using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Sprites;
using Breakout.Core.Screen;

namespace Breakout.Core.Map
{
    public class Image : IUnit
    {
        public float Alpha;
        public string Text, FontName, Path;

        private Texture2D _texture;

        public Texture2D Texture 
        { 
            get
            {
                if (_texture != null)
                {
                    return _texture;
                }
                else
                {
                    return TextureManager.Instance[this.TextureMap].Texture;
                }
            }
            set
            {
                _texture = value;
            }
        }
        public Vector2 Position, Scale;
        public Rectangle SourceRectangle { get; set; }

        public int TextureMap { get; set; }
        public int TextureIndex { get; set; }

        Vector2 origin;
        ContentManager content;
        SpriteFont font;

        private TextureMapper _textureMapper;

        public TextureMapper TextureMapper
        {
            get 
            {
                if (_textureMapper == null)
                    _textureMapper = new TextureMapper();
                return _textureMapper; 
            }
            set { _textureMapper = value; }
        }
        
        public Image()
        {
            Path = String.Empty;
            FontName = "Fonts/TobagoPoster";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRectangle = Rectangle.Empty;
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (!string.IsNullOrEmpty(this.Path))
            {
                this.Texture = content.Load<Texture2D>(this.Path);

                if (!string.IsNullOrEmpty(this.FontName))
                    font = content.Load<SpriteFont>(FontName);

                Vector2 dimensions = Vector2.Zero;

                if (this.Texture != null)
                    dimensions.X += this.Texture.Width;
                if ((font != null) && (Text != null))
                    dimensions.X += font.MeasureString(Text).X;

                if (this.Texture != null)
                    if ((font != null) && (Text != null))
                        dimensions.Y = Math.Max(this.Texture.Height, font.MeasureString(Text).Y);
                    else
                        dimensions.Y = this.Texture.Height;
                else
                    if (font != null)
                        dimensions.Y = font.MeasureString(Text).Y;

                if (SourceRectangle == Rectangle.Empty)
                    SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    if (!string.IsNullOrEmpty(this.FontName))
                        font = content.Load<SpriteFont>(FontName);
                }
                this.SourceRectangle = TextureManager.Instance[this.TextureMap][this.TextureIndex];
            }
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

            spriteBatch.Draw(this.Texture, this.Position + origin, SourceRectangle, Color.White * this.Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
            if (!string.IsNullOrEmpty(this.Text))
            {
                spriteBatch.DrawString(font, this.Text, this.Position + new Vector2(origin.X - ( font.MeasureString(Text).X / 2 ), 8.0f), Color.Black);
            }
        }

        public virtual Rectangle Bounds
        {
            get
            {
                var position = this.Position;// +origin;
                return new Rectangle((int)position.X, (int)position.Y, SourceRectangle.Width, SourceRectangle.Height);
            }
        }
    }
}

