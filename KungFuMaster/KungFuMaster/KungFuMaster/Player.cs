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
    class Player/* : Entity*/
    {
        public List<List<Rectangle>> sRecs;
        public Rectangle rect, hitBox;
        public Texture2D text;
        public int indx;
        public int health, damage, s, velY;
        public bool isKicking, isPunching, isJumping, isLeft;
        public readonly int IDLE = 0, WALK = 1, CROUCH = 4, JUMP = 7;

        public Player(Texture2D t, Rectangle r, int h, int d, List<List<Rectangle>> sRecs)/* : base(t, r, h, d, sRecs)*/
        {
            //Input Data
            text = t;
            rect = r;
            health = h;
            damage = d;
            isLeft = true;
            this.sRecs = sRecs;
            hitBox = new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);
            //Loading IDLE
            List<Rectangle> input = new List<Rectangle>();
            input.Add(new Rectangle(0, 0, 50, 100));
            sRecs.Add(input);
            //Loading WALK
            input = new List<Rectangle>();
            int x = 50;
            for (int i = 0; i < 4; i++)
            {
                input.Add(new Rectangle(x, 0, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
            //Loading PUNCH
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 3; i++)
            {
                input.Add(new Rectangle(x, 100, 50, 100));
                if (i == 1)
                {
                    x -= 50;
                }
                else
                {
                    x += 50;
                }
            }
            sRecs.Add(input);
            //Loading Kick
            input = new List<Rectangle>();
            x = 100;
            for (int i = 0; i < 3; i++)
            {
                input.Add(new Rectangle(x, 100, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
            //Loading Crouch
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 1; i++)
            {
                input.Add(new Rectangle(x, 200, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
            //Loading Crouch Punch
            input = new List<Rectangle>();
            x = 50;
            for (int i = 0; i < 3; i++)
            {
                input.Add(new Rectangle(x, 200, 50, 100));
                if (i == 1)
                {
                    x -= 50;
                }
                else
                {
                    x += 50;
                }
            }
            sRecs.Add(input);
            //Loading Crouch Kick
            input = new List<Rectangle>();
            x = 150;
            for (int i = 0; i < 3; i++)
            {
                input.Add(new Rectangle(x, 200, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
            //Loading Jump
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 6; i++)
            {
                input.Add(new Rectangle(x, 300, 50, 100));
                if (i == 2)
                {
                    continue;
                }
                x += 50;
            }
            sRecs.Add(input);
            //Loading Jump Punch
            input = new List<Rectangle>();
            x = 250;
            for (int i = 0; i < 4; i++)
            {
                input.Add(new Rectangle(x, 300, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
            //Loading Jump Kick
            input = new List<Rectangle>();
            x = 450;
            for (int i = 0; i < 4; i++)
            {
                input.Add(new Rectangle(x, 300, 50, 100));
                x += 50;
            }
            sRecs.Add(input);
        }
        
        //Boolean check used to limit spams from the user *Zaaim
        public bool isEntityAttacking()
        {
            if (isKicking || isPunching)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //......Draw thingy *Zaaim
        public void draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            //Second safe check to avoid OutOfRangeException errors
            if (indx >= sRecs[s].Count)
            {
                indx = 0;
            }
            //if else statement to allow the textures to work in both directions (isLeft is declared based on the input in Game.cs from the user)
            if(isLeft)
            {
                //SpriteEffects.FlipHorizontally allows the texture to flip accross the current origin (0, 0)
                spriteBatch.Draw(text, rect, sRecs[s][indx], Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1f);
            }
            else
            {
                spriteBatch.Draw(text, rect, sRecs[s][indx], Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1f);
            }
            
        }

        // Updates the textures used for movement depending on the input 
        //(state is a Int variable manipulated in Game.cs to specify the row indx of the loaded Texture animations)
        public void update(int timer, int state)
        {
            if(isLeft)
            {
                switch (s)
                {
                    case 0:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 1:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 2:
                        if (indx == 0)
                        {
                            hitBox.X = rect.X + rect.Width / 2;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height / 2;
                        }
                        else
                        {
                            hitBox.X = rect.X + rect.Width * 1 / 3;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height / 2;
                        }
                        break;
                    case 3:
                        if (indx == 0 || indx == 2)
                        {
                            hitBox.X = rect.X + rect.Width / 2;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height / 2;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width;
                            hitBox.Height = rect.Height / 2;
                        }
                        break;
                    case 4:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height * 3 / 5;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height * 2 / 5;
                        break;
                    case 5:
                        if (indx == 0)
                        {
                            hitBox.X = rect.X + rect.Width / 2;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        else
                        {
                            hitBox.X = rect.X + rect.Width * 1 / 3;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        break;
                    case 6:
                        if (indx == 0 || indx == 2)
                        {
                            hitBox.X = rect.X + rect.Width / 2;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        break;
                    case 7:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 8:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 9:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                }
            }
            else if (!isLeft)
            {
                switch (s)
                {
                    case 0:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 1:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 2:
                        if (indx == 0)
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height / 2;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height / 2;
                        }
                        break;
                    case 3:
                        if (indx == 0 || indx == 2)
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height / 2;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height / 2;
                            hitBox.Width = rect.Width;
                            hitBox.Height = rect.Height / 2;
                        }
                        break;
                    case 4:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height * 3 / 5;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height * 2 / 5;
                        break;
                    case 5:
                        if (indx == 0)
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width / 2;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        break;
                    case 6:
                        if (indx == 0 || indx == 2)
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width * 2 / 3;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        else
                        {
                            hitBox.X = rect.X;
                            hitBox.Y = rect.Y + rect.Height * 3 / 5;
                            hitBox.Width = rect.Width;
                            hitBox.Height = rect.Height * 2 / 5;
                        }
                        break;
                    case 7:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width / 2;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 8:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width * 2 / 3;
                        hitBox.Height = rect.Height / 2;
                        break;
                    case 9:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 2;
                        hitBox.Width = rect.Width;
                        hitBox.Height = rect.Height / 2;
                        break;
                }
            }
            
            //Standard Motion Animations (Walk kick Jump) (Repeatedly play when player isnt attacking)
            if (!isPunching && !isKicking && !isJumping)
            {
                s = state;
                if (timer % 10 == 0)
                {
                    indx++;
                }
                if (indx >= sRecs[s].Count)
                {
                    indx = 0;
                }
                //(commented out to use later when player reaches bounds)
                if (s == WALK)
                {
                    //rect.X += 1;
                }
            }

            //Checks to see if the user has set the Player to jump 
            //(isJumping is a boolean that is a state variable of Player and is manipulated in Game.cs according to the user input)
            if (isJumping)
            {
                //if the user still tries to go forward while jumping, it will essentially forward jump
                //(commented out to use later when player reaches bounds)
                if (state == WALK)
                {
                    //rect.X += 1;
                }
                //limits the Players jump to the flor value of 700
                if (rect.Y + rect.Height > 700) 
                {
                    //ends the jump and resets values
                    rect.Y = 700 - rect.Height;
                    isJumping = false;
                    s = IDLE;
                }
                else
                {
                    //implements jump logic using a velY which is accelerated by 1 and incrementing the jump animations
                    rect.Y += velY;
                    if (timer % 10 == 0)
                    {
                        indx++;
                        velY += 1;
                    }
                    if (indx >= sRecs[s].Count)
                    {
                        indx = sRecs[s].Count - 1;
                    }
                }
            }

            //Checks to see if the user has made the Player punch and safeguards against inputting kicking values
            //(isKicking and isPunching are state variables of Player class and are manipulated in game.cs according to user input)
            if (isPunching && !isKicking)
            {
                //Implements logic for user kicking or punching while jumping
                if (isJumping)
                {
                    if (s != 9 && s != 8 && indx < 5)
                    {
                        s = 8;
                    }
                }
                else
                {
                    //switches the animation
                    if (indx == 0)
                    {
                        switch (s)
                        {
                            case 0:
                                s += 2;
                                break;
                            case 1:
                                s += 1;
                                break;
                            case 4:
                                s += 1;
                                break;
                        }
                    }

                    //ends the animation and switches back
                    if (indx >= sRecs[s].Count - 1)
                    {
                        isPunching = false;
                        switch (s)
                        {
                            case 0 + 2:
                                s -= 2;
                                break;
                            case 5:
                                s -= 1;
                                break;
                        }
                    }
                    else
                    {
                        //plays the animation
                        if (timer % 20 == 0)
                        {
                            indx++;
                        }
                    }
                }
            }

            //Checks to see if the user has made the Player kick and safeguards against inputting punching values
            //(isKicking and isPunching are state variables of Player class and are manipulated in game.cs according to user input)
            if (isKicking && !isPunching)
            {
                //Implements logic for user kicking or punching while jumping
                if (isJumping)
                {
                    if (s != 9 && s != 8 && indx < 5)
                    {
                        s = 9;
                    }
                }
                else
                {
                    //switches animation
                    if (indx == 0)
                    {
                        switch (s)
                        {
                            case 0:
                                s += 3;
                                break;
                            case 1:
                                s += 2;
                                break;
                            case 4:
                                s += 2;
                                break;
                        }
                    }

                    //ends and resets animation
                    if (indx >= sRecs[s].Count - 1)
                    {
                        isKicking = false;
                        switch (s)
                        {
                            case 0 + 3:
                                s -= 3;
                                break;
                            case 6:
                                s -= 2;
                                break;
                        }
                    }
                    else
                    {
                        //plays animation
                        if (timer % 20 == 0)
                        {
                            indx++;
                        }
                    }
                }
            }
        }
    }
}
