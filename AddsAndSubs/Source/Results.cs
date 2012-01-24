#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Layouts;
#endregion

namespace AddsAndSubs
{
    class Results : Container<CoordLayout>
    {
        public delegate void ResultsEventHandler(Results source, ResultsEventArgs e);
        public event ResultsEventHandler OnSelect;

        public Results()
            : base(new CoordLayout())
        {
            BackgroundColor = Color.Transparent;

            // These buttons will hold each number independently
            Button[] bResults = new Button[11];
            // These images will hold each number released independently
            Image[] iReleased = new Image[11];
            // These images will hold each number pressed independently
            Image[] iPressed = new Image[11];
            // Sprite sheet with released numbers
            Image iReleasedSheet = Image.CreateImage("Images/ResultsReleased");
            // Sprite sheet with pressed numbers
            Image iPressedSheet = Image.CreateImage("Images/ResultsPressed");
            // 6 is the amount of sprites per row on the sprite sheet
            int numberX = iReleasedSheet.Width / 6;
            // 2 is the amount of rows
            int numberY = iReleasedSheet.Height / 2;

            for (int i = 0, j = -1, x, y; i < bResults.Length; i++)
            {
                int column = (i % 6);

                if (column == 0) j++;

                x = numberX * column;
                y = numberY * j;

                // From 0 to 9
                if (i != bResults.Length - 1)
                    bResults[i] = new Button(
                        iReleasedSheet.SubImage(x, y, numberX, numberY),
                        iPressedSheet.SubImage(x, y, numberX, numberY));
                // 10, which gets twice the space of a single number
                else
                {
                    int temp2 = iReleasedSheet.Width - numberX * 2;

                    bResults[i] = new Button(
                        iReleasedSheet.SubImage(x, y, temp2, numberY),
                        iPressedSheet.SubImage(x, y, temp2, numberY));
                }

                bResults[i].Position = new Vector2(x, y);
            }

            bResults[0].Pressed += delegate { FireOnSelect(0); };
            bResults[1].Pressed += delegate { FireOnSelect(1); };
            bResults[2].Pressed += delegate { FireOnSelect(2); };
            bResults[3].Pressed += delegate { FireOnSelect(3); };
            bResults[4].Pressed += delegate { FireOnSelect(4); };
            bResults[5].Pressed += delegate { FireOnSelect(5); };
            bResults[6].Pressed += delegate { FireOnSelect(6); };
            bResults[7].Pressed += delegate { FireOnSelect(7); };
            bResults[8].Pressed += delegate { FireOnSelect(8); };
            bResults[9].Pressed += delegate { FireOnSelect(9); };
            bResults[10].Pressed += delegate { FireOnSelect(10); };

            foreach (var v in bResults)
                Layout.AddComponent(v, v.Position.X, v.Position.Y);
        }

        void FireOnSelect(int result)
        {
            if (OnSelect != null)
                OnSelect(this, new ResultsEventArgs(result));
        }
    }

    /// <summary>
    /// Holds the number so the consumer can know which one was selected
    /// </summary>
    class ResultsEventArgs : EventArgs
    {
        public int Result;

        public ResultsEventArgs(int result)
        {
            Result = result;
        }
    }
}
