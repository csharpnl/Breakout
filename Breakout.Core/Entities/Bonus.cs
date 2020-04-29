using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Breakout.Core.Sprites;
using Breakout.Core.Map;
using Breakout.Core.Screen;
using MvvmCross.Plugin.Messenger;
using Breakout.Core.Messages;
using Microsoft.Xna.Framework.Audio;
using Breakout.Core.Sound;
using MvvmCross;

namespace Breakout.Core.Entities
{
    public class Bonus : IUnit
    {
        public Image Image { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public enum BonusType
        {
            Multiball,
            //Faster,
            //Slower,
            Bigger,
            Smaller
        }

        public BonusType Type { get; set; }

        private Dictionary<BonusType, dynamic> _bonusDefinitions = new Dictionary<BonusType, dynamic>();
        private float frameCounter;
        private int switchFrame;
        private Vector2 currentFrame;
        private int framesImmune;

        public Bonus()
        {
            _bonusDefinitions = new Dictionary<BonusType, dynamic>();
            _bonusDefinitions.Add(BonusType.Multiball, new { TextureMap = 2, TextureIndex = 0 });
            //_bonusDefinitions.Add(BonusType.Faster, new { TextureMap = 3, TextureIndex = 0 });
            //_bonusDefinitions.Add(BonusType.Slower, new { TextureMap = 4, TextureIndex = 0 });
            _bonusDefinitions.Add(BonusType.Bigger, new { TextureMap = 5, TextureIndex = 0 });
            _bonusDefinitions.Add(BonusType.Smaller, new { TextureMap = 6, TextureIndex = 0 });
        }

        public void Initialize()
        {
            this.Position = Vector2.Zero;
            this.Image = new Image()
            {
                TextureMap = _bonusDefinitions[this.Type].TextureMap,
                TextureIndex = _bonusDefinitions[this.Type].TextureIndex,
                //Path = "Sprites/breakout_sprites(no shadow)_5",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
        }

        public void LoadContent()
        {
            //this.Image.SourceRectangle = TextureManager.Instance[0][200];
            frameCounter = 0;
            switchFrame = 80;
            currentFrame = new Vector2(0, 0);
            this.Image.LoadContent();
            this.Image.SourceRectangle = new Rectangle((int)currentFrame.X * 32, (int)currentFrame.Y, 32, 32);
        }

        public void UnloadContent()
        {
            this.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            this.Velocity += Acceleration;
            this.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10;

            int MaxX = (int)(ScreenManager.Instance.Dimensions.X - this.Image.SourceRectangle.Width);
            //int MaxX = ScreenManager.Instance.GraphicsDevice.Viewport.Width - this.Image.SourceRectangle.Width;
            int MinX = 0;
            int MaxY = (int)ScreenManager.Instance.Dimensions.Y;
            //int MaxY = ScreenManager.Instance.GraphicsDevice.Viewport.Height;

            // Check for bounce.
            if ((this.Position.X > MaxX) ||
                (this.Position.X < MinX) ||
                (this.Position.Y > MaxY)
               )
            {
                var messenger = Mvx.Resolve<IMvxMessenger>();

                messenger.Publish<RemoveBonusMessage>(new RemoveBonusMessage(this));
            }

            frameCounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if ( frameCounter > switchFrame )
            {
                currentFrame.X++;
                if  ( currentFrame.X * 32 > this.Image.Texture.Width )
                {
                    currentFrame.X = 0;
                };
                frameCounter = 0;
            }

            if (framesImmune > 0)
                framesImmune--;

            this.Image.SourceRectangle = new Rectangle((int)currentFrame.X * 32, (int)currentFrame.Y, 32, 32);
            this.Image.Position = this.Position;
            this.Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Image.Draw(spriteBatch);
        }

        public void CheckCollision(Player player)
        {
            // spawn bonus
            bool collision = false;
            if (framesImmune <= 0)
            {
                collision = player.Image.Bounds.Intersects(this.Image.Bounds);
                if (collision)
                {
                    framesImmune = 20;

                    var messenger = Mvx.Resolve<IMvxMessenger>();

                    messenger.Publish<ActionBonusMessage>(new ActionBonusMessage(this));
                }
            }
        }

        public static Bonus.BonusType RandomType()
        {
            var random = new Random();
            return (Bonus.BonusType)random.Next( 0, Enum.GetValues(typeof(Bonus.BonusType)).Length);
        }
    }
}
