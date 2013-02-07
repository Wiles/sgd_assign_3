using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter;


namespace Asteroids
{
    class Player
    {

        // Position of the Player relative to the upper left side of the screen
        public double X { get; set; }
        public double Y { get; set; }
        private Texture2D _texture;
        public Vector2 Position {
            get { return new Vector2((int)X, (int)Y);}
        }
        // Amount of hit points that player has
        public int Health;

        // Get the width of the player ship
        public int Width
        {
            get { return _texture.Width; }
        }

        // Get the height of the player ship
        public int Height {
            get { return _texture.Height; }
        }

        public double Angle
        {
            get; set;
        }

        // Initialize the player
        public void Initialize(Texture2D texture, Vector2 position)
        {
            // Set the starting position of the player around the middle of the screen and to the back
            X = position.X;
            Y = position.Y;
            _texture = texture;
            // Set the player health
            Health = 100;
        }


        // Update the player animation
        public void Update()
        {
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, (float)(Angle + Math.PI/2),
            new Vector2((int)(_texture.Width / 2.0), (int)(_texture.Height / 2.0)), 1f, SpriteEffects.None, 0f);
        }

    }
}
