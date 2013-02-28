using System;
using System.Collections.Generic;
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

        public Circle(double X, double Y, double Radius)
        {
            this.X = X;
            this.Y = Y;
            this.Radius = Radius;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        public bool Intersects(Circle that)
        {
            if(that.Radius <= 0.0 || this.Radius <= 0.0){
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

        public static Boolean Intersects(Circle[] one, Circle[] two)
        {
            foreach (var c in one)
            {
                foreach (var d in two)
                {
                    if (c.Intersects(d))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}