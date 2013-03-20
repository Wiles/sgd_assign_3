//File:     IEntity.cs
//Name:     Samuel Lewis (5821103) & Thomas Kempton (5781000)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Interface for a game Entity

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// Interface used by all object that can be drawn to the screen.
    /// </summary>
    internal interface IEntity
    {
        /// <summary>
        /// Gets the circle for rough collision detections.
        /// </summary>
        /// <returns></returns>
        Circle GetCircle();

        /// <summary>
        /// Gets the circles used to fine collision detection.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Circle> GetCircles();

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Updates the Entity.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="input">The input.</param>
        /// <param name="delta">The delta.</param>
        void Update(GraphicsDevice graphics, Input input, long delta);
    }
}