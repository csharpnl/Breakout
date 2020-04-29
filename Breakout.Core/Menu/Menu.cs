using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Breakout.Core.Screen;

namespace Breakout.Core.Menu
{
    public class Menu
    {
        public event EventHandler OnMenuChange;

        public string Axis;
        public string Effects;

        public List<MenuItem> Items;
        int itemNumber;
        string id;

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            foreach (var item in this.Items)
                dimensions += new Vector2(item.Image.SourceRectangle.Width, item.Image.SourceRectangle.Height);

            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X) / 2, (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2);

            foreach( var item in this.Items )
            {
                if (Axis == "X")
                    item.Image.Position = new Vector2(dimensions.X, (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRectangle.Height) / 2);
                else if (Axis == "Y")
                    item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - item.Image.SourceRectangle.Width) / 2, dimensions.Y);

                dimensions += new Vector2(item.Image.SourceRectangle.Width, item.Image.SourceRectangle.Height + 8);
            }
        }

        public Menu()
        {
            id = String.Empty;
            itemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void AddItem(string text, string linkId, string linkType)
        {
            var item = new MenuItem()
            {
                linkID = linkId,
                LinkType = linkType,
            };
            item.Image = new Map.Image();
            item.Image.TextureMap = 1;
            item.Image.TextureIndex = 100;
            item.Image.Text = text;
            this.Items.Add(item);
        }

        public void LoadContent() 
        {
            string[] split = Effects.Split(':');
            foreach( var item in this.Items)
            {
                item.Image.LoadContent();
                //foreach( var s in split)
                //    item.Image.Active
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach( var item in this.Items )
            {
                item.Image.UnloadContent();
            }
        }

        public void Update( GameTime gameTime )
        {
            InputManager.Instance.Update(gameTime);

            foreach (var item in this.Items)
            {
                if (item.Image.Bounds.Contains(InputManager.Instance.ScaledTouchPosition))
                {
                    this.ID = item.linkID;
                }

                item.Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in this.Items)
            {
                item.Image.Draw(spriteBatch);
            }
        }
    }
}
