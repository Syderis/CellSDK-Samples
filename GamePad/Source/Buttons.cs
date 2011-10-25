using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.Core.Graphics;

namespace GamePad
{
    class Buttons : Container<CoordLayout>
    {
        public Buttons()
            : base(new CoordLayout())
        {
            Size = new Vector2(197, 179);

            Image iGamePadSpriteSheet = Image.CreateImage("GamePadSpriteSheet");
            BackgroundImage = iGamePadSpriteSheet.SubImage(0, 0, 197, 179);
            Button bA = new Button(iGamePadSpriteSheet.SubImage(181, 181, 97, 95), iGamePadSpriteSheet.SubImage(379, 0, 97, 95));
            Layout.AddComponent(bA, 96, 77);
            Button bB = new Button(iGamePadSpriteSheet.SubImage(181, 277, 97, 95), iGamePadSpriteSheet.SubImage(279, 181, 97, 95));
            Layout.AddComponent(bB, 6, 6);
        }
    }
}
