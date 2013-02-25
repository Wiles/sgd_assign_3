using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    interface IEntity
    {
        Circle GetCircle();
        void Draw(SpriteBatch spriteBatch);
        void Update(GraphicsDevice graphics, KeyboardState input, long delta);
    }
}
