using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Player
    {
        private double _speed;
        private Texture2D _texture;
        public double X { get; set; }
        public double Y { get; set; }
        public double MaxSpeed { get; set; }

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value <= 0)
                {
                    _speed = 0;
                }
                else if (value > MaxSpeed)
                {
                    _speed = 5;
                }
                else
                {
                    _speed = value;
                }
            }
        }

        public Vector2 Position
        {
            get { return new Vector2((int) X, (int) Y); }
        }

        public int Width
        {
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }

        public double Angle { get; set; }

        public void Initialize(Texture2D texture, Vector2 position, float MaxSpeed)
        {
            this.MaxSpeed = MaxSpeed;
            X = position.X;
            Y = position.Y;
            _texture = texture;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, (float) (Angle + Math.PI/2),
                             new Vector2((int) (_texture.Width/2.0), (int) (_texture.Height/2.0)), 1f,
                             SpriteEffects.None, 0f);
        }
    }
}