using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Define;
using System.Linq;
using Eonix.DB;

namespace Eonix.UI
{
    public class FristDungeonUI : UIWindow
    {
        int warpStageIndex;

        public override void Start()
        {
            isOpen = true;

            canCloseESC = true;

            base.Start();
        }

        public void WarpVoid0()
        {
            warpStageIndex = 100; Warp();
        }

        public void WarpVoid1()
        {
            warpStageIndex = 101; Warp();
        }

        public void WarpVoid2()
        {
            warpStageIndex = 102; Warp();
        }

        void Warp()
        {
            var stageManager = StageManager.Instance;
            var user = GameManager.User;


            user.boStage.sdStage = GameManager.SD.sdStages.Where(_ => _.num == warpStageIndex).SingleOrDefault();

            GameManager.Instance.LoadScene(SceneType.Battle,stageManager.BattleStage(), stageManager.OnChangeBattleSceneComplete);
        }
    }
}
