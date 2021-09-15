using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.SD
{
    [Serializable]
    public class SDMonsterInfo : StaticData
    {
        public string name;
        public int monsterStatInfoRef;
        public string monsterResourcePath;
        public float getExp;
    }
}
