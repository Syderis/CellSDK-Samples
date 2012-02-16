/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
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
#endregion

namespace ImageLoader.Components
{
    public class ZoomPanel : Container<CoordLayout>
    {
        #region Variables
        private Label lblFullImage;
        private Button btnClose;
        private Application app;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">The Application instance</param>
        /// <param name="image">Image to zoom</param>
        public ZoomPanel(Application app)
            : base(new CoordLayout())
        {
            //Set the application instance
            this.app = app;

            //Get the panel size
            BringToFront = false;
            Size = new Vector2(app.Width, app.Height);
            
            //Add a semitransparent background
            BackgroundImage = Image.CreateImage(new Color(0, 0, 0, 0.6f),app.Width,app.Height);

            //Add a close button.
            btnClose = new Button(Image.CreateImage("Images\\bt_X"), Image.CreateImage("Images\\bt_X_press"));
            btnClose.BringToFront = false;
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
            lblFullImage = new Label(image) { Draggable = true, Scalable = true, Rotable = true, BringToFront = false };
            
            //Add components to the application layout
            app.AddComponent(this, 0, 0);
            app.AddComponent(lblFullImage, Size.X / 2 - image.Width / 2, Size.Y / 2 - image.Height / 2);
            app.AddComponent(btnClose, app.Width - 16 - btnClose.Size.X, 16);            
        }

        /// <summary>
        /// Close the Zoom panel
        /// </summary>
        private void Close()
        {
            //Remove all components
            app.RemoveComponent(btnClose);
            app.RemoveComponent(lblFullImage);
            app.RemoveComponent(this);
            
            lblFullImage = null;
        }
        #endregion
    }
}
