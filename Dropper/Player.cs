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
//using PROG2370CollisionLibrary;
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

            this.spriteBatch = spriteBatch;
            this.content = content;
            this.b = b;
            player = new Rectangle(75, 363 - 48, picSize * scale, picSize * scale);
            spriteDirection = SpriteEffects.None;
            velocity = new Vector2(0, 0);
            playerAlive = true;
            playerFrames = new List<Rectangle>();
            endGameBox = new Rectangle(350, 250, 300, 120);


            #region oldplayerAnimationFrames
            /*
            //stand 5 frames
            playerFrames.Add(new Rectangle(8, 0, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(40, 0, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(72, 0, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(104, 0, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(136, 0, playerWidth, playerHeight));


            //Walk 9 frames
            playerFrames.Add(new Rectangle(39, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(71, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(103, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(135, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(166, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(199, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(231, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(263, 64, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(295, 64, playerWidth, playerHeight));

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
            playerFrames.Add(new Rectangle(10,  123, playerWidth, playerHeight));
            playerFrames.Add(new Rectangle(66,  113, playerWidth, playerHeight));
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
                    spriteBatch.Draw(spriteSheet_powerUp,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
                } else
                {
                    spriteBatch.Draw(spriteSheet,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
                }
            }
            else
            {

                spriteBatch.Draw(spriteSheet,
                        player, playerFrames.ElementAt<Rectangle>(currentFrame), Color.AliceBlue, 0f,
                        new Vector2(0), spriteDirection, 0f);
            }
         

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
            velocity.X = 0;      //motionless for constant speed

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;


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
                powerUpActive = false;
                powerupTimer = randomPowerUpTime.Next(60 * 5, 90 * 5);
            }



            //Starts new round
            if (!playerAlive)
            {
                keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    newGame = true;
                }
            }

            if (playerAlive == true && !TitleScreen.titleScreen)
            {
                //check if player is hitting floor, if so set velocity to 0
                if (player.Intersects(Background.floorBounds))
                {
                    velocity.Y = 0;
                    midJump = false;
                }
                else if (velocity.Y != 0)
                {
                    midJump = true;
                }

                

                //move right
                if (keyState.IsKeyDown(Keys.Right))
                {
                    velocity.X = +speed;

                }

                //move left
                if (keyState.IsKeyDown(Keys.Left))
                {
                    velocity.X = -speed;

                }

                /*
                if (keyState.IsKeyDown(Keys.LeftAlt) && !midJump)
                {
                    isSliding = true;
                } else
                {
                    isSliding = false;
                }
                */

                //jump
                if (keyState.IsKeyDown(Keys.Space) && midJump == false 
                    && isSliding == false)
                {
                    velocity.Y -= playerJump;
                    landInstance.Play();
                }



                Rectangle proposedPlayer = new Rectangle(player.X + (int)velocity.X,
                                                   player.Y + (int)velocity.Y,
                                                   player.Width,
                                                   player.Height);

                //Gravity application
                velocity.Y += gravityVect.Y * deltaTime;


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

                //Flip sprite
                if (velocity.X < 0)
                {
                    spriteDirection = SpriteEffects.FlipHorizontally;
                }

                else if (velocity.X > 0)
                {
                    spriteDirection = SpriteEffects.None;
                }



                #region Legacy Animation Frame Logic (Old Warrior Spritesheet)
                /*
                if ((velocity.X != 0 && currentFrame < 7))
                {
                    currentFrame = 7;
                }

                if (velocity.X == 0)
                {
                    currentFrame = 0;
                }

                if (currentFrame < 10)
                {

                    frameDelay++;

                    if (frameDelay > frameDelayCount)
                    {
                        frameDelay = 0;
                        currentFrame++;
                    }
                }

                else if (velocity.X != 0 && midJump == false)
                {

                    frameDelay++;

                    if (frameDelay > frameDelayCount)
                    {
                        frameDelay = 0;
                        currentFrame++;
                    }
                    if (currentFrame > 13)
                        currentFrame = 5;
                }
                */
                #endregion

                #region New Animation Frame Logic (Adventurer Sheet)


                if ((velocity.X != 0 && currentFrame < 4))
                {
                    currentFrame = 4;
                }

         

                 

                if (velocity.X != 0 && currentFrame >= 8 && !midJump && !isSliding)
                {
                    currentFrame = 4;
                }

                if (velocity.X == 0 && !midJump && !isSliding)
                {
                    if (currentFrame >= 3)
                    {
                        currentFrame = 0;
                    }
                }



                if (midJump)
                {
                    if (currentFrame < 13)
                    {
                        currentFrame = 13;
                    }

                   
                }

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

                if (currentFrame < 20 && currentFrame != 9)
                {

                    frameDelay++;

                    if (frameDelay > frameDelayCount)
                    {
                        frameDelay = 0;
                        currentFrame++;
                    }
                }

                
                
                #endregion

                //Update Player position values
                player.X = player.X + (int)velocity.X;
                player.Y = player.Y + (int)velocity.Y;

                base.Update(gameTime);
            }

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

