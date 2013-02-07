using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Shooter;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Player player;

        private KeyboardState currentKeyboardState;

        private TimeSpan fireTime;
        private TimeSpan previousFireTime;
        private float playerMoveSpeed = 2.0f;
        private float projectileMoveSpeed = 4.0f;

        private Texture2D projectileTexture;
        private Texture2D playerTexture;
        private List<Projectile> projectiles = new List<Projectile>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            player = new Player();
            fireTime = TimeSpan.FromSeconds(.25f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            projectileTexture = Content.Load<Texture2D>("Projectile");

            playerTexture = Content.Load<Texture2D>("Player");


            var playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X
                                             + GraphicsDevice.Viewport.TitleSafeArea.Width / 2
                ,
                                             GraphicsDevice.Viewport.TitleSafeArea.Y
                                             + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerTexture, playerPosition);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            currentKeyboardState = Keyboard.GetState();
            UpdatePlayer(gameTime);
            UpdateProjectiles();
            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update();

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.Angle -= 0.1f;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.Angle += 0.1f;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Y += playerMoveSpeed * Math.Sin(player.Angle);
                player.X += playerMoveSpeed * Math.Cos(player.Angle);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                // Fire only every interval we set as the fireTime
                if (gameTime.TotalGameTime - previousFireTime > fireTime)
                {
                    // Reset our current time
                    previousFireTime = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile(player.Position + new Vector2((int)(player.Width / 2.0), (int)(player.Height / 2.0)));
                }
            }
            
            // Make sure that the player does not go out of bounds
            if (player.X > GraphicsDevice.Viewport.Width + player.Width)
            {
                player.X = -player.Width + 1;
            } 
            else if (player.X < -player.Width)
            {
                player.X = GraphicsDevice.Viewport.Width + player.Width;
            }
            if (player.Y > GraphicsDevice.Viewport.Height + player.Height)
            {
                player.Y = -player.Height + 1;
            }
            else if (player.Y < -player.Height)
            {
                player.Y = GraphicsDevice.Viewport.Height + player.Height;
            }
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
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

            spriteBatch.Begin();
            player.Draw(spriteBatch);

            foreach (var t in projectiles)
            {
                t.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void AddProjectile(Vector2 position)
        {
            var projectile = new Projectile();
            var pos = new Vector2();
            var angle = player.Angle;
            pos.X = player.Position.X + (int)(playerTexture.Width / 2.0 * Math.Cos(angle));
            pos.Y = player.Position.Y + (int)(playerTexture.Height / 2.0 * Math.Sin(angle));
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, pos, angle, projectileMoveSpeed);
            projectiles.Add(projectile);
        }
    }
}
