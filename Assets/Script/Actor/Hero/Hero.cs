using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Battle;
using Eonix.DB;
using System;

namespace Eonix.Actor
{
    public class Hero : Actor
    {
        #region HeroInfo
        [SerializeField]
        private BoHero heroInfo;
        public BoHero HeroInfo
        {
            get { return heroInfo; }
            set { heroInfo = value; }
        }
        #endregion

        #region Value for Battle
        [SerializeField]
        private float power;
        public float Power
        {
            get { return power; }
            set 
            { 
                power = value; 
            }
        }

        [SerializeField]
        private float defense;
        public float Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        [SerializeField]
        private float currentHp;
        public float CurrentHp
        {
            get { return currentHp; }
            set 
            { 
                currentHp = value;

                if (CurrentHp <= 0)
                    Dead();
            }
        }

        [SerializeField]
        private float maxHp;
        public float MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }



        #endregion

        #region Value for Animation

        [SerializeField]
        Animator object_AnimatorCompo = null;

        #endregion

        #region Value of Skill
        [SerializeField]
        private Monster targetMonster;
        public Monster TagetMonster
        {
            get { return targetMonster; }
            set { targetMonster = value; }
        }

        private Skill currentCastingSkill;
        public Skill CurrentCastingSkill
        {
            get { return currentCastingSkill; }
            set { currentCastingSkill = value; }
        }

        private bool currentInstantiate = false;
        [SerializeField]
        private float startInstantiateTime;
        private float maxInstantiateTime = 5.0f;
        [SerializeField]
        private float instantiateTime;

        [SerializeField]
        private GameObject skillEffectObject;
        #endregion

        #region Value of Move
        [SerializeField]
        private Vector3 moveEndVector;
        public Vector3 MoveEndVector
        {
            get { return moveEndVector; }
            set 
            {
                moveEndVector = value + (Vector3.up / 2);
                IsMoveStart = true;
            }
        }

        [SerializeField]
        private bool isMoveStart = false;
        public bool IsMoveStart
        {
            get { return isMoveStart; }
            set 
            { 
                isMoveStart = value;
                object_AnimatorCompo.SetBool("isWalk", IsMoveStart);

                if (IsMoveStart)
                {
                    var vec = MoveEndVector - transform.position;
                    vec.y = 1;

                    transform.rotation = Quaternion.LookRotation(vec);
                }
                else
                {
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        private ActorController actorController;
        #endregion

        public void InitHero()
        {
            var heroStatInfo = heroInfo.boHeroStatInfo;
            var heroLevel = heroInfo.heroLevel;

            CanMove = true;

            Power = heroStatInfo.Power * (1+(heroStatInfo.DefenseFactor*10*heroLevel));
            Defense = heroStatInfo.Defense * (1 + (heroStatInfo.DefenseFactor * heroLevel));
            MaxHp = CurrentHp = heroStatInfo.Hp * (1 + (heroStatInfo.HpFactor) * heroLevel);

            object_AnimatorCompo = GetComponent<Animator>();
            actorController = Controller.ControllerManager.Instance.GetController<ActorController>();
        }

        public void Update()
        {
            if (isMoveStart){MoveStart();}

            if (!currentInstantiate) return;

            instantiateTime = Time.time - startInstantiateTime;

            if(maxInstantiateTime <= instantiateTime)
            {
                Destroy(skillEffectObject);
                /*Controller.ControllerManager.Instance.GetController<Battle.BattleController>().DamageToMonster();*/
                transform.rotation = Quaternion.identity;
                currentInstantiate = false;
            }
        }

        public void MoveStart()
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position,MoveEndVector,2.5f*Time.deltaTime);

            if (gameObject.transform.position == MoveEndVector) MoveEnd();
        }

        private void MoveEnd()
        {
            IsMoveStart = false;
            CurrentIndex = MoveIndex;
            CanMove = false;

            actorController.CheckSkipOrEndTurn();
        }

        public void PlayAttackAnimation()
        {
            switch (CurrentCastingSkill.SkillInfo.sdHeroSkillInfo.skillType)
            {
                case Define.Actor.SkillType.Nomal:
                    object_AnimatorCompo.SetBool("castingNomalSkill", true);
                    break;
                case Define.Actor.SkillType.Upgrade:
                    object_AnimatorCompo.SetBool("castingUpgradeSkill", true);
                    break;
                case Define.Actor.SkillType.Consecutive:
                    object_AnimatorCompo.SetBool("castingConsecuriveSkill", true);
                    break;
            }
        }

        public void InstantiateSkillEffect()
        {
            /*var path = CurrentCastingSkill.SkillInfo.boSk;

            var loadPath = RM.ResourceManager.Instance.LoadObject(path);

            skillEffectObject = Instantiate(loadPath).gameObject;

            var instantiateVector = actorController.baseStage.Cells[targetMonster.CurrentIndex].transform.position + Vector3.up;

            var lookPosition = instantiateVector - transform.position;

            lookPosition.y = 1;

            skillEffectObject.transform.position = instantiateVector;

            transform.rotation = Quaternion.LookRotation(lookPosition);

            currentInstantiate = true;
            startInstantiateTime = Time.time;*/
        }

        #region Animation End
        public void EndAnimation_NomalSkill()
        {
            object_AnimatorCompo.SetBool("castingNomalSkill", false);
            gameObject.transform.position = actorController.baseStage.Cells[CurrentIndex].transform.position;
            MoveEnd();
        }

        public void EndAnimation_UpgardeSkill()
        {
            object_AnimatorCompo.SetBool("castingUpgradeSkill", false);
            gameObject.transform.position = actorController.baseStage.Cells[CurrentIndex].transform.position;
            MoveEnd();
        }

        public void EndAnimation_ConsecutiveSkill()
        {
            object_AnimatorCompo.SetBool("castingConsecuriveSkill", false);
            gameObject.transform.position = actorController.baseStage.Cells[CurrentIndex].transform.position;
            MoveEnd();
        }
        #endregion

        private void Dead()
        {
            MoveEnd();

            IsLive = false;

            gameObject.SetActive(false);

            actorController.heroList.Remove(this);

            actorController.deadHeroList.Add(this);

            /*var battleController = Controller.ControllerManager.Instance.GetController<Battle.BattleController>();

            battleController.CheckEndGame();*/
        }

        private float currentPercent = 0.0f;
        private int levelUpCount = 0;

        public Tuple<int, float> AddExp(float AddExp)
        {
            currentPercent = 0.0f;
            levelUpCount = 0;

            currentPercent = SetExp(AddExp);

            if (heroInfo.heroCurrentExp >= HeroInfo.maxExp)
            {
                LevelUp();
                currentPercent = SetExp(AddExp);
            }

            DB.DataBaseManager.Instance.UpdateMyData<DB.DtoRetainedHero>(Util.SerializationUtil.DtoToParam(new DB.DtoRetainedHero()));

            return new Tuple<int, float>(levelUpCount, currentPercent);
        }

        public float SetExp(float addExp)
        {
            Debug.Log($"AddExp : {addExp}");

            Debug.Log(HeroInfo.heroCurrentExp);

            HeroInfo.heroCurrentExp += addExp;

            Debug.Log(HeroInfo.heroCurrentExp);

            return HeroInfo.heroCurrentExp / HeroInfo.maxExp;
        }

        public void LevelUp()
        {
            levelUpCount++;

            HeroInfo.heroLevel++;
            HeroInfo.heroCurrentExp -= heroInfo.maxExp;
            HeroInfo.maxExp = GameManager.SD.sdMaxExpInfos[HeroInfo.heroLevel].maxExp;
        }
    }
}