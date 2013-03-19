//File:     Asteroid.cs
//Name:     Samuel Lewis (5821103)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Asteroid entity

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// Asteroid
    /// </summary>
    internal class Asteroid : IEntity
    {
        /// <summary>
        /// Is the Asteroid Active
        /// </summary>
        public bool Active;

        /// <summary>
        /// The position on the display
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The movement rate
        /// </summary>
        private float _projectileMoveSpeed;

        /// <summary>
        /// The direction of travel
        /// </summary>
        private double _radians;

        /// <summary>
        /// The viewport to draw to
        /// </summary>
        private Viewport _viewport;

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the radians.
        /// </summary>
        /// <value>
        /// The radians.
        /// </value>
        public double Radians
        {
            get { return _radians; }
        }

        /// <summary>
        /// Gets the generation.
        /// </summary>
        /// <value>
        /// The generation.
        /// </value>
        public int Generation { get; private set; }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public double Scale
        {
            get { return 1.0/Generation; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return (int) (Texture.Width*Scale); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return (int) (Texture.Height*Scale); }
        }

        /// <summary>
        /// Gets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public float Speed
        {
            get { return _projectileMoveSpeed; }
        }

        #region IEntity Members

        /// <summary>
        /// Updates the Entity.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="input">The input.</param>
        /// <param name="delta">The delta.</param>
        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            var change = (float) (_projectileMoveSpeed*delta/1000.0);

            double x = Position.X + change*Math.Cos(_radians);
            double y = Position.Y + change*Math.Sin(_radians);
            if (x > _viewport.Width + Texture.Width)
                x = -Texture.Width + 1;
            else if (x < -Texture.Width)
                x = _viewport.Width;
            else if (y < -Texture.Height)
                y = _viewport.Height;
            else if (y > _viewport.Height + Texture.Height)
                y = -Height + 1;

            Position = new Vector2((float) x, (float) y);
        }

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                             Position,
                             null,
                             Color.White,
                             0f,
                             new Vector2((int) (Texture.Width/2.0), (int) (Texture.Width/2.0)),
                             (float) Scale,
                             SpriteEffects.None,
                             0f);
            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Gets the circle for rough collision detections.
        /// </summary>
        /// <returns></returns>
        public Circle GetCircle()
        {
            double radius = Texture.Width/2.0*Scale;
            return new Circle(Position, radius);
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
        /// Initializes Asteroid
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="position">The position.</param>
        /// <param name="radians">The radians.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="generation">The generation.</param>
        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed,
                               int generation)
        {
            Texture = texture;
            Position = position;
            _viewport = viewport;

            Active = true;

            _projectileMoveSpeed = speed;

            Position = position;

            _radians = radians;

            Generation = generation;
        }
    }
}