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
using Syderis.CellSDK.Core.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using Microsoft.Xna.Framework.Graphics;
using Syderis.CellSDK.Core;
using System.IO;
using Syderis.CellSDK.Core.Layouts; 
#endregion

namespace ImageLoader.Components
{
    public class ImageItem : Container<CoordLayout>
    {
        #region Variables
		private System.IO.Stream textureStream;
		private Texture2D texture;
        private Image fullImage;
        private Image thumbnail;
		private Vector2 thumbnailSize;
        private bool loaded;
        private Label lblImage;
        #endregion

        #region statics
        private static Vector2 imageSize = new Vector2(200, 180);       
        private static Image BgImage = StaticContent.Resources.CreateImage("Images/bg_img");
        
        #endregion
        #region Properties
        public Image FullImage
        {
            get { return fullImage; }
        }

        public bool Loaded
        {
            get { return loaded; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Image Item constructor
        /// </summary>
        /// <param name="url">The image url</param>
        public ImageItem(String url):base(new CoordLayout())
        {
            loaded = false;
            BringToFront = false;
            BackgroundImage = ImageItem.BgImage;
            Size = BackgroundImage.Size;
            //Load the image from an URL
            LoadImageFromUrl(url);
        }
        #endregion

        #region Public Methods
		/// <summary>
		/// Update this instance.
		/// </summary>
        public override void Update(GameTime gameTime)
        {
			//If the thumbnail is loaded
			if(textureStream != null && !loaded)
			{
                //Load a texture from the HTTP result stream
				try
                {	
	            	texture = Texture2D.FromStream(StaticContent.Graphics.GraphicsDevice, textureStream);	
				}
				catch(Exception e)
				{
					Console.WriteLine(e);
					texture = null;
				
					//Dispose  the texture stream
					textureStream.Dispose();
					textureStream = null;
					
					return;
				}
				
				//Dispose  the texture stream
				textureStream.Dispose();
				textureStream = null;
				
				//Create an Image instance 
            	fullImage = Image.CreateImage(texture);
	
	            //Obtain the image thumbnail size
				if (fullImage.Width > fullImage.Height)
	                thumbnailSize = new Vector2(fullImage.Height * (Size.X / Size.Y), fullImage.Height);
	            else
	                thumbnailSize = new Vector2(fullImage.Width, fullImage.Width * (Size.X / Size.Y));
					
				//Obtain the thumbnail to the Image Item
            	thumbnail = fullImage.SubImage((int)(fullImage.Width / 2 - thumbnailSize.X / 2), (int)(fullImage.Height / 2 - thumbnailSize.Y / 2), (int)thumbnailSize.X, (int)thumbnailSize.Y);
                
				//Set the thumbnail to label image
				lblImage = new Label(fullImage);
                lblImage.Size = ImageItem.imageSize;
                this.Layout.AddComponent(lblImage, 7, 3);
				//Change the load status
                loaded = true;				
			}

            base.Update(gameTime);
		}
        #endregion

        #region Private Methods
        /// <summary>
        /// Load the image from an URL
        /// </summary>
        /// <param name="url">image url</param>
        private void LoadImageFromUrl(String url)
        {
            if (url == null)
            {
                return;
            }

            //Http request to obtain the image
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(WebClient_openReadCompleted);
            webClient.OpenReadAsync(new Uri(url));         
        }

        /// <summary>
        /// Handle the HTTP response
        /// </summary>
        private void WebClient_openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //Cache the Image HTTP result stream
                textureStream = CacheStream(e.Result);
            }
        }
             

        /// <summary>
        /// Clome a stream using a MemoryStream
        /// </summary>
        /// <returns>
        /// The cloned stream.
        /// </returns>
        /// <param name='stream'>
        /// The original stream
        /// </param>
        private Stream CacheStream(Stream stream)
        {
            Stream cachedStream = null;
            try
            {
                //Create a new memory stream
                cachedStream = new MemoryStream();
                //Temporal buffer
                const int bufferSize = 1500;
                byte[] buffer = new byte[bufferSize];

                int read = 0;

                //Read from the original stream and copy to memory stream
                while ((read = stream.Read(buffer, 0, bufferSize)) != 0)
                {
                    cachedStream.Write(buffer, 0, read);
                }

                cachedStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return cachedStream;
        } 

        #endregion
    }
}
