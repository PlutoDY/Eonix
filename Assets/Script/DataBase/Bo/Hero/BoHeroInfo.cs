using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eonix.SD;

namespace Eonix.DB
{
    [Serializable]
    public class BoHeroInfo : BoBase
    {
        public SDHeroInfo sdHeroinfo;

        public BoHeroInfo(SDHeroInfo sdheroInfo)
        {
            sdHeroinfo = sdheroInfo;
        }

    }
}
