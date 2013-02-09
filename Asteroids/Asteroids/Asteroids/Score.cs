using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Score
    {
        // Image representing the Projectile
        private SpriteFont _font;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // Represents the viewable boundary of the game
        Viewport _viewport;

        private int _points;

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
        }
    }
}
