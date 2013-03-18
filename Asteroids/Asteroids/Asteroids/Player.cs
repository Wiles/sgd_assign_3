using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Player : IEntity
    {
        private double _speed;
        public Texture2D Texture { get; private set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double MaxSpeed { get; set; }
        public double RadPerSecond { get; set; }
        public double AccelerationPerSecond { get; set; }
        private int _lives;
        public int Lives { get { return _lives; } set { _lives = value; LastDeath = 0; } }
        private Viewport _viewport;
        public long LastDeath { get; set; }

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value <= 0)
                {
                    _speed = 0;
                }
                else if (value > MaxSpeed)
                {
                    _speed = MaxSpeed;
                }
                else
                {
                    _speed = value;
                }
            }
        }

        public Vector2 Position
        {
            get { return new Vector2((int)X, (int)Y); }
        }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        public double Angle { get; set; }

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, float maxSpeed, int lives)
        {
            MaxSpeed = maxSpeed;
            RadPerSecond = Math.PI * 2;
            AccelerationPerSecond = 100;
            Angle = MathHelper.ToRadians(-90);
            X = position.X;
            Y = position.Y;
            Texture = texture;
            Lives = lives;
            _viewport = viewport;
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            LastDeath += delta;
            double percent = delta / 1000.0;
            if (input.Left())
            {
                Angle -= RadPerSecond * percent;
            }
            if (input.Right())
            {
                Angle += RadPerSecond * percent;
            }
            if (input.Thrusters())
            {
                Speed += AccelerationPerSecond * percent;
            }
            else
            {
                Speed -= AccelerationPerSecond * percent;
            }


            Y += Speed * percent * Math.Sin(Angle);
            X += Speed * percent * Math.Cos(Angle);

            if (X > graphics.Viewport.Width + Width)
            {
                X = -Width + 1;
            }
            else if (X < -Width)
            {
                X = graphics.Viewport.Width + Width;
            }
            if (Y > graphics.Viewport.Height + Height)
            {
                Y = -Height + 1;
            }
            else if (Y < -Height)
            {
                Y = graphics.Viewport.Height + Height;
            }
        }

        public Circle GetCircle()
        {
            if(LastDeath <= 1000){
                return new Circle(0,0,0);
            }
            return new Circle(X,
                Y, 
                (Width / 2.0) * 1.25);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (LastDeath > 1000 || LastDeath / 100 % 2 == 0)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, (float)(Angle + Math.PI / 2),
                                 new Vector2((int)(Texture.Width / 2.0), (int)(Texture.Height / 2.0)), 1f,
                                 SpriteEffects.None, 0f);
            
            }
            for (int i = 0; i < Lives; i++)
            {
                spriteBatch.Draw(Texture, new Vector2(_viewport.Width - Texture.Width * (i + 2), 10), null, Color.White, 0,
                                 new Vector2(0, 0), 1f,
                                 SpriteEffects.None, 0f);
                
            }

            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] { 
                new Circle(X, Y, Width / 3.0), 
                new Circle(X + Width/3.0 * Math.Cos(Angle), Y + Width/3.0 * Math.Sin(Angle), Width / 10.0), 
                new Circle(X + Width/2.5 * Math.Cos(Angle), Y + Width/2.5 * Math.Sin(Angle), Width / 15.0), 
                new Circle(X + Width/3.0 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/3.0 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 7.0), 
                new Circle(X + Width/2.5 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/2.5 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 6.0), 
                new Circle(X + Width/2.0 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/2.0 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 9.0), 
                new Circle(X + Width/3.0 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/3.0 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 7.0), 
                new Circle(X + Width/2.5 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/2.5 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 6.0), 
                new Circle(X + Width/2.0 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/2.0 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 9.0)};
        }
    }
}