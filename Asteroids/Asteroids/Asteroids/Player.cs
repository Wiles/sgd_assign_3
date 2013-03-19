using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Player : IEntity
    {
        private int _lives;
        private Viewport _viewport;
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        private double RadPerSecond { get; set; }
        private double AccelerationPerSecond { get; set; }

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

        private long LastDeath { get; set; }

        public Vector2 Velocity { get; private set; }
        
        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        public double Angle { get; private set; }

        #region IEntity Members

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            LastDeath += delta;
            var percent = delta/1000.0;
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
                    (float) (Math.Cos(Angle) * AccelerationPerSecond * percent),
                    (float) (Math.Sin(Angle) * AccelerationPerSecond * percent));
            }
            else
            {
                Velocity = new Vector2((float) (Velocity.X *  .98), (float) (Velocity.Y * .98));
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

        public Circle GetCircle()
        {
            if (LastDeath <= 1000)
            {
                return new Circle(Vector2.Zero, 0);
            }
            return new Circle(Position, (Width/2.0)*1.25);
        }

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

        public IEnumerable<Circle> GetCircles()
        {
            return new[]
                {
                    new Circle(Position, Width/3.0),
                    new Circle(new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle)), (float) (Position.Y + Width/3.0*Math.Sin(Angle))), Width/10.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle)), (float) (Position.Y + Width/2.5*Math.Sin(Angle))), Width/15.0),
                    new Circle(new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle + MathHelper.ToRadians(135))),
                               (float) (Position.Y + Width/3.0*Math.Sin(Angle + MathHelper.ToRadians(135)))), Width/7.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle + MathHelper.ToRadians(135))),
                               (float) (Position.Y + Width/2.5*Math.Sin(Angle + MathHelper.ToRadians(135)))), Width/6.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.0*Math.Cos(Angle + MathHelper.ToRadians(135))),
                               (float) (Position.Y + Width/2.0*Math.Sin(Angle + MathHelper.ToRadians(135)))), Width/9.0),
                    new Circle(new Vector2((float) (Position.X + Width/3.0*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                               (float) (Position.Y + Width/3.0*Math.Sin(Angle + MathHelper.ToRadians(-135)))), Width/7.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.5*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                               (float) (Position.Y + Width/2.5*Math.Sin(Angle + MathHelper.ToRadians(-135)))), Width/6.0),
                    new Circle(new Vector2((float) (Position.X + Width/2.0*Math.Cos(Angle + MathHelper.ToRadians(-135))),
                               (float) (Position.Y + Width/2.0*Math.Sin(Angle + MathHelper.ToRadians(-135)))), Width/9.0)
                };
        }

        #endregion

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