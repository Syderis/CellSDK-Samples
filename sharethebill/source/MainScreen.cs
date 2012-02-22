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
using System.Xml.Serialization;
using Syderis.CellSDK.Core.Storage;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.Core.Animations;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.Core.Sounds;
using System.Globalization; 
#endregion

namespace ShareBill
{
    public class MainScreen: Screen
    {
        #region Constants and Statics
        // Consts
        private const int MIN_GAP_INCREMENT = 50;
        private const int DEFAULT_GAP = 500;
        private const int PEN_OFFSET_X = 109;
        private const int PEN_OFFSET_Y = 30;
        #endregion

        #region Variables
        // Vars
        private bool moneySelected = true;
        private bool eurosSelected = true;
        private bool morePressed;
        private bool lessPressed;

        private int interval;
        private float total = 0f;
        private int heads = 2;
        private int gap = 500;
        private Padding padding;
        private Vector2[] penRelativePositions;

        // Controls
        private Container<CoordLayout> mainContainer;
        private Label lEuros;
        private Label lCents;
        private Label lPeople;
        private Label lTotal;
        private Label lPen;
        private Label lEurosMask;
        private Label lCentsMask;
        private Label lPeopleMask;
        private Button bPlus;
        private Button bMinus;
        private Image iApplicationBackground;
        private Image iMainContainerBackground;
        private Image iPen;
        private Image iButtonPlusReleased;
        private Image iButtonPlusPressed;
        private Image iButtonMinusReleased;
        private Image iButtonMinusPressed;
        private SoundInstance writingPlayer;
        private Font fSmall;
        private Font fLarge;
        #endregion

        #region Events
        /// <summary>
        /// Plus Button Pressed
        /// </summary>
        /// <param name="source"></param>
        private void HandleBplusPressed(Component source)
        {
            this.writingPlayer.Play();
            this.morePressed = true;
            this.interval = 0;
            if (this.moneySelected)
            {
                if (this.eurosSelected)
                {
                    if (this.total <= 998.99f)
                    {
                        this.total += 1f;
                    }
                }
                else
                {
                    if (this.total <= 999.98f)
                    {
                        this.total += 0.01f;
                    }
                }
            }
            else
            {
                if (this.heads < 99)
                {
                    this.heads++;
                }
            }
        }

        /// <summary>
        /// Plus Button Released
        /// </summary>
        /// <param name="source"></param>
        private void HandleBplusReleased(Component source)
        {
            this.writingPlayer.Stop();
            this.morePressed = false;
            this.interval = 0;
            this.gap = 500;
			
			//Use a culture info that uses comma to separate decimal digits (es for instance)
            this.lTotal.Text = string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:f2}", this.total / (float)this.heads);
        }

        /// <summary>
        /// Minus Button Pressed
        /// </summary>
        /// <param name="source"></param>
        private void HandleBminusPressed(Component source)
        {
            this.writingPlayer.Play();
            this.lessPressed = true;
            this.interval = 0;
            if (this.moneySelected)
            {
                if (this.eurosSelected)
                {
                    if (this.total >= 1f)
                    {
                        this.total -= 1f;
                    }
                }
                else
                {
                    if (this.total >= 0.01f)
                    {
                        this.total -= 0.01f;
                    }
                }
            }
            else
            {
                if (this.heads > 2)
                {
                    this.heads--;
                }
            }
        }

        /// <summary>
        /// Minus Button Released
        /// </summary>
        /// <param name="source"></param>
        private void HandleBminusReleased(Component source)
        {
            this.writingPlayer.Stop();
            this.lessPressed = false;
            this.interval = 0;
            this.gap = 500;
            this.lTotal.Text = string.Format("{0:f2}", this.total / (float)this.heads);
        }
	    #endregion

        #region Public Methods
        /// <summary>
        /// Sets the screen up (UI components, multimedia content, etc.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Load Contents
            this.fSmall = ResourceManager.CreateFont("Fonts/SmallFontMonospaced");
            this.fSmall.Sprite.Spacing = -15f;
            this.fLarge = ResourceManager.CreateFont("Fonts/LargeFontMonospaced");
            this.fLarge.Sprite.Spacing = -15f;
            this.padding = new Padding(0, 0, 150, 0);
            this.iApplicationBackground = ResourceManager.CreateImage("Images/appBackground");
            this.iMainContainerBackground = ResourceManager.CreateImage("Images/napkin");
            this.iPen = ResourceManager.CreateImage("Images/pen");
            this.iButtonPlusReleased = ResourceManager.CreateImage("Images/plusButtonReleased");
            this.iButtonPlusPressed = ResourceManager.CreateImage("Images/plusButtonPressed");
            this.iButtonMinusReleased = ResourceManager.CreateImage("Images/minusButtonReleased");
            this.iButtonMinusPressed = ResourceManager.CreateImage("Images/minusButtonPressed");
            this.writingPlayer = ResourceManager.CreateSound("Sounds/Writing").CreateInstance();

            // Config Application
            base.SetBackground(this.iApplicationBackground, Adjustment.STRETCH);

