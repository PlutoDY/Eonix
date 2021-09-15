using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eonix.SD;

namespace Eonix.DB
{
    [Serializable]
    public class BoHeroSkillInfo : BoBase
    {
        public SDHeroSkillInfo sdHeroSkillInfo;

        public BoHeroSkillInfo(SDHeroSkillInfo sdheroSkillInfo)
        {
            sdHeroSkillInfo = sdheroSkillInfo;
        }
    }
}
