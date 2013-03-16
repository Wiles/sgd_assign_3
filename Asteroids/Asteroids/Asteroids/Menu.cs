using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private SoundEffect _menuMove;
        private SoundEffect _menuSelect;

        public int selectedMenuScreen { get; set; }

        public int MainMenuIndex { get; set; }

        private List<MenuScreen> screens = new List<MenuScreen>();
        public List<MenuScreen> Screens
        {
            get { return screens; }
        }

        private bool down;
        private bool up;
        private bool enter;
        private bool escape;

        public void Initialize(Game1 game, Viewport viewport, SpriteFont font, SoundEffect move, SoundEffect select)
        {
            _viewport = viewport;
            _font = font;
            _game = game;
            _menuMove = move;
            _menuSelect = select;
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

        public void Update(GraphicsDevice graphics, Input input, long delta)
        {
            var screen = screens[selectedMenuScreen];
            if (input.Down())
            {
                if(down == false)
                {
                    down = true;
                    _menuMove.Play();
                    screen.selectedIndex += 1;
                    if (screen.selectedIndex >= screen.elements.Count)
                    {
                        screen.selectedIndex = 0;
                    }
                }
            }
            else
            {
                down = false;
            }
            if (input.Up())
            {
                if (up == false)
                {
                    up = true;
                    _menuMove.Play();
                    screen.selectedIndex -= 1;
                    if (screen.selectedIndex < 0)
                    {
                        screen.selectedIndex = screen.elements.Count - 1;
                    }
                }
            }
            else
            {
                up = false;
            }
            if(input.Select())
            {
                if (enter == false)
                {
                    enter = true;
                    var action = screen.elements.Values.ToArray()[screen.selectedIndex];
                    if (action != null)
                    {
                        _menuSelect.Play();
                        action();
                    }
                }
            }
            else
            {
                enter = false;
            }

            if(input.Escape())
            {
                selectedMenuScreen = MainMenuIndex;
            }
        }

        public void AddMenuScreen(MenuScreen screen)
        {
            screens.Add(screen);
        }
    }
}
