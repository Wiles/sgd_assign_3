using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Input
    {
        private KeyboardState _keyboard;
        private GamePadState _gamePad;

        public Input(KeyboardState keyboard, GamePadState gamepad)
        {
            _keyboard = keyboard;
            _gamePad = gamepad;
        }

        public bool Up()
        {
            return _keyboard.IsKeyDown(Keys.Up) || _gamePad.IsButtonDown(Buttons.DPadUp);
        }

        public bool Thrusters()
        {
            return _keyboard.IsKeyDown(Keys.Up) || _gamePad.IsButtonDown(Buttons.A);
        }

        public bool Down()
        {
            return _keyboard.IsKeyDown(Keys.Down) || _gamePad.IsButtonDown(Buttons.DPadDown);
        }

        public bool Left()
        {
            return _keyboard.IsKeyDown(Keys.Left) || _gamePad.IsButtonDown(Buttons.DPadLeft);
        }

        public bool Right()
        {
            return _keyboard.IsKeyDown(Keys.Right) || _gamePad.IsButtonDown(Buttons.DPadRight);
        }

        public bool Escape()
        {
            return _keyboard.IsKeyDown(Keys.Escape) || _gamePad.IsButtonDown(Buttons.Back);
        }

        public bool Fire()
        {
            return _keyboard.IsKeyDown(Keys.Space) || _gamePad.IsButtonDown(Buttons.RightTrigger);
        }
    }
}
