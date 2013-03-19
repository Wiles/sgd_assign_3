using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Projectile : IEntity
    {
        public bool Active;
        private Vector2 _position;
        private Vector2 _distanceTravelled = new Vector2();
        private float _projectileMoveSpeed;

        private double _radians;
        private Texture2D _texture;

        private Viewport _viewport;

        public bool Hostile { get; private set; }

        private int Width
        {
            get { return _texture.Width; }
        }

        private int Height
        {
            get { return _texture.Height; }
        }

        #region IEntity Members

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            double percent = delta/1000.0;

            var travel = new Vector2((float) (_projectileMoveSpeed*percent*Math.Cos(_radians)),
                                     (float) (_projectileMoveSpeed*percent*Math.Sin(_radians)));
            _distanceTravelled += travel;
            var x = _position.X + travel.X;
            var y = _position.Y +  travel.Y;

            if (x > _viewport.Width + Width)
                x = -Width + 1;
            else if (x < -Width)
                x = _viewport.Width;
            else if (y < -Height)
                y = _viewport.Height;
            else if (y > _viewport.Height + Height)
                y = -Height + 1;

            _position = new Vector2(x,y);
            if (_distanceTravelled.Length() > _viewport.Width/2.0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f,
                             new Vector2((int) (Width/2.0), (int) (Height/2.0)), 1f, SpriteEffects.None, 0f);
            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public Circle GetCircle()
        {
            return new Circle(_position, Width/2.0);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[] {GetCircle()};
        }

        #endregion

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, double radians, float speed, bool hostile)
        {
            _texture = texture;
            _position = position;
            _viewport = viewport;
            Hostile = hostile;
            Active = true;

            _projectileMoveSpeed = speed;

            _radians = radians;
        }
    }
}