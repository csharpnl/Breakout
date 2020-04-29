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
    public class Background : IUnit
    {
        public Image Image { get; set; }
        public Vector2 Position { get; set; }

        public void Initialize()
        {
            this.Position = Vector2.Zero;
            this.Image = new Image()
            {
                Path = "Sprites/breakout_bg",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
        }

        public void LoadContent()
        {
            //this.Image.Scale = new Vector2(0.5f, 0.5f);
            this.Image.LoadContent();
            this.Image.SourceRectangle = new Rectangle(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);
        }

        public void UnloadContent()
        {
            this.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            //this.Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            //this.Image.Draw(spriteBatch);
        }
    }
}
