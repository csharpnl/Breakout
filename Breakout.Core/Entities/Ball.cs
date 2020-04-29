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
using Breakout.Core.Messages;
using Microsoft.Xna.Framework.Audio;
using Breakout.Core.Sound;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace Breakout.Core.Entities
{
    public class Ball : IUnit
    {
        public Image Image { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        private int framesImmune;
        public const float BaseBallSpeed = 2.5f;
        public void Initialize()
        {
            this.Position = new Vector2(50, 50);
            this.Image = new Image()
            {
                TextureMap = 0,
                TextureIndex = 200,
                //Path = "Sprites/breakout_sprites(no shadow)_5",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
        }

        public void LoadContent()
        {
            //this.Image.SourceRectangle = TextureManager.Instance[0][200];
            this.Image.LoadContent();
        }

        public void UnloadContent()
        {
            this.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            //this.Position += Velocity;
            this.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10;

            int MaxX = (int)(ScreenManager.Instance.Dimensions.X - this.Image.SourceRectangle.Width);
            //int MaxX = ScreenManager.Instance.GraphicsDevice.Viewport.Width - this.Image.SourceRectangle.Width;
            int MinX = 0;
            int MaxY = (int)ScreenManager.Instance.Dimensions.Y;
            //int MaxY = ScreenManager.Instance.GraphicsDevice.Viewport.Height;
            int MinY = 128;

            // Check for bounce.
            if (this.Position.X > MaxX)
            {
                Velocity = new Vector2( -Velocity.X, Velocity.Y );
                this.Position = new Vector2( MaxX, this.Position.Y);
            }
            else if (this.Position.X < MinX)
            {
                Velocity = new Vector2( -Velocity.X, Velocity.Y);
                this.Position = new Vector2(MinX, this.Position.Y);
            }

            if (this.Position.Y > MaxY)
            {
                //Velocity = new Vector2(Velocity.X, -Velocity.Y);
                //this.Position = new Vector2(this.Position.X, MaxY);

                var messenger = Mvx.Resolve<IMvxMessenger>();

                messenger.Publish<RemoveBallMessage>(new RemoveBallMessage(this));
                //if (BallLost != null)
                //    BallLost(this, null);
            }
            else if (this.Position.Y < MinY)
            {
                Velocity = new Vector2(Velocity.X, -Velocity.Y);
                this.Position = new Vector2(this.Position.X, MinY);
            }

            if (framesImmune > 0)
                framesImmune--;

            this.Image.Position = this.Position;
            this.Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Image.Draw(spriteBatch);
        }

        public void CheckCollision( Player player )
        {
            bool collision = false;
            if (framesImmune <= 0)
            {
                collision = player.Image.Bounds.Intersects(this.Image.Bounds);
                if (collision)
                {
                    SoundManager.Instance.Play("paddle");

                    framesImmune = 20;

                    //var relativeIntersectX = (_player.Position.X + (_player.Image.SourceRectangle.Width / 2)) - ball.Position.X;
                    //var normalizedRelativeIntersectionX = (relativeIntersectX / (_player.Image.SourceRectangle.Width / 2));
                    //var bounceAngle = normalizedRelativeIntersectionX * 75;
                    //ball.Velocity = new Vector2(ball.Velocity.X * (float)-Math.Sin(bounceAngle), ball.Velocity.Y * (float)-Math.Cos(bounceAngle));
                    //ball.Velocity = new Vector2(ball.Velocity.X, ball.Velocity.Y * (float)-Math.Sin(bounceAngle));

                    float radius = this.Image.SourceRectangle.Width / 2;

                    // Reflect based on which part of the paddle is hit

                    // By default, set the normal to "up"
                    Vector2 normal = -1.0f * Vector2.UnitY;

                    // Distance from the leftmost to rightmost part of the paddle
                    float dist = player.Image.SourceRectangle.Width + radius * 2;
                    // Where within this distance the ball is at
                    float ballLocation = this.Position.X -
                        (player.Position.X - radius - player.Image.SourceRectangle.Width / 2);
                    ballLocation = this.Position.X - player.Position.X;
                    // Percent between leftmost and rightmost part of paddle
                    float pct = ballLocation / dist;

                    if (pct < 0.16f)
                    {
                        normal = new Vector2(-0.196f, -0.981f);
                    }
                    else if (pct > 0.84f)
                    {
                        normal = new Vector2(0.196f, -0.981f);
                    }

                    this.Velocity = Vector2.Reflect(this.Velocity, normal);

                    // Fix up the direction if it's too steep
                    float dotResult = Vector2.Dot(this.Velocity, Vector2.UnitX);
                    if (dotResult > 0.9f)
                    {
                        this.Velocity = new Vector2(0.906f, -0.423f);
                    }
                    dotResult = Vector2.Dot(this.Velocity, -Vector2.UnitX);
                    if (dotResult > 0.9f)
                    {
                        this.Velocity = new Vector2(-0.906f, -0.423f);
                    }
                    dotResult = Vector2.Dot(this.Velocity, -Vector2.UnitY);
                    if (dotResult > 0.9f)
                    {
                        // We need to figure out if we're clockwise or counter-clockwise
                        Vector3 crossResult = Vector3.Cross(new Vector3(this.Velocity, 0),
                            -Vector3.UnitY);
                        if (crossResult.Z < 0)
                        {
                            this.Velocity = new Vector2(0.423f, -0.906f) * BaseBallSpeed;
                        }
                        else
                        {
                            this.Velocity = new Vector2(-0.423f, -0.906f) * BaseBallSpeed;
                        }
                    }

                    var baseSpeed = Ball.BaseBallSpeed;
                    this.Velocity = new Vector2(this.Velocity.X * baseSpeed, this.Velocity.Y * baseSpeed);
                    //float unitAcc = 0.01F;
                    //ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                    //ball.Velocity = new Vector2(ball.Velocity.X * unitAcc * 10, ball.Velocity.Y);
                }
            }
        }
    }
}
