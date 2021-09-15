using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.SD
{
    [Serializable]
    public class SDMonsterStatInfo : StaticData
    {
        public int power;
        public int defense;
        public int definiteAttackStat;
        public float hp;
    }
}
