using Breakout.Core;
using Breakout.Core.Map;
using Breakout.Core.Screen;
using Breakout.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout.Droid
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();

            ScreenManager.Instance.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            DefineBreakoutMapper();
            DefinePuzzleGraphicsMapper();
            DefineBonusMapper();

            // TODO: use this.Content to load your game content here
            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(this.Content);

            base.LoadContent();
        }

        private void DefineBreakoutMapper()
        {
            var mapper = TextureManager.Instance.CreateMapper(0, "Sprites/breakout_sprites", this.Content);

            // player
            mapper.AddMap(0, new Rectangle(0, 200, 96, 24));   // small
            mapper.AddMap(1, new Rectangle(0, 240, 112, 24));   // medium
            mapper.AddMap(2, new Rectangle(0, 280, 128, 24));   // large

            // blocks
            mapper.AddMap(100, new Rectangle(0, 0, 32, 32));   // red
            mapper.AddMap(101, new Rectangle(40, 0, 32, 32));   // purple
            mapper.AddMap(102, new Rectangle(80, 0, 32, 32));   // yellow
            mapper.AddMap(103, new Rectangle(120, 0, 32, 32));   // blue #1 s
            mapper.AddMap(104, new Rectangle(160, 0, 32, 32));   // blue #2 n
            mapper.AddMap(105, new Rectangle(200, 0, 32, 32));   // green #1 s
            mapper.AddMap(106, new Rectangle(240, 0, 32, 32));   // green #2 n
            mapper.AddMap(107, new Rectangle(280, 0, 32, 32));   // star
            mapper.AddMap(108, new Rectangle(320, 0, 32, 32));   // rock #1 s cracked
            mapper.AddMap(109, new Rectangle(360, 0, 32, 32));   // rock #1 s solid

            mapper.AddMap(110, new Rectangle(0, 40, 32, 32));   // rock #2 s tiled
            mapper.AddMap(111, new Rectangle(40, 40, 32, 32));   // rock #3 ml cracked
            mapper.AddMap(112, new Rectangle(80, 40, 32, 32));   // rock #3 mr cracked
            mapper.AddMap(113, new Rectangle(120, 40, 32, 32));   // rock #3 ml solid
            mapper.AddMap(114, new Rectangle(160, 40, 32, 32));   // rock #3 mr solid
            mapper.AddMap(115, new Rectangle(200, 40, 32, 32));   // face #1 black - angry
            mapper.AddMap(116, new Rectangle(240, 40, 32, 32));   // face #2 green - happy
            mapper.AddMap(117, new Rectangle(280, 40, 32, 32));   // face #3 blue - undecided
            mapper.AddMap(118, new Rectangle(320, 40, 32, 32));   // face #4 red - sad
            mapper.AddMap(119, new Rectangle(360, 40, 32, 32));   // rock #4 blocked cracked

            mapper.AddMap(120, new Rectangle(0, 80, 32, 32));   // rock #4 blocked solid
            mapper.AddMap(121, new Rectangle(40, 80, 32, 32));   // rcok #5 solid
            mapper.AddMap(122, new Rectangle(80, 80, 32, 32));   // octagon #1 green
            mapper.AddMap(123, new Rectangle(120, 80, 32, 32));   // octagon #2 magenta
            mapper.AddMap(124, new Rectangle(160, 80, 32, 32));   // octagon #3 cyna
            mapper.AddMap(125, new Rectangle(200, 80, 32, 32));   // square action #1 ice
            mapper.AddMap(126, new Rectangle(240, 80, 32, 32));   // square action #2 fire
            mapper.AddMap(127, new Rectangle(280, 80, 32, 32));   // flask #1
            mapper.AddMap(128, new Rectangle(320, 80, 32, 32));   // rock #6 moon solid
            mapper.AddMap(129, new Rectangle(360, 80, 32, 32));   // rock #6 moon dented

            mapper.AddMap(130, new Rectangle(0, 120, 32, 32));   // rock #6 moon cracked
            mapper.AddMap(131, new Rectangle(40, 120, 32, 32));   // button #1 off
            mapper.AddMap(132, new Rectangle(80, 120, 32, 32));   // button #1 on
            mapper.AddMap(133, new Rectangle(120, 120, 32, 32));   // circle
            mapper.AddMap(134, new Rectangle(160, 120, 32, 32));   // circle #1 red
            mapper.AddMap(135, new Rectangle(200, 120, 32, 32));   // circle #1 blue
            mapper.AddMap(136, new Rectangle(240, 120, 32, 32));   // pillar #1 solid
            mapper.AddMap(137, new Rectangle(280, 120, 32, 32));   // pillar #1 cracked
            mapper.AddMap(138, new Rectangle(320, 120, 32, 32));   // arrow #1 left
            mapper.AddMap(139, new Rectangle(360, 120, 32, 32));   // arrow #1 right

            mapper.AddMap(140, new Rectangle(0, 160, 32, 32));   // arrow #1 top
            mapper.AddMap(141, new Rectangle(40, 160, 32, 32));   // arrow #1 down

            // ball
            mapper.AddMap(201, new Rectangle(160, 240, 24, 24));   // large
            mapper.AddMap(200, new Rectangle(160, 200, 16, 16));   // small

            // coins
            mapper.AddMap(301, new Rectangle(0, 276, 384, 24));    // coin #1 spinnings horizontal
        }

        public void DefinePuzzleGraphicsMapper()
        {
            var mapper = TextureManager.Instance.CreateMapper(1, "PuzzleGraphics/puzzleGraphics", this.Content);

            mapper.AddMap(0, new Rectangle(0, 178, 64, 32));      // element_blue_rectangle
            mapper.AddMap(1, new Rectangle(72, 178, 64, 32));     // element_green_rectangle
            mapper.AddMap(2, new Rectangle(144, 154, 64, 32));     // element_grey_rectangle
            mapper.AddMap(3, new Rectangle(198, 0, 64, 32));     // element_purple_rectangle
            mapper.AddMap(4, new Rectangle(270, 0, 64, 32));     // element_red_rectangle
            mapper.AddMap(5, new Rectangle(342, 0, 64, 32));     // element_yellow_rectangle

            mapper.AddMap(100, new Rectangle(0, 0, 190, 49));       // button default
            mapper.AddMap(101, new Rectangle(0, 57, 190, 49));      // button selected
        }

        public void DefineBonusMapper()
        {
            var mapper = TextureManager.Instance.CreateMapper(2, "Bonus/bonus_10", this.Content);
            mapper.AddMap(0, new Rectangle(0, 0, 512, 32));       // multiball

            mapper = TextureManager.Instance.CreateMapper(3, "Bonus/bonus_11", this.Content);
            mapper.AddMap(0, new Rectangle(0, 0, 512, 32));         // 

            mapper = TextureManager.Instance.CreateMapper(4, "Bonus/bonus_12", this.Content);
            mapper.AddMap(0, new Rectangle(0, 0, 512, 32));         // 

            mapper = TextureManager.Instance.CreateMapper(5, "Bonus/bonus_13", this.Content);
            mapper.AddMap(0, new Rectangle(0, 0, 512, 32));         // 

            mapper = TextureManager.Instance.CreateMapper(6, "Bonus/bonus_14", this.Content);
            mapper.AddMap(0, new Rectangle(0, 0, 512, 32));         // 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            TextureManager.Instance.UnloadContent();
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (!ScreenManager.Instance.Previous())
                {
                    Exit();
                }
            }

            // Exit game if selected from the menu
            if (ScreenManager.Instance.CanExitGame)
                Exit();

            // TODO: Add your update logic here
            ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            ScreenManager.Instance.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
