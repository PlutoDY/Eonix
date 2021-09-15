using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.SD
{
    [Serializable]
    public class SDHeroStatInfo : StaticData
    {
        public float hp;
        public float power;
        public float powerFactor;
        public float defense;
        public float defenseFactor;
        public float hpFactor;
    }
}
