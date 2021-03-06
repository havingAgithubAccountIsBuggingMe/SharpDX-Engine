﻿using SharpDX_Engine.Utitities;
using SharpDX.DirectInput;
using System.Drawing;
using System.Windows.Forms;

namespace SharpDX_Engine.Input
{
    public class Mouse
    {
        public bool FormHasFocus = true;
        public bool LockMouse = false;

        internal Point Point = new Point();
        private SharpDX.DirectInput.Mouse _Mouse;
        private MouseState CurrentState;
        private MouseState LastState;

        public Mouse(DirectInput DirectInput)
        {
            _Mouse = new SharpDX.DirectInput.Mouse(DirectInput);
            _Mouse.Acquire();
            UpdateMouseState();
            UpdateMouseState();
        }

        public void ShowCursor()
        {
            Cursor.Show();
        }

        public void HideCursor()
        {
            Cursor.Hide();
        }

        public void UpdateMouseState()
        {
            LastState = CurrentState;
            CurrentState = _Mouse.GetCurrentState();
            if (LockMouse && FormHasFocus)
            {
                Cursor.Position = Point;
            }
        }

        public void SetMousePosition(Coordinate Position)
        {
            if (FormHasFocus)
            {
                Cursor.Position = new System.Drawing.Point((int)Position.X, (int)Position.Y);
            }
        }

        /// <summary>
        /// Returns current Mouse-Position
        /// </summary>
        /// <returns></returns>
        public Coordinate GetCurrentMousePosition()
        {
            return new Coordinate()
            {
                X = CurrentState.X,
                Y = CurrentState.Y
            };
        }

        public Coordinate GetLastMousePosition()
        {
            return new Coordinate()
            {
                X = LastState.X,
                Y = LastState.Y
            };
        }

        public bool CheckButtonDown(Button Button)
        {
            return CurrentState.Buttons[(int)Button];
        }

        public bool CheckButtonClickDown(Button Button)
        {
            if (!LastState.Buttons[(int)Button])
            {
                return CurrentState.Buttons[(int)Button];
            }
            return false;
        }

        public bool CheckButtonClickUp(Button Button)
        {
            if (LastState.Buttons[(int)Button])
            {
                return !CurrentState.Buttons[(int)Button];
            }
            return false;
        }

        public enum Button
        {
            LeftMouse = 0,
            RightMouse
        }
    }
}
