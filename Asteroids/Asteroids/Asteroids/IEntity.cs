using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    interface IEntity
    {
        Circle GetCircle();
        Circle[] GetCircles();
        void Draw(SpriteBatch spriteBatch);
        void Update(GraphicsDevice graphics, Input input, long delta);
    }
}
