/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using Microsoft.Xna.Framework; 
#endregion

namespace ScreenManager
{
    /// <summary>
    /// Interface a Screen must have
    /// </summary>
    public interface IScreen:IDisposable
    {
        void LoadScreen();

        void ClearScreen();

        void Update(GameTime gameTime);

        
    }
}
