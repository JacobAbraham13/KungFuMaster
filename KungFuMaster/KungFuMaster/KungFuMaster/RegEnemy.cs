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
    class RegEnemy
    {

        public enum State
        {
            IDLE, WALK, THROW, CROUCH, CTHROW, HIT, DEFEATED
        }

        Rectangle rect;
        public Rectangle hitBox;
        public List<Knife> knives;
        List<List<Rectangle>> sRecs;
        int health;
        public int damage;
        Texture2D text;
        int fightTimer;
        State state;
        bool isRight, inRange, stance, isAttacking, isHit;
        int indx;
        Random rand;

        public RegEnemy(Texture2D t, Rectangle r, int h, int d, List<List<Rectangle>> sRecs)
        {
            //Input Data
            knives = new List<Knife>();
            text = t;
            rect = r;
            health = h;
            damage = d;
            this.sRecs = sRecs;
            fightTimer = 0;
            isRight = true;
            state = State.IDLE;
            indx = 0;
            rand = new Random();
            //Loading IDLE
            List<Rectangle> input = new List<Rectangle>();
            input.Add(new Rectangle(0, 0, 40, 60));
            sRecs.Add(input);
            //Loading WALK
            input = new List<Rectangle>();
            int x = 0;
            for (int i = 0; i < 4; i++)
            {
                input.Add(new Rectangle(x, 60, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading THROW
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 2; i++)
            {
                input.Add(new Rectangle(x, 120, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading CROUCH
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 1; i++)
            {
                input.Add(new Rectangle(x, 180, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading CTHROW
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 2; i++)
            {
                input.Add(new Rectangle(x, 180, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading HIT
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 2; i++)
            {
                input.Add(new Rectangle(x, 240, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading DEFEATED
            input = new List<Rectangle>();
            x = 0;
            for (int i = 0; i < 2; i++)
            {
                input.Add(new Rectangle(x, 300, 40, 60));
                x += 40;
            }
            sRecs.Add(input);
            //Loading KNIFE
            input = new List<Rectangle>();
            x = 0;
            input.Add(new Rectangle(x, 360, 12, 8));
            sRecs.Add(input);
            //Setting Hitbox
            hitBox = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
        }
        
        //Update method handles most of enemy AI. (ONLY PAUL CHANGE RIGHT NOW)
        public void Update(GameTime gameTime, Player player, int timer, int w, int h, bool r, bool l, List<RegEnemy> list)
        {
            if(isDead())
            {
                knives.Clear();
                return;
            }
            else if (rect.Center.X < player.rect.Center.X)
            {
                isRight = true;
                switch (state)
                {
                    case State.IDLE:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.WALK:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.THROW:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.CROUCH:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 3;
                        hitBox.Width = rect.Width * 3 / 4;
                        hitBox.Height = rect.Height * 2 / 3;
                        break;
                    case State.CTHROW:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y + rect.Height / 3;
                        hitBox.Width = rect.Width * 3 / 4;
                        hitBox.Height = rect.Height * 2 / 3;
                        break;
                    case State.HIT:
                        hitBox.X = rect.X;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.DEFEATED:
                        break;
                }
            }
            else if (rect.Center.X > player.rect.Center.X)
            {
                isRight = false;
                switch (state)
                {
                    case State.IDLE:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.WALK:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.THROW:
                        hitBox.X = rect.X + rect.Width / 2;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.CROUCH:
                        hitBox.X = rect.X + rect.Width / 3;
                        hitBox.Y = rect.Y + rect.Height / 3;
                        hitBox.Width = rect.Width * 3 / 4;
                        hitBox.Height = rect.Height * 2 / 3;
                        break;
                    case State.CTHROW:
                        hitBox.X = rect.X + rect.Width / 3;
                        hitBox.Y = rect.Y + rect.Height / 3;
                        hitBox.Width = rect.Width * 3 / 4;
                        hitBox.Height = rect.Height * 2 / 3;
                        break;
                    case State.HIT:
                        hitBox.X = rect.X + rect.Width /2;
                        hitBox.Y = rect.Y;
                        hitBox.Height = rect.Height;
                        hitBox.Width = rect.Width / 2;
                        break;
                    case State.DEFEATED:
                        break;
                }
            }

            
            if (this.hitBox.Intersects(player.hitBox) && player.isEntityAttacking())
            {
                state = State.HIT;
                isHit = true;
                health -= 1;
            }
            if (timer % 20 == 0)
            {
                indx++;
            }
            if (indx >= sRecs[(int)state].Count)
            {
                indx = 0;
            }

            if (!isHit)
            {
                int addDist = 0;
                for(int i = 0; i < list.Count; i++)
                {
                    if(list[i].inRange)
                    {
                        addDist += 100;
                    }
                }
                if (distanceTo(player.rect) <= 250 + addDist && !inRange)
                {
                    inRange = true;
                    stance = true;
                }
                //animationCounter++;
                if (inRange) //If enemy is in range, they attack
                {
                    if (distanceTo(player.rect) > 250 + addDist && !isAttacking)
                    {
                        inRange = false;
                    }
                    if (stance && !isAttacking)
                    {
                        int choice = rand.Next(0, 2);
                        switch (choice)
                        {
                            case 0:
                                state = State.IDLE;
                                break;
                            case 1:
                                state = State.CROUCH;
                                break;
                        }
                        stance = false;
                    }
                    if (l)
                    {
                        rect.X += 5;
                    }
                    else if (r)
                    {
                        rect.X -= 5;
                    }

                    if (timer % 120 == 0 && !isAttacking)
                    {
                        stance = true;
                    }

                    if (fightTimer % 240 == 0 && !isAttacking)
                    {
                        isAttacking = true;
                        switch (state)
                        {
                            case State.IDLE:
                                state = State.THROW;
                                break;
                            case State.CROUCH:
                                state = State.CTHROW;
                                break;
                        }
                    }

                    switch (state)
                    {
                        case State.THROW:
                            if (indx == 1)
                            {
                                knives.Add(new Knife(new Rectangle(rect.X + 30, rect.Y + 30, 50, 25), isRight));
                                state = State.IDLE;
                                isAttacking = false;
                            }
                            break;
                        case State.CTHROW:
                            if (indx == 1)
                            {
                                knives.Add(new Knife(new Rectangle(rect.X + 30, rect.Y + 100, 50, 25), isRight));
                                state = State.CROUCH;
                                isAttacking = false;
                            }
                            break;
                    }
                }
                else if (!inRange) //If they are not in range, call the move method
                {
                    state = State.WALK;
                    if (isRight)
                    {
                        rect.X += 1;
                    }
                    else if (!isRight)
                    {
                        rect.X -= 1;
                    }
                    if (l)
                    {
                        rect.X += 5;
                    }
                    else if (r)
                    {
                        rect.X -= 5;
                    }
                }
                for (int i = knives.Count - 1; i >= 0; i--)
                {
                    if (knives[i].isRight)
                    {
                        knives[i].rect.X += 3;
                    }
                    else
                    {
                        knives[i].rect.X -= 3;
                    }
                    if (l)
                    {
                        knives[i].rect.X += 5;
                    }
                    else if (r)
                    {
                        knives[i].rect.X -= 5;
                    }
                    if (knives[i].rect.X + knives[i].rect.Width < 0 || knives[i].rect.X > w)
                    {
                        knives.Remove(knives[i]);
                    }
                }
            }
            else if (isHit)
            {
                state = State.HIT;
                if (l)
                {
                    rect.X += 5;
                }
                else if (r)
                {
                    rect.X -= 5;
                }
                if (indx == sRecs[(int)state].Count - 1)
                {
                    isHit = false;
                    state = State.IDLE;
                }
            }
            if(inRange)
            {
                fightTimer++;
            } 
        }
        
        //Draws the enemy according to direction. ONLY SPRITEBATCH IS PASSED IN
        public void Draw(SpriteBatch spriteBatch)
        {
            if (indx >= sRecs[(int)state].Count)
            {
                indx = 0;
            }
            if (!isRight)
            {
                spriteBatch.Draw(text, rect, sRecs[(int)state][indx], Color.White, 0f, new Vector2(0), SpriteEffects.FlipHorizontally, 1f);
            }
            else if (isRight)
            {
                spriteBatch.Draw(text, rect, sRecs[(int)state][indx], Color.White, 0f, new Vector2(0), SpriteEffects.None, 1f);
            }
            for (int i = 0; i < knives.Count; i++)
            {
                if(knives[i].isRight)
                {
                    spriteBatch.Draw(text, knives[i].rect, sRecs[7][0], Color.White, 0f, new Vector2(0), SpriteEffects.None, 1f);
                }
                else if (!knives[i].isRight)
                {
                    spriteBatch.Draw(text, knives[i].rect, sRecs[7][0], Color.White, 0f, new Vector2(0), SpriteEffects.FlipHorizontally, 1f);
                }
            }
        }

        //Checks the distance from the center of the enemy to the center of the player
        public int distanceTo(Rectangle playRect)
        {
            int distance = 0;
            if(isRight)
            {
                distance = playRect.Center.X - rect.Center.X;
            }
            else if(!isRight)
            {
                distance = rect.Center.X - playRect.Center.X;
            }
            return distance;
        }

        //Checks whether enemy health is below or equal to zero
        public Boolean isDead()
        {
            return health <= 0;
        }
    }
}
