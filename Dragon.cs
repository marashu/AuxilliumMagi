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
    public class Dragon
    {
        private AnimatedSprite myBody;
        private List<Sprite> leftNeck;
        private List<Sprite> rightNeck;
        private List<Explosion> explosions;
        private Vector2 leftNeckBase;
        private Vector2 rightNeckBase;
        private AnimatedSprite leftHead;
        private AnimatedSprite rightHead;
        private Vector2 leftSpawn;
        private Vector2 rightSpawn;
        private Vector2 leftTarget = Vector2.Zero;
        private Vector2 rightTarget = Vector2.Zero;

        private bool bRightMoving = false;
        private bool bLeftMoving = false;
        private int leftSpeed = 0;
        private int rightSpeed = 0;

        private byte leftColour = 255;
        private byte rightColour = 255;
        private sbyte health;
        public sbyte Health { get { return health; } }

        private bool bDead = false;
        public bool Dead { get { return bDead; } }


        private TimeSpan leftTimer = TimeSpan.Zero;
        private TimeSpan rightTimer = TimeSpan.Zero;

        private const int TIMELIMIT = 1500;
        private const int TIMEBASEPERCENT = 25;
        private const int TIMEADDPERCENT = 3;
        private const int TIMEADDLIMIT = 400;
        private byte leftAddPercent = TIMEBASEPERCENT;
        private byte rightAddPercent = TIMEBASEPERCENT;
        private Random rand;

        private TimeSpan animTime = TimeSpan.Zero;
        private TimeSpan deathTime = TimeSpan.Zero;
        private TimeSpan boomTime = TimeSpan.Zero;


        public Vector2 LeftSpawn { get { return leftSpawn; } }
        public Vector2 RightSpawn { get { return rightSpawn; } }

        //get rid of this quickly
        public Sprite LeftHead { get { return leftHead; } }

        public Dragon()
        {
            health = 5;
            myBody = new AnimatedSprite(GameCore.PublicDragonBody);
            rand = new Random();
            myBody.Position = new Vector2(640 - myBody.Size.X / 2 - 40, -100);

            leftNeckBase = new Vector2(565, 90);
            rightNeckBase = new Vector2(655, 90);

            leftNeck = new List<Sprite>();
            rightNeck = new List<Sprite>();
            explosions = new List<Explosion>();

            //add neck pieces to the left neck
            Sprite tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[0].Position = leftNeckBase;
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[1].Position = new Vector2(480, 120);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[2].Position = new Vector2(460, 130);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[3].Position = new Vector2(440, 140);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[4].Position = new Vector2(430, 150);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[5].Position = new Vector2(410, 160);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[6].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[7].Position = new Vector2(380, 170);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[8].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[9].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[10].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[11].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[12].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[13].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[14].Position = new Vector2(400, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[15].Position = new Vector2(380, 170);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[16].Position = new Vector2(370, 180);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[17].Position = new Vector2(350, 190);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[18].Position = new Vector2(330, 200);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[19].Position = new Vector2(310, 210);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[20].Position = new Vector2(290, 220);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            leftNeck.Add(tempNeck);
            leftNeck[21].Position = new Vector2(270, 230);



            //neck pieces for right neck
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[0].Position = rightNeckBase;
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[1].Position = new Vector2(680, 120);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[2].Position = new Vector2(700, 130);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[3].Position = new Vector2(720, 140);
            tempNeck = new Sprite(GameCore.PublicDragonNeckL);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[4].Position = new Vector2(730, 150);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[5].Position = new Vector2(750, 160);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[6].Position = new Vector2(770, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[7].Position = new Vector2(790, 170);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[8].Position = new Vector2(830, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[9].Position = new Vector2(850, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[10].Position = new Vector2(870, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[11].Position = new Vector2(890, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[12].Position = new Vector2(890, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[13].Position = new Vector2(890, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[14].Position = new Vector2(890, 165);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[15].Position = new Vector2(910, 170);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[16].Position = new Vector2(920, 180);
            tempNeck = new Sprite(GameCore.PublicDragonNeckM);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[17].Position = new Vector2(930, 190);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[18].Position = new Vector2(940, 200);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[19].Position = new Vector2(960, 210);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[20].Position = new Vector2(980, 220);
            tempNeck = new Sprite(GameCore.PublicDragonNeckS);
            tempNeck.Pivot = new Vector2(tempNeck.Size.X / 2, tempNeck.Size.Y / 2);
            rightNeck.Add(tempNeck);
            rightNeck[21].Position = new Vector2(1000, 230);


            leftHead = new AnimatedSprite(GameCore.PublicDragonHead);
            
            leftHead.Pivot = new Vector2(leftHead.Size.X / 2, leftHead.Size.Y / 2);
            leftHead.Position = new Vector2(220, 160);
            rightHead = new AnimatedSprite(GameCore.PublicDragonHead);
            rightHead.Pivot = new Vector2(rightHead.Size.X / 2, rightHead.Size.Y / 2);
            rightHead.Position = new Vector2(940, 160);
            myBody.FrameMod = 1.2f;
            myBody.Animate(0, 3);
            leftHead.Animate(0);
            rightHead.Animate(0);
        }

        public void Update(GameTime gameTime)
        {
            //handle animation
            //myBody.Update(gameTime);
            if (health > 0)
            {
                //Erica says to do this at random, so.........
                animTime += gameTime.ElapsedGameTime;
                if (animTime > TimeSpan.FromMilliseconds(500))
                {
                    animTime -= TimeSpan.FromMilliseconds(500);

                    leftHead.Animate((byte)(new Random().Next(0, 3)));
                    rightHead.Animate((byte)(new Random().Next(0, 3)));
                }

                //handle movement
                //head AI - get it to move to a point, wait, repeat.
                if (bLeftMoving)
                {
                    Vector2 dist = leftTarget - leftHead.Position;
                    if (dist.Length() < 10)
                    {
                        bLeftMoving = false;
                    }
                    else
                    {
                        dist.Normalize();

                        leftHead.Position += dist * leftSpeed;
                    }
                }
                else
                {
                    leftTimer += gameTime.ElapsedGameTime;
                    if (leftTimer >= TimeSpan.FromMilliseconds(TIMELIMIT))
                    {
                        if (rand.Next(0, 100) < leftAddPercent)
                        {
                            leftTimer = TimeSpan.Zero;
                            leftAddPercent = TIMEBASEPERCENT;
                            leftTarget = new Vector2(rand.Next(90, 270), rand.Next(70, 580));
                            leftSpeed = rand.Next(3, 9);
                            bLeftMoving = true;
                        }
                        else
                        {
                            leftAddPercent += TIMEADDPERCENT;
                            leftTimer -= TimeSpan.FromMilliseconds(TIMEADDLIMIT);
                        }
                    }
                }
                if (bRightMoving)
                {
                    Vector2 dist = rightTarget - rightHead.Position;
                    if (dist.Length() < 10)
                    {
                        bRightMoving = false;
                    }
                    else
                    {
                        dist.Normalize();

                        rightHead.Position += dist * rightSpeed;
                    }
                }
                else
                {
                    rightTimer += gameTime.ElapsedGameTime;
                    if (rightTimer >= TimeSpan.FromMilliseconds(TIMELIMIT))
                    {
                        if (rand.Next(0, 100) < rightAddPercent)
                        {
                            rightTimer = TimeSpan.Zero;
                            rightAddPercent = TIMEBASEPERCENT;
                            rightTarget = new Vector2(rand.Next(920, 1060), rand.Next(70, 580));
                            rightSpeed = rand.Next(3, 9);
                            bRightMoving = true;
                        }
                        else
                        {
                            rightAddPercent += TIMEADDPERCENT;
                            rightTimer -= TimeSpan.FromMilliseconds(TIMEADDLIMIT);
                        }
                    }
                }

                Move(leftNeck[leftNeck.Count() - 1], leftHead, 5, leftSpeed);
                for (int i = leftNeck.Count() - 2; i >= 0; i--)
                {
                    Move(leftNeck[i], leftNeck[i + 1], 40, leftSpeed);
                }
                if (leftNeck[0].Position != leftNeckBase)
                {
                    leftNeck[0].Position = leftNeckBase;
                    for (int i = 1; i < leftNeck.Count() - 1; i++)
                    {
                        Move(leftNeck[i], leftNeck[i - 1], 40, leftSpeed);
                    }
                    Move(leftHead, leftNeck[leftNeck.Count() - 1], 5, leftSpeed);
                }
                Move(rightNeck[rightNeck.Count() - 1], rightHead, 5, rightSpeed);
                for (int i = rightNeck.Count() - 2; i >= 0; i--)
                {
                    Move(rightNeck[i], rightNeck[i + 1], 40, rightSpeed);
                }
                if (rightNeck[0].Position != rightNeckBase)
                {
                    rightNeck[0].Position = rightNeckBase;
                    for (int i = 1; i < rightNeck.Count() - 1; i++)
                    {
                        Move(rightNeck[i], rightNeck[i - 1], 40, rightSpeed);
                    }
                    Move(rightHead, rightNeck[rightNeck.Count() - 1], 5, rightSpeed);
                }



                //handle rotation
                for (int i = 0; i < leftNeck.Count() - 1; i++)
                {
                    SetDirection(leftNeck[i], leftNeck[i + 1]);
                }
                SetDirection(leftNeck[leftNeck.Count() - 1], leftHead);
                SetDirection(leftHead, new Vector2(640, 360));
                for (int i = 0; i < rightNeck.Count() - 1; i++)
                {
                    SetDirection(rightNeck[i], rightNeck[i + 1]);
                }
                SetDirection(rightNeck[rightNeck.Count() - 1], rightHead);
                SetDirection(rightHead, new Vector2(640, 360));


                //fetch the spawn points
                leftSpawn = GetSpawnPoint(leftHead);
                rightSpawn = GetSpawnPoint(rightHead);

                //handle colours
                if (leftColour < 255)
                {
                    leftColour += 5;
                    if (leftColour >= 250)
                        leftColour = 255;
                    leftHead.MyColour = new Color(leftColour, leftColour, 255);
                }
                if (rightColour < 255)
                {
                    rightColour += 5;
                    if (rightColour >= 250)
                        rightColour = 255;
                    rightHead.MyColour = new Color(rightColour, rightColour, 255);
                }
                //handle animation
                myBody.Update(gameTime);
                leftHead.Update(gameTime);
                rightHead.Update(gameTime);
            }//end health > 0
            else
            {
                deathTime += gameTime.ElapsedGameTime;

                if (deathTime > TimeSpan.FromMilliseconds(3000))
                {
                    bDead = true;
                }

                boomTime += gameTime.ElapsedGameTime;
                if (boomTime > TimeSpan.FromMilliseconds(100))
                {
                    boomTime -= TimeSpan.FromMilliseconds(100);
                    Explosion tempExplosion = new Explosion();
                    tempExplosion.MySprite.Position = new Vector2(rand.Next(20, 1100), rand.Next(20, 300));
                    explosions.Add(tempExplosion);
                }
                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Update(gameTime);
                    if (explosions[i].CanEnd)
                    {
                        explosions.Remove(explosions[i]);
                        i--;
                        continue;
                    }
                }

                //handle colours
                if (leftColour < 255)
                {
                    leftColour += 5;
                    if (leftColour >= 250)
                        leftColour = 255;
                    leftHead.MyColour = new Color(leftColour, leftColour, 255);
                }
                if (rightColour < 255)
                {
                    rightColour += 5;
                    if (rightColour >= 250)
                        rightColour = 255;
                    rightHead.MyColour = new Color(rightColour, rightColour, 255);
                }

                myBody.Update(gameTime);
                leftHead.Update(gameTime);
                rightHead.Update(gameTime);
            }
        }//end update

        

        public void SetDirection(Sprite subject, Sprite target)
        {
            Vector2 AngleVector = subject.Center - target.Center;
            AngleVector.Normalize();
            subject.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
        }

        public void SetDirection(Sprite subject, Vector2 target)
        {
            Vector2 AngleVector = subject.Center - target;
            AngleVector.Normalize();
            subject.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
        }

        public void Move(Sprite subject, Sprite target, int distance)
        {
            Vector2 targetVector = target.Center - subject.Center;
            if (targetVector.Length() > distance)
            {
                targetVector.Normalize();
                subject.Position += targetVector * 3;
            }
        }
        public void Move(Sprite subject, Sprite target, int distance, int speed)
        {
            Vector2 targetVector = target.Center - subject.Center;
            if (targetVector.Length() > distance)
            {
                targetVector.Normalize();
                subject.Position += targetVector * speed;
            }
        }

        public Vector2 GetSpawnPoint(AnimatedSprite head)
        {
            Vector2 nRad = head.Center - head.Position;
            //Vector2 nRad = head.Center;
            nRad.Normalize();
            float Angle = MathHelper.ToDegrees((float)Math.Atan2(nRad.X, nRad.Y));
            Angle += 180;
            //float Angle = MathHelper.ToDegrees(head.Rotation);
            if (Angle < 0)
                Angle += 360;
            else if (Angle == 360)
                Angle = 0;
            //float Angle = head.Rotation;
            Vector2 vRad = new Vector2(0, head.Size.Y / 2);
            float radius = vRad.Length();
            float displace = 30;
            if (Angle > 90 && Angle < 270)
                displace = 80;
            //return head.Center;
            return new Vector2(radius * ((float)(Math.Cos(Angle))) + head.Position.X + head.Size.X / 2 - displace, radius * ((float)(Math.Sin(Angle))) + head.Position.Y + head.Size.Y / 2);
            //return new Vector2(radius * ((float)(Math.Cos(Angle))) + thisCenter.X, radius * ((float)(Math.Sin(Angle))) + thisCenter.Y);
        }

        public bool GetCollision(Sprite target)
        {
            //make a distance vector
            Vector2 dist = leftHead.Position - target.Center;
            //check both heads
            if (dist.Length() < 60)
            {
                health--;
                leftColour = 0;
                leftHead.MyColour = new Color(0,0, 255);
                if (health <= 0)
                {
                    myBody.Animate(4);
                    leftHead.Animate(4);
                    rightHead.Animate(4);
                }
                return true;
            }
            dist = rightHead.Position - target.Center;
            //check both heads
            if (dist.Length() < 60)
            {
                health--;
                rightColour = 0;
                rightHead.MyColour = new Color(0,0, 255);
                if (health <= 0)
                {
                    myBody.Animate(4);
                    leftHead.Animate(4);
                    rightHead.Animate(4);
                }
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch sBatch)
        {
            
            foreach (Sprite sprite in leftNeck)
                sprite.Draw(sBatch);
            foreach (Sprite sprite in rightNeck)
                sprite.Draw(sBatch);
            leftHead.Draw(sBatch);
            rightHead.Draw(sBatch);
            myBody.Draw(sBatch);
            foreach (Explosion boom in explosions)
                boom.Draw(sBatch);
        }

    }
}
