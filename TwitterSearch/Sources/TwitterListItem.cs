/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */
#region using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syderis.CellSDK.Core.Interfaces;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Controls;
using System.Net;
using Microsoft.Xna.Framework.Graphics;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Layouts;
using Microsoft.Xna.Framework;
using System.IO;
#endregion

namespace TwitterSearch
{
    /// <summary>
    /// Listbox element that contains a single tweet info
    /// </summary>
    class TwitterListItem : Container<CoordLayout>,IListBoxObject
    {

        #region Constants and Statics

        private const int ICON_SIZE = 48;
        private const int SPACING = 5;
        private const int ITEM_HEIGHT = 250;
        private const int ITEM_WIDTH = 480;
        private const int LINE_CHARS = 30;

        #endregion

        #region Variables

        private Label backIconLabel;
        private Label iconLabel;        
        private Stream imageStream;
        private WebClient client;
        private string User;
        private string Text;
        private string IconUrl;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a twitter list item
        /// </summary>
        /// <param name="user">tweet user</param>
        /// <param name="text">tweet text</param>
        /// <param name="iconUrl">tweet user icon</param>
        public TwitterListItem(string user, string text, string iconUrl):base(new CoordLayout())
        {
            User = user;
            Text = text;
            IconUrl = iconUrl;

            //CoordLayout creation  
            Size = new Vector2(ITEM_HEIGHT);

            client = new WebClient();
            client.OpenReadCompleted -= ImageReadCompleted;
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(ImageReadCompleted);
            client.OpenReadAsync(new Uri(IconUrl));

        }

        #endregion

        #region Events
        #endregion

        #region Public Methods

        /// <summary>
        /// Return the cell of the ListBox component.
        /// </summary>
        /// <returns>The coordContainerLayout objet of the tweet list item</returns>
        public Component CellRenderer()
        {
            return this;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Spawn the item components when the icon is ready to Load
            if (this.IsReadyToLoad())
            {
                this.Spawn();
            }
            
        }
       
        #endregion

        #region Private Methods

        /// <summary>
        /// Icon request complete handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void ImageReadCompleted(object sender, OpenReadCompletedEventArgs ev)
        {
            if (ev.Error != null)
            {
                return;
            }

            this.imageStream = ev.Result;
            
        }

        /// <summary>
		/// Spawn the item components, in a thredsafe environment
		/// </summary>
		private void Spawn()
		{
            //Creation of the label containing the tweet message
            string formattedText = this.FormatText(Text, LINE_CHARS);
            Label contentLabel = new Label(formattedText);
            contentLabel.Font = MainScreen.Font;
            contentLabel.TextColor = new Color(160, 160, 160);
            contentLabel.Padding = new Padding(15, 15, 15, 40);
            contentLabel.Font.LineSpacing = 25;
            contentLabel.Align = Label.AlignType.TOPLEFT;
            contentLabel.Image = StaticContent.Resources.CreateImage("Resources/bg_tweet");
            contentLabel.Size = contentLabel.Image.Size;
            Layout.AddComponent(contentLabel, 128, 0);

            //Creation of the background icon Label;
            this.backIconLabel = new Label(StaticContent.Resources.CreateImage("Resources/bg_photo"));
            Layout.AddComponent(this.backIconLabel, 0, 0);

            //Creation of the icon label (with a default pic)
            Texture2D imageTexture;
            imageTexture = Texture2D.FromStream(StaticContent.SpriteBatch.GraphicsDevice, imageStream);
            Image image = Image.CreateImage(imageTexture);
            this.iconLabel = new Label(image);
            this.iconLabel.Size = new Vector2(114, 113);
            Layout.AddComponent(this.iconLabel, 15, 15);

            //Creation of the label of the twitter user name
            Label userLabel = new Label(User);
            userLabel.Image = Image.CreateImage(Color.Transparent, 1, 1);
            userLabel.Align = Label.AlignType.MIDDLECENTER;
            //userLabel.Pivot = Vector2.One;
            userLabel.Font = MainScreen.Font;
            userLabel.TextColor = new Color(113, 192, 239);
            Layout.AddComponent(userLabel, ITEM_WIDTH - userLabel.Size.X - 10, 195);

            imageStream = null;
		}

        /// <summary>
        /// Determines whether this item is ready to load its Icon Image.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is ready to load; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReadyToLoad()
        {
            return (this.imageStream != null);
        }

        /// <summary>
        /// Format the text, splitting it in multipe lines, adding '\n' character  
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="lineLength">length of the line</param>
        /// <returns>formatted string</returns>
        private string FormatText(string text, int lineLength)
        {
            string result = text;
            int initIndex = 0;
            int lastIndex = lineLength - 1;

            //Iterate until last line
            while (lastIndex < result.Length)
            {
                bool found = false;
                //Iterate backward from the last enter until finding a space character.
                for (int i = lastIndex; i >= initIndex; i--)
                {
                    char resultChar = result[i];
                    if (resultChar == ' ')
                    {
                        initIndex = i;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    initIndex++;
                }
                else
                {
                    //Update init and end line indexes
                    result = result.Insert(initIndex + 1, "\n");
                    initIndex += 2;
                }

                lastIndex = initIndex + lineLength - 1;
                
            }
            return result;
        }

        #endregion


    }
}
