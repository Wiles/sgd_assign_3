using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Asteroid : IEntity
    {
        public bool Active;
        public Vector2 Position;
        private float _projectileMoveSpeed;
        private double _radians;
        private Viewport _viewport;

        public Texture2D Texture { get; private set; }

        public double Radians
        {
            get { return _radians; }
        }

        public int Generation { get; private set; }

        public double Scale
        {
            get { return 1.0/Generation; }
        }

        public int Width
        {
            get { return (int) (Texture.Width*Scale); }
        }

        public int Height
        {
            get { return (int) (Texture.Height*Scale); }
        }

        public float Speed
        {
            get { return _projectileMoveSpeed; }
        }

        #region IEntity Members

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            var change = (float) (_projectileMoveSpeed*delta/1000.0);
            Position.X += change;
            var x = Position.X + change*Math.Cos(_radians);
            var y = Position.Y + change * Math.Sin(_radians);
            if (x > _viewport.Width + Texture.Width)
                x = -Texture.Width + 1;
            else if (x < -Texture.Width)
                x = _viewport.Width;
            else if (y < -Texture.Height)
                y = _viewport.Height;
            else if (y > _viewport.Height + Texture.Height)
                y = -Height + 1;

            Position = new Vector2((float) x,(float) y);
        }

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

        public Circle GetCircle()
        {
            double radius = Texture.Width/2.0*Scale;
            return new Circle(Position, radius);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] {GetCircle()};
        }

        #endregion

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