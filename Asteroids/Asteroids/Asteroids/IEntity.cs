using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    interface IEntity
    {
        Circle GetCircle();
        IEnumerable<Circle> GetCircles();
        void Draw(SpriteBatch spriteBatch);
        void Update(GraphicsDevice graphics, Input input, long delta);
    }
}
