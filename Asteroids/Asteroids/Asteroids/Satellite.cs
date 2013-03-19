using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Satellite : IEntity
    {
        private Vector2 _position;
        private double _scale;
        private Texture2D _texture;

        public Vector2 Position
        {
            get { return _position; }
        }

        public bool Active { get; set; }
        public bool Accurate { get; private set; }

        public double Scale
        {
            get { return _scale; }
        }

        public long LastFired { get; set; }

        private double Radius
        {
            get { return _texture.Width*_scale/2.0; }
        }

        #region IEntity Members

        public Circle GetCircle()
        {
            return new Circle(_position, Radius);
        }

        public IEnumerable<Circle> GetCircles()
        {
            return new[]
                {
                    new Circle(new Vector2(_position.X, (float) (_position.Y - Radius/2.0)), Radius/3.0),
                    new Circle(new Vector2((float) (_position.X + Radius/2.5), _position.Y), Radius/2.0),
                    new Circle(new Vector2((float) (_position.X - Radius/2.5), _position.Y), Radius/2.0)
                };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0,
                             new Vector2((int) (_texture.Width/2.0), (int) (_texture.Height/2.0)), (float) _scale,
                             SpriteEffects.None, 0f);

            foreach (Circle circle in GetCircles())
            {
                circle.Draw(spriteBatch);
            }
        }

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            LastFired += delta;
            _position = new Vector2((float) (_position.X + 250.0*delta/1000.0/_scale), _position.Y);
            if (_position.X > graphics.Viewport.Width)
            {
                Active = false;
            }
        }

        #endregion

        public void Initialize(Texture2D texture, Vector2 position, bool accurate, double scale)
        {
            _position = position;
            _texture = texture;
            Accurate = accurate;
            _scale = scale;
            Active = true;
        }
    }
}