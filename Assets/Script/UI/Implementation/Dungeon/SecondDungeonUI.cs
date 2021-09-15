using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eonix.UI
{
    public class SecondDungeonUI : UIWindow
    {
        int warpStageIndex;

        public override void Start()
        {
            isOpen = true;

            canCloseESC = true;

            base.Start();


        }

        public void WarpDreamyForest0()
        {
            warpStageIndex = 200; Warp();
        }

        public void WarpDreamyForest1()
        {
            warpStageIndex = 201; Warp();
        }

        public void WarpDreamyForest2()
        {
            warpStageIndex = 202; Warp();
        }

        void Warp()
        {

            var stageManager = StageManager.Instance;
            var user = GameManager.User;


            user.boStage.sdStage = GameManager.SD.sdStages.Where(_ => _.num == warpStageIndex).SingleOrDefault();

            GameManager.Instance.LoadScene(Define.SceneType.Battle,stageManager.BattleStage(), stageManager.OnChangeBattleSceneComplete);
        }
    }
}