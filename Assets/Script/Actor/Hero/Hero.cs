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

        #region Speed Value

        public bool upSpeed = false;

        private const float nomalSpeed = 2.5f;

        private const float upgardeSpeed = 5f;

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
            CanMove = true;

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
            if (!upSpeed)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, MoveEndVector, nomalSpeed * Time.deltaTime);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, MoveEndVector, upgardeSpeed * Time.deltaTime);
            }

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

        #region Exp Setting

        public void SetExp(float getExp)
        {
            heroInfo.currentExp += getExp;

            if (heroInfo.currentExp >= heroInfo.maxExp && heroInfo.heroLevel < 10)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            heroInfo.currentExp -= heroInfo.maxExp;

            heroInfo.heroLevel++;

            if(heroInfo.heroLevel == 10) 
            { 
                heroInfo.currentExp = int.MinValue;
                heroInfo.maxExp = int.MaxValue;
            }

            heroInfo.maxExp = GameManager.SD.sdMaxExpInfos[heroInfo.heroLevel].maxExp;

            SetExp(0);
        }

        #endregion

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
        }
    }
}