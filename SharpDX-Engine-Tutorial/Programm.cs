﻿//! Make sure to add the following references to the Project first:
using NekuSoul.SharpDX_Engine;
using NekuSoul.SharpDX_Engine.Utitities;
using NekuSoul.SharpDX_Engine_Tutorial.Scenes;
using System;

namespace NekuSoul.SharpDX_Engine_Tutorial
{
    class Programm
    {
        public static Game Game;
        public static Size Size = new Size(800, 600);

        static void Main()
        {
            //! If any errors occur, don't display the default Windows-Error-Message, but a custom MessageBox
            try
            {
                //! This creates a new Game that can be accessed from everywhere.
                Game = new Game(Size);
                //! This locks the mouse position so that the mouse can't leave the window.
                Game.Input.Mouse.LockMouse = true;
                //! This starts the Game at the mainmenu and makes the window visible.
                Game.Run(new MainMenu());
            }
            catch (Exception e)
            {
                //! Writes the default Errormessage in the MessageBox 
                System.Windows.Forms.MessageBox.Show(e.Message, "BOOM! ERROR!");
            }
        }
    }
}