using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout.Core.Map
{
    public interface IUnit
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
