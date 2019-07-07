using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace Fireball_Dodger
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TitleScreen : DrawableGameComponent
    {
        #region Variables
        const int SCREENWIDTH = 1000;
        const int SCREENHEIGHT = 500;
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private int v1;
        private int v2;
        int screenWidth;
        int screenHeight;
        SpriteFont font;
        Rectangle speakerShape;
        Texture2D speakerImage;
        GraphicsDeviceManager graphics;
        Song backgroundSoundTrack;
        public static bool titleScreen;
        Texture2D titleScreenBG;
        private Texture2D flameDemonTitle;
        private Texture2D flameDemonTitle_dark1;
        private Texture2D flameDemonTitle_dark2;
        private Texture2D flameDemonTitle_dark3;
        private Texture2D characterImage;
        Rectangle flameDemonShape;
        private Rectangle characterShape;
        Rectangle titleScreenShape;
        private int darkenTimer;
        private int darkenIncrement;
        private bool darkenImage = false;
        private Texture2D flameDemonOriginal;
        List<Rectangle> characterFrames;
        #endregion

        public TitleScreen(Game game) : base(game)
        {

        }

        public TitleScreen(Game game, SpriteBatch spriteBatch, ContentManager content, int v1, int v2) : base(game)
        {
            //Setup initial variables and title screen shapes
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.v1 = v1;
            this.v2 = v2;
            titleScreen = true;
            titleScreenShape = new Rectangle(0, 0, 1000, 1000);
            flameDemonShape = new Rectangle(470, 260, 128, 128);
            characterShape = new Rectangle(350, 310, 80, 80);
            characterFrames = new List<Rectangle>();
            speakerShape = new Rectangle(200, 30, 128 / 2, 128 / 2);
            LoadContent();

             
            MediaPlayer.Volume = 0.05f;
            MediaPlayer.Play(backgroundSoundTrack);
            characterFrames.Add(new Rectangle(150, 265, 36, 32));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Load textures and music
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleScreenBG = content.Load<Texture2D>(@"Background\titleBG");
            flameDemonTitle = content.Load<Texture2D>(@"FlameDemon\flameDemonTitle");
            flameDemonOriginal = content.Load<Texture2D>(@"FlameDemon\flameDemon_Original");
            flameDemonTitle_dark1 = content.Load<Texture2D>(@"FlameDemon\flameDemonTitle_dark1");
            flameDemonTitle_dark2 = content.Load<Texture2D>(@"FlameDemon\flameDemonTitle_dark2");
            flameDemonTitle_dark3 = content.Load<Texture2D>(@"FlameDemon\flameDemonTitle_dark3");
            characterImage = content.Load<Texture2D>(@"Player\adventurerSheet");
            font = content.Load<SpriteFont>("font");
            backgroundSoundTrack = content.Load<Song>(@"Sound\soundtrack_title");
            //speakerImage = content.Load<Texture2D>("speaker_on");
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();

            //Enters into game
            if (keyState.IsKeyDown(Keys.Enter))
            {
                titleScreen = false;
                Console.WriteLine("title screen set false");
            }


            //Returns to title screen
            if (keyState.IsKeyDown(Keys.Escape))
            {
                titleScreen = true;
                Console.WriteLine("title screen set true");
            }




            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(backgroundSoundTrack);
            }

            //Turns on and off music
            if (keyState.IsKeyDown(Keys.M))
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                else
                {
                    MediaPlayer.Play(backgroundSoundTrack);
                }
            }


            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (titleScreen)
            {
                //Draws title screen bg and text
                spriteBatch.Draw(titleScreenBG, titleScreenShape, Color.White);
                //spriteBatch.Draw(flameDemonTitle, flameDemonShape, Color.White);
                spriteBatch.DrawString(font, "Press enter to play", new Vector2(410, 420), Color.White);
                spriteBatch.Draw(characterImage,
                        characterShape, characterFrames.ElementAt<Rectangle>(0), Color.AliceBlue, 0f,
                        new Vector2(0), SpriteEffects.None, 0f);

                darkenTimer++;

                //Waits for darkenTimer to increment to 3 before switch to darker image
                if (darkenTimer >= 10)
                {
                    darkenTimer = 0;

                    if (darkenIncrement == 0 || darkenImage == true)
                    {
                        darkenIncrement++;
                        darkenImage = true;
                    }

                    if (darkenIncrement == 4 || darkenImage == false)
                    {
                        darkenImage = false;
                        darkenIncrement--;
                    }

                }


                //Switches flame demon texture depending on increment value
                switch (darkenIncrement)
                {
                    case 0:
                        spriteBatch.Draw(flameDemonTitle, flameDemonShape, Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(flameDemonOriginal, flameDemonShape, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(flameDemonTitle_dark1, flameDemonShape, Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(flameDemonTitle_dark2, flameDemonShape, Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(flameDemonTitle_dark3, flameDemonShape, Color.White);
                        break;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
