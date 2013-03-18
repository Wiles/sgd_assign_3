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
        public Texture2D Texture{get; private set;}
        private Viewport _viewport;

        private double _x;

        private double _y;
        
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


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed,
                               int generation)
        {
            Texture = texture;
            Position = position;
            _viewport = viewport;

            Active = true;

            _projectileMoveSpeed = speed;

            _x = position.X;
            _y = position.Y;

            _radians = radians;

            Generation = generation;
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            var change = (float)(_projectileMoveSpeed * delta / 1000.0);
            Position.X += change;
            _x += change * Math.Cos(_radians);
            _y += change * Math.Sin(_radians);
            if (_x > _viewport.Width + Texture.Width)
                _x = -Texture.Width + 1;
            else if (_x < -Texture.Width)
                _x = _viewport.Width;
            else if (_y < -Texture.Height)
                _y = _viewport.Height;
            else if (_y > _viewport.Height + Texture.Height)
                _y = -Height + 1;

            Position.X = (int) _x;
            Position.Y = (int) _y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, 
                Position, 
                null, 
                Color.White, 
                0f,
                new Vector2((int)(Texture.Width / 2.0), (int)(Texture.Width / 2.0)), 
                (float)Scale, 
                SpriteEffects.None, 
                0f);
            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public Circle GetCircle()
        {
            double radius = Texture.Width/2.0*Scale;
            return new Circle(_x, _y, radius);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] { GetCircle() };
        }
    }
}