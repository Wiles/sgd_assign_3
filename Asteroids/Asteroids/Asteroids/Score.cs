//File:     Score.cs
//Name:     Samuel Lewis (5821103)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Players overall score

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// 
    /// </summary>
    internal class Score : IEntity
    {
        private SpriteFont _font;

        private int _points;
        private Vector2 _position;

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public int Points
        {
            get { return _points; }
        }

        #region IEntity Members

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "score: " + _points, _position, Color.White);
            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Updates the Entity.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="input">The input.</param>
        /// <param name="delta">The delta.</param>
        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
        }

        /// <summary>
        /// Gets the circle for rough collision detections.
        /// </summary>
        /// <returns></returns>
        public Circle GetCircle()
        {
            return new Circle(Vector2.Zero, 0);
        }

        /// <summary>
        /// Gets the circles used to fine collision detection.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Circle> GetCircles()
        {
            return new[] {GetCircle()};
        }

        #endregion

        /// <summary>
        /// Initializes the specified font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="position">The position.</param>
        public void Initialize(SpriteFont font, Vector2 position)
        {
            _font = font;
            _position = position;
        }

        /// <summary>
        /// Adds the points.
        /// </summary>
        /// <param name="points">The points.</param>
        public void AddPoints(int points)
        {
            _points += points;
        }
    }
}