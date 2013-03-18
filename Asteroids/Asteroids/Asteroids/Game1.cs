using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class Game1 : Game
    {
        private bool _running;
        private bool _hard;
        private bool _debug;

        private MenuScreen _pause;
        private MenuScreen _gameOver;

        private Menu _menu;

        private const float MaxSpeed = 500.0f;
        private const float ProjectileMoveSpeed = MaxSpeed + 1;
        private readonly List<Asteroid> _asteroids = new List<Asteroid>();
        private readonly List<Projectile> _projectiles = new List<Projectile>();
        private readonly Random _rand = new Random();
        private Score _score = new Score();
        private Texture2D _asteroidTexture;
        private Texture2D _collisionTexture;

        private TimeSpan _fireTime;
        private Player _player;
        private Texture2D _playerTexture;
        private Satellite _satellite;

        private Texture2D _projectileTexture;
        private SpriteFont _scoreFont;
        private SpriteBatch _spriteBatch;

        private SoundEffect _shootSound;
        private SoundEffect _explosionSound;
        private SoundEffect _menuMove;
        private SoundEffect _menuSelect;
        private SoundEffect _menuBack;
        private SoundEffect _playerExplosion;

        private Texture2D _ufoTexture;

        private List<Explosion> _explosions;

        private long _lastFire;

        public Game1()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _satellite = new Satellite();
            _fireTime = TimeSpan.FromSeconds(.25f);
            _menu = new Menu();
            _explosions = new List<Explosion>();

            base.Initialize();
        }

        private void InitMenu()
        {
            var start = new MenuScreen("Asteroids", null);
            var options = new MenuScreen("Options", null);
            var diff = new MenuScreen("Difficulty", options);
            var debugger = new MenuScreen("Debug", options);
            var about = new MenuScreen("About", null);
            _pause = new MenuScreen("Paused", null);
            _gameOver = new MenuScreen("Game Over", null);
            var controls = new MenuScreen("Controls", null);

            var e = new Dictionary<string, Action>
                {
                    {"Hard", () => { _hard = true; 
                        _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);}}, 
                    {"Easy", () => { _hard = false; 
                         _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);}}
                };
            diff.Elements = e;


            e = new Dictionary<string, Action>
                {
                    {"On", () => { _debug = true;
                                     Circle.Texture = _collisionTexture;
                        _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);}}, 
                    {"Off", () => { _debug = false;
                                      Circle.Texture = null;
                         _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);}}
                };
            debugger.Elements = e;

            e = new Dictionary<string, Action>
                {
                    {"Start Game", NewGame},
                    {"Options", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options); }},
                    {"About", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(about); }},
                    {"Controls", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(controls); }},
                    {"Quit", Exit}
                };

            start.Elements = e;
            
            e = new Dictionary<string, Action>
                {
                    {"Resume", () => { _running = true; }},
                    {"New Game", NewGame },
                    {"Options", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options); }},
                    {"About", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(about); }},
                    {"Controls", () => { _menu.SelectedMenuScreen = _menu.Screens.IndexOf(controls); }},
                    {"Quit", Exit}
                };

            _pause.Elements = e;

            e = new Dictionary<string, Action>
                {
                    {"New game", NewGame},
                    {"Quit", Exit}
                };

            _gameOver.Elements = e;

            e = new Dictionary<string, Action>
                {
                    {"Samuel Lewis & Thomas Kempton", null},
                    {"Simulation and Game Development", null},
                    {"Assignment #3 - SET Asteroids", null}
                };

            about.Elements = e;


            e = new Dictionary<string, Action>
                {
                    {"Debug", () =>
                        {
                            debugger.SelectedIndex = _debug ? 0 : 1;
                            _menu.SelectedMenuScreen = _menu.Screens.IndexOf(debugger);
                        }},
                    {"Difficulty", () =>
                        {
                            diff.SelectedIndex = _hard ? 0 : 1;
                            _menu.SelectedMenuScreen = _menu.Screens.IndexOf(diff);
                        }
                    }
                };

            options.Elements = e;

            e = new Dictionary<string, Action>
            {
                {"Fire/Select        Space/R1", null},
                {"Pause/Back         Esc/Back", null},
                {"Thrusters       Up/D-pad up", null},
                {"Up            Left/D-pad Up", null},
                {"Down        Left/D-pad Down", null},
                {"Left        Left/D-pad left", null},
                {"Right     Right/D-pad right", null}
            };

            controls.Elements = e;

            _menu.AddMenuScreen(start);
            _menu.AddMenuScreen(diff);
            _menu.AddMenuScreen(_gameOver);
            _menu.AddMenuScreen(options);
            _menu.AddMenuScreen(debugger);
            _menu.AddMenuScreen(about);
            _menu.AddMenuScreen(_pause);
            _menu.AddMenuScreen(controls);

            _menu.SelectedMenuScreen = _menu.Screens.IndexOf(start);
            _menu.MainMenuIndex = _menu.Screens.IndexOf(start);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _projectileTexture = Content.Load<Texture2D>("Projectile");

            _playerTexture = Content.Load<Texture2D>("Player");

            _asteroidTexture = Content.Load<Texture2D>("Asteroid");
            
            //Don't set this if you don't want to draw the hitboxes
            _collisionTexture = Content.Load<Texture2D>("Collision");

            _scoreFont = Content.Load<SpriteFont>("Mono");

            _menuMove = Content.Load<SoundEffect>("menuMove");
            _menuSelect = Content.Load<SoundEffect>("menuSelect");
            _menuBack = Content.Load<SoundEffect>("menuBack");
            _playerExplosion = Content.Load<SoundEffect>("playerExplosion");

            _menu.Initialize( GraphicsDevice.Viewport, _scoreFont, _menuMove, _menuSelect, _menuBack);
            InitMenu();

            _shootSound = Content.Load<SoundEffect>("Shoot");

            _ufoTexture = Content.Load<Texture2D>("UFO");
            _satellite.Initialize(_ufoTexture, new Vector2(50, 50));

            _explosionSound = Content.Load<SoundEffect>("Explosion");
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
            var inputState = new Input(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
            if (_running)
            {
                if (inputState.Escape())
                {
                    _running = false;
                    _menu.SelectedMenuScreen = _menu.MainMenuIndex;
                    return;
                }

                _player.Update(GraphicsDevice, inputState, delta);

                _lastFire += delta;

                if (inputState.Fire())
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
                    _projectiles[i].Update(GraphicsDevice, inputState, delta);

                    if (_projectiles[i].Active == false)
                    {
                        _projectiles.RemoveAt(i);
                    }
                }


                for (int i = _explosions.Count - 1; i >= 0; i--)
                {
                    _explosions[i].Update(GraphicsDevice, inputState, delta);

                    if (_explosions[i].Active == false)
                    {
                        _explosions.RemoveAt(i);
                    }
                }
                UpdateAsteroids(delta, inputState);
                base.Update(gameTime);
            }
            else
            {
                _menu.Update(GraphicsDevice, inputState, delta);
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
                        _playerExplosion.Play();
                        var explosion = new Explosion
                        {
                            Active = true,
                            Direction = _player.Angle,
                            Position = _player.Position - new Vector2((float) (_player.Width / 2.0), (float) (_player.Height / 2.0)),
                            Scale = 1.0f,
                            Texture = _player.Texture,
                            Speed = _player.Speed
                        };

                        _explosions.Add(explosion);
                    _player.X = GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                    _player.Y = GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height / 2;
                    _player.Angle = MathHelper.ToRadians(-90);
                    _player.Speed = 0;
                    if(_player.Lives < 0)
                    {
                        _running = false;
                        _menu.MainMenuIndex = _menu.Screens.IndexOf(_gameOver);
                        _menu.SelectedMenuScreen = _menu.MainMenuIndex;
                    }
                }
            }
        }

        private void UpdateAsteroids(long delta, Input inputState)
        {
            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update(GraphicsDevice, inputState, delta);

                if (_asteroids[i].Active == false)
                {
                    Asteroid parent = _asteroids[i];

                    switch (parent.Generation)
                    {
                        case (1):
                            _score.AddPoints(20);
                            break;
                        case (2):
                            _score.AddPoints(50);
                            break;
                        case (3):
                            _score.AddPoints(100);
                            break;
                    }

                    if (parent.Generation <= 2)
                    {
                        var asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians - _rand.Next(10, 30)*Math.PI/180, parent.Speed,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);

                        asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians + _rand.Next(10, 30)*Math.PI/180, parent.Speed,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);
                    }
                    var explosion = new Explosion
                    {
                        Active = true,
                        Direction = parent.Radians,
                        Position = parent.Position - new Vector2((float) (parent.Width/2.0), (float) (parent.Height/2.0)),
                        Scale = (float)parent.Scale,
                        Texture = parent.Texture,
                        Speed = parent.Speed
                    };
                    _explosions.Add(explosion);
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

            if (_running)
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

                foreach (Explosion explosion in _explosions)
                {
                    explosion.Draw(_spriteBatch);
                }

                _score.Draw(_spriteBatch);
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
                var direction = _rand.NextDouble() * Math.PI * 2;
                var x = GraphicsDevice.Viewport.Width/2.0 + GraphicsDevice.Viewport.Width * Math.Cos(direction);
                
                var y = GraphicsDevice.Viewport.Height/2.0 + GraphicsDevice.Viewport.Height * Math.Sin(direction);
                x = MathHelper.Clamp((float)x, 0, GraphicsDevice.Viewport.Width);
                y = MathHelper.Clamp((float)y, 0, GraphicsDevice.Viewport.Height);
                var init = _rand.NextDouble() * Math.PI * 2;
                var asteroid = new Asteroid();
                asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, new Vector2((int)x, (int)y), init, 50.0f, 1);
                _asteroids.Add(asteroid);
            }
        }

        private void NewGame()
        {
            _explosions.Clear();
            _asteroids.Clear();
            _projectiles.Clear();
            _player = new Player();
            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width / 2
                                             ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            _player.Initialize(GraphicsDevice.Viewport, _playerTexture, playerPosition, MaxSpeed, 3);

            StartWave(5);
            _score = new Score();
            _score.Initialize(_scoreFont, new Vector2(0, 0));
            _running = true;
            _menu.MainMenuIndex = _menu.Screens.IndexOf(_pause);
        }
    }
}