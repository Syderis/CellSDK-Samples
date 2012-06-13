/*
* Copyright 2012 Syderis Technologies S.L. All rights reserved.
* Use is subject to license terms.
*/

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Screens;
using Syderis.CellSDK.Core.Graphics;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.Core.Physics;
using System.Diagnostics; 
#endregion

namespace BulletMan
{
    public class MainScreen : Screen
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const int sandline = 425;

        /// <summary>
        /// Attribute
        /// </summary>
        private bool isCanyonPressed = false;
        private SpriteSheet spriteSheet;
        private ParticleSystem psSmoke;
        private ProgressBar pbPower;
        private Slider slOrientation;
        private Sprite s_Canyon;
        private Sprite ball; //borrar
        private Image iball;
        private Vector2 aux_offset;
        private TimeSpan timer;
        private List<Sprite> balls;
        private List<Sprite> clowns;
        private Sprite sHead, sBody, sRightArm, sLeftArm, sRightLeg, sLeftLeg;
        private Layer layer;

        /// <summary>
        /// Sets the screen up (UI components, multimedia content, etc.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Create Physicworld
            CreatePhysicWorld(new Vector2(0, -20), true, false, Vector2.Zero);

            Sprite sand = new Sprite("sand", ResourceManager.CreateImage(Color.Transparent, (int)Preferences.Width, 65));
            AddComponent(sand, 0, 415, BodyShape.SQUARE, BodyType.STATIC, Category.All);

            //Background
            SetBackground(ResourceManager.CreateImage("Images/background"), Adjustment.CENTER);

            spriteSheet = ResourceManager.CreateSpriteSheet("Images/spritesheet");

            //Canyon
            CreateCanyon(new Vector2(620,274));          

            //Clowns castle
            CreateCastle(new Vector2(28,310));

            //Helpers
            aux_offset = Vector2.Zero;
            timer = TimeSpan.Zero;
            iball = ResourceManager.CreateImage("Images/ball");
            layer = new Layer();

