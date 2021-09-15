using System;
using Eonix.Define;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.SD
{
    [Serializable]
    public class SDHeroSkillInfo : StaticData
    {
        public Define.Actor.SkillType skillType;
        public string skillName;
        public float skillDamage;
        public string skillImageResourcePath;
    }
}
