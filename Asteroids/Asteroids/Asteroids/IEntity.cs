using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    interface IEntity
    {
        Circle GetCircle();
        void Draw(SpriteBatch spriteBatch);
        void Update(long delta);
    }
}
