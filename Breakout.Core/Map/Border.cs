using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Screen;

namespace Breakout.Core.Map
{
    public class Border
    {
        public Image Image { get; set; }
        public Vector2 Position { get; set; }

        public void Initialize()
        {
            this.Position = Vector2.Zero;
            this.Image = new Image()
            {
                Path = "Sprites/dark_border_top",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
        }

        public void LoadContent()
        {
            //this.Image.Scale = new Vector2(0.5f, 0.5f);
            //this.Image.SourceRectangle = new Rectangle(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);
            this.Image.LoadContent();
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
            //spriteBatch.GraphicsDevice.Clear(Color.Black);

            int x = 0;
            while ( x < ScreenManager.Instance.Dimensions.X )
            { 
                this.Image.Position = new Vector2(x, 600);
                this.Image.Draw(spriteBatch);

                x += 40;
            }
        }
    }
}
