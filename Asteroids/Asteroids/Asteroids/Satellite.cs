//File:     Satallite.cs
//Name:     Samuel Lewis (5821103) & Thomas Kempton (5781000)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Enemy ship

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// 
    /// </summary>
    internal class Satellite : IEntity
    {
        private Vector2 _position;
        private double _scale;
        private Texture2D _texture;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Satellite"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Satellite"/> is accurate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if accurate; otherwise, <c>false</c>.
        /// </value>
        public bool Accurate { get; private set; }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public double Scale
        {
            get { return _scale; }
        }

        /// <summary>
        /// Gets or sets the last fired.
        /// </summary>
        /// <value>
        /// The last fired.
        /// </value>
        public long LastFired { get; set; }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        private double Radius
        {
            get { return _texture.Width*_scale/2.0; }
        }

        #region IEntity Members

        /// <summary>
        /// Gets the circle for rough collision detections.
        /// </summary>
        /// <returns></returns>
        public Circle GetCircle()
        {
            return new Circle(_position, Radius);
        }

        /// <summary>
        /// Gets the circles used to fine collision detection.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Circle> GetCircles()
        {
            return new[]
                {
                    new Circle(new Vector2(_position.X, (float) (_position.Y - Radius/2.0)), Radius/3.0),
                    new Circle(new Vector2((float) (_position.X + Radius/2.5), _position.Y), Radius/2.0),
                    new Circle(new Vector2((float) (_position.X - Radius/2.5), _position.Y), Radius/2.0)
                };
        }

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0,
                             new Vector2((int) (_texture.Width/2.0), (int) (_texture.Height/2.0)), (float) _scale,
                             SpriteEffects.None, 0f);

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
            LastFired += delta;
            _position = new Vector2((float) (_position.X + 250.0*delta/1000.0/_scale), _position.Y);
            if (_position.X > graphics.Viewport.Width)
            {
                Active = false;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the specified texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="position">The position.</param>
        /// <param name="accurate">if set to <c>true</c> [accurate].</param>
        /// <param name="scale">The scale.</param>
        public void Initialize(Texture2D texture, Vector2 position, bool accurate, double scale)
        {
            _position = position;
            _texture = texture;
            Accurate = accurate;
            _scale = scale;
            Active = true;
        }
    }
}