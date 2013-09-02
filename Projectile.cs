//projectile class created on July 11 2010


using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuxilliumMagi
{
    public class Projectile
    {

        //enum for projectile types
        public enum ProjectileType
        {
            Fire,
            Ice,
            Antipode,
            Arrow,
            Stun,
            Counter
        }

        //private Sprite myDot;

        private Vector2 movementDir = Vector2.Zero;
        public Vector2 MovementDir { get { return movementDir; } }
        private ProjectileType myType;
        public ProjectileType MyType { get { return myType; } }

        private float mySpeed = 1;
        //private Vector2 center = Vector2.Zero;
        //public Vector2 Center { get { return center; } set { center = value; } }

        private bool bReflected = false;
        public bool Reflected { get { return bReflected; } set { bReflected = value; } }

        //sprite for the projectile
        protected AnimatedSprite mySprite;
        public AnimatedSprite MySprite { get { return mySprite; } }

        public Projectile(ProjectileType newType)
        {
            //myDot = new Sprite(GameCore.Dot);
            myType = newType;
            switch (myType)
            {
                case ProjectileType.Fire:
                    mySprite = new AnimatedSprite(GameCore.PublicFireballTexture);
                    //center = mySprite.Center;
                    break;
                case ProjectileType.Ice:
                    mySprite = new AnimatedSprite(GameCore.PublicIceballTexture);
                    //center = new Vector2(30, 90);
                    break;
                case ProjectileType.Antipode:
                    mySprite = new AnimatedSprite(GameCore.PublicFireiceballTexture);
                    //center = mySprite.Center;
                    break;
                case ProjectileType.Arrow:
                    mySprite = new AnimatedSprite(GameCore.PublicArrowTexture);
                    //center = mySprite.Center;
                    break;
                case ProjectileType.Stun:
                    mySprite = new AnimatedSprite(GameCore.PublicStunTexture);
                    //center = mySprite.Center;
                    break;
                case ProjectileType.Counter:
                    mySprite = new AnimatedSprite(GameCore.PublicCounterTexture);
                   // center = mySprite.Center;
                    break;
                default:
                    mySprite = new AnimatedSprite(GameCore.PublicFireballTexture);
                    break;
            }
            
        }

        public void Update(GameTime gameTime)
        {
            //myDot.Position = mySprite.Position;
            mySprite.Update(gameTime);
            
        }

        public void BoostSpeed()
        {
            mySpeed += 0.3f;
        }

        public void Move(Vector2 destination)
        {
            destination -= mySprite.Center;
            movementDir = destination;
            destination.Normalize();
           
            destination *= 3.4f;
            if (bReflected)
                destination *= -1;

            destination *= mySpeed;
            mySprite.Position += destination;
        }

        public void Draw(SpriteBatch sBatch)
        {
            mySprite.Draw(sBatch);
            //myDot.Draw(sBatch);
        }

    }

}
