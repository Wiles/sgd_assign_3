using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    class Explosion:IEntity
    {
        private const int Segments = 4;

        public bool Active {get;set;}
        public Vector2 Position { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        private long _elapsedTime;

        public Circle GetCircle()
        {
            return new Circle(0, 0, 0);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new [] { GetCircle()};
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var x in Enumerable.Range(0, Segments))
            {
                foreach (var y in Enumerable.Range(0, Segments))
                {
                    var position = Position;
                    var width = Texture.Width / Segments;
                    var height = Texture.Height / Segments;
                    var rec = new Rectangle( width * x, width * y , width, height);

                    position += new Vector2() + new Vector2(width * x * Scale, height * y * Scale);
                    var centre = Position + new Vector2((float) (Texture.Width / 2.0 * Scale), (float) (Texture.Height / 2.0 * Scale));
                    var angle = Math.Atan2(position.Y - centre.Y, position.X - centre.X);

                    position.X += (float)((50.0 * _elapsedTime / 1000.0) * Math.Cos(angle));
                    position.Y += (float)((50.0 * _elapsedTime / 1000.0) * Math.Sin(angle));

                    spriteBatch.Draw(
                        Texture,
                        position,
                        rec,//source
                        Color.White,
                        ((x + y) % 2 == 0)?1:-1 * (float)(((_elapsedTime % 1000.0 /1000.0) * (MathHelper.TwoPi))),//rotation
                        new Vector2((float) (width/2.0), (float) (height/2.0)),//origin
                        Scale,
                        SpriteEffects.None,
                        0
                        );
                }
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            Position += new Vector2((float)(Speed * (delta / 1000.0) * Math.Cos(Direction)), (float)(Speed * (delta / 1000.0) * Math.Sin(Direction)));

            _elapsedTime += delta;
            if(_elapsedTime > 1000){
                Active = false;
            }
        }
    }
}
