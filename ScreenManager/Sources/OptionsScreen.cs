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
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core; 
#endregion

namespace ScreenManager
{
    public class OptionsScreen : AdjustedScreen
    {
        private Image iBackground, iBack, iBack_Pressed;
        private Button bBack;

        public override void Initialize()
        {
            base.Initialize();

            //Load Images
            iBackground = ResourceManager.CreateImage("Options/background_options");
            iBack = ResourceManager.CreateImage("Options/bt_back");
            iBack_Pressed = ResourceManager.CreateImage("Options/bt_back_pressed");

            //Sets the background:
            SetBackground(iBackground, Adjustment.CENTER);

            //CreateButton
            bBack = new Button(iBack, iBack_Pressed);
            bBack.Released += new Component.ComponentEventHandler(bBack_Released);

            //Add components to the screen           
            AddComponent(bBack, 25, 17);
        }

        void bBack_Released(Component source)
        {
            BackButtonPressed();
        }

        public override void BackButtonPressed()
        {
            StaticContent.ScreenManager.PopScreen(StaticContent.TransitionFactory.FadeBlackTransition);
        }
    }
}
