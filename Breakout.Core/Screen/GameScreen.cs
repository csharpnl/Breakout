using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MvvmCross;
using MvvmCross.Plugin.Messenger;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Breakout.Core.Entities;
using Breakout.Core.Map;
using Breakout.Core.Messages;
using Breakout.Core.Sound;

namespace Breakout.Core.Screen
{
    public class GameScreen : Screen
    {
        private Player _player;
        private List<Ball> _balls;
        private Level _level;
        private List<Bonus> _spawns;
        private List<IUnit> _units;

        public static int PlayerOffsetY = 80;

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public Level Level
        {
            get { return _level; }
            set { _level = value; }
        }


        private Background _background;

        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }

        private Border _border;

        public Border Border
        {
            get { return _border; }
            set { _border = value; }
        }

        private Scoreboard _scoreboard;

        public Scoreboard Scoreboard
        {
            get { return _scoreboard; }
            set { _scoreboard = value; }
        }


        public GameScreen()
        {

        }

        public override void Initialize()
        {
            _background = new Background()
            {
                Position = Vector2.Zero
            };
            _background.Initialize();

            _border = new Border()
            {
                Position = Vector2.Zero
            };
            _border.Initialize();

            _scoreboard = new Map.Scoreboard()
            {
                ScorePosition = Vector2.Zero,
                Score = 0,
                Level = 1
            };
            _scoreboard.Initialize();

            _level = new Level()
            {

            };
            _level.Initialize();

            _player = new Player()
            {
                Position = Vector2.Zero
            };
            _player.Initialize();

            // balls
            _balls = new List<Ball>()
            {
                new Ball()
                {
                    Position = Vector2.Zero,
                    Velocity = new Vector2(+Ball.BaseBallSpeed, -Ball.BaseBallSpeed)
                }
            };
            foreach (var ball in _balls)
            {
                ball.Initialize();
            }

            // spawns
            _spawns = new List<Bonus>();

            // units
            _units = new List<IUnit>();

            _tokens = new List<MvxSubscriptionToken>();

            var messenger = Mvx.Resolve<IMvxMessenger>();
            _tokens.Add(messenger.SubscribeOnMainThread<SpawnBonusMessage>(OnSpawnBonusMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<ActionBonusMessage>(OnActionBonusMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<RemoveBallMessage>(OnRemoveBallMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<RemoveBonusMessage>(OnRemoveBonusMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<RemoveUnitMessage>(OnRemoveUnitMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<BrickDestroyedMessage>(OnBrickDestroyedMessage));

            SoundManager.Instance.Initialize();
        }

        private List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

        private void OnRemoveBallMessage(RemoveBallMessage message)
        {
            var ball = message.Sender as Ball;

            lock (_balls)
            {
                _balls.Remove(ball);
                if (_balls.Count <= 0)
                {
                    this.Scoreboard.Lives--;

                    if (this.Scoreboard.Lives == 0)
                    {
                        // game over
                        var messenger = Mvx.Resolve<IMvxMessenger>();

                        messenger.Publish<GameOverMessage>(new GameOverMessage(this));
                    }
                    else
                    {
                        var b = new Ball()
                        {
                            Position = Vector2.Zero,
                            Velocity = new Vector2(+Ball.BaseBallSpeed, -Ball.BaseBallSpeed)
                        };
                        b.Initialize();
                        b.LoadContent();
                        b.Position = ball.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y - ball.Image.SourceRectangle.Height);
                        _balls.Add(b);
                    }
                }
            }
        }

        private void OnRemoveBonusMessage(RemoveBonusMessage message)
        {
            var bonus = message.Sender as Bonus;

            lock (_spawns)
            {
                _spawns.Remove(bonus);
            }
        }

        private void OnRemoveUnitMessage(RemoveUnitMessage message)
        {
            var unit = message.Sender as IUnit;

            lock (_units)
            {
                _units.Remove(unit);
            }
        }

        private void OnBrickDestroyedMessage(BrickDestroyedMessage message)
        {
            var brick = message.Sender as Brick;

            this.Scoreboard.Score += 100;

            this.Scoreboard.TotalBricks++;
            if (this.Scoreboard.TotalBricks % 7 == 0)
            {
                var bonus = new Bonus()
                {
                    Type = Bonus.RandomType(),
                    Velocity = (brick.Position.X > (ScreenManager.Instance.Dimensions.X / 2) ? new Vector2(-0.5f, -1.0f) : new Vector2(+0.5f, -1.0f)),
                    Acceleration = new Vector2(0.0f, +0.02f)
                };
                bonus.Initialize();
                bonus.LoadContent();
                bonus.Position = new Vector2(brick.Position.X + (brick.Image.SourceRectangle.Width / 2) - (bonus.Image.SourceRectangle.Width / 2), brick.Position.Y);

                lock (_spawns)
                {
                    _spawns.Add(bonus);
                }
            }

            // increase levels
            if (this.Scoreboard.TotalBricks % 49 == 0)
            {
                this.Scoreboard.Level++;

                // speed up row generation
                if (this.Level.TimerBrick > 5.0)
                    this.Level.TimerBrick -= 1.0f;

                if ((this.Scoreboard.TotalBricks % (2 * 49)) == 0)
                {
                    this.Level.InitialHitCount++;
                }
            }
        }

        private void OnSpawnBonusMessage(SpawnBonusMessage message)
        {
            if (!(message.Sender is Brick))
                return;

            var brick = message.Sender as Brick;
            var bonus = new Bonus()
            {
                Type = Bonus.RandomType(),
                Velocity = (brick.Position.X > (ScreenManager.Instance.Dimensions.X / 2) ? new Vector2(-0.5f, -1.0f) : new Vector2(+0.5f, -1.0f)),
                Acceleration = new Vector2(0.0f, +0.02f)
            };
            bonus.Initialize();
            bonus.LoadContent();
            bonus.Position = new Vector2(brick.Position.X + (brick.Image.SourceRectangle.Width / 2) - (bonus.Image.SourceRectangle.Width / 2), brick.Position.Y);

            lock (_spawns)
            {
                _spawns.Add(bonus);
            }
        }
        private void OnActionBonusMessage(ActionBonusMessage message)
        {
            var bonus = message.Sender as Bonus;

            SoundManager.Instance.Play("bonus");

            switch (bonus.Type)
            {
                case Bonus.BonusType.Multiball:     // spawn two extra balls
                    for (int i = 0; i < 2; i++)
                    {
                        if (_balls.Count < 5)
                        {
                            var ball = new Ball()
                            {
                                Position = Vector2.Zero,
                                Velocity = (i % 2 == 0 ? new Vector2(+Ball.BaseBallSpeed, -Ball.BaseBallSpeed) : new Vector2(-Ball.BaseBallSpeed, -Ball.BaseBallSpeed))
                            };
                            ball.Initialize();
                            ball.LoadContent();
                            ball.Position = _balls[0].Position;

                            lock (_balls)
                            {
                                _balls.Add(ball);
                            }
                        }
                    }

                    ThrowBonusText("Multiball");

                    break;

                case Bonus.BonusType.Bigger:
                    this.Player.IncreaseSize();

                    ThrowBonusText("Bigger");
                    break;

                case Bonus.BonusType.Smaller:
                    this.Player.DecreaseSize();

                    ThrowBonusText("Smaller");
                    break;
            }

            lock (_spawns)
            {
                _spawns.Remove(bonus);
            }
        }
        public override void LoadContent()
        {
            this.Background.LoadContent();
            this.Border.LoadContent();
            this.Scoreboard.LoadContent();
            this.Level.LoadContent();
            this.Player.LoadContent();

            this.Player.Position = new Vector2(
                    (ScreenManager.Instance.Dimensions.X / 2),
                    ScreenManager.Instance.Dimensions.Y - Player.Image.SourceRectangle.Height - 40 - PlayerOffsetY);

            lock(_balls)
            {
                foreach (var ball in _balls)
                {
                    ball.LoadContent();
                    ball.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y - ball.Image.SourceRectangle.Height);
                };
            }

            InputManager.Instance.CurrentTouchPosition = this.Player.Position;

            SoundManager.Instance.LoadContent();
        }

        public override void UnloadContent()
        {
            this.Background.UnloadContent();
            this.Border.UnloadContent();
            this.Scoreboard.UnloadContent();
            this.Level.UnloadContent();
            this.Player.UnloadContent();
            lock(_balls)
            {

                foreach (var ball in _balls)
                {
                    ball.UnloadContent();
                };
            }
            lock (_spawns)
            {
                foreach (var spawn in _spawns)
                {
                    spawn.UnloadContent();
                }
            }

            SoundManager.Instance.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            SoundManager.Instance.Update(gameTime);

            this.Background.Update(gameTime);
            this.Border.Update(gameTime);
            this.Scoreboard.Update(gameTime);
            this.Level.Update(gameTime);

            InputManager.Instance.Update(gameTime);
            var position = new Vector2(InputManager.Instance.ScaledTouchPosition.X - (this.Player.Image.SourceRectangle.Width / 2), this.Player.Position.Y);
            if (position.X < 0)
                position = new Vector2(0.0f, this.Player.Position.Y);
            else if (position.X >= ScreenManager.Instance.Dimensions.X - this.Player.Image.SourceRectangle.Width)
                position = new Vector2(ScreenManager.Instance.Dimensions.X - this.Player.Image.SourceRectangle.Width, this.Player.Position.Y);

            this.Player.Position = position;
            this.Player.Update(gameTime);

            lock (_balls)
            {
                try
                {
                    foreach (var ball in _balls)
                    {
                        ball.Update(gameTime);
                    }
                }
                catch { }
            }

            lock (_spawns)
            {
                try
                {
                    foreach (var spawn in _spawns)
                    {
                        spawn.Update(gameTime);
                        spawn.CheckCollision(this.Player);
                    }
                }
                catch { }
            }

            lock (_balls)
            {
                try
                {
                    foreach (var ball in _balls)
                    {
                        ball.CheckCollision(this.Player);
                        this.Level.CheckCollision(ball);
                    }
                }
                catch { }
            }

            lock (_units)
            {
                try
                {
                    foreach (var unit in _units)
                    {
                        unit.Update(gameTime);
                    }
                }
                catch { }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Begin(0, null, null, null, null, null, ScreenManager.Instance.SpriteScale);

            SoundManager.Instance.Draw(spriteBatch);
            
            this.Background.Draw(spriteBatch);
            //this.Border.Draw(spriteBatch);
            this.Scoreboard.Draw(spriteBatch);
            this.Level.Draw(spriteBatch);

            lock (_units)
            {
                foreach (var unit in _units)
                {
                    unit.Draw(spriteBatch);
                }
            }
            this.Player.Draw(spriteBatch);
            lock (_spawns)
            {
                foreach (var spawn in _spawns)
                {
                    spawn.Draw(spriteBatch);
                }
            }
            lock (_balls)
            {
                foreach (var ball in _balls)
                {
                    ball.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private void ThrowBonusText(string text)
        {
            var unit = new BonusText()
            {
                Text = text,
                Velocity = new Vector2(+0.0f, -0.5f)
            };
            unit.Initialize();
            unit.LoadContent();
            unit.Position = new Vector2(this.Player.Position.X + this.Player.Image.SourceRectangle.Width / 2, this.Player.Position.Y);
            lock (_units)
            {
                _units.Add(unit);
            }
        }
    }
}
