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
using Syderis.CellSDK.Core.Controls;
using ImageLoader.Components;
using Syderis.CellSDK.Core.Layouts;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Common;
using System.Runtime.Serialization.Json;
using ImageLoader.Beans;
using System.Net; 
#endregion

namespace ImageLoader
{
    public class MainScreen: AdjustedScreen
    {

        #region Consts and Statics
        private const string SEARCH_TEXTAREA_TEXT = "CellSDK";
        private const string SEARCH_BUTTON_TEXT = "Search";
        private const int BUTTON_WITH = 100;
        private const int BUTTON_HEIGHT = 48;
        private const int SPACING = 5;
        private const int GRID_ROWS = 3;
        private const int GRID_COLUMNS = 2;
        private const int GRID_SIZE = GRID_ROWS * GRID_COLUMNS;
        #endregion

        #region Variables
        private TextArea searchTextArea;
        private Button searchButton;
        private TabPanel contentTabPanel;
        private Container<GridLayout> currentGridContainer;
        private ZoomPanel zoomPanel;
        private int currentGridElementCount = 0;
        private List<Container<GridLayout>> gridContainerList;
        private int currentGridElementIndex = 0;
        private List<ImageItem> imageList;
        private List<String> pendingUrlImages;
        private Vector2 offset;
        #endregion

        public override void Initialize()
        {
            base.Initialize();

            SetBackground(ResourceManager.CreateImage("Images/background"), Adjustment.CENTER);

            //Init all lists
            gridContainerList = new List<Container<GridLayout>>();
            imageList = new List<ImageItem>();
            pendingUrlImages = new List<string>();

            //Search Text Area
            searchTextArea = new TextArea(SEARCH_TEXTAREA_TEXT, 1, 100);
            searchTextArea.BackgroundImage = ResourceManager.CreateImage("Images/top_search");
            this.searchTextArea.Padding = new Padding(25, 0, 0, 30);
            this.searchTextArea.Size = new Vector2(searchTextArea.BackgroundImage.Size.X, searchTextArea.BackgroundImage.Size.Y);
            AddComponent(searchTextArea, securityZone.X / 2 - searchTextArea.Size.X / 2, top);

            //Search Button
            searchButton = new Button(ResourceManager.CreateImage("Images/bt_search"), ResourceManager.CreateImage("Images/bt_search_pressed"));
            searchButton.Released += delegate
            {
                //Clear previous images
                ClearContent();
                //Search new images using TextArea text
                SearchImages(searchTextArea.Text);
            };
            AddComponent(searchButton, 393 + offset.X, top+12);

            //Content Tab Panel
            contentTabPanel = new TabPanel(458, 632);
            //AddComponent(contentTabPanel, 0, BUTTON_HEIGHT + SPACING);
            AddComponent(contentTabPanel, 11 + offset.X, top+90);
            //ZoomPanel
            zoomPanel = new ZoomPanel(this);

        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Lock the pending Url list to avoid concurrent problems
            lock (pendingUrlImages)
            {
                //Check for images to add
                if (pendingUrlImages.Count > 0)
                {
                    foreach (String imageUrl in pendingUrlImages)
                    {
                        AddImage(imageUrl);
                    }
                    pendingUrlImages.Clear();
                }
            }

            base.Update(gameTime);
        }

        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        }

        #region Private methods
        /// <summary>
        /// Search images using a query
        /// </summary>
        /// <param name="query">query used to search images</param>
        private void SearchImages(String query)
        {
            //Search images on Google Images
            //Load 1st page [0 - 7]
            SearchImagesFromGoogle(query, 8, 0);
            //Load 2nd page [8 - 15]
            SearchImagesFromGoogle(query, 8, 8);
            //Load 3rd page [16 - 23]
            SearchImagesFromGoogle(query, 8, 16);
        }

