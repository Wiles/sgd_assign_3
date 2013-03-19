//File:     Circle.cs
//Name:     Samuel Lewis (5821103)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Circle used to collision detection

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// Circle used for collision
    /// </summary>
    internal class Circle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="radius">The radius.</param>
        public Circle(Vector2 position, double radius)
        {
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public static Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        private Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        private double Radius { get; set; }

        /// <summary>
        /// Intersects with the other circle
        /// </summary>
        /// <param name="that">The other circle</param>
        /// <returns></returns>
        public bool Intersects(Circle that)
        {
            if (that.Radius <= 0.0 || Radius <= 0.0)
            {
                return false;
            }
            double dx = Position.X - that.Position.X;
            double dy = Position.Y - that.Position.Y;
            double radii = Radius + that.Radius;
            return (dx*dx) + (dy*dy) < radii*radii;
        }

        /// <summary>
        /// Draws the circle
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    new Vector2(Position.X, Position.Y),
                    null,
                    Color.White,
                    0f,
                    new Vector2(Texture.Width/2f, Texture.Width/2f),
                    (float) (Radius*2f)/Texture.Width,
                    SpriteEffects.None,
                    0f
                    );
            }
        }

        /// <summary>
        /// Intersectses the specified one.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns></returns>
        public static Boolean Intersects(IEnumerable<Circle> one, IEnumerable<Circle> two)
        {
            return (from c in one from d in two where c.Intersects(d) select c).Any();
        }
    }
}