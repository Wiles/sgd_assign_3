//File:     Player.cs
//Name:     Samuel Lewis (5821103) & Thomas Kempton (5781000)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     The players spaceship

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /// <summary>
    /// 
    /// </summary>
    internal class Player : IEntity
    {
        private int _lives;
        private Viewport _viewport;
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        private double RadPerSecond { get; set; }
        private double AccelerationPerSecond { get; set; }

        /// <summary>
        /// Gets or sets the lives.
        /// </summary>
        /// <value>
        /// The lives.
        /// </value>
        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                if (value < 0)
                {
                    LastDeath = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the time since last death last death.
        /// </summary>
        /// <value>
        /// The last death.
        /// </value>
        private long LastDeath { get; set; }

        /// <summary>
        /// Gets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return Texture.Width; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// Gets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle { get; private set; }

        #region IEntity Members

        /// <summary>
        /// Updates the Entity.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="input">The input.</param>
        /// <param name="delta">The delta.</param>
        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            LastDeath += delta;
            double percent = delta/1000.0;
            if (input.Left())
            {
                Angle -= RadPerSecond*percent;
            }
            if (input.Right())
            {
                Angle += RadPerSecond*percent;
            }
            if (input.Thrusters())
            {
                Velocity += new Vector2(
                    (float) (Math.Cos(Angle)*AccelerationPerSecond*percent),
                    (float) (Math.Sin(Angle)*AccelerationPerSecond*percent));
            }
            else
            {
                Velocity = new Vector2((float) (Velocity.X*.98), (float) (Velocity.Y*.98));
            }

            Position += Velocity;

            if (Position.X > graphics.Viewport.Width + Width)
            {
                Position = new Vector2(-Width + 1, Position.Y);
            }
            else if (Position.X < -Width)
            {
                Position = new Vector2(graphics.Viewport.Width + Width, Position.Y);
            }
            if (Position.Y > graphics.Viewport.Height + Height)
            {
                Position = new Vector2(Position.X, -Height + 1);
            }
            else if (Position.Y < -Height)
            {
                Position = new Vector2(Position.X, graphics.Viewport.Height + Height);
            }
        }

        /// <summary>
        /// Gets the circle for rough collision detections.
        /// </summary>
        /// <returns></returns>
        public Circle GetCircle()
        {
            if (LastDeath <= 1000)
            {
                return new Circle(Vector2.Zero, 0);
            }
            return new Circle(Position, (Width/2.0)*1.25);
        }

        /// <summary>
        /// Draws the Entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (LastDeath > 1000 || LastDeath/100%2 == 0)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, (float) (Angle + Math.PI/2),
                                 new Vector2((int) (Texture.Width/2.0), (int) (Texture.Height/2.0)), 1f,
                                 SpriteEffects.None, 0f);
            }
            for (int i = 0; i < Lives; i++)
            {
                spriteBatch.Draw(Texture, new Vector2(_viewport.Width - Texture.Width*(i + 2), 10), null, Color.White, 0,
                                 new Vector2(0, 0), 1f,
                                 SpriteEffects.None, 0f);
            }

            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Gets the circles used to fine collision detection.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Circle> GetCircles()
        {
            return new[]
                {
                    new Circle(Position, Width/3.0),
                    new Circle(
                        new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle)),
                                    (float) (Position.Y + Width/3.0*Math.Sin(Angle))), Width/10.0),
                    new Circle(
                        new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle)),
                                    (float) (Position.Y + Width/2.5*Math.Sin(Angle))), Width/15.0),
                    new Circle(new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle + MathHelper.ToRadians(135))),
                                           (float) (Position.Y + Width/3.0*Math.Sin(Angle + MathHelper.ToRadians(135)))),
                               Width/7.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle + MathHelper.ToRadians(135))),
                                           (float) (Position.Y + Width/2.5*Math.Sin(Angle + MathHelper.ToRadians(135)))),
                               Width/6.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.0*Math.Cos(Angle + MathHelper.ToRadians(135))),
                                           (float) (Position.Y + Width/2.0*Math.Sin(Angle + MathHelper.ToRadians(135)))),
                               Width/9.0),
                    new Circle(
                        new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                                    (float) (Position.Y + Width/3.0*Math.Sin(Angle + MathHelper.ToRadians(-135)))),
                        Width/7.0),
                    new Circle(
                        new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                                    (float) (Position.Y + Width/2.5*Math.Sin(Angle + MathHelper.ToRadians(-135)))),
                        Width/6.0),
                    new Circle(
                        new Vector2((float) (Position.X + Width/2.0*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                                    (float) (Position.Y + Width/2.0*Math.Sin(Angle + MathHelper.ToRadians(-135)))),
                        Width/9.0)
                };
        }

        #endregion

        /// <summary>
        /// Initializes the specified viewport.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="position">The position.</param>
        /// <param name="lives">The lives.</param>
        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, int lives)
        {
            RadPerSecond = MathHelper.TwoPi;
            AccelerationPerSecond = 5;
            Angle = MathHelper.ToRadians(-90);
            Position = position;
            Texture = texture;
            Lives = lives;
            _viewport = viewport;
        }
    }
}