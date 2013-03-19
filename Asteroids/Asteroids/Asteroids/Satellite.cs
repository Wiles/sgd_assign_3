using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Satellite : IEntity
    {
        private Texture2D _texture;
        private Vector2 _position;

        private double Radius
        {
            get { return _texture.Width/2.0; }
        }

        #region IEntity Members

        public Circle GetCircle()
        {
            return new Circle(_position, Radius);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[]
                {
                    new Circle(_position, Radius/2),
                    new Circle(new Vector2( _position.X, (float) (_position.Y - Radius/2.0)), Radius/3.0),
                    new Circle(new Vector2( (float) (_position.X + Radius/2.5), _position.Y), Radius/2.0),
                    new Circle(new Vector2( (float) (_position.X - Radius/2.5), _position.Y), Radius/2.0)
                };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0,
                             new Vector2((int) (_texture.Width/2.0), (int) (_texture.Height/2.0)), 1f,
                             SpriteEffects.None, 0f);

            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
        }

        #endregion

        public void Initialize(Texture2D texture, Vector2 position)
        {
            _position = position;
            _texture = texture;
        }
    }
}