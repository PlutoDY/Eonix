using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.DB;
using System.Linq;

namespace Eonix.Actor
{
    public class Monster : Actor
    {
        [SerializeField]
        private BoMonster monsterInfo;
        public BoMonster Monsterinfo
        {
            get { return monsterInfo; }
            set { monsterInfo = value; }
        }

        #region Value of Battle
        [SerializeField]
        private int power;
        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        [SerializeField]
        private int defense;
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        [SerializeField]
        private int definiteAttackStat;
        public int DefiniteAttackStat
        {
            get { return definiteAttackStat; }
            set { definiteAttackStat = value; }
        }

        [SerializeField]
        private float hp;
        public float Hp
        {
            get { return hp; }
            set 
            { 
                hp = value;
                if (hp <= 0) Dead();
            }
        }
        #endregion

        #region Value of Move
        [SerializeField]
        MonsterMoveHelper monsterMoveHelper;

        [SerializeField]
        private Hero targetHero;
        public Hero TargetHero
        {
            get { return targetHero; }
            set { targetHero = value; }
        }

        private Vector3 moveEndVector;
        public Vector3 MoveEndVector
        {
            get { return moveEndVector; }
            set { moveEndVector = value + Vector3.up/2; }
        }

        [SerializeField]
        ActorController actorController;

        [SerializeField]
        private bool nowMoving = false;
        public bool NowMoving
        {
            get { return nowMoving; }
            set { nowMoving = value; }
        }

        [SerializeField]
        private List<int> pathWay = new List<int>();
        public List<int> PathWay
        {
            get { return pathWay; }
            set { pathWay = value; }
        }
        #endregion

        public void Update()
        {
            if(NowMoving)
            {
                Moving();
            }
        }

        #region Method For Init
        public void MonsterInit()
        {
            var monsterStatInfo = monsterInfo.boMonsterStatInfo;

            Power = monsterStatInfo.Power;
            Defense = monsterStatInfo.Defense;
            DefiniteAttackStat = monsterStatInfo.DefiniteAttackStat;
            Hp = monsterStatInfo.Hp;

            monsterMoveHelper = GetComponent<MonsterMoveHelper>();
        }

        public void MonsterValueInit()
        {
            actorController = Controller.ControllerManager.Instance.GetController<ActorController>();
            monsterMoveHelper.BaseStage = actorController.baseStage;
            monsterMoveHelper.monsters = actorController.monsterList;
            monsterMoveHelper.CurrentMoveMonster = this;
        }

        public void InitMonsterMove()
        {
            targetHero = monsterMoveHelper.SetTargetHero(actorController.heroList);

            monsterMoveHelper.InitListValues();

            PathWay = monsterMoveHelper.CalculationMove();

            if (PathWay.Count == 0)
            {
                MoveIndex = CurrentIndex;
            }
            else
            {
                MoveIndex = PathWay.Last();
            }

            MoveEndVector = actorController.baseStage.Cells[MoveIndex].transform.position;

            NowMoving = true;
        }
        #endregion

        #region Method For Move

        private void Moving()
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, MoveEndVector, 2.5f * Time.deltaTime);

            if(gameObject.transform.position == MoveEndVector)
            {
                EndMoving();
            }
        }

        private void EndMoving()
        {
            NowMoving = false;
            CurrentIndex = MoveIndex;
            CanMove = false;

            actorController.EndMonsterMove = true;
        }
        #endregion

        private void Dead()
        {
            var battleController = Controller.ControllerManager.Instance.GetController<Battle.BattleController>();

            IsLive = false;
            battleController.CurrentGetExp += Monsterinfo.boMonsterInfo.GetExp;

            gameObject.SetActive(false);
            actorController.monsterList.Remove(this);
            actorController.deadMonsterList.Add(this);

            /*battleController.CheckEndGame();*/
        }
    }
}
