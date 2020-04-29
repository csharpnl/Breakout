using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout.Core.Map
{
    public class Map : DrawableGameComponent
    {
        public List<Layer> Layers { get; set; }

        public Map(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var spritebatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            //spritebatch.Draw()
            base.Draw(gameTime);
        }
    }
}
