#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
#endregion

namespace GamePad
{
    class ThumbStick : Canvas
    {
        const int PAD_OFFSET = 27;

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
     
        private Image bgPadImage, padImage;
        private Vector2 padPosition;

        public float X
        {
            get { return padPosition.X / PAD_OFFSET; }
        }

        public float Y
        {
            get { return padPosition.Y / PAD_OFFSET; }
        }

        public ThumbStick()
            : base(180, 180)
        {
            SpriteSheet ssGamePadSpriteSheet = StaticContent.Resources.CreateSpriteSheet("GamePadSpriteSheet");
            bgPadImage = ssGamePadSpriteSheet["bg_pad"];
            padImage = ssGamePadSpriteSheet["pad"];

            spriteBatch = StaticContent.SpriteBatch;
            graphicsDevice = StaticContent.Graphics.GraphicsDevice;

            padPosition = Vector2.Zero;
        }

        public override void CanvasDraw()
        {
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(bgPadImage.Texture, Vector2.Zero, bgPadImage.Rectangle, Color.White);
            spriteBatch.Draw(padImage.Texture, padPosition, padImage.Rectangle, Color.White);
            spriteBatch.End();

            base.CanvasDraw();
        }

        public override void CanvasTouchMoved(List<Syderis.CellSDK.Common.IBlob> blobs)
        {
            Vector2 temp = padPosition;
            temp.X = MathHelper.Clamp(temp.X + Vector2.Subtract(blobs[0].Position, blobs[0].Preposition).X, -PAD_OFFSET, PAD_OFFSET);
            temp.Y = MathHelper.Clamp(temp.Y + Vector2.Subtract(blobs[0].Position, blobs[0].Preposition).Y, -PAD_OFFSET, PAD_OFFSET);
            padPosition = temp;

            base.CanvasTouchMoved(blobs);
        }

        public override void CanvasTouchReleased(List<Syderis.CellSDK.Common.IBlob> blobs)
        {
            padPosition = Vector2.Zero;
            
            base.CanvasTouchReleased(blobs);
        }
    }
}