        /// <summary>
        /// Search images on Google Images using JSON
        /// </summary>
        /// <param name="query">query used to search images</param>
        /// <param name="nImagesPerPage">The number of result images per page</param>
        /// <param name="startImage">The start index of the first search result</param>
        private void SearchImagesFromGoogle(string query, int nImagesPerPage, int startImage)
        {
            //Create the web client and make the HTTP request to Google Images
            WebClient webClientGoogle = new WebClient();
            webClientGoogle.OpenReadCompleted += new OpenReadCompletedEventHandler(WebClientGoogle_openReadCompleted);
            webClientGoogle.OpenReadAsync(new Uri("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=" + nImagesPerPage + "&start=" + startImage + "&q=" + query));
        }

        /// <summary>
        /// When the HTTP request is retourned it shows the images to the grid
        /// </summary>
        private void WebClientGoogle_openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
                //HTTP Error!!
                Console.WriteLine("Error");
            else
            {
                try
                {

                    //JSON Serializer using Google Result data contract
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(GoogleResultBean));
                    //Pass the HTTP result to the JSON serializer to obtain the Google Result
                    GoogleResultBean googleResult = jsonSerializer.ReadObject(e.Result) as GoogleResultBean;
                    //Check if the Google Result is valid
                    if (googleResult == null || googleResult.responseData == null || googleResult.responseData.results == null)
                        return;

                    //Foreach retourned image..
                    foreach (ResultType result in googleResult.responseData.results)
                    {
                        //Get the image
                        String imageUrl = result.url;
                        //Lock the pending Url list to avoid concurrent problems
                        lock (pendingUrlImages)
                        {
                            //Add to the user interface
                            pendingUrlImages.Add(imageUrl);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Add a new image to the user interface
        /// </summary>
        /// <param name="url">url of the image</param>
        private void AddImage(String url)
        {
            //Check if the URL is valid
            if (url == null || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return;

            //Each tab has a grid layout container (currentGridContainer). 
            //Instantiate a new grid container if the previous one is full
            if (currentGridContainer == null || (currentGridElementCount == GRID_SIZE && currentGridElementIndex == gridContainerList.Count))
            {
                //Reset the grid element counter
                currentGridElementCount = 0;

                //New Grid Layout container
                currentGridContainer = new Container<GridLayout>(new GridLayout(GRID_ROWS, GRID_COLUMNS));
                currentGridContainer.BackgroundColor = Color.Transparent;
                currentGridContainer.BringToFront = false;
                currentGridContainer.Size = contentTabPanel.Size;

                //Add the new grid to the content tab panel
                contentTabPanel.AddTab("", currentGridContainer);
                gridContainerList.Add(currentGridContainer);
                currentGridElementIndex = gridContainerList.Count;
            }
            else if (currentGridElementCount == GRID_SIZE && currentGridElementIndex < gridContainerList.Count)
            {
                //Reset the grid element counter
                currentGridElementCount = 0;
                //Get a previous instantiated grid container
                currentGridContainer = gridContainerList.ElementAt(currentGridElementIndex);
                currentGridElementIndex++;
                currentGridContainer.Layout.RemoveAllComponents();
            }


            //New ImageItem
            ImageItem imageItem = new ImageItem(url);

            //If the user press the image item, zoom the image
            imageItem.Released += delegate
            {
                //If image is loadaed
                if (imageItem.Loaded)
                    //Show the Zoom Panel with the selected image
                    zoomPanel.Show(imageItem.FullImage);
            };

            //Add the ImageItem to the list
            imageList.Add(imageItem);

            //Add the ImageItem to the Grid Layout container and increment the counter
            currentGridContainer.Layout.AddComponent(imageItem);
            currentGridElementCount++;

        }

        /// <summary>
        /// Clear all image content
        /// </summary>
        private void ClearContent()
        {
            //Remove the content of all grids
            foreach (var gridContainer in gridContainerList)
            {
                gridContainer.Layout.RemoveAllComponents();
            }
            currentGridElementCount = 0;
            currentGridElementIndex = 0;
            if (gridContainerList.Count > 0)
                currentGridContainer = gridContainerList.ElementAt(0);
            else
                currentGridContainer = null;

            //Clear the image list
            imageList.Clear();
        }
        #endregion
    }
}
