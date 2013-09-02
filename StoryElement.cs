using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuxilliumMagi
{
    class StoryElement
    {
        private  Vector2 texturePosition = new Vector2(130, 115);
        private  Vector2 namePosition = new Vector2(800, 440);
        private  Vector2 d1Position = new Vector2(150, 500);
        private  Vector2 d2Position = new Vector2(150, 550);
        private  Vector2 d3Position = new Vector2(150, 600);

        private Texture2D speakerTexture;

        private string speakerName;
        private string dialogue1;
        private string dialogue2;
        private string dialogue3;

        public string SpeakerName { get { return speakerName; } }
        public string Dialogue1 { get { return dialogue1; } }
        public string Dialogue2 { get { return dialogue2; } }
        public string Dialogue3 { get { return dialogue3; } }


        private byte level;
        public byte Level { get { return level; } }

        //1 line of dialogue
        public StoryElement(byte lvl, string speaker, string d1)
        {
            level = lvl;
            speakerName = speaker;
            dialogue1 = d1;
            dialogue2 = "";
            dialogue3 = "";

            if (speakerName == "Aruna")
            {
                speakerTexture = GameCore.PublicStoryFireMage;
            }
            else if (speakerName == "Zaganos")
            {
                speakerTexture = GameCore.PublicStoryIceMage;
            }
            else if (speakerName == "Elder")
            {
                speakerTexture = GameCore.PublicStoryElderMage;
            }
            else
            {
                speakerName = "IAmError";
                speakerTexture = GameCore.PublicStoryElderMage;
            }
        }

        //2 lines of dialogue
        public StoryElement(byte lvl, string speaker, string d1, string d2)
        {
            level = lvl;
            speakerName = speaker;
            dialogue1 = d1;
            dialogue2 = d2;
            dialogue3 = "";

            if (speakerName == "Aruna")
            {
                speakerTexture = GameCore.PublicStoryFireMage;
            }
            else if (speakerName == "Zaganos")
            {
                speakerTexture = GameCore.PublicStoryIceMage;
            }
            else if (speakerName == "Elder")
            {
                speakerTexture = GameCore.PublicStoryElderMage;
            }
            else
            {
                speakerName = "IAmError";
                speakerTexture = GameCore.PublicStoryElderMage;
            }
        }

        //3 lines of dialogue
        public StoryElement(byte lvl, string speaker, string d1, string d2, string d3)
        {
            level = lvl;
            speakerName = speaker;
            dialogue1 = d1;
            dialogue2 = d2;
            dialogue3 = d3;

            if (speakerName == "Aruna")
            {
                speakerTexture = GameCore.PublicStoryFireMage;
            }
            else if (speakerName == "Zaganos")
            {
                speakerTexture = GameCore.PublicStoryIceMage;
            }
            else if (speakerName == "Elder")
            {
                speakerTexture = GameCore.PublicStoryElderMage;
            }
            else
            {
                speakerName = "IAmError";
                speakerTexture = GameCore.PublicStoryElderMage;
            }
        }

        public void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(speakerTexture, texturePosition, Color.White);
            sBatch.DrawString(GameCore.StoryPericles, speakerName, namePosition, Color.White);
            sBatch.DrawString(GameCore.StoryPericles, dialogue1, d1Position, Color.White);
            sBatch.DrawString(GameCore.StoryPericles, dialogue2, d2Position, Color.White);
            sBatch.DrawString(GameCore.StoryPericles, dialogue3, d3Position, Color.White);
        }

    }
}
