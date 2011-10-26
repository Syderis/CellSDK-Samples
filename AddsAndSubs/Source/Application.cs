using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Animations;

namespace AddsAndSubs
{
    class Application : MultitouchApplication
    {
        const int FRAMES = 10;
        const int OPERATIONS = 25;

        private int currentOperation;
        private Label lBackground;
        private Label lTopBackground;
        private Label lBottomBackground;
        private Label lFrame;
        private Results results;
        private Operation[] operations;
        private OperationBridge[] temporalOperations;
        private Animation[] animations;

        /// <summary>
        /// The main method for loading controls and resources.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Don't show the blob
            IsTouchVisible = false;

            #region Background
            // Board background
            // Instead of calling SetBackground(), I make use of a same image placing it in the middle of the screen
            // SetBackground could have a parameter like POSITIONING.CENTER
            lBackground = new Label(Image.CreateImage("Images/Background")) { Pivot = Vector2.One / 2, BringToFront = false };
            AddComponentDeviceAgnostic(lBackground, Width / 2, Height / 2);

            // Top background for masking the operations done
            lTopBackground = new Label(Image.CreateImage("Images/BackgroundTop")) { BringToFront = false };
            AddComponentDeviceAgnostic(lTopBackground, 0, 0);

            // Bottom background for masking the following operations
            lBottomBackground = new Label(Image.CreateImage("Images/BackgroundBottom")) { BringToFront = false };
            AddComponentDeviceAgnostic(lBottomBackground, 0, 572);
            #endregion Background

            // Frame where the operation will take place
            lFrame = new Label(Image.CreateImage("Images/Frame")) { BringToFront = false };
            AddComponentDeviceAgnostic(lFrame, 0, 218);

            results = new Results() { BringToFront = false };
            results.OnSelect += new Results.ResultsEventHandler(results_OnSelect);
            AddComponentDeviceAgnostic(results, 22, 612);

            #region Animations
            // 4 animations will move the operations
            animations = new Animation[4];
            Vector2 moveUp = new Vector2(0, -lFrame.Image.Height);

            animations[0] = Animation.CreateAnimation(FRAMES);
            animations[0].AnimationType = AnimationType.Relative;
            animations[0].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, .3f));
            animations[0].AddKey(new KeyFrame(animations[0].NumFrames - 1, moveUp, 0, 1, 0));

            animations[1] = Animation.CreateAnimation(FRAMES);
            animations[1].AnimationType = AnimationType.Relative;
            animations[1].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 1));
            animations[1].AddKey(new KeyFrame(animations[1].NumFrames - 1, moveUp, 0, 1, .3f));

            animations[2] = Animation.CreateAnimation(FRAMES);
            animations[2].AnimationType = AnimationType.Relative;
            animations[2].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, .3f));
            animations[2].AddKey(new KeyFrame(animations[2].NumFrames - 1, moveUp, 0, 1, 1));

            animations[3] = Animation.CreateAnimation(FRAMES);
            animations[3].AnimationType = AnimationType.Relative;
            animations[3].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 0));
            animations[3].AddKey(new KeyFrame(animations[3].NumFrames - 1, moveUp, 0, 1, .3f));
            #endregion Animations

            #region Operations
            temporalOperations = new OperationBridge[OPERATIONS];
            Random r = new Random();

            // This loop will generate every operation based on random numbers
            for (int i = 0; i < temporalOperations.Length; i++)
            {
                temporalOperations[i].LeftOperand = r.Next(0, 10);
                temporalOperations[i].IsAdd = r.Next(0, 2) == 0; // 0 means +, 1 means -
                temporalOperations[i].Result = r.Next(
                    temporalOperations[i].IsAdd ? temporalOperations[i].LeftOperand : 0,
                    temporalOperations[i].IsAdd ? 10 : temporalOperations[i].LeftOperand);
                temporalOperations[i].RightOperand = Math.Abs(temporalOperations[i].Result - temporalOperations[i].LeftOperand);
            }

            operations = new Operation[temporalOperations.Length];
            Font font = Font.CreateFont("Fonts/Numbers");
            Image[] iNumbers = new Image[11];

            for (int i = 0; i < iNumbers.Length; i++)
                iNumbers[i] = Image.CreateImage("Images/" + i.ToString());

            for (int i = 0; i < operations.Length; i++)
                operations[i] = new Operation(temporalOperations[i], font, iNumbers) { Visible = false, BringToFront = false };

            currentOperation = 0;

            operations[0].Visible = true;
            operations[0].Position = new Vector2(0, 217);
            operations[1].Position = new Vector2(0, operations[0].Position.Y + 186);
            operations[1].Visible = true;
            operations[1].Alpha = .5f;
            operations[2].Position = new Vector2(0, operations[1].Position.Y + 186);
            operations[2].Visible = true;

            foreach (var v in operations)
                AddComponentDeviceAgnostic(v, v.Position.X, v.Position.Y);
            #endregion Operations

            ReorderComponents();
        }

        public override void BackButtonPressed()
        {
            Program.Instance.Exit();
        }

        void results_OnSelect(Results source, ResultsEventArgs e)
        {
            if (currentOperation < OPERATIONS && e.Result == temporalOperations[currentOperation].Result)
            {
                operations[currentOperation].AddResult();
                NextOperation();
            }
        }

        private void NextOperation()
        {
            currentOperation++;

            // Focus the operation at index 1
            if (currentOperation == 1)
            {
                animations[1].Play(operations[0]);
                animations[2].Play(operations[1]);
                animations[3].Play(operations[2]);

                operations[3].Visible = true;
                operations[3].Position = new Vector2(0, 589);
            }
            // Focus the operation at index currentOperation
            else if (currentOperation > 1 && currentOperation < operations.Length - 2)
            {
                animations[0].Play(operations[currentOperation - 2]);
                animations[1].Play(operations[currentOperation - 1]);
                animations[2].Play(operations[currentOperation]);
                animations[3].Play(operations[currentOperation + 1]);

                //animations[3].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);

                operations[currentOperation + 2].Visible = true;
                operations[currentOperation + 2].Position = new Vector2(0, 589);
            }
            // Focus the operation just before the last one
            else if (currentOperation == operations.Length - 2)
            {
                animations[0].Play(operations[currentOperation - 2]);
                animations[1].Play(operations[currentOperation - 1]);
                animations[2].Play(operations[currentOperation]);
                animations[3].Play(operations[currentOperation + 1]);

                //animations[3].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);
            }
            // Focus the last operation
            else if (currentOperation == operations.Length - 1)
            {
                animations[0].Play(operations[currentOperation - 2]);
                animations[1].Play(operations[currentOperation - 1]);
                animations[2].Play(operations[currentOperation]);

                //animations[2].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);
            }
            // The end, no more operations
            else
            {
                animations[0].Play(operations[currentOperation - 2]);
                animations[1].Play(operations[currentOperation - 1]);
            }
        }

        void AddComponentDeviceAgnostic(Component c, float x, float y)
        { 
#if IPHONE
            AddComponent(c, x + 81, y + 79);
#endif
            AddComponent(c, x, y);
        }

        void ReorderComponents()
        {
            SendToFront(lBackground);

            foreach (var v in operations)
                SendToFront(v);

            SendToFront(lTopBackground);
            SendToFront(lBottomBackground);
            SendToFront(lFrame);
            SendToFront(results);
        }
    }

    struct OperationBridge
    {
        public int LeftOperand, RightOperand, Result;
        public bool IsAdd;
    }
}
