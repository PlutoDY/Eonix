using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Eonix.SD
{
    [Serializable]
    public class SDHeroInfo : StaticData
    {
        public string heroName;
        public int heroInfoRef;
        public int[] heroHasSkillList;
        public string heroResourcePath;
        public string heroCardResourcePath;
    }
}
