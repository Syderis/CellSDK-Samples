#region Using Statements
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.Core;
#endregion

namespace GamePad
{
    class Buttons : Container<CoordLayout>
    {
        public Buttons()
            : base(new CoordLayout())
        {
            SpriteSheet iGamePadSpriteSheet = StaticContent.Resources.CreateSpriteSheet("GamePadSpriteSheet");
            BackgroundImage = iGamePadSpriteSheet["bg_bt"];
            Button bA = new Button(iGamePadSpriteSheet["bt_A"], iGamePadSpriteSheet["bt_A_pressed"]);
            Layout.AddComponent(bA, 6, 96);
            Button bB = new Button(iGamePadSpriteSheet["bt_B"], iGamePadSpriteSheet["bt_B_pressed"]);
            Layout.AddComponent(bB, 77, 6);

            Size = new Vector2(179, 197);
        }
    }
}
