/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Layouts;
using ImageLoader;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.Core.Screens;
#endregion

namespace ImageLoader.Components
{
    public class ZoomPanel : Container<CoordLayout>
    {
        #region Variables
        private Label lblFullImage;
        private Button btnClose;
        private Screen screen;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">The Application instance</param>
        /// <param name="image">Image to zoom</param>
        public ZoomPanel(Screen screen)
            : base(new CoordLayout())
        {
            this.screen = screen;

            //Get the panel size
            BringToFront = false;
            Size = new Vector2(ViewportManager.ScreenWidth, ViewportManager.ScreenHeight);
            
            //Add a semitransparent background
            BackgroundImage = Image.CreateImage(new Color(0, 0, 0, 0.6f), ViewportManager.ScreenWidth, ViewportManager.ScreenHeight);

            //Add a close button.
            btnClose = new Button(StaticContent.Resources.CreateImage("Images\\bt_X"), StaticContent.Resources.CreateImage("Images\\bt_X_press"));
            btnClose.BringToFront = false;
            btnClose.Pivot = new Vector2(1, 0);
            btnClose.Released += delegate
            {
                //Close the panel if the cancel button is released
                Close();
            };
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Show the image
        /// </summary>
        /// <param name="image">Instance of the Image to show</param>
        public void Show(Image image)
        {
            //Check if the image is valid
            if (image == null)
                return;

            //Instantiate the label with the image
            lblFullImage = new Label(image) { Pivot = new Vector2(0.5f, 0.5f), Draggable = true, Scalable = true, Rotable = true, BringToFront = false };
            
            //Add components to the application layout
            screen.AddComponent(this, Preferences.ViewportManager.TopLeftAnchor);
            screen.AddComponent(lblFullImage, Preferences.ViewportManager.MiddleCenterAnchor);
            screen.AddComponent(btnClose, Preferences.ViewportManager.TopRightAnchor);            
        }

        /// <summary>
        /// Close the Zoom panel
        /// </summary>
        private void Close()
        {
            //Remove all components
            screen.RemoveComponent(btnClose);
            screen.RemoveComponent(lblFullImage);
            screen.RemoveComponent(this);
            
            lblFullImage = null;
        }
        #endregion
    }
}
