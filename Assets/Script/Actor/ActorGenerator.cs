using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.DB;
using Eonix.Stage;
using Eonix.RM;

namespace Eonix.Actor
{
    using PrefabsPanels = Define.PrefabsPath.PrefabsPanels;

    public class ActorGenerator : MonoBehaviour
    {
        #region OutterScript & Object
        BaseStage baseStage;
        #endregion

        #region Hero & Monster List
        List<BoHero> boHeroes = new List<BoHero>();
        List<BoMonster> boMonsters = new List<BoMonster>();
        #endregion

        #region Value of Instantiate Actors
        string currentInstantiateObjectPath = string.Empty;
        GameObject currentLoadGameObject = null;
        GameObject currentInstantiateGameObject = null;

        BoHero currentInstantiateHero_BoHeroCompo = null;
        BoMonster currentinstantiateMonster_BoMonsterCompo = null;
        
        [SerializeField]
        Transform heroGroupObject_TransformCompo = null;
        [SerializeField]
        Transform monsterGroupObject_TrasformCompo = null;

        public Transform HeroGroupObject_TransformCompo { get { return heroGroupObject_TransformCompo; } }
        public Transform MonsterGroupObject_TransformCompo { get { return monsterGroupObject_TrasformCompo; } }

        Hero currentInstantiateHero_HeroCompo = null;
        Monster currentInstantiateMonster_MonsterCompo = null;
        #endregion

        #region Value of Can Instantiate Actors
        int stageSize = -1;
        int stageWidth = -1;

        int checkIndex = -1;

        int valueIncDec = 0;

        List<int> instantiatedIndex = new List<int>();

        int instantiateMonsterCounter = 0;
        #endregion

        public void Awake()
        {
            heroGroupObject_TransformCompo = transform.GetChild(0).transform;
            monsterGroupObject_TrasformCompo = transform.GetChild(1).transform;
        }

        public void GenerationActors(BaseStage baseStage ,List<BoHero> boHeroes, List<BoMonster> boMonsters)
        {
            this.baseStage = baseStage;

            this.boHeroes = boHeroes;
            this.boMonsters = boMonsters;

            stageSize = baseStage.Cells.Length-1;
            stageWidth = baseStage.Width;

            InstantiateHeroes();
            InstantiateMonster();
        }

        private void InstantiateHeroes()
        {
            var RM = ResourceManager.Instance;

            foreach(BoHero boHero in boHeroes)
            {
                valueIncDec = 0;

                currentInstantiateHero_BoHeroCompo = boHero;

                currentInstantiateObjectPath = boHero.boHeroInfo.sdHeroinfo.heroResourcePath;

                currentLoadGameObject = RM.LoadObject(currentInstantiateObjectPath);
                currentInstantiateGameObject = Instantiate(currentLoadGameObject, heroGroupObject_TransformCompo.transform).gameObject;

                Check_CanInstanceHero();

                var sapwnVector = baseStage.Cells[checkIndex].transform.position;

                currentInstantiateGameObject.transform.position = sapwnVector;

                currentInstantiateHero_HeroCompo = currentInstantiateGameObject.GetComponent<Hero>();

                currentInstantiateHero_HeroCompo.HeroInfo = currentInstantiateHero_BoHeroCompo;
                currentInstantiateHero_HeroCompo.CurrentIndex = checkIndex;
                currentInstantiateHero_HeroCompo.CanMove = true;
            }
        }

        private void InstantiateMonster()
        {
            var RM = ResourceManager.Instance;

            foreach(BoMonster boMonster in boMonsters)
            {
                valueIncDec = 0;

                currentinstantiateMonster_BoMonsterCompo = boMonster;

                currentInstantiateObjectPath = currentinstantiateMonster_BoMonsterCompo.boMonsterInfo.sdMonsterInfo.monsterResourcePath;

                currentLoadGameObject = RM.LoadObject(currentInstantiateObjectPath);

                currentInstantiateGameObject = Instantiate(currentLoadGameObject, monsterGroupObject_TrasformCompo).gameObject;

                Check_CanInstanceMonster();

                instantiateMonsterCounter++;

                var spawnVector = baseStage.Cells[checkIndex].transform.position + (Vector3.up / 2);

                currentInstantiateGameObject.transform.position = spawnVector;

                currentInstantiateMonster_MonsterCompo = currentInstantiateGameObject.GetComponent<Monster>();

                currentInstantiateMonster_MonsterCompo.Monsterinfo = currentinstantiateMonster_BoMonsterCompo;
                currentInstantiateMonster_MonsterCompo.CurrentIndex = checkIndex;

                currentInstantiateMonster_MonsterCompo.MonsterInit();
            }
        }

        private void Check_CanInstanceHero()
        {
            switch (currentInstantiateHero_BoHeroCompo.heroPartyIndex)
            {
                case 0:
                    checkIndex = (stageWidth / 2) + stageWidth + valueIncDec;
                    break;
                case 1:
                    checkIndex = (stageWidth/2) - 1 + valueIncDec;
                    break;
                case 2:
                    checkIndex = (stageWidth/2) + 1 + valueIncDec;
                    break;
            }

            CheckIndex();
        }

        private int Check_CanInstanceMonster()
        {
            switch (instantiateMonsterCounter)
            {
                case 0:
                    checkIndex = stageSize - stageWidth/2 - stageWidth;
                    break;
                case 1:
                    checkIndex = stageSize - (stageWidth/2) + 1;
                    break;
                case 2:
                    checkIndex = stageSize - (stageWidth / 2) - 1;
                    break;
            }

            CheckIndex();

            return checkIndex;
        }

        private int CheckIndex() 
        {
            if (checkIndex < 0 || checkIndex >= stageSize)
            {
                IncDecValue();
                return CheckIndex();
            }
            else if (baseStage.Cells[checkIndex].PanelName == PrefabsPanels.Floor3X3)
            {
                IncDecValue();
                return CheckIndex();
            }
            else if (instantiatedIndex.Contains(checkIndex))
            {
                IncDecValue();
                return CheckIndex();
            }

            return checkIndex;
        }

        private void IncDecValue()
        {
            if((valueIncDec >= 0))
            {
                valueIncDec++;
            }
            else if(valueIncDec >= stageWidth + 1)
            {
                valueIncDec = -1;
            }
            else if(valueIncDec < 0)
            {
                valueIncDec--;
            }
        }
    }
}