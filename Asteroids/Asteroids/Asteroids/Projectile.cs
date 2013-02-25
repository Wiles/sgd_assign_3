using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    internal class Projectile : IEntity
    {
        public bool Active;
        public Vector2 Position;
        private float _projectileMoveSpeed;

        private double _radians;
        private Texture2D _texture;

        private Viewport _viewport;
        private double _x;
        private double _xTravel;
        private double _y;
        private double _yTravel;

        public int Width
        {
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed)
        {
            _texture = texture;
            Position = position;
            _viewport = viewport;

            Active = true;

            _projectileMoveSpeed = speed;

            _x = position.X;
            _y = position.Y;

            _radians = radians;
        }

        public void Update(GraphicsDevice graphics, KeyboardState input, long delta)
        {
            double percent = delta / 1000.0;
            Position.X += (float)(_projectileMoveSpeed * percent);

            _x += _projectileMoveSpeed * percent * Math.Cos(_radians);
            _y += _projectileMoveSpeed * percent * Math.Sin(_radians);
            _xTravel += _projectileMoveSpeed * percent * Math.Cos(_radians);
            _yTravel += _projectileMoveSpeed * percent * Math.Sin(_radians);
            if (_x > _viewport.Width + Width)
                _x = -Width + 1;
            else if (_x < -Width)
                _x = _viewport.Width;
            else if (_y < -Height)
                _y = _viewport.Height;
            else if (_y > _viewport.Height + Height)
                _y = -Height + 1;

            Position.X = (int) _x;
            Position.Y = (int) _y;
            if (new Vector2((int) _xTravel, (int) _yTravel).Length() > _viewport.Width/2.0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f,
                             new Vector2((int) (Width/2.0), (int) (Height/2.0)), 1f, SpriteEffects.None, 0f);
        }

        public Circle GetCircle()
        {
            double radius = Width/2.0;
            return new Circle(_x + radius, _y + radius, radius);
        }
    }
}