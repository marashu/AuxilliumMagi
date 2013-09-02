using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AuxilliumMagi
{
    class Priest
    {
        private AnimatedSprite myBody;
        private AnimatedSprite teleport;
        private sbyte health = 8;
        public sbyte Health { get { return health; } }
        //what side of the screen is she on?
        private bool bLeft = false;

        private bool bAttacking = false;
        public bool Attacking { get { return bAttacking; } }

        private const int BASETIME = 300;
        private int idleTimeLimit;
        private TimeSpan idleTime = TimeSpan.Zero;
        private TimeSpan deathTime = TimeSpan.Zero;
        private TimeSpan attackTime = TimeSpan.Zero;
        private byte myColour = 255;

        private BossStates currentState = BossStates.Idle;
        public BossStates CurrentState { get { return currentState; } }

        public enum BossStates
        {
            Idle,
            Teleport,
            Attack,
            Dying,
            Dead
        }

        public Priest()
        {
            myBody = new AnimatedSprite(GameCore.PublicPriestBody);
            teleport = new AnimatedSprite(GameCore.PublicTeleport);
            myBody.FrameMod = 1.5f;
            myBody.Animate(0);
            idleTimeLimit = new Random().Next(5, 20) * BASETIME;
            int distX = new Random().Next(830, 930);
            if(new Random().Next(0,100) < 50)
            {
                distX -= 700;
                bLeft = true;
            }
            myBody.Position = new Vector2(distX, new Random().Next(120, 500));
            teleport.Position = myBody.Position;
            teleport.FrameMod = 0.75f;
        }

        public void Update(GameTime gameTime)
        {
            //behaviour based on state
            switch (currentState)
            {
                case BossStates.Idle:
                    idleTime += gameTime.ElapsedGameTime;
                    if (idleTime >= TimeSpan.FromMilliseconds(idleTimeLimit))
                    {
                        idleTime = TimeSpan.Zero;
                        idleTimeLimit = new Random().Next(5, 20) * BASETIME;
                        myBody.Animate(1);
                        teleport.Animate(0, 7);
                        currentState = BossStates.Teleport;
                        
                    }
                    break;
                case BossStates.Attack:
                    bAttacking = false;
                    attackTime += gameTime.ElapsedGameTime;
                    if (attackTime >= TimeSpan.FromMilliseconds(500))
                    {
                        attackTime = TimeSpan.Zero;
                        myBody.Animate(0);
                        currentState = BossStates.Idle;

                        
                    }
                    break;
                case BossStates.Teleport:
                    if (teleport.CurrentFrame == 7)
                    {
                        teleport.Animate(0);
                        int distX = new Random().Next(130, 230);
                        if (!bLeft)
                            distX += 700;
                        bLeft = !bLeft;
                        myBody.Position = new Vector2(distX, new Random().Next(120, 500));
                        teleport.Position = myBody.Position;
                        currentState = BossStates.Attack;
                        bAttacking = true;
                    }
                    break;
                case BossStates.Dying:
                    if (myBody.CurrentFrame == 2)
                    {
                        deathTime += gameTime.ElapsedGameTime;
                        if (deathTime > TimeSpan.FromMilliseconds(2600))
                        {
                            deathTime = TimeSpan.Zero;
                            myBody.Animate(3);
                        }
                    }
                    if (myBody.CurrentFrame == 3)
                    {
                        //myBody.Animate(3);
                        deathTime += gameTime.ElapsedGameTime;
                        if (deathTime > TimeSpan.FromMilliseconds(2600))
                        {
                            currentState = BossStates.Dead;
                        }
                    }
                    break;
            }
            if (myColour < 255)
            {
                myColour += 5;
                if (myColour >= 250)
                    myColour = 255;
                myBody.MyColour = new Color(myColour, myColour, 255);
            }
            //update animation
            myBody.Update(gameTime);
            teleport.Update(gameTime);
        }

        public Vector2 GetSpawn()
        {
            if (bLeft)
            {
                return new Vector2(myBody.Position.X + myBody.Size.X + 30, myBody.Center.Y); 
            }
            else
            {
                return new Vector2(myBody.Position.X - 50, myBody.Center.Y);
            }
        }

        public bool GetCollision(Sprite target)
        {
            Rectangle tempRect = new Rectangle((int)(myBody.Position.X), (int)(myBody.Position.Y), (int)(myBody.Size.X), (int)(myBody.Size.Y));
            if (tempRect.Intersects(new Rectangle((int)(target.Position.X), (int)(target.Position.Y), (int)(target.Size.X), (int)(target.Size.Y))))
            {
                health--;
                myColour = 0;
                myBody.MyColour = new Color(0, 0, 255);
                if (health <= 0)
                {
                    currentState = BossStates.Dying;
                    myBody.Animate(2);
                }
                return true;
            }

           
            

            return false;
        }

        public void Draw(SpriteBatch sBatch)
        {
            myBody.Draw(sBatch);
            if (currentState == BossStates.Teleport)
                teleport.Draw(sBatch);
        }

    }
}
