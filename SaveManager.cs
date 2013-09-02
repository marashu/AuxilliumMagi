//File to manage storage devices
//File created Aug 06 2010
//Created by Michael C. A. Patoine

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace AuxilliumMagi
{
    public class SaveManager
    {
        //private static SaveManager myInstance = null;
        private static StorageDevice myDevice;
        public static StorageDevice MyDevice { get { return myDevice; } set { myDevice = value; } }

        public SaveManager(string fullpath)
        {
            //myDevice = new StorageDevice;
            //myDevice.OpenContainer(fullpath);
        }

        /*
        public SaveManager GetInstance()
        {
            if(myInstance == null)
            {
                myInstance = new SaveManager();
            }
            return myInstance;
        }*/
    }
}
