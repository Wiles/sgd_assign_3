using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Shooter
{
    class Projectile
    {
        // Image representing the Projectile
        private Texture2D _texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        private double xTravel = 0.0;
        private double yTravel = 0.0;

        private double _x;

        private double _y;

        private double _radians;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Represents the viewable boundary of the game
        Viewport _viewport;

        // Get the width of the projectile ship
        public int Width
        {
            get { return _texture.Width; }
        }

        // Get the height of the projectile ship
        public int Height
        {
            get { return _texture.Height; }
        }

        // Determines how fast the projectile moves
        float _projectileMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed)
        {
            _texture = texture;
            Position = position;
            _viewport = viewport;

            Active = true;

            Damage = 2;

            _projectileMoveSpeed = speed;

            _x = position.X;
            _y = position.Y;

            _radians = radians;
        }

        public void Update()
        {
            // Projectiles always move to the right
            Position.X += _projectileMoveSpeed;

            _x += _projectileMoveSpeed  * Math.Cos(_radians);
            _y += _projectileMoveSpeed  * Math.Sin(_radians);
            xTravel += _projectileMoveSpeed * Math.Cos(_radians);
            yTravel += _projectileMoveSpeed * Math.Sin(_radians);
            // Deactivate the bullet if it goes out of screen
            if (_x > _viewport.Width + Width)
                _x = -Width + 1;
            else if (_x < -Width)
                _x = _viewport.Width;
            else if (_y < -Height)
                _y = _viewport.Height;
            else if (_y > _viewport.Height + Height)
                _y = -Height + 1;

            Position.X = (int)_x;
            Position.Y = (int)_y;
            if ( new Vector2((int)xTravel, (int)yTravel).Length() > _viewport.Width/2.0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f,
            new Vector2((int)(Width / 2.0), (int)(Height / 2.0)), 1f, SpriteEffects.None, 0f);
        }
    }
}
