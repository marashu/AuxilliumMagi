using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AuxilliumMagi
{
    //[Serializable]
    public struct HighScoreData
    {

        public uint[] Score;
        public System.DateTime[] Times;

        public byte Count;

        public HighScoreData(byte count)
        {

            Score = new uint[count];
            Times = new System.DateTime[count];


            Count = count;
        }
    }
}
