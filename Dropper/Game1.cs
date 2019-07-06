using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
//using C3.XNA;
using System.Media;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;

namespace Fireball_Dodger
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int SCREENWIDTH = 1000;
        const int SCREENHEIGHT = 500;
        Random rndNum = new Random();
        int spawnTimer = 0;
        List<Projectile> projectileList = new List<Projectile>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Song backgroundSoundTrack;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);




            Background b = new Background(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
            Components.Add(b);


            /*
            MovingBackground mb = new MovingBackground(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
            Components.Add(mb);
            */

            Player p = new Player(this, spriteBatch, Content, b);
            Components.Add(p);

            Projectile proj1 = new Projectile(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
            Components.Add(proj1);
            projectileList.Add(proj1);
            

            TitleScreen t = new TitleScreen(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
            Components.Add(t);

            Powerup powerUp = new Powerup(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
            Components.Add(powerUp);



        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (Player.newGame)
            {
                //set up new game variables
                Player.player = new Rectangle(75, 330 - 64, Player.picSize * Player.scale, Player.picSize * Player.scale);
                Player.spriteDirection = SpriteEffects.None;
                Player.velocity = new Vector2(0, 0);
                Player.playerAlive = true;
                Player.newGame = false;
                Player.endGame = false;
                Projectile.projectileShape.X = 990;
                gameTime.TotalGameTime = TimeSpan.MinValue;
                Player.captureTime = "0";
                Projectile.projectilesFired = 0;
                Player.powerUpActive = false;
                Player.powerupTimer = 0;
            }

            /*
            if (spawnTimer > 500)
            {

                int randomNumProjectiles = rndNum.Next(1, 3);

                for (int i = 0; i < randomNumProjectiles; i++)
                {
                    Projectile p = new Projectile(this, spriteBatch, Content, SCREENWIDTH, SCREENHEIGHT);
                    Components.Add(p);
                    projectileList.Add(p);
                    Console.WriteLine("new projectile object instance");
                }

                spawnTimer = 0;
            }

            spawnTimer++;



    */            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
