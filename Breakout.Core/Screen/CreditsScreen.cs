using Breakout.Core.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Breakout.Core.Screen
{
    public class Credit
    {
        public Vector2 Position { get; set; }
        public string Text { get; set; }
    }
    public class CreditsScreen : Screen
    {
        private List<Credit> _lines = null;
        private List<string> _textLines = null;

        private ContentManager content;
        private SpriteFont font;

        private float scrollCounter;
        private int scrollFrame;

        public CreditsScreen()
        {
            _textLines = new List<string>();
            _textLines.Add("Breakout Adventures");
            _textLines.Add("");
            _textLines.Add("By csharp.nl");
            _textLines.Add("");
            _textLines.Add("Feel free to contact us");
            _textLines.Add("with feedback or requests");
            _textLines.Add("");
            _textLines.Add("apps@csharp.nl");

            scrollCounter = 0;
            scrollFrame = 100;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            font = content.Load<SpriteFont>("Fonts/TobagoPoster");

            _lines = new List<Credit>();

            int index = 0;
            foreach (var text in _textLines)
            {
                var x = ( ScreenManager.Instance.Dimensions.X - font.MeasureString(text).X ) / 2;
                _lines.Add(new Credit(){ Position = new Vector2(x, 20.0f + ( index * 24 )), Text = text });
                index++;
            }
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            scrollCounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (scrollCounter > scrollFrame)
            {
                int index = 0;
                foreach (var line in _lines)
                {
                    line.Position = new Vector2(line.Position.X, 20.0f + ( index * 24 ));

                    index++;
                }

                scrollCounter = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(0, null, null, null, null, null, ScreenManager.Instance.SpriteScale);

            foreach (var line in _lines)
            {
                spriteBatch.DrawString(font, line.Text, line.Position, Color.White);
            }

            spriteBatch.End();
        }
    }
}