            // Controls
            // Main Container
            this.mainContainer = new Container<CoordLayout>(new CoordLayout());
            this.mainContainer.BackgroundImage = this.iMainContainerBackground;
            this.mainContainer.Draggable = false;
            this.mainContainer.Size = this.iMainContainerBackground.Size;
            this.mainContainer.BringToFront = false;

            // Euros Label
            this.lEuros = new Label("0", Color.White, Color.Transparent);
            this.lEuros.Pivot = Vector2.UnitX;
            this.lEuros.Align = Label.AlignType.MIDDLERIGHT;
            this.lEuros.Draggable = false;
            this.lEuros.Padding = this.padding;
            this.lEuros.Font = this.fLarge;

            // Cents Label
            this.lCents = new Label("0", Color.White, Color.Transparent);
            this.lCents.Pivot = Vector2.UnitX;
            this.lCents.Align = Label.AlignType.MIDDLERIGHT;
            this.lCents.Draggable = false;
            this.lCents.Padding = this.padding;
            this.lCents.Font = this.fSmall;

            // People Label
            this.lPeople = new Label("2", Color.White, Color.Transparent);
            this.lPeople.Pivot = Vector2.UnitX;
            this.lPeople.Align = Label.AlignType.MIDDLERIGHT;
            this.lPeople.Draggable = false;
            this.lPeople.Padding = this.padding;
            this.lPeople.Font = this.fLarge;

            // total Label
            this.lTotal = new Label("0", Color.White, Color.Transparent);
            this.lTotal.Pivot = Vector2.UnitX;
            this.lTotal.Align = Label.AlignType.MIDDLERIGHT;
            this.lTotal.Draggable = false;
            this.lTotal.Padding = this.padding;
            this.lTotal.Font = this.fSmall;
            this.lTotal.Text = "0,00";

            // Masks 
            this.penRelativePositions = new Vector2[3];
            this.lEurosMask = new Label(string.Empty, Color.Transparent, Color.Transparent);
            this.lEurosMask.Draggable = false;
            this.lEurosMask.Size = new Vector2(this.mainContainer.Size.X, this.lEuros.Size.Y);
            this.lEurosMask.Pressed += delegate
            {
                this.moneySelected = true;
                this.eurosSelected = true;
                this.lPen.Position = this.penRelativePositions[0];
            };

            this.lCentsMask = new Label(string.Empty, Color.Transparent, Color.Transparent);
            this.lCentsMask.Draggable = false;
            this.lCentsMask.Size = new Vector2(this.mainContainer.Size.X, this.lCents.Size.Y);
            this.lCentsMask.Pressed += delegate
            {
                this.moneySelected = true;
                this.eurosSelected = false;
                this.lPen.Position = this.penRelativePositions[1];
            };

            this.lPeopleMask = new Label(string.Empty, Color.Transparent, Color.Transparent);
            this.lPeopleMask.Draggable = false;
            this.lPeopleMask.Size = new Vector2(this.mainContainer.Size.X, this.lPeople.Size.Y);
            this.lPeopleMask.Pressed += delegate
            {
                this.moneySelected = false;
                this.lPen.Position = this.penRelativePositions[2];
            };

            // Main Container Layout
            this.mainContainer.Layout.AddComponent(this.lEuros, 314f, 73f);
            this.mainContainer.Layout.AddComponent(this.lCents, 300f, 180f);
            this.mainContainer.Layout.AddComponent(this.lPeople, 300f, 351f);
            this.mainContainer.Layout.AddComponent(this.lTotal, 314f, 572f);
            this.mainContainer.Layout.AddComponent(this.lEurosMask, 0f, 73f);
            this.mainContainer.Layout.AddComponent(this.lCentsMask, 0f, 180f);
            this.mainContainer.Layout.AddComponent(this.lPeopleMask, 0f, 351f);

            // Pen Label
            this.lPen = new Label(this.iPen);
            this.lPen.Pivot = Vector2.One;
            this.lPen.Touchable = false;
            this.lPen.Draggable = false;

            // Plus Button
            this.bPlus = new Button(this.iButtonPlusReleased, this.iButtonPlusPressed);
            this.bPlus.Draggable = false;
            this.bPlus.Pressed -= new Component.ComponentEventHandler(this.HandleBplusPressed);
            this.bPlus.Pressed += new Component.ComponentEventHandler(this.HandleBplusPressed);
            this.bPlus.Released -= new Component.ComponentEventHandler(this.HandleBplusReleased);
            this.bPlus.Released += new Component.ComponentEventHandler(this.HandleBplusReleased);

            // Minus Button
            this.bMinus = new Button(this.iButtonMinusReleased, this.iButtonMinusPressed);
            this.bMinus.Draggable = false;
            this.bMinus.Pressed -= new Component.ComponentEventHandler(this.HandleBminusPressed);
            this.bMinus.Pressed += new Component.ComponentEventHandler(this.HandleBminusPressed);
            this.bMinus.Released -= new Component.ComponentEventHandler(this.HandleBminusReleased);
            this.bMinus.Released += new Component.ComponentEventHandler(this.HandleBminusReleased);

