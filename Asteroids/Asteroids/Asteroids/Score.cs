using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Score : IEntity
    {
        private Vector2 _position;
        private SpriteFont _font;

        private int _points;

        public void Initialize(SpriteFont font, Vector2 position)
        {
            _font = font;
            _position = position;
        }

        public void AddPoints(int points)
        {
            _points += points;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "score: " + _points, _position, Color.White);
            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            
        }

        public Circle GetCircle()
        {
            return new Circle(0,0,0);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new []{GetCircle()};
        }
    }
}