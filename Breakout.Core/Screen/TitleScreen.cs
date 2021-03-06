﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Breakout.Core.Menu;

namespace Breakout.Core.Screen
{
    public class TitleScreen : Screen
    {
        private MenuManager _menuManager;

        public TitleScreen()
        {
            _menuManager = new MenuManager(0);
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            _menuManager.LoadContent("");
        }

        public override void UnloadContent()
        {
            _menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
 
            _menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(0, null, null, null, null, null, ScreenManager.Instance.SpriteScale);

            _menuManager.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
