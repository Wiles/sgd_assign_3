using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Explosion : IEntity
    {
        private const int Segments = 4;
        private long _elapsedTime;

        public bool Active { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public double Direction { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }

        #region IEntity Members

        public Circle GetCircle()
        {
            return new Circle(Vector2.Zero, 0);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] {GetCircle()};
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (int x in Enumerable.Range(0, Segments))
            {
                foreach (int y in Enumerable.Range(0, Segments))
                {
                    Vector2 position = Position;
                    int width = Texture.Width/Segments;
                    int height = Texture.Height/Segments;
                    var rec = new Rectangle(width*x, width*y, width, height);

                    position += new Vector2() + new Vector2(width*x*Scale, height*y*Scale);
                    Vector2 centre = Position +
                                     new Vector2((float) (Texture.Width/2.0*Scale), (float) (Texture.Height/2.0*Scale));
                    double angle = Math.Atan2(position.Y - centre.Y, position.X - centre.X);

                    position.X += (float) ((50.0*_elapsedTime/1000.0)*Math.Cos(angle));
                    position.Y += (float) ((50.0*_elapsedTime/1000.0)*Math.Sin(angle));

                    spriteBatch.Draw(
                        Texture,
                        position,
                        rec, //source
                        Color.White,
                        (float)(((_elapsedTime / 10000.0) * (((x + y) % 2 == 0) ? MathHelper.TwoPi : -MathHelper.TwoPi))),//rotation
                        new Vector2((float) (width/2.0), (float) (height/2.0)), //origin
                        Scale,
                        SpriteEffects.None,
                        0
                        );
                }
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            Position += Velocity;

            _elapsedTime += delta;
            if (_elapsedTime > 1000)
            {
                Active = false;
            }
        }

        #endregion
    }
}
