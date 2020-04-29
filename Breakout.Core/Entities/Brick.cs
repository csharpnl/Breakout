using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Map;
using Breakout.Core.Sprites;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Breakout.Core.Messages;
using Microsoft.Xna.Framework.Audio;
using Breakout.Core.Screen;

namespace Breakout.Core.Entities
{
    public class Brick : IUnit
    {
        public Image Image { get; set; }
        public Vector2 Position { get; set; }
        
        public int HitCount { get; set; }
        private int framesImmune;

        public enum BrickType
        {
            Blue,
            Green,
            Grey,
            Purple,
            Red,
            Yellow
        }

        public BrickType Type { get; set; }

        private Dictionary<BrickType, dynamic> _brickDefinitions = new Dictionary<BrickType, dynamic>();

        public Brick()
        {
            _brickDefinitions = new Dictionary<BrickType, dynamic>();
            _brickDefinitions.Add(BrickType.Blue, new { TextureMap = 1, TextureIndex = 0 });
            _brickDefinitions.Add(BrickType.Green, new { TextureMap = 1, TextureIndex = 1 });
            _brickDefinitions.Add(BrickType.Grey, new { TextureMap = 1, TextureIndex = 2 });
            _brickDefinitions.Add(BrickType.Purple, new { TextureMap = 1, TextureIndex = 3 });
            _brickDefinitions.Add(BrickType.Red, new { TextureMap = 1, TextureIndex = 4 });
            _brickDefinitions.Add(BrickType.Yellow, new { TextureMap = 1, TextureIndex = 5 });
        }

        public void Initialize()
        {
            this.Position = Vector2.Zero;
            this.Image = new Image()
            {
                TextureMap = _brickDefinitions[this.Type].TextureMap,
                TextureIndex = _brickDefinitions[this.Type].TextureIndex,
                //Path = "Sprites/breakout_sprites(no shadow)_5",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
            this.HitCount = 1;
        }

        public void LoadContent()
        {
            this.Image.LoadContent();
        }

        public void UnloadContent()
        {
            this.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (this.Image != null)
            {
                this.Image.Position = this.Position;
                this.Image.Update(gameTime);
            }

            if (framesImmune > 0)
                framesImmune--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if ( this.Image != null)
            { 
                this.Image.Draw(spriteBatch);
            }
        }

        public bool CheckCollision(Ball ball)
        {
            if (this.Image == null)
                return false;

            bool collision = false;
            if (framesImmune <= 0)
            {
                collision = ball.Image.Bounds.Intersects(this.Image.Bounds);
                if (collision)
                {
                    framesImmune = 20;
                    this.HitCount--;

                    // Assume that if our Y is close to the top or bottom of the block,
                    // we're colliding with the top or bottom
                    if ((ball.Position.Y <
                        (this.Position.Y - this.Image.SourceRectangle.Height / 2)) ||
                        (ball.Position.Y >
                        (this.Position.Y + this.Image.SourceRectangle.Height / 2)))
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                    }
                    else // otherwise, we have to be colliding from the sides
                    {
                        ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                    }

                    if (this.HitCount <= 0)
                    {
                        // spawn bonus
                        var messenger = Mvx.Resolve<IMvxMessenger>();

                        messenger.Publish<BrickDestroyedMessage>(new BrickDestroyedMessage(this));

                        //SpawnBonus();
                    }

                    // Method 2
                    //int x = (this.Image.SourceRectangle.X + (this.Image.SourceRectangle.Width / 2)) - (ball.Image.SourceRectangle.X + (ball.Image.SourceRectangle.Width / 2));
                    //int y = (this.Image.SourceRectangle.Y + (this.Image.SourceRectangle.Height / 2)) - (ball.Image.SourceRectangle.Y + (ball.Image.SourceRectangle.Height / 2));
                    //if (Math.Abs(x) > Math.Abs(y))
                    //{
                    //    // reflect horizontally
                    //    ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                    //}
                    //else
                    //{
                    //    // reflect vertically
                    //    ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                    //}
                }
            }
            return collision;
        }

        public void SpawnBonus()
        {
            // spawn bonus
            var messenger = Mvx.Resolve<IMvxMessenger>();

            messenger.Publish<SpawnBonusMessage>(new SpawnBonusMessage(this));
        }
    }
}
