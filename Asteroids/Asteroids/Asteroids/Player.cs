using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    internal class Player : IEntity
    {
        private double _speed;
        private Texture2D _texture;
        public Texture2D Texture { get { return _texture; } }
        public double X { get; set; }
        public double Y { get; set; }
        public double MaxSpeed { get; set; }
        public double radPerSecond { get; set; }
        public double accelerationPerSecond { get; set; }
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
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }

        public double Angle { get; set; }

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, float MaxSpeed, int lives)
        {
            this.MaxSpeed = MaxSpeed;
            radPerSecond = Math.PI * 2;
            accelerationPerSecond = 100;
            Angle = MathHelper.ToRadians(-90);
            X = position.X;
            Y = position.Y;
            _texture = texture;
            Lives = lives;
            _viewport = viewport;
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            LastDeath += delta;
            double percent = delta / 1000.0;
            if (input.Left())
            {
                Angle -= radPerSecond * percent;
            }
            if (input.Right())
            {
                Angle += radPerSecond * percent;
            }
            if (input.Thrusters())
            {
                Speed += accelerationPerSecond * percent;
            }
            else
            {
                Speed -= accelerationPerSecond * percent;
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
                (Width / 2) * 1.25);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (LastDeath > 1000 || LastDeath / 100 % 2 == 0)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, (float)(Angle + Math.PI / 2),
                                 new Vector2((int)(_texture.Width / 2.0), (int)(_texture.Height / 2.0)), 1f,
                                 SpriteEffects.None, 0f);
            
            }
            for (int i = 0; i < Lives; i++)
            {
                spriteBatch.Draw(_texture, new Vector2(_viewport.Width - _texture.Width * (i + 2), 10), null, Color.White, 0,
                                 new Vector2(0, 0), 1f,
                                 SpriteEffects.None, 0f);
                
            }

            foreach (var circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public Circle[] GetCircles()
        {
            return new Circle[] { 
                new Circle(X, Y, Width / 3), 
                new Circle(X + Width/3 * Math.Cos(Angle), Y + Width/3 * Math.Sin(Angle), Width / 10), 
                new Circle(X + Width/2.5 * Math.Cos(Angle), Y + Width/2.5 * Math.Sin(Angle), Width / 15), 
                new Circle(X + Width/3 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/3 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 7), 
                new Circle(X + Width/2.5 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/2.5 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 6), 
                new Circle(X + Width/2 * Math.Cos(Angle + MathHelper.ToRadians(135)), Y + Width/2 * Math.Sin(Angle + MathHelper.ToRadians(135)), Width / 9), 
                new Circle(X + Width/3 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/3 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 7), 
                new Circle(X + Width/2.5 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/2.5 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 6), 
                new Circle(X + Width/2 * Math.Cos(Angle + MathHelper.ToRadians(-135)), Y + Width/2 * Math.Sin(Angle + MathHelper.ToRadians(-135)), Width / 9)};
        }
    }
}