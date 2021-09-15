using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.SD;

namespace Eonix.DB
{
    [SerializeField]
    public class BoMonsterInfo : BoBase
    {
        public SDMonsterInfo sdMonsterInfo;

        [SerializeField]
        private float getExp;
        public float GetExp
        {
            get { return getExp; }
            set { getExp = value; }
        }

        public BoMonsterInfo(SDMonsterInfo sdMonsterInfo)
        {
            this.sdMonsterInfo = sdMonsterInfo;
            GetExp = this.sdMonsterInfo.getExp;
        }
    }
}