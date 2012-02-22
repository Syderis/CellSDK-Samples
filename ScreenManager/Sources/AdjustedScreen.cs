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
using Syderis.CellSDK.Core.Physics;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Common; 
#endregion

namespace ScreenManager
{
    /// <summary>
    /// Class that adjust the element position in a safety zone
    /// </summary>
    public class AdjustedScreen : Screen
    {
        #region Variables

        private Vector2 securityZone = new Vector2(480, 800);

        public Vector2 adjust;
		protected float top;
		protected float bottom;
		protected float left;
		protected float right;
		protected float maxScale;
		protected Vector2 scale;

        #endregion

        #region Constructors

        public AdjustedScreen()
        {
            int screenX = Preferences.Width;
            int screenY = Preferences.Height;
			this.scale = new Vector2(	Preferences.Width / (float) securityZone.X, 
			                         	Preferences.Height / (float) securityZone.Y);
			this.maxScale = (scale.X > scale.Y) ? scale.X : scale.Y;
			
				
			float aX = 0;
			float aY = 0;

            if ((screenX != securityZone.X) || (screenY != securityZone.Y))
            {
                aX = (screenX - securityZone.X) / 2.0f;
                aY = (screenY - securityZone.Y) / 2.0f;
                this.adjust = new Vector2(aX, aY);
				
				
            }
            else
            {
                this.adjust = Vector2.Zero;
            }
			
			this.left = -aX;
			this.right = securityZone.X + aX;
			this.top = -aY;
			this.bottom = securityZone.Y + aY;
        }

        #endregion

        #region Public functions

        public override void AddComponent(Component component, float x, float y)
        {
            base.AddComponent(component, x + this.adjust.X, y + this.adjust.Y);
        }

        public override void AddComponent(Component component, float x, float y, BodyShape shape, BodyType bodyType, Category category)
        {
            base.AddComponent(component, x + this.adjust.X, y + this.adjust.Y, shape, bodyType, category);
        }

        #endregion
    }
}