            Button breset = new Button(spriteSheet["bt_reset"], spriteSheet["bt_reset_pressed"]);
            breset.Released += new Component.ComponentEventHandler(breset_Released);
            AddComponent(breset, Preferences.ViewportManager.RightAnchor - breset.Size.X, 0);

        }       

        /// <summary>
        /// Create clowns castle
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="offset"></param>
        private void CreateCastle(Vector2 offset)
        {
            clowns = new List<Sprite>();
            Image iclown = spriteSheet["clown"];            

            int N = 3;
            Vector2 gap = Vector2.Zero;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < (N - i); j++)
                {
                    Sprite clown = new Sprite("clown"+i+","+j, iclown);                    
                    clown.Draggable = true;
                    AddComponent(clown, offset.X + (iclown.Rectangle.Width * j) + gap.X, offset.Y - gap.Y);
                    PhysicWorld.AddBody(clown, BodyType.DYNAMIC, BodyShape.SQUARE, Category.All);
                    clowns.Add(clown);
                }
                gap.X += iclown.Rectangle.Width / 2;
                gap.Y += iclown.Rectangle.Height;               
            }
        }

        /// <summary>
        /// Create component canyon
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="offset"></param>
        private void CreateCanyon(Vector2 offset)
        {           
            //Cañon            
            s_Canyon = new Sprite("canyon", spriteSheet["canyon"]);
            s_Canyon.Pivot = new Vector2(0.8f,0.5f);
            s_Canyon.Pressed += new Component.ComponentEventHandler(s_Canyon_Pressed);
            s_Canyon.Released += new Component.ComponentEventHandler(s_Canyon_Released);

            Sprite s_Pedestal = new Sprite("pedestal", spriteSheet["pedestal"]);

            AddComponent(s_Canyon, 87 + offset.X, 81 + offset.Y);
            AddComponent(s_Pedestal, 32 + offset.X, 110 + offset.Y);

            //PowerBar
            pbPower = new ProgressBar(spriteSheet["power_bar_empty"], spriteSheet["power_bar_full"], 0);
            AddComponent(pbPower, 25 + offset.X, offset.Y);

            //OrientationBar            
            slOrientation = new Slider(0, 67, 0, 130, spriteSheet["angle_slide_bar_bottom"], spriteSheet["angle_slide_bar_center"], spriteSheet["angle_slide_bar_top"], spriteSheet["bullet"], 0);
            slOrientation.Rotation = MathHelper.Pi / 2;
            slOrientation.ValueChangeEvent += new Component.ComponentEventHandler(slOrientation_ValueChangeEvent);
            AddComponent(slOrientation, 150 + offset.X, 13 + offset.Y);

            //Smoke Particles
            psSmoke = new ParticleSystem(spriteSheet["smoke"]);
            AddComponent(psSmoke, Vector2.Zero);

            //Helpers
            balls = new List<Sprite>();

        }

        /// <summary>
        /// Create ragdoll
        /// </summary>
        /// <param name="offset"></param>
        private void CreateBulletMan(Vector2 offset)
        {
            //Clear Ragdoll before
            foreach (Sprite ball in balls)
            {
                layer.RemoveSprite(ball);
                RemoveComponent(ball);
            }
            balls.Clear();

            //Create parts
            sHead = new Sprite("head", spriteSheet["head"]);
            sBody = new Sprite("body", spriteSheet["body"]);
            sRightArm = new Sprite("rarm", spriteSheet["right_arm"]);
            sLeftArm = new Sprite("larm", spriteSheet["left_arm"]);
            sRightLeg = new Sprite("rleg", spriteSheet["right_leg"]);
            sLeftLeg = new Sprite("lleg", spriteSheet["left_leg"]);

            AddComponent(sLeftArm, 50+offset.X, 0+offset.Y,BodyShape.SQUARE,BodyType.DYNAMIC,Category.Cat3);
            AddComponent(sRightArm, 50+offset.X, 52+offset.Y,BodyShape.SQUARE,BodyType.DYNAMIC,Category.Cat3);
            AddComponent(sRightLeg, 82+offset.X, 38+offset.Y,BodyShape.SQUARE,BodyType.DYNAMIC,Category.Cat4);
            AddComponent(sLeftLeg, 84+offset.X, 13+offset.Y,BodyShape.SQUARE,BodyType.DYNAMIC,Category.Cat4);
            AddComponent(sBody, 48+offset.X, 9+offset.Y,BodyShape.SQUARE,BodyType.DYNAMIC,Category.Cat2);
            AddComponent(sHead, 0+offset.X, 12+offset.Y,BodyShape.CIRCLE,BodyType.DYNAMIC,Category.Cat1);

            //Joints
            PhysicWorld.AddJoint(sBody,sHead,new Vector2(50+offset.X,35+offset.Y),true, -MathHelper.PiOver4,MathHelper.PiOver4);
            PhysicWorld.AddJoint(sBody, sLeftArm, new Vector2(53+offset.X, 14+offset.Y), true, 0, MathHelper.PiOver2);
            PhysicWorld.AddJoint(sBody, sRightArm, new Vector2(53 + offset.X, 56 + offset.Y), true, -MathHelper.PiOver2, 0);
            PhysicWorld.AddJoint(sBody, sLeftLeg, new Vector2(90 + offset.X, 24 + offset.Y), true, -MathHelper.PiOver2, MathHelper.PiOver2);
            PhysicWorld.AddJoint(sBody, sRightLeg, new Vector2(90 + offset.X, 46 + offset.Y), true, -MathHelper.PiOver2, MathHelper.PiOver2);

            sBody.Draggable = true;

            //Add to clear list
            balls.Add(sHead);
            balls.Add(sBody);
            balls.Add(sRightArm);
            balls.Add(sLeftArm);
            balls.Add(sRightLeg);
            balls.Add(sLeftLeg);

            //Add to layer
            foreach (Sprite sprite in balls)
                layer.AddSprite(sprite);
            layer.Rotate(slOrientation.Value, sHead.Position); //TODO: No rotan los sprites
        }

        #region Events
        /// <summary>
        /// Move the canyon
        /// </summary>
        /// <param name="source"></param>
        void slOrientation_ValueChangeEvent(Component source)
        {            
            s_Canyon.Rotation = MathHelper.ToRadians(slOrientation.Value);

            //Vector2 aux = s_Canyon.Position;
            //float radians = MathHelper.ToRadians(slOrientation.Value);
            //Vector2 offset = new Vector2((float)Math.Cos(radians) * s_Canyon.Size.X,
            //                             (float)Math.Sin(radians) * s_Canyon.Size.X);
            //Vector2 final = aux - offset;
            //rectangle.Position = final;                       
        }

        /// <summary>
        /// Release canyon fire button
        /// </summary>
        /// <param name="source"></param>
        void s_Canyon_Released(Component source)
        {
            isCanyonPressed = false;
            
            float radians = MathHelper.ToRadians(slOrientation.Value);
            aux_offset.X = (float)Math.Cos(radians) * s_Canyon.Size.X;
            aux_offset.Y = (float)Math.Sin(radians) * s_Canyon.Size.X;   
         
            //Smoke
            psSmoke.Position = s_Canyon.Position - aux_offset;            
            psSmoke.Play();

            //Bullet
            //ball = new Sprite("aux", iball);
            //ball.Draggable = true;
            //AddComponent(ball, psSmoke.Position);
            //PhysicWorld.AddBody(ball,BodyType.DYNAMIC, BodyShape.CIRCLE, Category.All);
            //balls.Add(ball);
            
            //Vector2 aux2 = psSmoke.Position-s_Canyon.Position;
            //aux2.Y *= -1;
            //aux2.Normalize();
            //aux2 *= pbPower.Value;
            //ball.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            //SendToFront(psSmoke);

            CreateBulletMan(psSmoke.Position);            
            
            Vector2 aux2 = psSmoke.Position - s_Canyon.Position;
            aux2.Y *= -1;
            aux2.Normalize();
            aux2 *= pbPower.Value/2;
            sHead.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2*4);
            sBody.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            sLeftArm.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            sRightArm.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            sLeftLeg.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            sRightLeg.PhysicBody.ApplyForce(ActionType.IMPULSE, aux2);
            SendToFront(psSmoke);

            //Rest power bar
            pbPower.Value = 0;

            //Rumble
            Preferences.App.Vibrate(500);
        }

        /// <summary>
        /// Pressed canyon fire button
        /// </summary>
        /// <param name="source"></param>
        void s_Canyon_Pressed(Component source)
        {
            isCanyonPressed = true;
            
        }

        /// <summary>
        /// Reset released button
        /// </summary>
        /// <param name="source"></param>
        void breset_Released(Component source)
        {
            Reset();
        }

        #endregion

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isCanyonPressed)
            {
               timer += gameTime.ElapsedGameTime;
               if (timer > TimeSpan.FromMilliseconds(2f))
               {
                   timer = TimeSpan.Zero;
                   if(pbPower.Value < pbPower.MaxValue)
                        pbPower.Value++;
               }
            }
        }

        /// <summary>
        /// Pops this screen returning to the previous one, or exiting the app if there is no more left.
        /// This method is called when the hardware back button is pressed (only Windows Phone & Android)
        /// </summary>
        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        }

        /// <summary>
        /// Initialize game
        /// </summary>
        private void Reset()
        {
            foreach (Sprite ball in balls)
            {
                layer.RemoveSprite(ball);
                RemoveComponent(ball);
            }
            balls.Clear();

            foreach (Sprite box in clowns)
                RemoveComponent(box);
            clowns.Clear();

            CreateCastle(new Vector2(28, 310));
        }
    }
}
