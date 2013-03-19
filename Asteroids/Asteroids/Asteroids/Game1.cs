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
        private const float MaxSpeed = 500.0f;
        private const float ProjectileMoveSpeed = MaxSpeed + 1;
        private const long ExtraLife = 10000L;
        private const long Ufo = 5000;
        private const long SmallUfo = 3;
        private readonly List<Asteroid> _asteroids = new List<Asteroid>();
        private readonly List<Projectile> _projectiles = new List<Projectile>();
        private readonly Random _rand = new Random();
        private Texture2D _asteroidTexture;
        private Texture2D _collisionTexture;
        private bool _debug;
        private List<Satellite> _enemies;
        private SoundEffect _explosionSound;
        private List<Explosion> _explosions;

        private TimeSpan _fireTime;
        private MenuScreen _gameOver;
        private long _lastExtraLife;
        private long _lastFire;
        private long _lastUfo;
        private Menu _menu;
        private SoundEffect _menuBack;
        private SoundEffect _menuMove;
        private SoundEffect _menuSelect;
        private MenuScreen _pause;
        private Player _player;
        private SoundEffect _playerExplosion;
        private Texture2D _playerTexture;

        private Texture2D _projectileTexture;
        private bool _running;
        private Score _score = new Score();
        private SpriteFont _scoreFont;

        private SoundEffect _shootSound;
        private SpriteBatch _spriteBatch;

        private long _ufoCount;
        private Texture2D _ufoTexture;
        private int _wave = 1;

        public Game1()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _enemies = new List<Satellite>();
            _fireTime = TimeSpan.FromSeconds(.25f);
            _menu = new Menu();
            _explosions = new List<Explosion>();

            base.Initialize();
        }

        private void InitMenu()
        {
            var start = new MenuScreen("Asteroids", null);
            var options = new MenuScreen("Options", null);
            var debugger = new MenuScreen("Debug", options);
            var about = new MenuScreen("About", null);
            _pause = new MenuScreen("Paused", null);
            _gameOver = new MenuScreen("Game Over", null);
            var controls = new MenuScreen("Controls", null);


            var e = new Dictionary<string, Action>
                {
                    {
                        "On", () =>
                            {
                                _debug = true;
                                Circle.Texture = _collisionTexture;
                                _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);
                            }
                    },
                    {
                        "Off", () =>
                            {
                                _debug = false;
                                Circle.Texture = null;
                                _menu.SelectedMenuScreen = _menu.Screens.IndexOf(options);
                            }
                    }
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
                    {"New Game", NewGame},
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
                    {
                        "Debug", () =>
                            {
                                debugger.SelectedIndex = _debug ? 0 : 1;
                                _menu.SelectedMenuScreen = _menu.Screens.IndexOf(debugger);
                            }
                    }
                };

            options.Elements = e;

            e = new Dictionary<string, Action>
                {
                    {"Fire/Select        Space/RT", null},
                    {"Pause/Back         Esc/Back", null},
                    {"Thrusters       Up/D-pad up", null},
                    {"Up            Left/D-pad Up", null},
                    {"Down        Left/D-pad Down", null},
                    {"Left        Left/D-pad left", null},
                    {"Right     Right/D-pad right", null}
                };

            controls.Elements = e;

            _menu.AddMenuScreen(start);
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

            _collisionTexture = Content.Load<Texture2D>("Collision");

            _scoreFont = Content.Load<SpriteFont>("Mono");

            _menuMove = Content.Load<SoundEffect>("menuMove");
            _menuSelect = Content.Load<SoundEffect>("menuSelect");
            _menuBack = Content.Load<SoundEffect>("menuBack");
            _playerExplosion = Content.Load<SoundEffect>("playerExplosion");

            _menu.Initialize(GraphicsDevice.Viewport, _scoreFont, _menuMove, _menuSelect, _menuBack);
            InitMenu();

            _shootSound = Content.Load<SoundEffect>("Shoot");

            _ufoTexture = Content.Load<Texture2D>("UFO");

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

                for (int i = _enemies.Count - 1; i >= 0; i--)
                {
                    _enemies[i].Update(GraphicsDevice, inputState, delta);

                    if (_enemies[i].Active == false)
                    {
                        _enemies.RemoveAt(i);
                    }
                    else
                    {
                        if (_enemies[i].LastFired > _fireTime.Milliseconds)
                        {
                            _enemies[i].LastFired = 0;
                            double angle;
                            if (_enemies[i].Accurate)
                            {
                                angle = Math.Atan2(_player.Position.Y - _enemies[i].Position.Y,
                                                   _player.Position.X - _enemies[i].Position.X);
                            }
                            else
                            {
                                angle = _rand.NextDouble()*MathHelper.TwoPi;
                            }

                            Vector2 position = _enemies[i].Position +
                                               new Vector2((float) (32*Math.Cos(angle)), (float) (32*Math.Sin(angle)));

                            var projectile = new Projectile();
                            projectile.Initialize(GraphicsDevice.Viewport, _projectileTexture, position, angle,
                                                  ProjectileMoveSpeed, true);
                            _projectiles.Add(projectile);
                        }
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
                    if (projectile.Hostile)
                    {
                        if (projectile.GetCircle().Intersects(_player.GetCircle()))
                        {
                            if (Circle.Intersects(projectile.GetCircles(), _player.GetCircles()))
                            {
                                projectile.Active = false;
                                NewLife();
                                break;
                            }
                        }
                    }
                    else
                    {
                        var pointsEarned = 0;
                        foreach (Satellite enemy in _enemies)
                        {
                            if (projectile.GetCircle().Intersects(enemy.GetCircle()))
                            {
                                if (Circle.Intersects(projectile.GetCircles(), enemy.GetCircles()))
                                {
                                    if (enemy.Accurate)
                                    {
                                        pointsEarned += 1000;
                                    }
                                    else
                                    {
                                        pointsEarned += 200;
                                    }
                                    enemy.Active = false;
                                    projectile.Active = false;
                                    var explosion = new Explosion
                                        {
                                            Active = true,
                                            Position = enemy.Position,
                                            Scale = (float) enemy.Scale,
                                            Texture = _ufoTexture,
                                            Velocity = new Vector2(2, 0)
                                        };
                                    _explosionSound.Play();
                                    _explosions.Add(explosion);
                                }
                            }
                        }
                        if (pointsEarned != 0)
                        {
                            AddPoints(pointsEarned);
                        }
                    }


                    foreach (Asteroid asteroid in _asteroids)
                    {
                        if (asteroid.Active)
                        {
                            if (asteroid.GetCircle().Intersects(projectile.GetCircle()))
                            {
                                asteroid.Active = false;
                                projectile.Active = false;
                                if (projectile.Hostile == false)
                                {
                                    switch (asteroid.Generation)
                                    {
                                        case (1):
                                            AddPoints(20);
                                            break;
                                        case (2):
                                            AddPoints(50);
                                            break;
                                        case (3):
                                            AddPoints(100);
                                            break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            foreach (Asteroid asteroid in _asteroids)
            {
                if (asteroid.GetCircle().Intersects(_player.GetCircle())
                    && Circle.Intersects(asteroid.GetCircles(), _player.GetCircles()))
                {
                    NewLife();
                }
            }

            foreach (Satellite ufo in _enemies)
            {
                if (ufo.GetCircle().Intersects(_player.GetCircle()))
                {
                    if (Circle.Intersects(ufo.GetCircles(), _player.GetCircles()))
                    {
                        NewLife();
                        ufo.Active = false;
                        var explosion = new Explosion
                            {
                                Active = true,
                                Position = ufo.Position,
                                Scale = (float) ufo.Scale,
                                Texture = _ufoTexture,
                                Velocity = new Vector2(2, 0)
                            };
                        _explosionSound.Play();
                        _explosions.Add(explosion);
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

                    if (parent.Generation <= 2)
                    {
                        var asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians - _rand.Next(10, 30)*Math.PI/180, parent.Speed * 1.1f,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);

                        asteroid = new Asteroid();
                        asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, parent.Position,
                                            parent.Radians + _rand.Next(10, 30) * Math.PI / 180, parent.Speed * 1.1f,
                                            parent.Generation + 1);
                        _asteroids.Add(asteroid);
                    }
                    var explosion = new Explosion
                        {
                            Active = true,
                            Position =
                                parent.Position - new Vector2((float) (parent.Width/2.0), (float) (parent.Height/2.0)),
                            Scale = (float) parent.Scale,
                            Texture = parent.Texture,
                            Velocity = new Vector2()
                        };
                    _explosionSound.Play();
                    _explosions.Add(explosion);
                    _asteroids.RemoveAt(i);
                    if (_asteroids.Count == 0)
                    {
                        _wave += 1;
                        StartWave(_wave);
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

                foreach (Satellite satellite in _enemies)
                {
                    satellite.Draw(_spriteBatch);
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
            projectile.Initialize(GraphicsDevice.Viewport, _projectileTexture, pos, angle, ProjectileMoveSpeed, false);
            _projectiles.Add(projectile);
        }

        private void StartWave(int wave)
        {
            int asteroids = (wave + 4 < 12) ? wave + 4 : 12;

            for (int i = 0; i < asteroids; ++i)
            {
                double direction = _rand.NextDouble()*Math.PI*2;
                double x = GraphicsDevice.Viewport.Width/2.0 + GraphicsDevice.Viewport.Width*Math.Cos(direction);

                double y = GraphicsDevice.Viewport.Height/2.0 + GraphicsDevice.Viewport.Height*Math.Sin(direction);
                x = MathHelper.Clamp((float) x, 0, GraphicsDevice.Viewport.Width);
                y = MathHelper.Clamp((float) y, 0, GraphicsDevice.Viewport.Height);
                double init = _rand.NextDouble()*Math.PI*2;
                var asteroid = new Asteroid();
                float speed = 50.0f;
                if (wave > 12)
                {
                    speed += wave - 12*10;
                }
                asteroid.Initialize(GraphicsDevice.Viewport, _asteroidTexture, new Vector2((int) x, (int) y), init,
                                    (float) Math.Floor(speed + ((_rand.NextDouble() - .5))*25), 1);
                _asteroids.Add(asteroid);
            }
        }

        private void NewGame()
        {
            _explosions.Clear();
            _asteroids.Clear();
            _projectiles.Clear();

            _wave = 1;

            _player = new Player();
            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width/2
                                             ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height/2);
            _player.Initialize(GraphicsDevice.Viewport, _playerTexture, playerPosition, 3);

            StartWave(_wave);
            _score = new Score();
            _score.Initialize(_scoreFont, new Vector2(0, 0));
            _running = true;
            _menu.MainMenuIndex = _menu.Screens.IndexOf(_pause);
        }

        private void AddPoints(int points)
        {
            _score.AddPoints(points);
            if (_score.Points - _lastExtraLife > ExtraLife)
            {
                _player.Lives += 1;
                _lastExtraLife = _score.Points;
            }
            if (_score.Points - _lastUfo > Ufo)
            {
                _lastUfo = _score.Points;
                _ufoCount += 1;
                var ufo = new Satellite();
                if (_ufoCount%SmallUfo == 0)
                {
                    ufo.Initialize(_ufoTexture, new Vector2(-50, _rand.Next(50, GraphicsDevice.Viewport.Height - 50)),
                                   true, .5);
                }
                else
                {
                    ufo.Initialize(_ufoTexture, new Vector2(-50, _rand.Next(50, GraphicsDevice.Viewport.Height - 50)),
                                   false, 1.0);
                }
                _enemies.Add(ufo);
            }
        }

        private void NewLife()
        {
            var explosion = new Explosion
                {
                    Active = true,
                    Position =
                        _player.Position -
                        new Vector2((float) (_player.Width/2.0), (float) (_player.Height/2.0)),
                    Scale = 1.0f,
                    Texture = _player.Texture,
                    Velocity = _player.Velocity
                };

            _playerExplosion.Play();
            _explosions.Add(explosion);
            int lives = _player.Lives - 1;
            _player = new Player();
            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width/2
                                             ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height/2);
            _player.Initialize(GraphicsDevice.Viewport, _playerTexture, playerPosition, lives);
            if (_player.Lives < 0)
            {
                _running = false;
                _menu.MainMenuIndex = _menu.Screens.IndexOf(_gameOver);
                _menu.SelectedMenuScreen = _menu.MainMenuIndex;
            }
        }
    }
}