using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    internal class Score : IEntity
    {
        public Vector2 Position;
        private SpriteFont _font;

        private int _points;
        private Viewport _viewport;

        public void Initialize(Viewport viewport, SpriteFont font, Vector2 position)
        {
            _font = font;
            Position = position;
            _viewport = viewport;
        }

        public void AddPoints(int points)
        {
            _points += points;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "score: " + _points, Position, Color.White);
            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public void Update(GraphicsDevice graphics, KeyboardState input, long delta)
        {
            
        }

        public Circle GetCircle()
        {
            return new Circle(0,0,0);
        }

        public Circle[] GetCircles()
        {
            return new Circle[]{GetCircle()};
        }
    }
}