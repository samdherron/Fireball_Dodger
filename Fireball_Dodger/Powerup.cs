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
using Microsoft.Xna.Framework.Audio;

namespace Fireball_Dodger
{
    class Powerup : DrawableGameComponent
    {
        #region Variables
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private int v1;
        private int v2;
        Texture2D shieldPowerupImage;
        private Texture2D lavaPowerupImage;
        Random rndColor = new Random();
        Random rndSpawnX = new Random();
        const float gravity = 0.006f;
        public static float speed = 1.7f;
        Vector2 gravityVect = new Vector2(0, gravity);
        Random rndY = new Random();
        SpriteFont font;
        public static Rectangle powerupShape;
        public static bool powerupImageReady;
        public static Vector2 velocity;
        public static bool multiSpawn;
        int multiCounter = 0;
        Random rndSpawnCount = new Random();
        int spawnTimer;
        bool spawnPowerup = false;
        public static int powerupType;
        string powerupTypeString;
        Rectangle powerupFlare;
        Texture2D powerupFlareImage1;
        private int darkenTimer;
        private int darkenIncrement;
        private bool darkenImage;
        private Texture2D powerupFlareImage2;
        private Texture2D powerupFlareImage3;
        private Texture2D powerupFlareImage4;
        private SoundEffect lavaPowerupActivate;
        SoundEffectInstance lavaPowerupActivateInstance;
        #endregion

        public Powerup(Game game) : base(game)
        {

        }

        public Powerup(Game game, SpriteBatch spriteBatch, ContentManager content, int v1, int v2) : base(game)
        {
            //Setup initial variables and powerup rectangles
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.v1 = v1;
            this.v2 = v2;
            velocity.X = velocity.X += -speed;
            powerupShape = new Rectangle(900, rndY.Next(550, 600), 128 / 2, 128 / 2);
            powerupFlare = new Rectangle(Player.player.X, Player.player.Y, Player.player.Width, Player.player.Height);
            spawnTimer = 60 * rndY.Next(20, 30);
            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Draws powerup type and time left if active
            if (Player.powerUpActive && Player.playerAlive)
            {
                spriteBatch.DrawString(font, "Powerup Left: " + Player.powerupTimer / 60, new Vector2(450, 50), Color.Black);
                spriteBatch.DrawString(font, "Powerup Type: " + powerupTypeString, new Vector2(450, 120), Color.Black);

                //Draws lava flare
                if (powerupType == 1)
                {
                    darkenTimer++;

                    //Waits for darkenTimer to increment to 10 before switching to darker image and back to light
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


                    //Switches player powerup texture depending on increment value
                    switch (darkenIncrement)
                    {
                        case 0:
                            spriteBatch.Draw(powerupFlareImage1, powerupFlare, Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(powerupFlareImage2, powerupFlare, Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(powerupFlareImage3, powerupFlare, Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(powerupFlareImage4, powerupFlare, Color.White);
                            break;
                    }
                }
            }

            //Draws either the stoned or lava block for the powerup
            switch (powerupType)
            {
                case 0:
                    spriteBatch.Draw(shieldPowerupImage, powerupShape, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(lavaPowerupImage, powerupShape, Color.White);

                    break;

            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            powerupFlare.X = Player.player.X;
            powerupFlare.Y = Player.player.Y;
            velocity.X = 0;

            if (Player.playerAlive && !TitleScreen.titleScreen)
            {

                CheckCollision_Player();

                //Checks if it's time to spawn a new powerup
                if (spawnTimer == 0)
                {
                    spawnPowerup = true;
                    //Sets new random value to spawn timer.
                    spawnTimer = rndSpawnCount.Next(60 * 15, 90 * 15);
                    Console.WriteLine("Spawn timer value assigned: " + spawnTimer);
                }
                else
                {
                    spawnTimer--;
                }

                if (spawnPowerup)
                {
                    powerupShape.X = rndSpawnX.Next(100, 200);
                    powerupShape.Y = -10;
                    powerupType = rndY.Next(0, 2);
                    spawnPowerup = false;
                    powerupImageReady = true;

                    //Setting string value of enum for debugging label purposes
                    switch (powerupType)
                    {
                        case 0:
                            powerupTypeString = "Rock";
                            break;

                        case 1:
                            powerupTypeString = "Lava";
                            break;
                    }
                }


                //adds speed to the powerup so it falls
                if (powerupImageReady)
                {
                    velocity.Y = 0;
                    velocity.Y += speed;
                    powerupImageReady = false;
                }

                powerupShape.X = powerupShape.X + (int)velocity.X;
                powerupShape.Y = powerupShape.Y + (int)velocity.Y;

            }
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            lavaPowerupActivate = content.Load<SoundEffect>(@"Sound\lavaActivation");
            lavaPowerupActivateInstance = lavaPowerupActivate.CreateInstance();
            lavaPowerupActivateInstance.Volume = 0.25f;
            shieldPowerupImage = content.Load<Texture2D>(@"Powerups\shieldPowerup");
            lavaPowerupImage = content.Load<Texture2D>(@"Powerups\lavapowerup");
            powerupFlareImage1 = content.Load<Texture2D>(@"Powerups\lavaFlare");
            powerupFlareImage2 = content.Load<Texture2D>(@"Powerups\lavaFlare2");
            powerupFlareImage3 = content.Load<Texture2D>(@"Powerups\lavaFlare3");
            powerupFlareImage4 = content.Load<Texture2D>(@"Powerups\lavaFlare4");
            font = content.Load<SpriteFont>("font");
            base.LoadContent();
        }

        //Checks each frame to see if 
        public void CheckCollision_Player()
        {

            if (powerupShape.Intersects(Player.player))
            {
                //Moves powerup out of sight and calls activation methods
                powerupShape.Y = 1500;
                shieldPowerupActivate();
                powerupActivationSound(powerupType);
            }
        }

        //Sets bool in Player class to signal powerup is active
        public void shieldPowerupActivate()
        {
            Player.powerUpActive = true;
        }

        enum PowerupType
        {
            shield_Rock,
            _Lava
        }



        public void powerupActivationSound(int powerupType)
        {

            switch (powerupType)
            {
                case 1:
                    lavaPowerupActivateInstance.Play();
                    break;

                case 2:
                    lavaPowerupActivateInstance.Play();
                    break;
            }

        }

    }
}
