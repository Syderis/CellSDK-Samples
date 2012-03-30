/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syderis.CellSDK.Core.Screens;
using Syderis.CellSDK.Core.Graphics;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Controls;
using System.IO;
using System.Net;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Common;
using System.Xml.Linq; 
#endregion

namespace TwitterSearch
{
    public class MainScreen: AdjustedScreen
    {
        #region consts and statics
        private const string TWITTER_REQ_URL_TEMPLATE = "http://search.twitter.com/search.atom?q={0};";
        private const string SEARCH_BUTTON_TEXT = "Search";
        private const int BUTTON_WITH = 100;
        private const int BUTTON_HEIGHT = 48;
        private const int SPACING = 5;


        public static Font Font;        
        public static Vector2 offset;
        #endregion

        #region Variables
        private ListBox listBox;
        private TextArea searchTextArea;
        private Button searchButton;
        private WebClient client;
        private List<TwitterXMLItem> entries = null;
        #endregion

        public override void Initialize()
        {
            base.Initialize();

            SetBackground(ResourceManager.CreateImage("Resources/bg"), Adjustment.CENTER);
            StaticContent.DefaultFont.Sprite.DefaultCharacter = ' ';

            //Controls creation
            Font = ResourceManager.CreateFont("Resources/TweetFont");
            Font.Sprite.DefaultCharacter = ' ';

            //Search Text Area
            searchTextArea = new TextArea("", 1, 100);
            searchTextArea.Text = "cellsdk";
            searchTextArea.BackgroundImage = ResourceManager.CreateImage("Resources/top_search");
            searchTextArea.Padding = new Padding(25, 0, 0, 30);
            searchTextArea.Size = new Vector2(searchTextArea.BackgroundImage.Size.X, searchTextArea.BackgroundImage.Size.Y);
			AddComponent(searchTextArea,securityZone.X / 2 - searchTextArea.Size.X / 2,top);

            //Search Button
            searchButton = new Button(ResourceManager.CreateImage("Resources/bt_search"), ResourceManager.CreateImage("Resources/bt_search_press"));
            searchButton.Align = Label.AlignType.MIDDLECENTER;
            searchButton.Pressed -= HandleButtonPressed;
            searchButton.Pressed += new Component.ComponentEventHandler(HandleButtonPressed);
            AddComponent(this.searchButton, 414 + offset.X, top+13);

            //Listbox
            listBox = new ListBox(Preferences.Width, (Preferences.Height - BUTTON_HEIGHT - SPACING), ListBox.Orientation.VERTICAL);
            AddComponent(this.listBox, offset.X, top+searchTextArea.Position.Y + searchTextArea.Size.Y);

            // Set the response handler and send the request
            client = new WebClient();
            client.OpenReadCompleted -= HandleXmlResponse;
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(HandleXmlResponse);

      
        }

        /// <summary>
        /// Update method. Waits untill the list entries are ready.
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // There are tweet entries to add into the listbox
            if (this.entries != null)
            {
                //Clear the listbox
                listBox.RemoveAllItems();

                foreach (var entry in entries)
                {
                    //Create listbox objects and insert them into the listbox control
                    TwitterListItem item = new TwitterListItem(entry.User.Substring(0, entry.User.IndexOf('(')), entry.Text, entry.IconUrl);
                    listBox.AddItem(item);
                }

                this.entries = null;
            }
        }

        /// <summary>
        /// Method who sends a query request to Twitter and handles the response.
        /// </summary>
        /// <param name="hashtag">Twitter hashtag</param>
        private void SearchTag(string hashtag)
        {
            if (!client.IsBusy)
            {
                string uri = string.Format(TWITTER_REQ_URL_TEMPLATE, hashtag);

                // Make the search call           
                client.OpenReadAsync(new Uri(uri));
            }
        }


        /// <summary>
        /// Parses the XML response and add TwitterElement objects to listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void HandleXmlResponse(object sender, OpenReadCompletedEventArgs ev)
        {

            if (ev.Error != null)
            {
                return;
            }

            //Load XML document 
            XDocument xmlDoc = XDocument.Load(ev.Result);
            XNamespace xmlNamespace = "http://www.w3.org/2005/Atom";

            // Obtain entry list through a Linq query
            this.entries = (from entry in xmlDoc.Descendants(xmlNamespace + "entry")
                           select new TwitterXMLItem
                           {
                               User = entry.Element(xmlNamespace + "author").Element(xmlNamespace + "name").Value,
                               Text = entry.Element(xmlNamespace + "title").Value,
                               IconUrl = (from link in entry.Descendants(xmlNamespace + "link")
                                          where link.Attribute("type").Value.Contains("image/")
                                          select link.Attribute("href").Value).FirstOrDefault()
                           }).ToList();

            ////Clear the listbox
            //listBox.RemoveAllItems();

            //foreach (var entry in entries)
            //{
            //    //Create listbox objects and insert them into the listbox control
            //    TwitterListItem item = new TwitterListItem(entry.User.Substring(0, entry.User.IndexOf('(')), entry.Text, entry.IconUrl);
            //    listBox.AddItem(item);
            //}
        }


        /// <summary>
        /// Press Button Handler, start the query
        /// </summary>
        /// <param name="source"></param>
        private void HandleButtonPressed(Component source)
        {
            //Obtain text from textArea and start the query
            string query = this.searchTextArea.Text;
            if (query.Length > 0)
            {
                this.SearchTag(query);
            }
        }

        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        }

        /// <summary>
        /// Inner class that represents the twitter xml info we need to store and handle.
        /// </summary>
        private class TwitterXMLItem
        {
            public string User { get; set; }
            public string IconUrl { get; set; }
            public string Text { get; set; }
        }
    }
}
