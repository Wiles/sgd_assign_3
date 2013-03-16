using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Menu : IEntity
    {
        private Boolean firstRound = true;
        private Viewport _viewport;
        private SpriteFont _font;
        private Game1 _game;
        public int selectedMenuScreen { get; set; }
        private List<MenuScreen> screens = new List<MenuScreen>();
        public List<MenuScreen> Screens
        {
            get { return screens; }
        }

        private long updateDelay = 100;
        private long lastUpdate;

        public void Initialize(Game1 game, Viewport viewport, SpriteFont font)
        {
            _viewport = viewport;
            _font = font;
            _game = game;
        }

        public Circle GetCircle()
        {
            return new Circle(0,0,0);
        }

        public Circle[] GetCircles()
        {
            return new Circle[]{GetCircle()};
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var screen = screens[selectedMenuScreen];
                var textSize = _font.MeasureString("Asteroids");
                spriteBatch.DrawString(_font,
                    screen.Title,
                    new Vector2((_viewport.Width / 2) + 2, textSize.Y + 2),
                    Color.White, 0.0f,
                    new Vector2(textSize.X / 2, textSize.Y / 2),
                    1.0f, SpriteEffects.None, 0.0f);
            foreach(var i in Enumerable.Range(0, screen.elements.Count))
            {
                var ele = screen.elements.Keys.ToArray()[i];
                var text = (i == screen.selectedIndex) ? string.Format("> {0} <", ele) : ele;

                var eleSize = _font.MeasureString(text);
                spriteBatch.DrawString(_font,
                    text,
                    new Vector2((_viewport.Width / 2) + 2, textSize.Y * (i + 3)),
                    Color.White, 0.0f,
                    new Vector2(eleSize.X / 2, textSize.Y / 2),
                    1.0f, SpriteEffects.None, 0.0f);
            }
        }

        public void Update(GraphicsDevice graphics, KeyboardState input, long delta)
        {
            lastUpdate += delta;
            if (lastUpdate > updateDelay)
            {
                lastUpdate = 0;
                var screen = screens[selectedMenuScreen];
                if (input.IsKeyDown(Keys.Down))
                {
                    screen.selectedIndex += 1;
                    if (screen.selectedIndex >= screen.elements.Count)
                    {
                        screen.selectedIndex = 0;
                    }
                }
                if (input.IsKeyDown(Keys.Up))
                {
                    screen.selectedIndex -= 1;
                    if (screen.selectedIndex < 0)
                    {
                        screen.selectedIndex = screen.elements.Count - 1;
                    }
                }
                if(input.IsKeyDown(Keys.Enter))
                {
                    var action = screen.elements.Values.ToArray()[screen.selectedIndex];
                    action();
                }
            }
        }

        public void AddMenuScreen(MenuScreen screen)
        {
            screens.Add(screen);
        }
    }
}
