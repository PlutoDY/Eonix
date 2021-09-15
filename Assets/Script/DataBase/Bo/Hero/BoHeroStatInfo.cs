using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eonix.SD;

namespace Eonix.DB 
{    
    [Serializable]
    public class BoHeroStatInfo : BoBase
    {
        public SDHeroStatInfo sdHeroStatInfo;

        #region InitValue
        private float hp;

        public float Hp
        {
            get { return hp; }
            private set { hp = value; }
        }

        private float power;

        public float Power
        {
            get { return power;}
            private set { power = value; }
        }

        private float defense;

        public float Defense
        {
            get { return defense; }
            private set { defense = value; }
        }
        #endregion

        #region FactorValue

        private float powerFactor;

        public float PowerFactor
        {
            get { return powerFactor; }

            private set { powerFactor = value; }
        }

        private float defenseFactor;

        public float DefenseFactor
        {
            get { return defenseFactor; }
            private set { defenseFactor = value; }
        }

        private float hpFactor;

        public float HpFactor
        {
            get { return hpFactor; }
            set { hpFactor = value; }
        }

        #endregion

        public BoHeroStatInfo(SDHeroStatInfo sdheroStatInfo)
        {
            sdHeroStatInfo = sdheroStatInfo;

            Hp = sdHeroStatInfo.hp;
            Power = sdHeroStatInfo.power;
            Defense = sdHeroStatInfo.defense;

            PowerFactor = sdHeroStatInfo.powerFactor;
            DefenseFactor = sdHeroStatInfo.defenseFactor;
            HpFactor = sdHeroStatInfo.hpFactor;
        }
    }
}