            // Application Layout
            base.AddComponent(this.mainContainer, (float)(Preferences.Width / 2) - this.mainContainer.Size.X / 2f, (float)(Preferences.Height / 2) - this.mainContainer.Size.Y / 2f);
            base.AddComponent(this.bPlus, (float)Preferences.Width - this.bPlus.Size.X, (float)Preferences.Height - this.bPlus.Size.Y);
            base.AddComponent(this.bMinus, 0f, (float)Preferences.Height - this.bMinus.Size.Y);

            this.penRelativePositions[0] = new Vector2(this.mainContainer.Position.X + 109f, this.lEuros.Position.Y + this.lEuros.Size.Y / 2f - 30f);
            this.penRelativePositions[1] = new Vector2(this.mainContainer.Position.X + 109f, this.lCents.Position.Y + this.lCents.Size.Y / 2f - 30f);
            this.penRelativePositions[2] = new Vector2(this.mainContainer.Position.X + 109f, this.lPeople.Position.Y + this.lPeople.Size.Y / 2f - 30f);

            base.AddComponent(this.lPen, this.penRelativePositions[0].X, this.penRelativePositions[0].Y);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
            int milliseconds = elapsedGameTime.Milliseconds;
            if (this.morePressed)
            {
                if (this.moneySelected)
                {
                    if (this.eurosSelected)
                    {
                        this.PartialIncrementTotalUpdate(milliseconds, 1f, 999.99f);
                    }
                    else
                    {
                        this.PartialIncrementTotalUpdate(milliseconds, 0.01f, 999.99f);
                    }
                    Label arg_7F_0 = this.lEuros;
                    int num = (int)this.total;
                    arg_7F_0.Text = num.ToString();
                    Label arg_A9_0 = this.lCents;
                    num = (int)((this.total - (float)((int)this.total)) * 100f);
                    arg_A9_0.Text = num.ToString();
                }
                else
                {
                    this.PartialIncrementHeadsUpdate(milliseconds, 1, 99);
                    this.lPeople.Text = this.heads.ToString();
                }
            }
            else
            {
                if (this.lessPressed)
                {
                    if (this.moneySelected)
                    {
                        if (this.eurosSelected)
                        {
                            this.PartialDecrementTotalUpdate(milliseconds, 1f, 0f);
                        }
                        else
                        {
                            this.PartialDecrementTotalUpdate(milliseconds, 0.01f, 0f);
                        }
                        Label arg_143_0 = this.lEuros;
                        int num = (int)this.total;
                        arg_143_0.Text = num.ToString();
                        Label arg_16D_0 = this.lCents;
                        num = (int)((this.total - (float)((int)this.total)) * 100f);
                        arg_16D_0.Text = num.ToString();
                    }
                    else
                    {
                        this.PartialDecrementHeadsUpdate(milliseconds, 1, 2);
                        this.lPeople.Text = this.heads.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Pops this screen returning to the previous one, or exiting the app if there is no more left.
        /// This method is called when the hardware back button is pressed (only Windows Phone & Android)
        /// </summary>
        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Float Partial Increment 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="increment"></param>
        /// <param name="maximum"></param>
        private void PartialIncrementTotalUpdate(int milliseconds, float increment, float maximum)
        {
            if ((this.interval += milliseconds) >= this.gap)
            {
                this.interval = 0;
                if ((this.total += increment) > maximum)
                {
                    this.total = maximum;
                }
                if (this.gap > 50)
                {
                    this.gap -= 100;
                }
            }
        }

        /// <summary>
        /// Float Partial Decrement
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="increment"></param>
        /// <param name="minimum"></param>
        private void PartialDecrementTotalUpdate(int milliseconds, float increment, float minimum)
        {
            if ((this.interval += milliseconds) >= this.gap)
            {
                this.interval = 0;
                if ((this.total -= increment) < minimum)
                {
                    this.total = minimum;
                }
                if (this.gap > 50)
                {
                    this.gap -= 100;
                }
            }
        }

        /// <summary>
        /// Integer Partial Increment
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="increment"></param>
        /// <param name="maximum"></param>
        private void PartialIncrementHeadsUpdate(int milliseconds, int increment, int maximum)
        {
            if ((this.interval += milliseconds) >= this.gap)
            {
                this.interval = 0;
                if ((this.heads += increment) > maximum)
                {
                    this.heads = maximum;
                }
                if (this.gap > 50)
                {
                    this.gap -= 100;
                }
            }
        }

        /// <summary>
        /// Integer Partial Decrement
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="increment"></param>
        /// <param name="minimum"></param>
        private void PartialDecrementHeadsUpdate(int milliseconds, int increment, int minimum)
        {
            if ((this.interval += milliseconds) >= this.gap)
            {
                this.interval = 0;
                if ((this.heads -= increment) < minimum)
                {
                    this.heads = minimum;
                }
                if (this.gap > 50)
                {
                    this.gap -= 100;
                }
            }
        } 
        #endregion
    }
}
