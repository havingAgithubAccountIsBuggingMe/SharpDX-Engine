﻿using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX_Engine.Graphics;
using SharpDX_Engine.Input;
using SharpDX_Engine.Utitities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device1 = SharpDX.Direct3D10.Device1;
using DriverType = SharpDX.Direct3D10.DriverType;
using Factory = SharpDX.DXGI.Factory;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;

namespace SharpDX_Engine
{
    public class Game
    {
        static public Scene Scene;
        static public Sound.Sound Sound;
        static public InputManager Input;
        static public Renderer Renderer;
        static public TextureManager TextureManager;
        static public Coordinate WindowPosition;
        static public SharpDX_Engine.Utitities.Size Size;
        static internal RenderForm form;
        static private SwapChain swapChain;
        static private Device1 device;
        static private Stopwatch Stopwatch;
        static private Thread UpdateThread;

        /// <summary>
        /// A Game powered by SharpDX
        /// </summary>
        /// <param name="Width">The width of the window</param>
        /// <param name="Height">The height of the window</param>
        static public void Initialize(SharpDX_Engine.Utitities.Size Size)
        {
            GC.Collect();

            form = new RenderForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ClientSize = new System.Drawing.Size((int)Size.width, (int)Size.height);
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.FormClosed += form_FormClosed;

            // SwapChain description
            SwapChainDescription desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, FeatureLevel.Level_10_0, out device, out swapChain);

            var d2dFactory = new SharpDX.Direct2D1.Factory();

            // Ignore all windows events
            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            RenderTargetView renderView = new RenderTargetView(device, backBuffer);

            Surface surface = backBuffer.QueryInterface<Surface>();

            RenderTarget d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                                                            new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            form.SizeChanged += form_SizeChanged;
            form.GotFocus += form_GotFocus;
            form.LostFocus += form_LostFocus;
            form.Move += form_Move;
            WindowPosition = new Coordinate(
                form.Location.X + SystemInformation.FixedFrameBorderSize.Width + SystemInformation.DragSize.Width,
                form.Location.Y + SystemInformation.FixedFrameBorderSize.Height + SystemInformation.CaptionHeight + SystemInformation.DragSize.Height
                );
            TextureManager = new TextureManager(d2dRenderTarget);
            //x swapChain.IsFullScreen = true;
            Game.Size = Size;
            Renderer = new Renderer(d2dRenderTarget);
            Input = new InputManager();
            Sound = new Sound.Sound();

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            Scene = new DummyScene();

            UpdateThread = new Thread(UpdateScene);
        }

        static void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateThread.Abort();
            Application.Exit();
        }

        static void form_Move(object sender, EventArgs e)
        {
            Input.Mouse.Point = new Point(form.Location.X + (form.Size.Width / 2), form.Location.Y + (form.Size.Height / 2));
            WindowPosition = new Coordinate(form.Location.X + SystemInformation.BorderSize.Width, form.Location.Y + SystemInformation.BorderSize.Height);
        }

        static void form_LostFocus(object sender, EventArgs e)
        {
            Input.Mouse.FormHasFocus = false;
        }

        static void form_GotFocus(object sender, EventArgs e)
        {
            Input.Mouse.FormHasFocus = true;
        }

        static void form_SizeChanged(object sender, EventArgs e)
        {
            //ModeDescription MD = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
            //                            new Rational(60, 1), Format.R8G8B8A8_UNorm);
            //swapChain.ResizeTarget(ref MD);
            //swapChain.ResizeBuffers(1, form.Width, form.Height, Format.A8_UNorm, SwapChainFlags.AllowModeSwitch);
        }

        static void UpdateScene()
        {
            while (true)
            {
                if (Scene != null)
                {
                    if (Stopwatch.ElapsedMilliseconds > 1)
                    {
                        Input.Update();
                        Scene.Update();
                        Stopwatch.Restart();
                    }
                }
            }
        }

        static void DrawScene()
        {
            if (Scene != null)
            {
                Renderer.Draw(Scene);
            }
        }

        static public void Run()
        {
            GC.Collect();
            UpdateThread.Start();
            Input.Mouse.Point = new Point(form.Location.X + (form.Size.Width / 2), form.Location.Y + (form.Size.Height / 2));
            RenderLoop.Run(form, () =>
            {
                DrawScene();
                swapChain.Present(0, PresentFlags.None);
                swapChain.ContainingOutput.WaitForVerticalBlank();
            });

            #region Close
            //Release all resources
            //device.ClearState();
            //device.Flush();
            //device.Dispose();
            //swapChain.Dispose();
            #endregion
        }

        static public void SetScene(Scene _Scene)
        {
            Game.Scene = _Scene;
        }

        static public void Close()
        {
            form.Close();
        }

        static public void ShowMessageBox(string Caption, string Text)
        {
            MessageBox.Show(Text, Caption);
        }

        static public void SetTitle(string Name)
        {
            form.Text = Name;
        }
    }
}
