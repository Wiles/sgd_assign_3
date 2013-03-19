//File:     Projectile.cs
//Name:     Samuel Lewis (5821103)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Projectile shot by both the player and enemy ships

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// 
    /// </summary>
    internal class Projectile : IEntity
    {
        public bool Active;
        private Vector2 _distanceTravelled;
        private Vector2 _position;
        private float _projectileMoveSpeed;

        private double _radians;
        private Texture2D _texture;

        private Viewport _viewport;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Projectile"/> is hostile.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hostile; otherwise, <c>false</c>.
        /// </value>
        public bool Hostile { get; private set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        private int Width
        {
            get { return _texture.Width; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        private int Height
        {
            get { return _texture.Height; }
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
            double percent = delta/1000.0;

            var travel = new Vector2((float) (_projectileMoveSpeed*percent*Math.Cos(_radians)),
                                     (float) (_projectileMoveSpeed*percent*Math.Sin(_radians)));
            _distanceTravelled += travel;
            float x = _position.X + travel.X;
            float y = _position.Y + travel.Y;

            if (x > _viewport.Width + Width)
                x = -Width + 1;
            else if (x < -Width)
                x = _viewport.Width;
            else if (y < -Height)
                y = _viewport.Height;
            else if (y > _viewport.Height + Height)
                y = -Height + 1;

            _position = new Vector2(x, y);
            if (_distanceTravelled.Length() > _viewport.Width/2.0)
            {
                Active = false;
            }
        }

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f,
                             new Vector2((int) (Width/2.0), (int) (Height/2.0)), 1f, SpriteEffects.None, 0f);
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
            return new Circle(_position, Width/2.0);
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
        /// Initializes the specified viewport.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="position">The position.</param>
        /// <param name="radians">The radians.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hostile">if set to <c>true</c> [hostile].</param>
        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed,
                               bool hostile)
        {
            _texture = texture;
            _position = position;
            _viewport = viewport;
            Hostile = hostile;
            Active = true;

            _projectileMoveSpeed = speed;

            _radians = radians;
        }
    }
}