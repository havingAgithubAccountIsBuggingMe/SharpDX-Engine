﻿//! Make sure to add the following references to the Project first:
using NekuSoul.SharpDX_Engine;
using NekuSoul.SharpDX_Engine.Utitities;
using NekuSoul.SharpDX_Engine_Tutorial.Scenes;
using System;

namespace NekuSoul.SharpDX_Engine_Tutorial
{
    class Programm
    {
        //! This is static because it allows it to be accessed form anywhere
        public static Game Game;
        //! Defines the Size of the Window
        public static Size Size = new Size(800, 600);

        static void Main()
        {
            //! Turn on debug mode if compiled as debug
            bool debug = false;
            #if DEBUG
            debug = true;
            #endif

            if (debug)
            {
                //! Just run the game in debug mode
                StartGame();
            }
            else
            {
                //! If any errors occur, don't display the default Windows-Error-Message, but a custom MessageBox
                try
                {
                    StartGame();
                }
                catch (Exception e)
                {
                    //! Writes the default Errormessage in the MessageBox 
                    Programm.Game.ShowMessageBox("BOOM! ERROR!", e.Message);
                }
            }
        }

        static void StartGame()
        {
            //! This creates a new Game that can be accessed from everywhere.
            Game = new Game(Size);
            //! This locks the mouse position so that the mouse can't leave the window.
            Game.Input.Mouse.LockMouse = true;
            //! This starts the Game at the mainmenu and makes the window visible.
            Game.Run(new MainMenu());
        }
    }
}