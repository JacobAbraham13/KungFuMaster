using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KungFuMaster
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int timer, w, h, enemyTimer;
        //Making Player not an Entity *Zaaim
        Player player1;
        //Enemy List
        List<RegEnemy> enemies;
        //Enum method failed (creating a parallel list to represent states) *Zaaim
        List<int> states; 
        List<Texture2D> backgroundText;
        List<Texture2D> sSheets;
        Texture2D currentText;
        Texture2D enemyText;
        Rectangle position;
        Rectangle background;
        Rectangle girlRec;
        Texture2D playerSheet;
        //health bars
        Rectangle playerHealthBarBackground;
        Rectangle playerHealthBar;
        Rectangle enemyHealthBarBackground;
        Rectangle enemyHealthBar;
        int stage;
        int floor;
        int count;
        int enemyCount;
        int enemiesAlive;
        int firstFloorRight;
        int transitionTimer;
        bool oneToTwo;
        bool isThree;
        bool moveLeft;
        bool moveRight;
        //health bar and time fonts
        SpriteFont arcadeFont;
        //font size 50
        SpriteFont welcomeFont;
        Texture2D whiteBar;
        // Hard coded enum (readonly i final equivelent of java) *Zaaim
        readonly int IDLE = 0, WALK = 1, CROUCH = 4, JUMP = 7;
        //Adding old keyboardState for single press items: jump attack *Zaaim
        //GameState ENUMs
        enum GameState {START, PLAY, WIN, LOSE}
        GameState gameState;
        KeyboardState old = Keyboard.GetState();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //sets gameState to START
            gameState = GameState.START;
            //Setting enum values (C#'s enum usage wasn't working so I will hard code it)
            stage = 0;
            timer = 0;
            floor = 550;
            count = 0;
            transitionTimer = 0;
            oneToTwo = false;
            isThree = false;
            w = GraphicsDevice.Viewport.Width;
            h = GraphicsDevice.Viewport.Height;
            enemies = new List<RegEnemy>();
            position = new Rectangle(1585, 0, 400, 200);
            girlRec = new Rectangle(800, 605, 50, 100);
            firstFloorRight = position.X;
            backgroundText = new List<Texture2D>();
            // background = new Rectangle(0 - w * 6, -10, w * 7, h);
            background = new Rectangle(0, 200, 2000, 600);
            //Setting initial state of player to IDLE *Zaaim
            states = new List<int>();
            states.Add(IDLE);
            List<List<Rectangle>> s = new List<List<Rectangle>>();
            //Loads "playerSheet" in initialize rather than upload
            player1 = new Player(this.Content.Load<Texture2D>("playerSheet"), new Rectangle(w/2 - 50, 500, 100, 200), 25, 6, s);
            //Loads an Enemy
            enemyText = this.Content.Load<Texture2D>("enemySheet");
            List<List<Rectangle>> sE = new List<List<Rectangle>>();
            enemies.Add(new RegEnemy(enemyText, new Rectangle(0 , 550, 80, 150), 25, 6, sE));
            playerHealthBarBackground = new Rectangle(175, 85, 200, 20);
            playerHealthBar = new Rectangle(175, 85, 200, 20);
            enemyHealthBarBackground = new Rectangle(175, 125, 200, 20);
            enemyHealthBar = new Rectangle(175, 125, 200, 20);
            enemyCount = 1;
            enemiesAlive = 20;
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
            //loads background textures (1-7)
            for (int i = 1; i <= 4; i++)
            {
                Texture2D temp = this.Content.Load<Texture2D>(""+ i);
                backgroundText.Add(temp);
            }
            //arcade font size 22
            arcadeFont = this.Content.Load<SpriteFont>("timerFont");
            //start and end font size 50
            welcomeFont = this.Content.Load<SpriteFont>("welcomeFont");
            //base health bar
            whiteBar = this.Content.Load<Texture2D>("whiteRec");
            playerSheet = this.Content.Load<Texture2D>("playerSheet");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            //Added the Escape functionality to end game *Zaaim
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();
                
            switch(gameState)
            {
                case GameState.START:
                    if (kb.IsKeyDown(Keys.Enter) && !old.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.PLAY;
                    }
                    break;
                case GameState.PLAY:
                    //Implemented timer functionality
                    timer++;
                    enemyTimer++;

                    //Creates and removes enemies
                    if (enemyTimer == 300 && enemies.Count < 5 && enemyCount < 21)
                    {
                        enemyCount++;
                        enemyTimer = 0;
                        List<List<Rectangle>> sE = new List<List<Rectangle>>();
                        enemies.Add(new RegEnemy(enemyText, new Rectangle(0, 550, 80, 150), 2, 6, sE));
                    }
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].isDead())
                        {
                            enemies.Remove(enemies[i]);
                            enemiesAlive--;
                        }
                            
                    }

                    //Player takes damage from knives
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        for(int x = 0; x < enemies[i].knives.Count; x++)
                        {
                            if (enemies[i].knives[x].rect.Intersects(player1.hitBox))
                            {
                                player1.health -= enemies[i].damage;
                                enemies[i].knives.RemoveAt(0);
                            }
                                
                        }
                        
                    }

                    // TODO: Add your update logic here
                    //moves background left

                    //If statements check user input and is the player is already attacking
                    if (kb.IsKeyDown(Keys.Down))
                    {
                        //implementing crouch *Zaaim
                        states[0] = CROUCH;
                        //Implementing direction
                        if (kb.IsKeyDown(Keys.Left) && !player1.isEntityAttacking())
                        {
                            player1.isLeft = true;
                        }
                        if (kb.IsKeyDown(Keys.Right) && !player1.isEntityAttacking())
                        {
                            player1.isLeft = false;
                        }
                        moveLeft = false;
                        moveRight = false;
                    }
                    else if (kb.IsKeyDown(Keys.Left) && !player1.isEntityAttacking())
                    {
                        //Implementing Left Walk
                        position.X -= 1;
                        states[0] = WALK;
                        player1.isLeft = true;
                        moveLeft = true;
                        moveRight = false;
                    }
                    //Fixed moving enemies when trying to go off screen **Paul
                    else if (kb.IsKeyDown(Keys.Right) && !player1.isEntityAttacking() && position.X + 1 < firstFloorRight)
                    {
                        position.X += 1;
                        //implementing Right Walk *Zaaim
                        states[0] = WALK;
                        player1.isLeft = false;
                        moveLeft = false;
                        moveRight = true;
                    }
                    else
                    {
                        //implementing IDLE *Zaaim
                        states[0] = IDLE;
                        moveLeft = false;
                        moveRight = false;
                    }

                    //Makes sure that player is not already jumping
                    if (kb.IsKeyDown(Keys.Up) && old.IsKeyUp(Keys.Up) && !player1.isJumping)
                    {
                        //Implementing Jump (velY creates arc form)
                        player1.isJumping = true;
                        player1.s = JUMP;
                        player1.velY = -4;
                    }

                    // implementing Dynamic Punches and Kicks
                    //X - Punches
                    if (kb.IsKeyDown(Keys.X) && old.IsKeyUp(Keys.X) && !player1.isEntityAttacking())
                    {
                        player1.isPunching = true;
                        //Checks to see if the player is too late into its jump
                        if (player1.indx < 5)
                        {
                            player1.indx = 0;
                        }
                    }
                    //Z - Kicks
                    if (kb.IsKeyDown(Keys.Z) && old.IsKeyUp(Keys.Z) && !player1.isEntityAttacking())
                    {
                        player1.isKicking = true;
                        //Checks to see if the player is too late into its jump
                        if (player1.indx < 5)
                        {
                            player1.indx = 0;
                        }
                    }

                    //transition to first floor
                    if (position.X < 0 && count == 0)
                    {
                        count++;
                    }



                     if (count == 1 || count == 2 )
                    {
                        oneToTwo = true;
                    }


                    //transition from first to second texture
                     if (oneToTwo)
                    {
                        transitionTimer++;
                        if (transitionTimer > 60)
                        {
                            oneToTwo = false;
                            count++;
                            transitionTimer = 0;
                        }
                    }

                    if (count == 3)
                    {
                        isThree = true;
                    }

                    


                    //prevents player from going past right edge of texture
                    if (position.X > firstFloorRight)
                    {
                        position.X = firstFloorRight;
                    }

                    //prevents player from going past left edge of texture
                    if (position.X < 0)
                    {
                        position.X = 0;

                    }

                    //checks if game has reached time limit
                    if (timer / 60 > 100)
                    {
                        gameState = GameState.LOSE;
                    }
                    
                    if (player1.health <= 0)
                    {
                        gameState = GameState.LOSE;
                    }
                    
                    playerHealthBar.Width = 200 * player1.health / 25;
                    enemyHealthBar.Width = enemiesAlive * 10;
                    
                    //moves girl rec on second floor
                    if (moveLeft && count ==3)
                    {
                        girlRec.X += 5;
                    }
                    if (moveRight && count == 3)
                    {
                        girlRec.X -= 5;
                    }
                    
                    //ensures that no enemies spawn on second floor
                    if (count == 3)
                    {
                        enemyTimer = 0;
                    }
                    
                    //checks for intersection
                    if (count == 3 && player1.rect.Intersects(girlRec))
                    {
                        gameState = GameState.WIN;
                    }
                    
                
                    // Update method for Player
                    player1.update(timer, states[0]);
                    for(int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].Update(gameTime, player1, timer, w, h, moveRight, moveLeft, enemies);
                    }
                    
                    break;
                    
                 case GameState.WIN:
                    if (kb.IsKeyDown(Keys.R) && !old.IsKeyDown(Keys.R))
                    {
                        gameState = GameState.PLAY;
                        timer = 0;
                        List<List<Rectangle>> s = new List<List<Rectangle>>();
                        player1 = new Player(this.Content.Load<Texture2D>("playerSheet"), new Rectangle(w / 2 - 50, 500, 100, 200), 25, 6, s);
                        count = 0;
                        oneToTwo = false;
                        isThree = false;
                        transitionTimer = 0;
                        position.X = firstFloorRight;
                        enemies = new List<RegEnemy>();
                        List<List<Rectangle>> sE = new List<List<Rectangle>>();
                        enemies.Add(new RegEnemy(this.Content.Load<Texture2D>("enemySheet"), new Rectangle(0, 550, 80, 150), 25, 6, sE));
                        girlRec = new Rectangle(800, 605, 50, 100);
                        enemiesAlive = 20;
                        enemyCount = 1;
                    }
                    break;

                case GameState.LOSE:
                    if (kb.IsKeyDown(Keys.R) && !old.IsKeyDown(Keys.R))
                    {
                        gameState = GameState.PLAY;
                        timer = 0;
                        List<List<Rectangle>> s = new List<List<Rectangle>>();
                        player1 = new Player(this.Content.Load<Texture2D>("playerSheet"), new Rectangle(w / 2 - 50, 500, 100, 200), 25, 6, s);
                        count = 0;
                        oneToTwo = false;
                        isThree = false;
                        transitionTimer = 0;
                        position.X = firstFloorRight;
                        enemies = new List<RegEnemy>();
                        List<List<Rectangle>> sE = new List<List<Rectangle>>();
                        enemies.Add(new RegEnemy(this.Content.Load<Texture2D>("enemySheet"), new Rectangle(0, 550, 80, 150), 25, 6, sE));
                        girlRec = new Rectangle(800, 605, 50, 100);
                        enemiesAlive = 20;
                        enemyCount = 1;
                    }
                    break;
                  
            }    

            //Implementing single tap input *Zaaim
            old = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            if (gameState == GameState.START)
            {
                spriteBatch.DrawString(welcomeFont, "    W E LC O M E    TO\n\nKUNG - FU  MASTER", new Vector2(260, 150), Color.Pink);
                spriteBatch.DrawString(welcomeFont, "    Press  Enter  to  Start", new Vector2(150, 500), Color.Orange);
                spriteBatch.DrawString(arcadeFont, "    Press  Escape  to  Quit", new Vector2(340, 635), Color.Red);
                spriteBatch.DrawString(arcadeFont, "Miners Inc.", new Vector2(425, 700), Color.White);
            }
            else if (gameState == GameState.PLAY)
            {
                spriteBatch.Draw(backgroundText[count], background, position, Color.White);
                spriteBatch.Draw(whiteBar, playerHealthBarBackground, Color.Gray);
                spriteBatch.Draw(whiteBar, enemyHealthBarBackground, Color.Gray);
                spriteBatch.Draw(whiteBar, playerHealthBar, Color.Orange);
                spriteBatch.Draw(whiteBar, enemyHealthBar, Color.Pink);
                spriteBatch.DrawString(arcadeFont, "TIME", new Vector2(800, 85), Color.White);
                spriteBatch.DrawString(arcadeFont, "" + (6000 - timer), new Vector2(800, 120), Color.White);
                spriteBatch.DrawString(arcadeFont, "PLAYER", new Vector2(40, 80), Color.Orange);
                spriteBatch.DrawString(arcadeFont, "ENEMY", new Vector2(50, 120), Color.Pink);
                ////Calls player draw
                //spriteBatch.Draw(backgroundText[0], player1.hitBox, Color.Black);
                ////Enemy Draw
                //spriteBatch.Draw(backgroundText[0], enemies[0].hitBox, Color.Black);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
                }
                
                //Calls player draw
                player1.draw(gameTime, spriteBatch);
                //draws girl on second floor
                if (count == 3)
                {
                    spriteBatch.Draw(playerSheet, girlRec, new Rectangle(190, 634, 25, 57), Color.White);//, MathHelper.ToRadians(180), new Vector2(girlRec.X + girlRec.Width/2, girlRec.Y + girlRec.Height/2), SpriteEffects.None, 0);
                }
            }
            else if (gameState == GameState.WIN)
            {
                spriteBatch.DrawString(welcomeFont, "CONGRATULATIONS", new Vector2(230, 150), Color.White);
                spriteBatch.DrawString(welcomeFont, "YOU HAVE SAVED YOUR LOVE!", new Vector2(135, 275), Color.Pink);
                spriteBatch.DrawString(welcomeFont, "    Press  R  to  Play Again", new Vector2(130, 450), Color.Orange);
                spriteBatch.DrawString(arcadeFont, "    Press  Escape  to  Quit", new Vector2(340, 600), Color.Red);
                spriteBatch.DrawString(arcadeFont, "Miners Inc.", new Vector2(425, 700), Color.White);
            }
            else if (gameState == GameState.LOSE)
            {
                spriteBatch.DrawString(welcomeFont, "OH NO!", new Vector2(430, 150), Color.Red);
                spriteBatch.DrawString(welcomeFont, "YOU  FAILED  TO  SAVE  THE  GIRL", new Vector2(60, 275), Color.Red);
                spriteBatch.DrawString(welcomeFont, "    Press  R  to  Play Again", new Vector2(130, 450), Color.Orange);
                spriteBatch.DrawString(arcadeFont, "    Press  Escape  to  Quit", new Vector2(340, 600), Color.Red);
                spriteBatch.DrawString(arcadeFont, "Miners Inc.", new Vector2(425, 700), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
