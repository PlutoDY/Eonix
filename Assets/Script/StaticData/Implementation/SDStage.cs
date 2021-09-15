using System;

namespace Eonix.SD
{
    [Serializable]
    public class SDStage : StaticData
    {
        public string name;
        public int[] genMonsters;
        public string resourcePath;
    }
}
