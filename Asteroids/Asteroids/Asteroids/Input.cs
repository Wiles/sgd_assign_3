//File:     Asteroids.cs
//Name:     Samuel Lewis (5821103)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Input wrapper to handle both keyboard and Gamepad
//          Only controls needed by the game are broken out

using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    /// <summary>
    /// 
    /// </summary>
    internal class Input
    {
        /// <summary>
        /// The game pad
        /// </summary>
        private GamePadState _gamePad;

        /// <summary>
        /// The keyboard
        /// </summary>
        private KeyboardState _keyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="gamepad">The gamepad.</param>
        public Input(KeyboardState keyboard, GamePadState gamepad)
        {
            _keyboard = keyboard;
            _gamePad = gamepad;
        }

        /// <summary>
        /// Is Up pressed
        /// </summary>
        /// <returns></returns>
        public bool Up()
        {
            return _keyboard.IsKeyDown(Keys.Up) || _gamePad.IsButtonDown(Buttons.DPadUp);
        }

        /// <summary>
        /// Is thrusters button pressed
        /// </summary>
        /// <returns></returns>
        public bool Thrusters()
        {
            return _keyboard.IsKeyDown(Keys.Up) || _gamePad.IsButtonDown(Buttons.A);
        }

        /// <summary>
        /// Is down pressed
        /// </summary>
        /// <returns></returns>
        public bool Down()
        {
            return _keyboard.IsKeyDown(Keys.Down) || _gamePad.IsButtonDown(Buttons.DPadDown);
        }

        /// <summary>
        /// Is left pressed
        /// </summary>
        /// <returns></returns>
        public bool Left()
        {
            return _keyboard.IsKeyDown(Keys.Left) || _gamePad.IsButtonDown(Buttons.DPadLeft);
        }

        /// <summary>
        /// Is right pressed
        /// </summary>
        /// <returns></returns>
        public bool Right()
        {
            return _keyboard.IsKeyDown(Keys.Right) || _gamePad.IsButtonDown(Buttons.DPadRight);
        }

        /// <summary>
        /// Is escape pressed
        /// </summary>
        /// <returns></returns>
        public bool Escape()
        {
            return _keyboard.IsKeyDown(Keys.Escape) || _gamePad.IsButtonDown(Buttons.Back);
        }

        /// <summary>
        /// If fire pressed
        /// </summary>
        /// <returns></returns>
        public bool Fire()
        {
            return _keyboard.IsKeyDown(Keys.Space) || _gamePad.IsButtonDown(Buttons.RightTrigger);
        }
    }
}