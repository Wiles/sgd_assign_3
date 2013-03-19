using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Circle
    {
        public Circle(Vector2 position, double radius)
        {
            Position = position;
            Radius = radius;
        }

        public static Texture2D Texture { get; set; }

        public Vector2 Position { get; private set; }
        public double Radius { get; set; }

        public bool Intersects(Circle that)
        {
            if (that.Radius <= 0.0 || Radius <= 0.0)
            {
                return false;
            }
            double dx = Position.X - that.Position.X;
            double dy = Position.Y - that.Position.Y;
            double radii = Radius + that.Radius;
            return (dx*dx) + (dy*dy) < radii*radii;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    new Vector2((float)Position.X, (float)Position.Y),
                    null,
                    Color.White,
                    0f,
                    new Vector2(Texture.Width/2f, Texture.Width/2f),
                    (float) (Radius*2f)/Texture.Width,
                    SpriteEffects.None,
                    0f
                    );
            }
        }

        public static Boolean Intersects(IEnumerable<Circle> one, IEnumerable<Circle> two)
        {
            return (from c in one from d in two where c.Intersects(d) select c).Any();
        }
    }
}