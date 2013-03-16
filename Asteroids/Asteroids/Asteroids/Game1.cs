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
        private bool running;
        private bool hard;

        private Menu _menu;

        private const float MaxSpeed = 500.0f;
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
        private Satellite _satellite;

        private Texture2D _projectileTexture;
        private SpriteFont _scoreFont;
        private SpriteBatch _spriteBatch;

        private SoundEffect _shootSound;
        private SoundEffect _explosionSound;

        private Texture2D _ufoTexture;

        private long _lastFire;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _player = new Player();
            _satellite = new Satellite();
            _fireTime = TimeSpan.FromSeconds(.25f);
            _menu = new Menu();

            base.Initialize();
        }

        private void InitMenu()
        {
            var diff = new MenuScreen("Difficulty");
            var start = new MenuScreen("Asteroids");
            var gameOver = new MenuScreen("Game Over");
            var about = new MenuScreen("About");

            var e = new Dictionary<string, Action>
                {
                    {"Hard", () => { hard = true; 
                        _menu.selectedMenuScreen = _menu.MainMenuIndex;}}, 
                    {"Easy", () => { hard = false; 
                         _menu.selectedMenuScreen = _menu.MainMenuIndex;}}
                };
            diff.elements = e;

            e = new Dictionary<string, Action>
                {
                    {"Start Game", newGame},
                    {
                        "Difficulty", () =>
                            {
                                diff.selectedIndex = hard ? 0 : 1;
                                _menu.selectedMenuScreen = _menu.Screens.IndexOf(diff);
                            }
                    },
                    {"About", () => { _menu.selectedMenuScreen = _menu.Screens.IndexOf(about); }},
                    {"Quit", Exit}
                };

            start.elements = e;

            e = new Dictionary<string, Action>
                {
                    {"New game", newGame},
                    {"Quit", Exit}
                };

            gameOver.elements = e;

            e = new Dictionary<string, Action>
                {
                    {"Samuel Lewis & Thomas Kempton", null},
                    {"Simulation and Game Development", null},
                    {"Assignment #3 - SET Asteroids", null}
                };

            about.elements = e;

            _menu.AddMenuScreen(start);
            _menu.AddMenuScreen(diff);
            _menu.AddMenuScreen(gameOver);
            _menu.AddMenuScreen(about);

            _menu.selectedMenuScreen = _menu.Screens.IndexOf(start);
            _menu.MainMenuIndex = _menu.Screens.IndexOf(start);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _projectileTexture = Content.Load<Texture2D>("Projectile");

            _playerTexture = Content.Load<Texture2D>("Player");

            _asteroidTexture = Content.Load<Texture2D>("Asteroid");

            //Don't set this if you don't want to draw the hitboxes
            Circle.Texture = Content.Load<Texture2D>("Collision");

            _scoreFont = Content.Load<SpriteFont>("gameFont");

            _menu.Initialize(this, GraphicsDevice.Viewport, _scoreFont);
            InitMenu();

            _shootSound = Content.Load<SoundEffect>("Shoot");

            _ufoTexture = Content.Load<Texture2D>("UFO");
            _satellite.Initialize(_ufoTexture, new Vector2(50, 50));

            _explosionSound = Content.Load<SoundEffect>("Explosion");

            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width/2
                                             ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height/2);
            _player.Initialize(GraphicsDevice.Viewport, _playerTexture, playerPosition, MaxSpeed, 3);
            
            StartWave(1);

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
            _currentKeyboardState = Keyboard.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            if (running)
            {

                _player.Update(GraphicsDevice, _currentKeyboardState, delta);

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

                UpdateCollisions();
                for (int i = _projectiles.Count - 1; i >= 0; i--)
                {
                    _projectiles[i].Update(GraphicsDevice, _currentKeyboardState, delta);

                    if (_projectiles[i].Active == false)
                    {
                        _projectiles.RemoveAt(i);
                    }
                }
                UpdateAsteroids(delta);
                base.Update(gameTime);
            }
            else
            {
                _menu.Update(GraphicsDevice, _currentKeyboardState, delta);
            }
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

            foreach (Asteroid asteroid in _asteroids)
            {
                if(asteroid.GetCircle().Intersects(_player.GetCircle())
                    && Circle.Intersects(asteroid.GetCircles(), _player.GetCircles())){
                        _player.Lives -= 1;
                    _player.X = GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                    _player.Y = GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height / 2;
                    _player.Angle = MathHelper.ToRadians(-90);
                    _player.Speed = 0;
                }
            }
        }

        private void UpdateAsteroids(long delta)
        {
            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update(GraphicsDevice, _currentKeyboardState, delta);

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
                    if(_asteroids.Count == 0){
                        //TODO increase count with each wave
                        StartWave(8);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            _spriteBatch.Begin();
            if (running)
            {
                _satellite.Draw(_spriteBatch);
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
            }
            else
            {
                _menu.Draw(_spriteBatch);
            }
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

        private void StartWave(int asteroids)
        {
            for (int i = 0; i < asteroids; ++i)
            {
                var direction = rand.NextDouble() * Math.PI * 2;
                var x = GraphicsDevice.Viewport.Width/2 + GraphicsDevice.Viewport.Width * Math.Cos(direction);
                
                var y = GraphicsDevice.Viewport.Height/2 + GraphicsDevice.Viewport.Height * Math.Sin(direction);
                x = MathHelper.Clamp((float)x, (float)0, (float)GraphicsDevice.Viewport.Width);
                y = MathHelper.Clamp((float)y, (float)0, (float)GraphicsDevice.Viewport.Height);
                var init = rand.NextDouble() * Math.PI * 2;
                var asteroid = new Asteroid();
                asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, new Vector2((int)x, (int)y), init, 1.0f, 1);
                _asteroids.Add(asteroid);
            }
        }

        public void newGame()
        {
            running = true;
        }
    }
}