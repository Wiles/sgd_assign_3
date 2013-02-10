﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Asteroid
    {
        public bool Active;
        public Vector2 Position;
        private float _projectileMoveSpeed;
        private double _radians;
        private Texture2D _texture;
        private Viewport _viewport;

        private double _x;

        private double _y;

        public double Radians
        {
            get { return _radians; }
        }

        public int Generation { get; set; }

        private double Scale
        {
            get { return 1.0/Generation; }
        }

        public int Width
        {
            get { return (int) (_texture.Width*Scale); }
        }

        public int Height
        {
            get { return (int) (_texture.Height*Scale); }
        }

        public float Speed
        {
            get { return _projectileMoveSpeed; }
        }


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed,
                               int generation)
        {
            _texture = texture;
            Position = position;
            _viewport = viewport;

            Active = true;

            _projectileMoveSpeed = speed;

            _x = position.X;
            _y = position.Y;

            _radians = radians;

            Generation = generation;
        }

        public void Update()
        {
            Position.X += _projectileMoveSpeed;

            _x += _projectileMoveSpeed*Math.Cos(_radians);
            _y += _projectileMoveSpeed*Math.Sin(_radians);
            if (_x > _viewport.Width + _texture.Width)
                _x = -_texture.Width + 1;
            else if (_x < -_texture.Width)
                _x = _viewport.Width;
            else if (_y < -_texture.Height)
                _y = _viewport.Height;
            else if (_y > _viewport.Height + _texture.Height)
                _y = -Height + 1;

            Position.X = (int) _x;
            Position.Y = (int) _y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f,
                             new Vector2((int) (Width/2.0), (int) (Height/2.0)), (float) Scale, SpriteEffects.None, 0f);
        }

        public Circle GetCircle()
        {
            double radius = _texture.Width/2.0*Scale;
            return new Circle(_x, _y, radius);
        }
    }
}