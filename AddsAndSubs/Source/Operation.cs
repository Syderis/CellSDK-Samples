/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.Core.Graphics;
#endregion 

namespace AddsAndSubs
{
    class Operation : Container<CoordLayout>
    {
        // Y position where the subcomponents will be aligned
        const int Y = 50;

        private Label lResult;

        public Operation(OperationBridge operation, Font font, Image[] iNumbers)
            : base(new CoordLayout())
        {
            BackgroundColor = Color.Transparent;

            Padding padding = new Padding(0, 0, 100, 0); // 100 is enough to cover the font character
            Color red = new Color(215, 56, 56);
            Color yellow = new Color(255, 209, 81);

            #region Left operand
            Label lLeftOperand = new Label(operation.LeftOperand.ToString(), red, Color.Transparent)
            {
                Padding = padding,
                Font = font
            };
            Layout.AddComponent(lLeftOperand, 8, Y);
            #endregion Left operand

            #region Operator
            Label lOperator = new Label(operation.IsAdd ? "+" : "-", yellow, Color.Transparent)
            {
                Padding = padding,
                Font = font
            };
            // FIXME: Centrar en el eje horizontal para que '+' y '-' aparezcan igual de centrados
            Layout.AddComponent(lOperator, 90, Y);
            #endregion Operator

            #region Right operand
            Label lRightOperand = new Label(operation.RightOperand.ToString(), red, Color.Transparent)
            {
                Padding = padding,
                Font = font
            };
            Layout.AddComponent(lRightOperand, 169, Y);
            #endregion Right operand

            #region Equal
            Label lEqual = new Label("=", yellow, Color.Transparent)
            {
                Padding = padding,
                Font = font
            };
            Layout.AddComponent(lEqual, 243, Y);
            #endregion Equal

            #region Result
            lResult = new Label(iNumbers[operation.Result]) 
            { 
                // 12 is an offset to center the label
                Position = new Vector2(operation.Result != 10 ? 349 : 313, operation.Result != 10 ? 13 + 12 : 22 + 12) 
            };
            #endregion Result
        }

        public void AddResult()
        {
            Layout.AddComponent(lResult, lResult.Position.X, lResult.Position.Y);
        }
    }
}
