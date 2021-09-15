using Eonix.SD;
using System;
using System.Linq;
using UnityEngine;

namespace Eonix.DB
{

    [Serializable]
    public class BoStage : BoBase
    {
        public SDStage sdStage;

        public BoStage(SDStage sdStage)
        {
            this.sdStage = sdStage;
        }

        public BoStage(DtoStage dtoStage)
        {
            this.sdStage = GameManager.SD.sdStages.Where(_ => _.num == dtoStage.index).SingleOrDefault();
        }
    }
}
