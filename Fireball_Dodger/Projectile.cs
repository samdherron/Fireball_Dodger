using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fireball_Dodger
{
    class Projectile : DrawableGameComponent
    {
        #region Variables
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private int v1;
        private int v2;
        Texture2D projectileImage1;
        private Texture2D projectileEnergy;
        Random rndColor = new Random();
        Random rndSpawnX = new Random();
        const float gravity = 0.06f;
        public static float speed = 7.7f;
        Vector2 gravityVect = new Vector2(0, gravity);
        Random rndY = new Random();
        SpriteFont font;
        int bulletWaitFireCounter = 5;
        bool bulletInFlight;
        bool energyProjectile = false;
        Random energyRoll = new Random();
        public static Rectangle projectileShape;
        public static bool projectileImageReady;
        public static Vector2 velocity;
        public static int projectilesFired; //score count
        public static bool multiSpawn;
        int multiCounter = 0;
        SoundEffect projectHit;
        private SoundEffect projectHitPowerup;
        SoundEffectInstance projectileHitInstance;
        private SoundEffect energyHitPowerup;
        SoundEffectInstance energyHitInstance;
        private SoundEffect energy2HitPowerup;
        SoundEffectInstance energy2HitInstance;
        Rectangle powerupFlare;
        SoundEffectInstance projectHitPowerupInstance;
        private int frameTimer;
        private Texture2D projectileImage2;
        private Texture2D projectileImage3;
        private Texture2D projectileImage4;
        private Texture2D projectileImage5;
        private bool changeImage;
        private int frameIncrement;
        #endregion

        public Projectile(Game game) : base(game)
        {

        }

        public Projectile(Game game, SpriteBatch spriteBatch, ContentManager content, int v1, int v2) : base(game)
        {
            //Setup initial variables and projectile rectangle
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.v1 = v1;
            this.v2 = v2;
            velocity.X = velocity.X += -speed;
            projectileShape = new Rectangle(900, rndY.Next(175, 340), 512 / 2, 197 / 2);
            projectilesFired = 0;
            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {


            spriteBatch.Begin();

            if (Player.playerAlive)
            {
                //Draws coordinates, projectiles fired (score) and the projectile itself
                spriteBatch.DrawString(font, "Projectile Coordinates... X: " + projectileShape.X + " Y: " +
                projectileShape.Y, new Vector2(360, 70), Color.Black);
                spriteBatch.Draw(projectileImage1, projectileShape, Color.White);
                spriteBatch.DrawString(font, "Score: " + projectilesFired, new Vector2(450, 30), Color.Black);

                //Energy ball logic
                if (energyRoll.Next(0, 25) >= 20)
                {
                    energyProjectile = EnergyRandomRoll(energyRoll);
                }

                if (energyProjectile)
                {
                    spriteBatch.Draw(projectileEnergy, projectileShape, Color.White);
                }

                frameTimer++;

                //Waits for darkenTimer to increment to 3 before switch to darker image
                if (frameTimer >= 10)
                {
                    frameTimer = 0;

                    if (frameIncrement == 0 || changeImage == true)
                    {
                        frameIncrement++;
                        changeImage = true;
                    }

                    if (frameIncrement == 4 || changeImage == false)
                    {
                        changeImage = false;
                        frameIncrement--;
                    }

                }


                //Switches flame demon texture depending on increment value
                switch (frameIncrement)
                {
                    case 0:
                        spriteBatch.Draw(projectileImage1, projectileShape, Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(projectileImage2, projectileShape, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(projectileImage3, projectileShape, Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(projectileImage4, projectileShape, Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(projectileImage5, projectileShape, Color.White);
                        break;
                }
            }



        



            spriteBatch.End();

        }


        public override void Update(GameTime gameTime)
        {
            velocity.X = 0;
            velocity.Y = 0;


            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            /*
            //Apply Gravity to projectile randomly
            if (rndY.Next(1, 10) < 5 && projectileShape.X < 800)
            {
                velocity.Y += gravityVect.Y * deltaTime;
                Console.WriteLine("Gravity down... velocity Y is " + velocity.Y);
            }
                */



            if (Player.playerAlive && !TitleScreen.titleScreen)
            {

                if (projectileShape.Intersects(Player.player) && !Player.powerUpActive)
                {
                    //If projectile hits player, trigger death and play death sound
                    sendPlayerDeath();
                    projectileHitInstance.Play();

                }
                else if (projectileShape.Intersects(Player.player) && Player.powerUpActive)
                {
                    if (energyProjectile)
                    {
                        energyHitInstance.Play();
                    }
                    else
                    {
                        projectHitPowerupInstance.Play();
                    }
                }




                if (projectileShape.X < rndSpawnX.Next(-200, -50))
                {
                    //Any spawn after the first will use these values
                    velocity.X = 0;
                    projectileShape.X = 900;
                    projectileShape.Y = rndY.Next(100, 300);
                    Console.WriteLine("new random Y projectile value: " + projectileShape.Y);


                    //Checks if lava powerup is active.
                    //If positive, increment by 2. If negative, normal increment of 1.
                    if (Player.powerUpActive && Powerup.powerupType == 1)
                    {
                        projectilesFired += 2;
                    }
                    else
                    {
                        projectilesFired++;
                    }

                    //See EnergyRandomRoll method
                    if (energyProjectile)
                    {
                        projectilesFired--; //subtracts the incremented amount from the fireball
                        projectilesFired += 3; //adds 3 points instead of 1 as a bonus
                        energyProjectile = false;
                    }

                    //Gets either div2 or div3 for the projectile size
                    if (projectileSizeRandomizer(rndColor) == "div2")
                    {
                        projectileShape.Width = (512 / 2);
                        projectileShape.Height = (197 / 2);
                    }
                    else
                    {
                        projectileShape.Width = (512 / 3);
                        projectileShape.Height = (197 / 3);
                    }

                    

                }
  

                if (velocity.X == 0)
                {
                    bulletInFlight = false;
                }
                else
                {
                    bulletInFlight = true;
                }

                if (!bulletInFlight)
                {
                    projectileImageReady = true;
                    Console.WriteLine("bullet ready to fire");
                }

                if (projectileImageReady)
                {
                    Console.WriteLine("velocity addition hit");
                    velocity.X -= speed;
                    projectileImageReady = false;
                    bulletWaitFireCounter = 0;
                }

                projectileShape.X = projectileShape.X + (int)velocity.X;

                if (projectileShape.Intersects(Player.player) && Player.powerUpActive)
                {
                    if (rndColor.Next(0, 1) == 0)
                    {
                        velocity.Y += (gravityVect.Y * deltaTime * 15);
                    }
                    else
                    {
                        velocity.Y -= (gravityVect.Y * deltaTime * 15);
                    }
                    projectileShape.Y = projectileShape.Y + (int)velocity.Y;
                }
                else
                {

                    projectileShape.Y = projectileShape.Y + (int)velocity.Y;
                }

                bulletWaitFireCounter++;

                projectileShape.X -= rndSpawnX.Next(0, 15);
            }
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {            
            projectileImage1 = content.Load<Texture2D>(@"Fire_V3\fire_v3_1");
            projectileImage2 = content.Load<Texture2D>(@"Fire_V3\fire_v3_2");
            projectileImage3 = content.Load<Texture2D>(@"Fire_V3\fire_v3_3");
            projectileImage4 = content.Load<Texture2D>(@"Fire_V3\fire_v3_4");
            projectileImage5 = content.Load<Texture2D>(@"Fire_V3\fire_v3_5");   
            projectileEnergy = content.Load<Texture2D>("energy");
            font = content.Load<SpriteFont>("font");
            projectHit = content.Load<SoundEffect>(@"Sound\deathNoise");
            projectHitPowerup = content.Load<SoundEffect>(@"Sound\rockPowerupHitSound");
            energyHitPowerup = content.Load<SoundEffect>(@"Sound\energyWave_bitCrush");
            base.LoadContent();

            //Setting up death sound effect instance
            projectileHitInstance = projectHit.CreateInstance();
            projectileHitInstance.Volume = 0.05f;
            projectHitPowerupInstance = projectHitPowerup.CreateInstance();
            projectHitPowerupInstance.Volume = 0.25f;
            projectHitPowerupInstance.Pitch = 0.3f;
            energyHitInstance = energyHitPowerup.CreateInstance();
            energyHitInstance.Volume = 0.40f;
        }



        /// <summary>
        /// Sets playerAlive bool to false, which triggers game over box to draw
        /// </summary>
        public void sendPlayerDeath()
        {
            Player.playerAlive = false;
        }

        /// <summary>
        /// Rolls a 1/25 chance to see if an energy ball is used.
        /// Energy balls give +3 points instead of +1 and are dark blue
        /// </summary>
        /// <param name="energyRoll"></param>
        private bool EnergyRandomRoll(Random energyRoll)
        {
            bool result;
            int energyNumber = energyRoll.Next(1, 5);

            if (energyNumber == 5)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private string projectileSizeRandomizer(Random rnd)
        {
            string result;

            if (rnd.Next(0, 2) == 0)
            {
                result = "div2";
            } else
            {
                result = "div3";
            }

            return result;
        }


    }
}
