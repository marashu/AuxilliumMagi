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
    public class GameState
    {
        private bool visible;
        private bool enabled;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }


        public GameState(Game game)
        {
            Visible = false;
            Enabled = false;
        }

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }
        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;

        }

        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch sBatch)
        {

        }
    }//end class
}
