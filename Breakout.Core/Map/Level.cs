using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Breakout.Core.Entities;
using Breakout.Core.Screen;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Breakout.Core.Sound;

namespace Breakout.Core.Map
{
    public class Level// : DrawableGameComponent
    {
        private float _timerBrick;
        private float _elapsedTimerBrick;

        public float TimerBrick
        {
            get { return _timerBrick; }
            set { _timerBrick = value; }
        }
        
        private List<Brick> _bricks;

        public List<Brick> Bricks
        {
            get { return _bricks; }
            set { _bricks = value; }
        }

        private int OffsetY = 128;
        private int OffsetX = 15;
        private Brick.BrickType _lastBrickType = Brick.BrickType.Yellow;

        public int InitialHitCount { get; set; }
        private Brick.BrickType GetNextBrickType()
        {
            _lastBrickType++;
            if ( (int)_lastBrickType >= Enum.GetValues(typeof(Brick.BrickType)).Length )
            {
                _lastBrickType = Brick.BrickType.Blue;
            }
            return _lastBrickType;
        }

        private void GenerateBrickLayers(int count, bool initialize)
        {
            for (int loop = 0; loop < count; loop++)
            {
                GenerateBrickLayer(initialize);
            }

        }
        private void GenerateBrickLayer( bool initialize)
        {
            var brickType = GetNextBrickType();

            // move existing bricks one row down
            foreach (var brick in this.Bricks)
            {
                brick.Position = new Vector2(brick.Position.X, brick.Position.Y + 32);
            }

            List<Brick> bricks = new List<Brick>();

            // add new layer of bricks
            int numberOfBricks = 7; // (int)(ScreenManager.Instance.Dimensions.X / 64) - 1;
            for (int loop = 0; loop < numberOfBricks; loop++)
            {
                var brick = new Brick()
                {
                    Type = brickType,
                    HitCount = this.InitialHitCount
                };

                if (initialize)
                { 
                    brick.Initialize();
                    brick.LoadContent();
                }
                brick.Position = new Vector2(loop * 64 + OffsetX, OffsetY);
                brick.HitCount = this.InitialHitCount;
                bricks.Add(brick);
            }
            this.Bricks.InsertRange(0, bricks);

        }

        public void Initialize()
        {
            this.InitialHitCount = 1;

            this.Bricks = new List<Brick>();

            GenerateBrickLayers(5, false);

            foreach( var brick in this.Bricks )
            {
                brick.Initialize();
            }

            this.TimerBrick = 20.0f; // every 20 seconds
            _elapsedTimerBrick = 0.0f;
        }

        public void LoadContent()
        {
            int x = OffsetX;
            int y = OffsetY;
            foreach (var brick in this.Bricks)
            {
                brick.Position = new Vector2(x, y);
                brick.LoadContent();

                x += 64;
                if ( x + 64 > ScreenManager.Instance.Dimensions.X )
                {
                    x = OffsetX;
                    y += 32;
                }
            }
        }

        public void UnloadContent()
        {
            foreach (var brick in this.Bricks)
            {
                brick.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTimerBrick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTimerBrick >= _timerBrick)
            {
                GenerateBrickLayer(true);

                // reset counter
                _elapsedTimerBrick = 0.0f;
            }
            foreach (var brick in this.Bricks)
            {
                brick.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var brick in this.Bricks)
            {
                brick.Draw(spriteBatch);
            }
        }

        public void CheckCollision(Ball ball)
        {
            List<Brick> bricksToBeRemoved = new List<Brick>();

            foreach (var brick in this.Bricks)
            {
                if (brick.CheckCollision(ball))
                {
                    SoundManager.Instance.Play("brick");
                }
                if (brick.HitCount <= 0)
                {
                    bricksToBeRemoved.Add(brick);
                }
            }

            this.Bricks = this.Bricks.Except(bricksToBeRemoved).ToList();

            if ( this.Bricks.Count == 0 )
            {
                GenerateBrickLayers(5, true);
            }
        }
    }
}
