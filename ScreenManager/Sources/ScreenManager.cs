/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Controls;

#endregion

namespace ScreenManager
{

    /// <summary>
    /// Screen manager
    /// </summary>
    public class ScreenManager : IDisposable
    {
        /// <summary>
        /// Represents the diferent application screens.
        /// </summary>
        public enum Screens
        {
            INIT,
            LEVEL,
            OPTIONS,
        }

        #region Variables

        protected Dictionary<Screens, IScreen> screens; //Available screens
        protected Screens currentScreen; //Current sreen.
        private MobileApplication app; //Main Application reference.
        private Vector2 offset;

        #endregion

        #region Properties

        /// <summary>
        /// Main application reference.
        /// </summary>
        public MobileApplication App
        {
            get { return app; }
            set { app = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenManager(MobileApplication app)
        {
#if IPHONE
            offset= Vector2.Zero;
#else
            offset = new Vector2(81, 79);
#endif
            this.app = app;           
            screens = new Dictionary<Screens, IScreen>();
            currentScreen = Screens.INIT;
            LoadScreens();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Direct access to a screen
        /// </summary>
        /// <param name="i"></param>
        public void GoToScreen(Screens screen)
        {
            screens[currentScreen].ClearScreen();
            currentScreen = screen;
            screens[currentScreen].LoadScreen();
        }


        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            screens[currentScreen].Update(gameTime);
        }

        /// <summary>
        /// Release all resources.
        /// </summary>
        public void Dispose()
        {   
            foreach (IDisposable scr in screens.Values)
            {
                scr.Dispose();
            }
            screens = null;
        }
        
        /// <summary>
        /// Adds a component to the screen in a relative position 
        /// </summary>
        /// <param name="component">Component to add</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position</param>
        public void AddComponent(Component component, float x, float y)
        {
            app.AddComponent(component, x - offset.X, y - offset.Y);
        }

        /// <summary>
        /// Removes a component from the screen
        /// </summary>
        /// <param name="component"></param>
        public void RemoveComponent(Component component)
        {
            app.RemoveComponent(component);
        }

        

        /// <summary>
        /// Application Exit 
        /// </summary>
        public void BackButtonPressed()
        {
            if (currentScreen == Screens.INIT)
                Program.Instance.Exit();
            else
                GoToScreen(Screens.INIT);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Create the diferent screens.
        /// </summary>
        private void LoadScreens()
        {
            screens.Add(Screens.INIT, new InitScreen(this));
            screens.Add(Screens.OPTIONS, new OptionsScreen(this));
            screens.Add(Screens.LEVEL, new LevelScreen(this));

            screens[Screens.INIT].LoadScreen();
        }

        #endregion




        
    }
}
