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
    public class Background : DrawableGameComponent
    {
        #region Variables
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private int v1;
        private int v2;
        public static Rectangle leftBounds;
        public static Rectangle rightBounds;
        public static Rectangle floorBounds;
        public static Rectangle screenMover;
        public static Rectangle menuBackground;
        public static Rectangle gameBackground;
        public static Rectangle altBackground;
        public static Texture2D altMenuImage;
        public static int i = 0;
        Texture2D gameBG;
        private Texture2D gameBG_darken1;
        private Texture2D gameBG_darken2;
        private Texture2D gameBG_darken3;
        private Texture2D gameBG_darken4;
        int darkenIncrement = 0;
        int darkenTimer = 0;
        Random rndColor = new Random();
        #endregion

        public Background(Game game) : base(game)
        {

        }

        public Background(Game game, SpriteBatch spriteBatch, ContentManager content, int v1, int v2) : base(game)
        {
            //Setup initial variables, bounds and backgrounds
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.v1 = v1;
            this.v2 = v2;
            floorBounds = new Rectangle(0, 412, 1000, 0);
            rightBounds = new Rectangle(998, 100, 1, 5000);
            leftBounds = new Rectangle(1, 100, 1, 5000);
            screenMover = new Rectangle(750, 329, 1, 1);
            menuBackground = new Rectangle(0, 0, 1000, 500);
            altBackground = new Rectangle(0, 0, 1000, 500);
            gameBackground = new Rectangle(0, 0, 1000, 500);
            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.FillRectangle(leftBounds, Color.Red);
            spriteBatch.FillRectangle(rightBounds, Color.Blue);
            spriteBatch.FillRectangle(floorBounds, Color.White);
            spriteBatch.Draw(gameBG, gameBackground, Color.White);

            // Gradient darkening of background after player death
            if (!Player.playerAlive)
            {
                darkenTimer++;

                //Waits for darkenTimer to increment to 3 before switch to darker image
                if (darkenTimer >= 3)
                {
                    darkenTimer = 0;

                    //To stop it from incrementing past the number of images
                    if (darkenIncrement != 4)
                    {
                        darkenIncrement++;
                    }
                }
                //Draws sprite corresponding with increment value
                switch (darkenIncrement)
                {
                    case 0:
                        break;
                    case 1:
                        spriteBatch.Draw(gameBG_darken1, gameBackground, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(gameBG_darken2, gameBackground, Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(gameBG_darken3, gameBackground, Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(gameBG_darken4, gameBackground, Color.White);
                        break;

                }
            }

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            gameBG = content.Load<Texture2D>(@"Background\FlatNatureArt");
            gameBG_darken1 = content.Load<Texture2D>(@"Background\FlatNatureArt_darken1");
            gameBG_darken2 = content.Load<Texture2D>(@"Background\FlatNatureArt_darken2");
            gameBG_darken3 = content.Load<Texture2D>(@"Background\FlatNatureArt_darken3");
            gameBG_darken4 = content.Load<Texture2D>(@"Background\FlatNatureArt_darken4");
            base.LoadContent();
        }
    }
}
