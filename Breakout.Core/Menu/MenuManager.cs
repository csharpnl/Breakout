using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Breakout.Core.Messages;

namespace Breakout.Core.Menu
{
    public class MenuManager
    {
        private Menu menu;

        public MenuManager( int index)
        {
            menu = new Menu();

            if (index == 0)
            {
                menu.AddItem("New Game", "1", "");
                //menu.AddItem("Options", "2", "");
                menu.AddItem("Credits", "3", "");
                menu.AddItem("Exit Game", "4", "");
            }
            else
            {
                menu.AddItem("Game Over", "0", "");
                menu.AddItem("New Game", "1", "");
                menu.AddItem("Exit Game", "4", "");
            }
            menu.OnMenuChange += menu_OnMenuChange;
        }

        void menu_OnMenuChange(object sender, EventArgs e)
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();

            // load menu by id
            switch (menu.ID)
            {
                case "1":   // Start new game
                    messenger.Publish<StartNewGameMessage>(new StartNewGameMessage(menu));
                    break;  

                case "3":
                    messenger.Publish<ShowCreditsMessage>(new ShowCreditsMessage(menu));
                    break;

                case "4":
                    messenger.Publish<ExitGameMessage>(new ExitGameMessage(menu));
                    break;
            }
        }

        public void LoadContent(string menuPath)
        {
            if ( !string.IsNullOrEmpty(menuPath))
            {
                menu.ID = menuPath;
            }

            menu.LoadContent();
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
