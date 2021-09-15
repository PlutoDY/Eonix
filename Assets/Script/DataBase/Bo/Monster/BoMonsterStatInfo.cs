using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eonix.SD;

namespace Eonix.DB
{
    [Serializable]
    public class BoMonsterStatInfo : BoBase
    {
        public SDMonsterStatInfo sdMonsterStatInfo;

        private int power;
        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        private int defense;
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        private int definiteAttackStat;
        public int DefiniteAttackStat
        {
            get { return definiteAttackStat; }
            set { definiteAttackStat = value; }
        }

        private float hp;
        public float Hp
        {
            get { return hp;}
            set { hp = value; }
        }

        private float maxHp;
        public float MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }

        public BoMonsterStatInfo(SDMonsterStatInfo sdMonsterStatInfo)
        {
            this.sdMonsterStatInfo = sdMonsterStatInfo;

            Power = sdMonsterStatInfo.power;
            Defense = sdMonsterStatInfo.defense;
            DefiniteAttackStat = sdMonsterStatInfo.definiteAttackStat;
            MaxHp = Hp = sdMonsterStatInfo.hp;
        }
    }
}
