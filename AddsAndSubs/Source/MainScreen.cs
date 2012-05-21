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
using Syderis.CellSDK.Core.Animations;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.Core.Graphics; 
#endregion

namespace AddsAndSubs
{
    public class MainScreen: Screen
    {
        const int FRAMES = 10;
        const int OPERATIONS = 25;

        private int currentOperation;
        private Sprite sTopBackground;
        private Sprite sBottomBackground;
        private Sprite sFrame;
        private Results results;
        private Operation[] operations;
        private OperationBridge[] temporalOperations;
        private Animation[] opAnimations;

        //private Vector2 offset = new Vector2(81, 79);

        public override void Initialize()
        {
            base.Initialize();        

            #region Background
            // Board background
            SetBackground(ResourceManager.CreateImage("Images/Background"), Adjustment.CENTER);

            // Top background for masking the operations done
            sTopBackground = new Sprite("BackgroundTop", ResourceManager.CreateImage("Images/BackgroundTop")) { Pivot = new Vector2(0.5f, 0), BringToFront = false };
            AddComponent(sTopBackground, Preferences.ViewportManager.TopCenterAnchor - new Vector2(0, 2.0f));

            // Bottom background for masking the following operations
            sBottomBackground = new Sprite("BackgroundBottom", ResourceManager.CreateImage("Images/BackgroundBottom")) { Pivot = new Vector2(0.5f, 1), BringToFront = false };
            AddComponent(sBottomBackground, Preferences.ViewportManager.BottomCenterAnchor - new Vector2(0, 2.0f));
            #endregion Background

            // Frame where the operation will take place
            sFrame = new Sprite("Frame", ResourceManager.CreateImage("Images/Frame")) { BringToFront = false };
            AddComponent(sFrame, 0, 218);

            results = new Results() { BringToFront = false };
            results.OnSelect += new Results.ResultsEventHandler(results_OnSelect);
            AddComponent(results, 22, 612);

            #region Animations
            // 4 animations will move the operations
            opAnimations = new Animation[4];
            Vector2 moveUp = new Vector2(0, -sFrame.SpriteImage.Height);

            opAnimations[0] = Animation.CreateAnimation(FRAMES);
            opAnimations[0].AnimationType = AnimationType.Relative;
            opAnimations[0].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, .3f));
            opAnimations[0].AddKey(new KeyFrame(opAnimations[0].NumFrames - 1, moveUp, 0, 1, 0));
            AddAnimation(opAnimations[0]);

            opAnimations[1] = Animation.CreateAnimation(FRAMES);
            opAnimations[1].AnimationType = AnimationType.Relative;
            opAnimations[1].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 1));
            opAnimations[1].AddKey(new KeyFrame(opAnimations[1].NumFrames - 1, moveUp, 0, 1, .3f));
            AddAnimation(opAnimations[1]);

            opAnimations[2] = Animation.CreateAnimation(FRAMES);
            opAnimations[2].AnimationType = AnimationType.Relative;
            opAnimations[2].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, .3f));
            opAnimations[2].AddKey(new KeyFrame(opAnimations[2].NumFrames - 1, moveUp, 0, 1, 1));
            AddAnimation(opAnimations[2]);

            opAnimations[3] = Animation.CreateAnimation(FRAMES);
            opAnimations[3].AnimationType = AnimationType.Relative;
            opAnimations[3].AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 0));
            opAnimations[3].AddKey(new KeyFrame(opAnimations[3].NumFrames - 1, moveUp, 0, 1, .3f));
            AddAnimation(opAnimations[3]);
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
            Font font = ResourceManager.CreateFont("Fonts/Numbers");
            Image[] iNumbers = new Image[11];

            for (int i = 0; i < iNumbers.Length; i++)
                iNumbers[i] = ResourceManager.CreateImage("Images/" + i.ToString());

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
                AddComponent(v, v.Position.X, v.Position.Y);
            #endregion Operations

            ReorderComponents();
        }

        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
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
                opAnimations[1].Play(operations[0]);
                opAnimations[2].Play(operations[1]);
                opAnimations[3].Play(operations[2]);

                operations[3].Visible = true;
                operations[3].Position = new Vector2(0, 589);
            }
            // Focus the operation at index currentOperation
            else if (currentOperation > 1 && currentOperation < operations.Length - 2)
            {
                opAnimations[0].Play(operations[currentOperation - 2]);
                opAnimations[1].Play(operations[currentOperation - 1]);
                opAnimations[2].Play(operations[currentOperation]);
                opAnimations[3].Play(operations[currentOperation + 1]);

                //animations[3].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);

                operations[currentOperation + 2].Visible = true;
                operations[currentOperation + 2].Position = new Vector2(0, 589);
            }
            // Focus the operation just before the last one
            else if (currentOperation == operations.Length - 2)
            {
                opAnimations[0].Play(operations[currentOperation - 2]);
                opAnimations[1].Play(operations[currentOperation - 1]);
                opAnimations[2].Play(operations[currentOperation]);
                opAnimations[3].Play(operations[currentOperation + 1]);

                //animations[3].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);
            }
            // Focus the last operation
            else if (currentOperation == operations.Length - 1)
            {
                opAnimations[0].Play(operations[currentOperation - 2]);
                opAnimations[1].Play(operations[currentOperation - 1]);
                opAnimations[2].Play(operations[currentOperation]);

                //animations[2].EndEvent += new Animation.AnimationHandler(MatePlusAddAndSubScreen_EndEvent);
            }
            // The end, no more operations
            else
            {
                opAnimations[0].Play(operations[currentOperation - 2]);
                opAnimations[1].Play(operations[currentOperation - 1]);
            }
        }

        void ReorderComponents()
        {
            //SendToFront(lBackground);

            foreach (var v in operations)
                SendToFront(v);

            SendToFront(sTopBackground);
            SendToFront(sBottomBackground);
            SendToFront(sFrame);
            SendToFront(results);
        }
    }

    struct OperationBridge
    {
        public int LeftOperand, RightOperand, Result;
        public bool IsAdd;
    }
    
}
