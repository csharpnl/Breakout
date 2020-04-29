using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Breakout.Core.Messages;

namespace Breakout.Core.Screen
{
    public class ScreenManager
    {
        #region Singleton
        private static ScreenManager instance { get; set; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }
        #endregion

        public Vector2 Dimensions { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;

        public Screen currentScreen;
        public Screen newScreen;

        public Matrix SpriteScale;

        public bool CanExitGame { get; set; }

        public ScreenManager()
        {
            this.Dimensions = new Vector2(480,800);
            currentScreen = new TitleScreen();

            this.CanExitGame = false;
        }

        private List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

        public void Initialize()
        {
            _tokens = new List<MvxSubscriptionToken>();

            var messenger = Mvx.Resolve<IMvxMessenger>();
            _tokens.Add(messenger.SubscribeOnMainThread<StartNewGameMessage>(OnStartNewGameMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<ExitGameMessage>(OnExitGameMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<GameOverMessage>(OnGameOverMessage));
            _tokens.Add(messenger.SubscribeOnMainThread<ShowCreditsMessage>(OnShowCreditsMessage));
            
            currentScreen.Initialize();
        }

        private void OnStartNewGameMessage(StartNewGameMessage message)
        {
            _isTransitioning = true;

            newScreen = new GameScreen();
        }

        private void OnExitGameMessage(ExitGameMessage message)
        {
            this.CanExitGame = true;
        }

        private void OnGameOverMessage( GameOverMessage message)
        {
            _isTransitioning = true;

            newScreen = new GameOverScreen();
        }

        private void OnShowCreditsMessage( ShowCreditsMessage message )
        {
            _isTransitioning = true;

            newScreen = new CreditsScreen();
        }

        public void LoadContent( ContentManager content )
        {
            // Default resolution is 800x600; scale sprites up or down based on
            // current viewport
            float screenscale = (float)GraphicsDevice.Viewport.Width / this.Dimensions.X;

            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);

            this.Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
        }

        public Vector2 InputTranslate
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            }
        }

        public Matrix InputScale
        {
            get
            {
                return Matrix.Invert(SpriteScale);
            }
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
        }

        public void Update( GameTime gametime )
        {
            if (!IsTransitioning)
            {
                currentScreen.Update(gametime);
            }
            else
            {
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.Initialize();
                currentScreen.LoadContent();
                newScreen = null;

                _isTransitioning = false;
            }
        }

        public void Draw( SpriteBatch spriteBatch )
        {
            if ( !IsTransitioning )
                currentScreen.Draw(spriteBatch);
        }

        private bool _isTransitioning = false;

        public bool IsTransitioning
        {
            get
            {
                return _isTransitioning;
            }
        }

        public bool Previous()
        {
            if (( currentScreen is GameScreen ) ||
                ( currentScreen is CreditsScreen ))
            {
                InputManager.Instance.CurrentTouchPosition = Vector2.Zero;

                newScreen = new TitleScreen();

                _isTransitioning = true;

                return true;
            }
            return false;
        }
    }
}
