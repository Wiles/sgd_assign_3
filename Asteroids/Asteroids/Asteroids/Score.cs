using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Score : IEntity
    {
        private SpriteFont _font;

        private int _points;
        private Vector2 _position;

        #region IEntity Members

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "score: " + _points, _position, Color.White);
            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
        }

        public Circle GetCircle()
        {
            return new Circle(0, 0, 0);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] {GetCircle()};
        }

        #endregion

        public void Initialize(SpriteFont font, Vector2 position)
        {
            _font = font;
            _position = position;
        }

        public void AddPoints(int points)
        {
            _points += points;
        }
    }
}