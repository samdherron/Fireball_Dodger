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
    public class Player : DrawableGameComponent
    {

        #region Variables

        SpriteBatch spriteBatch;
        ContentManager content;
        Background b;
        private SpriteFont font;
        private Texture2D spriteSheet_death;
        private Texture2D sprite;
        private Vector2 gravityVect = new Vector2(0, gravity);
        private Texture2D spriteSheet;
        private Texture2D spriteSheet_powerUp;
        private Texture2D spriteSheet_powerUpLava;
        private Rectangle endGameBox;
        int playerHeight = 31;
        int playerWidth = 22;
        int i = 0;
        int tempX;
        int tempNX;
        const int firstFrame = 0;
        const int walkCount = 4;
        const int runCount = 9;
        const int deathCount = 10;
        const int attackCount = 10;
        const int totalCount = 33;
        int frameDelayCount = 8;
        const float speed = 8.7f;
        const float gravity = 0.02f;
        float playerJump = 13.5f;
        bool isAttacking;
        bool midJump;
        double health = 100;
        string gameWinner;
        public static Vector2 velocity;
        public static int playerScore = 0;
        public static bool playerAlive;
        public static double playerHealth = 100;
        public static Rectangle player;
        public static SpriteEffects spriteDirection;
        bool isSliding = false;
        int slidingHeight = 17;
        int slidingWidth = 36;
        /*
        public static int picSize = 32;
        public static int scale = 2;
        */
        public static int picSize = 48;
        public static int scale = 2;
        public static int frameDelay = 0;
        public static bool newGame = false;
        public static bool endGame;
        public static bool deathAnimation;
        public static int currentFrame = firstFrame;
        public static bool finalAnimation = false;
        List<Rectangle> playerFrames;
        public static string captureTime;
        public static bool powerUpActive = false;
        public static int powerupTimer = 60 * 15;
        Rectangle backgroundBlur = new Rectangle(0, 0, 1000, 500);
        Random randomPowerUpTime = new Random();
        SoundEffect landSound;
        SoundEffectInstance landInstance;


        #endregion

        public Player(Game game) : base(game)
        {

        }

        public Player(Game game, SpriteBatch spriteBatch, ContentManager content, Background b) : base(game)
        {
            //Setup initial variables
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.b = b;
            player = new Rectangle(75, 363 - 48, picSize * scale, picSize * scale);
            spriteDirection = SpriteEffects.None;
            velocity = new Vector2(0, 0);
            playerAlive = true;
            playerFrames = new List<Rectangle>();
            endGameBox = new Rectangle(350, 250, 300, 120);

            #region newplayerAnimationFrames
            //stand frames (0-3)
            playerFrames.Add(new Rectangle(13, 7, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(64, 6, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(114, 7, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(164, 7, playerWidth, playerHeight));
            /*
            playerFrames.Add(new Rectangle(136, 0, playerWidth, playerHeight));
            */


            //Walk frames (4-8)
            playerFrames.Add(new Rectangle(66, 44, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(115, 44, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(165, 44, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(217, 44, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(265, 44, playerWidth, playerHeight));


            //Slide Frames (Updated with new sprite sliding) (9-12)
            playerFrames.Add(new Rectangle(154, 132, slidingWidth, slidingHeight));
            playerFrames.Add(new Rectangle(204, 132, slidingWidth, slidingHeight));
            playerFrames.Add(new Rectangle(254, 132, slidingWidth, slidingHeight));
            playerFrames.Add(new Rectangle(308, 130, slidingWidth, slidingHeight));

            //Jump Frames (13-20)
            //playerFrames.Add(new Rectangle(64,  87, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(116, 81, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(163, 79, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(215, 80, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(263, 82, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(317, 82, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(10, 123, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(66, 113, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(117, 113, playerWidth, playerHeight));





            /*
            //attack 10 frames
            playerFrames.Add(new Rectangle(6, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(38, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(67, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(99, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(130, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(163, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(196, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(230, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(260, 256, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(293, 256, playerWidth, playerHeight));


            //death 10 frames
            playerFrames.Add(new Rectangle(6, 288, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(38, 288, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(67, 288, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(100, 288, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(134, 291, 21, 31));
            playerFrames.Add(new Rectangle(166, 293, 25, 30));
            playerFrames.Add(new Rectangle(197, 296, 22, 27));
            playerFrames.Add(new Rectangle(228, 298, 27, 22));
            playerFrames.Add(new Rectangle(260, 305, 28, 16));
            playerFrames.Add(new Rectangle(292, 312, 28, 9));
            */

            #endregion

            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();


            //Determine whether to display Coloured Sprite or Normal Sprite
            if (powerUpActive)
            {

                if (Powerup.powerupType == 0)
                {
                    //Draw rock powerup player sprite
                    spriteBatch.Draw(spriteSheet_powerUp,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
                }
                else

                {
                    //Draw lava lens flare
                    spriteBatch.Draw(spriteSheet,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
                }
            }
            else
            {
                //Draw normal player sprite
                spriteBatch.Draw(spriteSheet,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
            }

            //spriteBatch.FillRectangle(player, Color.Blue);
            spriteBatch.End();


            //Draw Gameover Box with Text
            if (!playerAlive && !newGame && !TitleScreen.titleScreen && !powerUpActive)
            {
                spriteBatch.Begin();
                spriteBatch.DrawRectangle(endGameBox, Color.Black);
                spriteBatch.FillRectangle(endGameBox, Color.Black);
                spriteBatch.DrawString(font, "Game over! Your score was: " + Projectile.projectilesFired, new Vector2(400, 270), Color.White);
                spriteBatch.DrawString(font, "Press enter to start a new game.", new Vector2(400, 300), Color.White);
                spriteBatch.Draw(spriteSheet_death,
                    player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                    new Vector2(0), spriteDirection, 0f);
                spriteBatch.End();
            }

            else
            {

            }

            base.Draw(gameTime);
        }


        public override void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();

            float deltaTime = FrameReset(gameTime);

            newGame = DeathHandler(keyState);

            if (playerAlive == true && !TitleScreen.titleScreen)
            {
                //Checks bounds & player position to prevent walking or jumping offscreen
                Check_BoundsIntersection();

                PowerupHandler();

                MovementHandler(keyState);

                midJump = Check_PlayerJumping();

                velocity.Y = JumpHandler(keyState);


                Rectangle proposedPlayer = new Rectangle(player.X + (int)velocity.X,
                                                   player.Y + (int)velocity.Y,
                                                   player.Width,
                                                   player.Height);

                //Gravity application
                velocity.Y += gravityVect.Y * deltaTime;

                //Flip sprite direction based on movement
                SpriteDirectionHandler();

                //Orchestrates player animation & changing frames based on movement
                AnimationHandler();

                finalAnimation = Check_FinalFrame();

                frameDelay = Check_FrameDelay();

                if (frameDelay == 0 && !finalAnimation)
                {
                    currentFrame++;
                }


                /*
                if (isSliding)
                {
                    if (currentFrame < 9 || currentFrame > 12)
                    {
                        currentFrame = 9;
                    }
                    if (currentFrame == 12)
                    {
                        currentFrame = 9;
                    }
                    Console.WriteLine("Slide actiated");

                }
                */


                //Update Player position values
                player.X = player.X + (int)velocity.X;
                player.Y = player.Y + (int)velocity.Y;

                base.Update(gameTime);
            }

        }

        private float JumpHandler(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Space) && midJump == false
                   && isSliding == false)
            {
                //Modify velocity
                velocity.Y -= playerJump;
                landInstance.Play();
            }

            return velocity.Y;
        }

        private int AnimationHandler()
        {
            //Switch to running frame
            if ((velocity.X != 0 && currentFrame < 4))
            {
                currentFrame = 4;
            }

            //Cycle Back to initial running frame
            if (velocity.X != 0 && currentFrame >= 8 && !midJump && !isSliding)
            {
                currentFrame = 4;
            }

            //Standing still frames
            if (velocity.X == 0 && !midJump && !isSliding)
            {

                if (currentFrame >= 3)
                {
                    currentFrame = 0;
                }
            }

            //Sets frames for jumping.
            if (midJump)
            {

                if (currentFrame < 13)
                {
                    currentFrame = 13;
                }

            }

            /*
            * Sliding will be added soon.
           if (keyState.IsKeyDown(Keys.LeftAlt) && !midJump)
           {
               isSliding = true;
           } else
           {
               isSliding = false;
           }
           */

            return currentFrame;
        }

        private bool Check_FinalFrame()
        {
            bool finalFrame = false;

            if (currentFrame == 20)
            {
                finalFrame = true;
            }

            return finalFrame;
        }

        private int Check_FrameDelay()
        {

            //Checks if the final animation frame has been reached.
            if (currentFrame < 20 && currentFrame != 9)
            {
                frameDelay++;

                if (frameDelay > frameDelayCount)
                {
                    frameDelay = 0;
                }
            }

            return frameDelay;
        }

        /*Changes the direction of the player sprite
          based on walking left & right.    */
        private void SpriteDirectionHandler()
        {

            if (velocity.X < 0)
            {
                spriteDirection = SpriteEffects.FlipHorizontally;
            }

            else if (velocity.X > 0)
            {
                spriteDirection = SpriteEffects.None;
            }
        }

        private void Check_BoundsIntersection()
        {
            //Stopping at left and right sides of screen
            if (player.Intersects(Background.leftBounds))
            {
                velocity.X = 0;
                player.X += 1;
            }

            if (player.Intersects(Background.rightBounds))
            {
                velocity.X = 0;
                player.X -= 5;
            }

        }

        private bool Check_PlayerJumping()
        {
            //Check if player is hitting floor, if so set velocity to 0
            if (player.Intersects(Background.floorBounds))
            {
                velocity.Y = 0;
                midJump = false;
                Console.WriteLine("Not jumping");
            }
            else if (velocity.Y != 0)
            {
                midJump = true;
                Console.WriteLine("Jumping");

                ShrinkPlayer_JumpingSize();
            }

            return midJump;
        }

        //Calculates a new height based on which jumping frame is active.
        private void ShrinkPlayer_JumpingSize()
        {
            if (currentFrame > 14 && currentFrame < 19)
            {
                player.Height = (picSize * scale) - 32;
            }

        }

        private void PowerupHandler()
        {
            if (powerupTimer >= 0 && powerUpActive)
            {
                switch (Powerup.powerupType)
                {
                    case 0:
                        //Overrides death function when powerup active
                        playerAlive = true;
                        powerupTimer--;
                        Console.WriteLine("Powerup effect stops death");
                        break;

                    case 1:
                        //Overrides death function when powerup active
                        playerAlive = true;
                        powerupTimer--;
                        Console.WriteLine("Powerup effect stops death");
                        break;
                }
            }
            else
            {
                //Turns off powerup and sets new random duration for next powerup
                powerUpActive = false;
                powerupTimer = randomPowerUpTime.Next(60 * 5, 90 * 5);
            }
        }

        private void MovementHandler(KeyboardState keyState)
        {

            //Move right
            if (keyState.IsKeyDown(Keys.Right))
            {
                velocity.X = +speed;

            }

            //Move left
            if (keyState.IsKeyDown(Keys.Left))
            {
                velocity.X = -speed;

            }

         
        }

        public bool DeathHandler(KeyboardState keyState)
        {
            bool newGame = false;

            //Starts new round if player has died
            if (!playerAlive)
            {

                if (keyState.IsKeyDown(Keys.Enter))
                {
                    newGame = true;
                }
            }

            return newGame;
        }

        //Sets up variables for each frame.
        private float FrameReset(GameTime gameTime)
        {

            //Reset to be motionless for constant speed
            velocity.X = 0;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            player.Height = GetCurrent_PlayerHeight();

            return deltaTime;
        }

        //Player height will change based on the height of the frame.
        //Important for jumping.
        private int GetCurrent_PlayerHeight()
        {
            int newHeight = picSize * scale;

            return newHeight;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //sprite = content.Load<Texture2D>("warrior1");
            spriteSheet = content.Load<Texture2D>(@"Player\adventurerSheet");
            //spriteSheet = content.Load<Texture2D>("warriorsprites");
            spriteSheet_powerUp = content.Load<Texture2D>(@"Player\adventurerSheet_powerup2");
            //spriteSheet_powerUpLava = content.Load<Texture2D>("lava_Shield");
            font = content.Load<SpriteFont>("font");
            spriteSheet_death = content.Load<Texture2D>(@"Player\adventurerSheet_Death");
            landSound = content.Load<SoundEffect>(@"Sound\landNoise");

            landInstance = landSound.CreateInstance();
            landInstance.Volume = 0.05f;

            //for healthBar if wanted
            /*
            p1HealthBar = content.Load<Texture2D>("healthbar");
            */
        }



    }

}

