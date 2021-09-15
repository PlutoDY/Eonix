using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.SD;
using System.Linq;
using System;

namespace Eonix.DB
{
    [Serializable]
    public class BoMonster : BoBase
    {
        public BoMonsterInfo boMonsterInfo;
        public BoMonsterStatInfo boMonsterStatInfo;

        public BoMonster(int index)
        {
            var SD = GameManager.SD;
            boMonsterInfo = new BoMonsterInfo(SD.sdMonsterInfos.Where(_ => _.num == index).SingleOrDefault());
            boMonsterStatInfo = new BoMonsterStatInfo(SD.sdMonsterStatInfos.Where
                (_ => _.num == boMonsterInfo.sdMonsterInfo.monsterStatInfoRef).SingleOrDefault());
        }
    }
}