using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    class Explosion:IEntity
    {
        private Random rand = new Random();
        public bool Active {get;set;}
        public Vector2 Position { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public double Rotation { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        private long elapsedTime;

        public Circle GetCircle()
        {
            return new Circle(0, 0, 0);
        }

        public Circle[] GetCircles()
        {
            return new Circle[] { GetCircle()};
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var x in Enumerable.Range(0, 4))
            {
                foreach (var y in Enumerable.Range(0, 4))
                {
                    var position = Position;
                    var width = Texture.Width / 4;
                    var height = Texture.Height / 4;
                    var rec = new Rectangle( width * x, width * y , width, height);

                    position += new Vector2() + new Vector2(width * x * Scale, height * y * Scale);
                    var centre = Position + new Vector2(Texture.Width / 2 * Scale, Texture.Height / 2 * Scale);
                    var angle = Math.Atan2(position.Y - centre.Y, position.X - centre.X);

                    position.X += (float)((50.0 * elapsedTime / 1000.0) * Math.Cos(angle));
                    position.Y += (float)((50.0 * elapsedTime / 1000.0) * Math.Sin(angle));

                    spriteBatch.Draw(
                        Texture,
                        position,
                        rec,//source
                        Color.White,
                        (x + y % 2 == 0)?1:-1 * (float)(((elapsedTime % 1000.0 /1000.0) * (MathHelper.TwoPi))),//rotation
                        new Vector2(width/2,height/2),//origin
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

            elapsedTime += delta;
            if(elapsedTime > 1000){
                Active = false;
            }
        }
    }
}
