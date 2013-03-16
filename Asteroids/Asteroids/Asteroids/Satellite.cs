using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    class Satellite : IEntity
    {
        public double X { get; set; }
        public double Y { get; set; }
        private Texture2D _texture;
        public double Radius { get { return _texture.Width/2; } }
        
        public void Initialize(Texture2D texture, Vector2 position)
        {
            X = position.X;
            Y = position.Y;
            _texture = texture;
        }

        public Circle GetCircle()
        {
            return new Circle(X, Y, Radius);
        }

        public Circle[] GetCircles()
        {
            return new Circle[] { 
                new Circle(X , Y - Radius / 2, Radius / 3),
                new Circle(X, Y, Radius / 2),
                new Circle(X + Radius / 2.5, Y, Radius / 2),
                new Circle(X - Radius / 2.5, Y, Radius / 2)};
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2((float)X, (float)Y), null, Color.White, 0,
                             new Vector2((int)(_texture.Width / 2.0), (int)(_texture.Height / 2.0)), 1f,
                             SpriteEffects.None, 0f);
            
            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }   

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {

        }
    }
}
