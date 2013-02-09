using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Player _player;
        private KeyboardState _currentKeyboardState;

        private TimeSpan _fireTime;
        private TimeSpan _previousFireTime;
        private const float MaxSpeed = 5.0f;
        private const float ProjectileMoveSpeed = MaxSpeed + 1;


        private Texture2D _projectileTexture;
        private Texture2D _playerTexture;
        private Texture2D _asteroidTexture;
        private readonly List<Projectile> _projectiles = new List<Projectile>();
        private readonly List<Asteroid> _asteroids = new List<Asteroid>(); 

        private readonly Random rand = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _player = new Player();
            _fireTime = TimeSpan.FromSeconds(.25f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            _projectileTexture = Content.Load<Texture2D>("Projectile");

            _playerTexture = Content.Load<Texture2D>("Player");

            _asteroidTexture = Content.Load<Texture2D>("Asteroid");

            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width / 2
                ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            _player.Initialize(_playerTexture, playerPosition, MaxSpeed);

            foreach (var n in Enumerable.Range(1,1))
            {
                var asteroid = new Asteroid();
                asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, new Vector2(0, 0), 0, 1.0f, 1);
                _asteroids.Add(asteroid);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _projectileTexture.Dispose();
            _asteroidTexture.Dispose();
            _playerTexture.Dispose();
            _spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            _currentKeyboardState = Keyboard.GetState();
            UpdatePlayer(gameTime);
            UpdateProjectiles();
            UpdateCollisions();
            UpdateAsteroids();
            base.Update(gameTime);
        }

        private void UpdateCollisions()
        {
            foreach (var projectile in _projectiles)
            {
                if (projectile.Active)
                {
                    foreach (var asteroid in _asteroids)
                    {
                        if (asteroid.Active)
                        {
                            if (asteroid.GetCircle().Intersects(projectile.GetCircle()))
                            {
                                asteroid.Active = false;
                                projectile.Active = false;
                                break;
                            }
                        }
                    }
                    
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            _player.Update();

            // Use the Keyboard / Dpad
            if (_currentKeyboardState.IsKeyDown(Keys.Left))
            {
                _player.Angle -= 0.1f;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right))
            {
                _player.Angle += 0.1f;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Up))
            {
                _player.Speed += .025;
            }
            else
            {
                _player.Speed -= .025;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Space))
            {
                // Fire only every interval we set as the fireTime
                if (gameTime.TotalGameTime - _previousFireTime > _fireTime)
                {
                    // Reset our current time
                    _previousFireTime = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile();
                }
            }
            _player.Y += _player.Speed * Math.Sin(_player.Angle);
            _player.X += _player.Speed * Math.Cos(_player.Angle);
            
            // Make sure that the player does not go out of bounds
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

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Update();

                if (_projectiles[i].Active == false)
                {
                    _projectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateAsteroids()
        {
            // Update the Asteroids
            for (var i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update();

                if (_asteroids[i].Active == false)
                {
                    var parent = _asteroids[i];
                    if (parent.Generation <= 2)
                    {
                        var asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position, parent.Radians - rand.Next(5, 25) * Math.PI / 180, parent.Speed, parent.Generation + 1);
                        _asteroids.Add(asteroid);

                        asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position, parent.Radians + rand.Next(5, 25) * Math.PI / 180, parent.Speed, parent.Generation + 1);
                        _asteroids.Add(asteroid);
                    }
                    _asteroids.RemoveAt(i);
                    
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _player.Draw(_spriteBatch);

            foreach (var t in _projectiles)
            {
                t.Draw(_spriteBatch);
            }

            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void AddProjectile()
        {
            var projectile = new Projectile();
            var pos = new Vector2();
            var angle = _player.Angle;
            pos.X = _player.Position.X + (int)(_playerTexture.Width / 2.0 * Math.Cos(angle));
            pos.Y = _player.Position.Y + (int)(_playerTexture.Height / 2.0 * Math.Sin(angle));
            projectile.Initialize(GraphicsDevice.Viewport, _projectileTexture, pos, angle, ProjectileMoveSpeed);
            _projectiles.Add(projectile);
        }
    }
}
