using Eonix.Define;
using Eonix.RM;
using Eonix.Stage;
using Eonix.UI;
using Eonix.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eonix
{
    public class StageManager : SingleTon<StageManager>
    {
        public bool isReady;
        private GameObject currentStage;

        private void Update()
        {
            if (!isReady)
                return;
        }

        public void OnChangeStageComplete()
        {

        }   

        public IEnumerator ChangeStage()
        {
            var sdStage = GameManager.User.boStage.sdStage;
            var resourceManager = ResourceManager.Instance; 

            if (currentStage != null)
                Destroy(currentStage);

            UI.UIWindowManager.Instance.Initialize();

            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));
            SceneManager.MoveGameObjectToScene(currentStage, SceneManager.GetSceneByName(SceneType.InGame.ToString()));

            if(GameManager.User.boHeroes.boHeroes.Count == 0)
            {
                var User = GameManager.User;

                List<int> heroNumbersIndex = new List<int>() { 100, 101 };

                User.boHeroes = new DB.BoHeroes(heroNumbersIndex);

                DB.DataBaseManager.Instance.UpdateMyData<DB.DtoRetainedHero>(SerializationUtil.DtoToParam(new DB.DtoRetainedHero()));
            }

            yield return null;
        }

        public void OnChangeBattleSceneComplete()
        {
            var path = Define.PrefabsPath.ControllerPath(PrefabsPath.Controller.ActorController);

            var actorControllerGameObject = Instantiate(RM.ResourceManager.Instance.LoadObject(path));

            actorControllerGameObject.GetComponent<Actor.ActorController>().InitController();

            path = Define.PrefabsPath.ControllerPath(PrefabsPath.Controller.BattleController);

            var battleControllerGameObject = Instantiate(RM.ResourceManager.Instance.LoadObject(path));

            battleControllerGameObject.GetComponent<Battle.BattleController>().InitController();

        }

        public IEnumerator BattleStage()
        {
            var sdStage = GameManager.User.boStage.sdStage;
            var resourceManager = ResourceManager.Instance;

            if (currentStage != null)
                Destroy(currentStage.gameObject);

            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath).gameObject);

            SceneManager.MoveGameObjectToScene(currentStage.gameObject, SceneManager.GetSceneByName(SceneType.Battle.ToString()));

            yield return null;
        }
    }
}