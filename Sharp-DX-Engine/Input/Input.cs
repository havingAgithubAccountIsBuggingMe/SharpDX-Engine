﻿using SharpDX.DirectInput;
namespace SharpDX_Engine.Input
{
    public class InputManager
    {
        private DirectInput DirectInput = new DirectInput();
        public Keyboard Keyboard;
        public Mouse Mouse;
        public Gamepad Gamepad;

        public InputManager()
        {
            Keyboard = new Keyboard(DirectInput);
            Mouse = new Mouse(DirectInput);
            Gamepad = new Gamepad();
        }

        internal void Update()
        {
            Keyboard.UpdateKeyboardState();
            Mouse.UpdateMouseState();
            Gamepad.UpdateGamepadState();
        }
    }
}
