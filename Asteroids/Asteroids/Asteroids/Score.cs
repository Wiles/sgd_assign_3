﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Score
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
        }
    }
}