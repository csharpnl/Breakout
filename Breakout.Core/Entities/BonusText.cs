using Breakout.Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Screen;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Breakout.Core.Messages;

namespace Breakout.Core.Entities
{
    public class BonusText : IUnit
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        
        private float fadeCounter;
        private int fadeFrame;
        private float opacity;
        private SpriteFont Font { get; set; }
        public void Initialize()
        {
            this.Position = Vector2.Zero;
        }

        public void LoadContent()
        {
            //this.Image.SourceRectangle = TextureManager.Instance[0][200];
            fadeCounter = 0;
            fadeFrame = 1500;
            opacity = 1.0f;
            var content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            Font = content.Load<SpriteFont>("Fonts/Unlearned");
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            this.Velocity += Acceleration;
            this.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10;

            fadeCounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (fadeCounter > fadeFrame)
            {
                var messenger = Mvx.Resolve<IMvxMessenger>();

                messenger.Publish<RemoveUnitMessage>(new RemoveUnitMessage(this));

                //fadeCounter = 0;
            }
            opacity = 1.0f - (fadeCounter / fadeFrame);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.Font, this.Text, this.Position, Color.White * opacity);
        }
    }
}
