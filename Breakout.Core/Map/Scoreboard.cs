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
    public class Scoreboard
    {
        public int Score { get; set; }
        public int Coins { get; set; }
        public int Level { get; set; }
        public int Lives { get; set; }
        public int TotalRows { get; set; }
        public int TotalBricks { get; set; }
        public Vector2 ScorePosition { get; set; }
        public Vector2 CoinsPosition { get; set; }
        public Vector2 LevelPosition { get; set; }
        public Vector2 LivesPosition { get; set; }

        private SpriteFont Font;

        public Scoreboard()
        {
            this.Score = 0;
            this.Coins = 0;
            this.Level = 0;
            this.TotalBricks = 0;
            this.TotalRows = 0;
            this.Lives = 3;
        }

        public void Initialize()
        {
            this.ScorePosition = new Vector2( 20.0f, 20.0f );
            this.LevelPosition = new Vector2( 20.0f, 60.0f);
            this.LivesPosition = new Vector2(320.0f, 20.0f);
            this.CoinsPosition = Vector2.Zero;
        }

        public void LoadContent()
        {
            var content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            Font = content.Load<SpriteFont>("Fonts/TobagoPoster");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scoreText = string.Format("Score: {0}", this.Score);
            var levelText = string.Format("Level: {0}", this.Level);
            var livesText = string.Format("Lives: {0}", this.Lives);

            spriteBatch.DrawString(this.Font, scoreText, this.ScorePosition, Color.White);
            spriteBatch.DrawString(this.Font, levelText, this.LevelPosition, Color.White);
            spriteBatch.DrawString(this.Font, livesText, this.LivesPosition, Color.White);
        }
    }
}
