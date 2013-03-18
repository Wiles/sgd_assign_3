using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Circle
    {
        public static Texture2D Texture
        {
            get;
            set;
        }

        public Circle(double x, double y, double radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        public bool Intersects(Circle that)
        {
            if(that.Radius <= 0.0 || Radius <= 0.0){
                return false;
            }
            double dx = X - that.X;
            double dy = Y - that.Y;
            double radii = Radius + that.Radius;
            return (dx*dx) + (dy*dy) < radii*radii;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
            spriteBatch.Draw(
                Texture, 
                new Vector2((float)X, (float)Y), 
                null, 
                Color.White, 
                0f,
                new Vector2(Texture.Width/2f, Texture.Width/2f), 
                (float)(Radius * 2f) / Texture.Width, 
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