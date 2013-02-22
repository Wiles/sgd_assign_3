using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class Game1 : Game
    {
        private const float MaxSpeed = 5.0f;
        private const float ProjectileMoveSpeed = MaxSpeed + 1;
        private readonly List<Asteroid> _asteroids = new List<Asteroid>();
        private readonly List<Projectile> _projectiles = new List<Projectile>();
        private readonly Random rand = new Random();
        private readonly Score score = new Score();
        private Texture2D _asteroidTexture;
        private KeyboardState _currentKeyboardState;

        private TimeSpan _fireTime;
        private GraphicsDeviceManager _graphics;
        private Player _player;
        private Texture2D _playerTexture;

        private Texture2D _projectileTexture;
        private SpriteFont _scoreFont;
        private SpriteBatch _spriteBatch;

        private SoundEffect _shootSound;
        private SoundEffect _explosionSound;

        private long _lastFire;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _player = new Player();
            _fireTime = TimeSpan.FromSeconds(.25f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _projectileTexture = Content.Load<Texture2D>("Projectile");

            _playerTexture = Content.Load<Texture2D>("Player");

            _asteroidTexture = Content.Load<Texture2D>("Asteroid");

            _scoreFont = Content.Load<SpriteFont>("gameFont");

            _shootSound = Content.Load<SoundEffect>("Shoot");

            _explosionSound = Content.Load<SoundEffect>("Explosion");

            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width/2
                                             ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height/2);
            _player.Initialize(GraphicsDevice.Viewport, _playerTexture, playerPosition, MaxSpeed, 3);

            foreach (int n in Enumerable.Range(1, 100))
            {
                var asteroid = new Asteroid();
                asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, new Vector2(32, 32), 0, 0.0f, 1);
                _asteroids.Add(asteroid);
            }

            score.Initialize(GraphicsDevice.Viewport, _scoreFont, new Vector2(0, 0));
        }

        protected override void UnloadContent()
        {
            _projectileTexture.Dispose();
            _asteroidTexture.Dispose();
            _playerTexture.Dispose();
            _spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {

            long delta = gameTime.ElapsedGameTime.Milliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            _currentKeyboardState = Keyboard.GetState();
            UpdatePlayer(delta);
            UpdateCollisions();
            UpdateProjectiles(delta);
            UpdateAsteroids(delta);
            base.Update(gameTime);
        }

        private void UpdateCollisions()
        {
            foreach (Projectile projectile in _projectiles)
            {
                if (projectile.Active)
                {
                    foreach (Asteroid asteroid in _asteroids)
                    {
                        if (asteroid.Active)
                        {
                            if (asteroid.GetCircle().Intersects(projectile.GetCircle()))
                            {
                                asteroid.Active = false;
                                projectile.Active = false;
                                _explosionSound.Play();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePlayer(long delta)
        {
            _player.Update(delta);

            if (_currentKeyboardState.IsKeyDown(Keys.Left))
            {
                _player.Angle -= 0.01f;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right))
            {
                _player.Angle += 0.01f;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Up))
            {
                _player.Speed += .025;
            }
            else
            {
                _player.Speed -= .025;
            }

            _lastFire += delta;

            if (_currentKeyboardState.IsKeyDown(Keys.Space))
            {
                if (_lastFire > _fireTime.Milliseconds)
                {
                    _lastFire = 0;
                    _shootSound.Play();
                    AddProjectile();
                }
            }
            _player.Y += _player.Speed*Math.Sin(_player.Angle);
            _player.X += _player.Speed*Math.Cos(_player.Angle);

            if (_player.X > GraphicsDevice.Viewport.Width + _player.Width)
            {
                _player.X = -_player.Width + 1;
            }
            else if (_player.X < -_player.Width)
            {
                _player.X = GraphicsDevice.Viewport.Width + _player.Width;
            }
            if (_player.Y > GraphicsDevice.Viewport.Height + _player.Height)
            {
                _player.Y = -_player.Height + 1;
            }
            else if (_player.Y < -_player.Height)
            {
                _player.Y = GraphicsDevice.Viewport.Height + _player.Height;
            }
        }

        private void UpdateProjectiles(long delta)
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Update(delta);

                if (_projectiles[i].Active == false)
                {
                    _projectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateAsteroids(long delta)
        {
            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update(delta);

                if (_asteroids[i].Active == false)
                {
                    Asteroid parent = _asteroids[i];

                    switch (parent.Generation)
                    {
                        case (1):
                            score.AddPoints(20);
                            break;
                        case (2):
                            score.AddPoints(50);
                            break;
                        case (3):
                            score.AddPoints(100);
                            break;
                    }

                    if (parent.Generation <= 2)
                    {
                        var asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians - rand.Next(10, 30)*Math.PI/180, parent.Speed,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);

                        asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians + rand.Next(10, 30)*Math.PI/180, parent.Speed,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);
                    }
                    _asteroids.RemoveAt(i);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _player.Draw(_spriteBatch);

            foreach (Projectile t in _projectiles)
            {
                t.Draw(_spriteBatch);
            }

            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.Draw(_spriteBatch);
            }

            score.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void AddProjectile()
        {
            var projectile = new Projectile();
            var pos = new Vector2();
            double angle = _player.Angle;
            pos.X = _player.Position.X + (int) (_playerTexture.Width/2.0*Math.Cos(angle));
            pos.Y = _player.Position.Y + (int) (_playerTexture.Height/2.0*Math.Sin(angle));
            projectile.Initialize(GraphicsDevice.Viewport, _projectileTexture, pos, angle, ProjectileMoveSpeed);
            _projectiles.Add(projectile);
        }
    }
}