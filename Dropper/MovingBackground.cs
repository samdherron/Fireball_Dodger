using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using C3.XNA;

namespace Fireball_Dodger
{
    public class MovingBackground : DrawableGameComponent
    {
        const int YOFFSET = 0;              // we don't have scrolling on Y (yet)
        const int SCROLLSENSTIVITY = 1;
        const int HORIZSCROLLBORDER = 75;
        const int BGIMAGEWIDTH = 1000;
        const int BGIMAGEHEIGHT = 402;
        const int FARBGIMAGEHEIGHT = 402;

        public static Rectangle leftBounds;
        public static Rectangle rightBounds;
        public static Rectangle floorBounds;
        public static Rectangle playerPos;
        public static Vector2 playerVelocity;
        int screenWidth;
        int screenHeight;
        public static int upperXScrollBoundary;

        public static bool SendPlayerPos(Rectangle PlayerRectangle, Vector2 velocity)
        {
            bool returnValue = false;       //this means no shifting of bg instead of shifting player.

            playerPos = PlayerRectangle;
            playerVelocity = velocity;

            if (playerVelocity.X > 0 && currentXoffset != -upperXScrollBoundary)
                if (playerPos.Intersects(rightHorizontalBound))
                    returnValue = true;

            if (playerVelocity.X < 0 && currentXoffset != 0)
                if (playerPos.Intersects(leftHorizontalBound))
                    returnValue = true;

            return returnValue;
        }

        public static Rectangle leftHorizontalBound;
        public static Rectangle rightHorizontalBound;

        public static int currentXoffset;
        public static int previousXoffset;
        public static int farBackgroundCurrentXOffset;        //this is the scrolling offset for the far background
                                                //it will move slower than the stuff in the foreground
        Texture2D farBackground;
        Texture2D background;
        SpriteBatch spriteBatch;
        ContentManager content;


        public MovingBackground(Game game, SpriteBatch spriteBatch, ContentManager content, int screenWidth, int screenHeight) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            currentXoffset = 0;
            previousXoffset = 0;
            farBackgroundCurrentXOffset = 0;

            playerPos = new Rectangle(0, 0, 0, 0);      //will be overwritten by Player call
            playerVelocity = new Vector2(0);            //will be overwritten by Player call


            floorBounds = new Rectangle(0, 329, 1000, 200);
            /*
             * rightBounds = new Rectangle(screenWidth - (2 * HORIZSCROLLBORDER) - SCROLLSENSTIVITY, 5, 2 * SCROLLSENSTIVITY, screenHeight - 10);
            leftBounds = new Rectangle(HORIZSCROLLBORDER - SCROLLSENSTIVITY, 5, 2 * SCROLLSENSTIVITY, screenHeight - 10);
            
            
            leftHorizontalBound = new Rectangle(HORIZSCROLLBORDER - SCROLLSENSTIVITY, 5, 2 * SCROLLSENSTIVITY, screenHeight - 10);
            rightHorizontalBound = new Rectangle(screenWidth - (2 * HORIZSCROLLBORDER) - SCROLLSENSTIVITY, 5, 2 * SCROLLSENSTIVITY, screenHeight - 10);
            */
            upperXScrollBoundary = BGIMAGEWIDTH - screenWidth;

            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            /*spriteBatch.FillRectangle(leftBounds, Color.Red);
            spriteBatch.FillRectangle(rightBounds, Color.Blue);
            */
            spriteBatch.FillRectangle(floorBounds, Color.Green);

            spriteBatch.Draw(background, new Rectangle(currentXoffset, YOFFSET, BGIMAGEWIDTH, BGIMAGEHEIGHT), Color.White);
            /*spriteBatch.Draw(farBackground, new Rectangle(farBackgroundCurrentXOffset, 0, BGIMAGEWIDTH, FARBGIMAGEHEIGHT), Color.White);
            */
            //start debug
            //foreach (RigidBody r in rigidBodyList)
            //    spriteBatch.DrawRectangle(r.Rect, Color.Yellow);

            //spriteBatch.DrawRectangle(playerPos, Color.Yellow);

            //spriteBatch.DrawRectangle(leftHorizontalBound, Color.Yellow);
            //spriteBatch.DrawRectangle(rightHorizontalBound, Color.Yellow);
            //end debug
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (playerVelocity.X > 0 && currentXoffset != -upperXScrollBoundary)
                if (playerPos.Intersects(rightHorizontalBound))
                {
                    currentXoffset -= (int)playerVelocity.X;
                    farBackgroundCurrentXOffset--;              //reduce by 1.  as long as velocity (above) is greater
                }                                               //than 1, the parallelax effect will hold true.

            if (playerVelocity.X < 0 && currentXoffset != 0)
                if (playerPos.Intersects(leftHorizontalBound))
                {
                    currentXoffset -= (int)playerVelocity.X;
                    farBackgroundCurrentXOffset++;          //increase by 1.  as long as velocity (above) is greater
                }                                           //than 1, the parallelax effect will hold true.

            /* int rigidBodyOffset = previousXoffset - currentXoffset;     // this normalizes to ofset shifts
            foreach (RigidBody r in rigidBodyList.ToList())      //i am taking an "eagerly evaluated" copy of the list
            {                                                   //to function as my working list because one is *not*
                rigidBodyList.Remove(r);                        //allowed to change a list item while iterating through it.
                rigidBodyList.Add(new RigidBody(r.Type, new Rectangle(r.Rect.X - rigidBodyOffset, r.Rect.Y, r.Rect.Width, r.Rect.Height)));
            }
            */

            previousXoffset = currentXoffset;
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {

            background = content.Load<Texture2D>("sky");
            /*
            rigidBodyList.Add(new RigidBody("leftBoundary", new Rectangle(0, 0, 1, screenHeight)));
            rigidBodyList.Add(new RigidBody("rightBoundary", new Rectangle(1400 - 1, 0, 1, screenHeight)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(0, 360, 917, 42)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(120, 320, 80, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(160, 280, 40, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(520, 320, 120, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(560, 280, 40, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(720, 280, 80, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(724, 320, 72, 40)));
            rigidBodyList.Add(new RigidBody("background", new Rectangle(1000, 360, 402, 40)));
            */

            base.LoadContent();
        }
    }
}
