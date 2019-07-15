﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fireball_Dodger
{
    class Projectile : DrawableGameComponent
    {
        #region Variables
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private int v1;
        private int v2;
        Texture2D projectileImage;
        private Texture2D projectileEnergy;
        Random rndColor = new Random();
        Random rndSpawnX = new Random();
        const float gravity = 0.06f;
        public static float speed = 9.7f;
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
                spriteBatch.Draw(projectileImage, projectileShape, Color.White);
                spriteBatch.DrawString(font, "Score: " + projectilesFired, new Vector2(450, 30), Color.Black);

                //Energy ball logic
                if (energyRoll.Next(0, 150) > 147)
                {
                    energyProjectile = EnergyRandomRoll(energyRoll);
                }

                if (energyProjectile)
                {
                    spriteBatch.Draw(projectileEnergy, projectileShape, Color.White);
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
                        energy2HitInstance.Play();
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
                    projectilesFired++;

                    //See EnergyRandomRoll method
                    if (energyProjectile)
                    {
                        projectilesFired--; //subtracts the incremented amount from the fireball
                        projectilesFired += 3; //adds 3 points instead of 1 as a bonus
                        energyProjectile = false;
                    }

                }

                multiCounter++;
                Console.WriteLine("multiCounter: " + multiCounter);

                multiSpawn = multiSpawner(projectileShape, rndSpawnX);

                if (multiSpawn && multiCounter > 30)
                {
                    Console.WriteLine("projectile Spawned");
                    Projectile p1 = new Projectile(Game);
                    multiCounter = 0;
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
                projectileShape.Y = projectileShape.Y + (int)velocity.Y;
                bulletWaitFireCounter++;

                projectileShape.X -= rndSpawnX.Next(0, 15);
            }
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            projectileImage = content.Load<Texture2D>("fire");
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
        /// Spawning multipe projectiles at once
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool multiSpawner(Rectangle projectile, Random r)
        {
            bool spawnNewProjectile = false;
            if (projectile.X < r.Next(100, 300))
            {
                spawnNewProjectile = true;
            }

            return spawnNewProjectile;
        }

        /// <summary>
        /// Rolls a 1/25 chance to see if an energy ball is used.
        /// Energy balls give +3 points instead of +1 and are dark blue
        /// </summary>
        /// <param name="energyRoll"></param>
        private bool EnergyRandomRoll(Random energyRoll)
        {
            bool result;
            int energyNumber = energyRoll.Next(1, 25);

            if (energyNumber == 17)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }



    }
}
