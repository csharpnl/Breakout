using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Sprites;

namespace Breakout.Core.Map
{
    public class Player : IUnit
    {
        public Image Image { get; set; }        
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public enum PlayerSize
        {
            Small,
            Medium,
            Large
        }

        public PlayerSize Size { get; set; }

        private Dictionary<PlayerSize, dynamic> _playerDefinitions = new Dictionary<PlayerSize, dynamic>();
        public Player()
        {
            _playerDefinitions = new Dictionary<PlayerSize, dynamic>();
            _playerDefinitions.Add(PlayerSize.Small, new { TextureMap = 0, TextureIndex = 0 });
            _playerDefinitions.Add(PlayerSize.Medium, new { TextureMap = 0, TextureIndex = 1 });
            _playerDefinitions.Add(PlayerSize.Large, new { TextureMap = 0, TextureIndex = 2 });
        }

        public void Initialize()
        {
            this.Position = new Vector2(50, 50);
            this.Image = new Image()
            {
                TextureMap = _playerDefinitions[this.Size].TextureMap,
                TextureIndex = _playerDefinitions[this.Size].TextureIndex,
                //Path = "Sprites/breakout_sprites(no shadow)_5",
                Position = this.Position,
                SourceRectangle = Rectangle.Empty
            };
            this.Image.Initialize();
        }

        public void LoadContent()
        {
            this.Image.Position = this.Position;
            this.Image.SourceRectangle = TextureManager.Instance[0][1];
            this.Image.LoadContent();
        }

        public void UnloadContent()
        {
            this.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            this.Image.Position = this.Position;
            this.Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Image.Draw(spriteBatch);
        }

        public void IncreaseSize()
        {
            if ( this.Size != PlayerSize.Large )
            {
                this.Size++;

                this.Image.TextureMap = _playerDefinitions[this.Size].TextureMap;
                this.Image.TextureIndex = _playerDefinitions[this.Size].TextureIndex;
                this.Image.LoadContent();
            }
        }

        public void DecreaseSize()
        {
            if (this.Size != PlayerSize.Small)
            {
                this.Size--;

                this.Image.TextureMap = _playerDefinitions[this.Size].TextureMap;
                this.Image.TextureIndex = _playerDefinitions[this.Size].TextureIndex;
                this.Image.LoadContent();
            }
        }
    }
}
