using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.UI;
using Eonix.Actor;
using System;

namespace Eonix.Battle
{
    #region Using
    using PrefabsPath = Define.PrefabsPath;
    using PrefabsUIs = Define.PrefabsPath.PrefabsUIs;
    using RM = RM.ResourceManager;

    using Controller = Eonix.Controller.Controller;
    using ControllerManager = Eonix.Controller.ControllerManager;

    using Battle = Eonix.Define.Battle;
    using BattlePhase = Eonix.Define.Battle.BattlePhase;

    using State = BattleCurrentStateViewerHelper.StateText.State;
    #endregion

    public class BattleController : Controller
    {
        #region Component

        // 전투 UI
        [SerializeField]
        UIBattle uIBattle;
        
        // 전투 종료 UI
        [SerializeField]
        EndBattleUI endBattleUI;

        // 전투 UI에서 주사위 값을 정해진 값에 따라서 이미지 변경해주는 스크립트
        BattleDiceHelper battleDiceHelper;

        // 현재 전투 상태를 나타내 주는 UI Object의 Text를 변경해 주는 스크립트
        BattleCurrentStateViewerHelper battleCurrentStateViewerHelper;

        // 전투에서 스킬 사용할 때 사용되는 스크립트
        BattleHeroSkillHelper battleHeroSkillHelper;

        // 현재 몬스터 또는 영웅의 Hp Bar와 Hp를 관리 해주는 스크립트
        BattleHpHelper battleHpHelper;

        // 전투 중 주사의의 값을 일정 범위에서 정해주는 스크립트
        BattleCalculateHelper battleCalculateHelper;
        #endregion

        #region Battle Target Component
        [SerializeField]
        private Hero hero;
        public Hero Hero
        {
            get { return hero; }
            set { hero = value; }
        }

        [SerializeField]
        private Monster monster;
        public Monster Monster
        {
            get { return monster; }
            set { monster = value; }
        }
        #endregion

        [SerializeField]
        private float currentGetExp;
        public float CurrentGetExp
        {
            get { return currentGetExp; }
            set { currentGetExp = value; }
        }

        #region Battle UI

        private bool isMoveStart;
        public bool IsMoveStart
        {
            get { return isMoveStart; }
            set { isMoveStart = value; }
        }
        #endregion

        #region Phase Value

        private bool canSkipPhase = true;
        public bool CanSkipPhase
        {
            set { canSkipPhase = value; }
        }

        #region Wait Time Value
        private bool nowWating = false;
        public bool NowWating { get { return nowWating; } }

        private const float valueChangedWait = 1f;
        private const float diceImageChangedWait = 1f;

        private float startWaitTime;
        private float currentWatingTime;

        private bool isChangedImage = false;
        #endregion

        #region Battle Result Value

        private int heroValue;
        private int monsterValue;
        private bool isWin;

        private float skillDamagePercent;

        private int[] skillHitRatePercent = new int[3];

        #endregion

        [SerializeField]
        private BattlePhase currentBattlePhase = BattlePhase.Power_Resistance;

        [SerializeField]
        private int attackCount = 1;

        [SerializeField]
        private int attackSuccessCount = 0;

        [SerializeField]
        private float heroGiveDamage = 0.0f;

        [SerializeField]
        private float monsterGiveDamage = 0.0f;

        [SerializeField]
        private Skill currentCastingSkill;
        #endregion

        #region Init Methods
        public override void Start()
        {
            battleCalculateHelper = GetComponent<BattleCalculateHelper>();
            battleDiceHelper = GetComponent<BattleDiceHelper>();
            battleCurrentStateViewerHelper = GetComponent<BattleCurrentStateViewerHelper>();
            battleHeroSkillHelper = GetComponent<BattleHeroSkillHelper>();
            battleHpHelper = GetComponent<BattleHpHelper>();

            base.Start();

            InstantiateUI();
        }

        public void Update()
        {
            if (nowWating)
            {
                currentWatingTime = Time.time - startWaitTime;

                if((valueChangedWait <= currentWatingTime) && !isChangedImage)
                {
                    battleDiceHelper.SetDicesImage(isWin);

                    SetStateAndDamagerPercent();

                    isChangedImage = true;
                }
                else if(valueChangedWait + diceImageChangedWait <= currentWatingTime)
                {
                    // 다음 Phase로 넘김

                    nowWating = false;
                    isChangedImage = false;

                    NextPhase();
                }
            }
        }

        private void SetStateAndDamagerPercent()
        {
            if (currentBattlePhase == BattlePhase.Power_Resistance)
            {
                if (isWin)
                {
                    battleCurrentStateViewerHelper.SetStateText(State.OverPowering);
                    battleCurrentStateViewerHelper.SetDamagePercentText(100);
                }
                else
                {
                    battleCurrentStateViewerHelper.SetStateText(State.NotOverPowering);
                }
            }
            else if (currentBattlePhase == BattlePhase.Power_Defense)
            {
                if (isWin)
                {
                    battleCurrentStateViewerHelper.SetStateText(State.AttackSuccess);
                    battleCurrentStateViewerHelper.SetDamagePercentText((heroValue - monsterValue) * 10);
                }
                else
                {
                    battleCurrentStateViewerHelper.SetStateText(State.End);
                }
            }
            else if(currentBattlePhase == BattlePhase.SkillAttack)
            {
                if (isWin)
                {
                    battleCurrentStateViewerHelper.SetStateText(State.Win);
                }
                else
                {
                    battleCurrentStateViewerHelper.SetStateText(State.Faild);
                }
            }

        }
        
        private void InstantiateUI()
        {
            var path = Define.PrefabsPath.UIPath(PrefabsUIs.BattleUI);
            var instantiatedObject = Instantiate(RM.Instance.LoadObject(path)).gameObject;
            uIBattle = instantiatedObject.GetComponent<UIBattle>();
            uIBattle.InitWindow();

            path = PrefabsPath.UIPath(PrefabsUIs.EndBattleUI);
            instantiatedObject = Instantiate(RM.Instance.LoadObject(path)).gameObject;
            endBattleUI = instantiatedObject.GetComponent<EndBattleUI>();

            InitDices();

            InitStateText();

            InitSkillObject();
        }

        private void InitDices()
        {
            battleDiceHelper.InitDices(uIBattle.Dices);
        }

        private void InitStateText()
        {
            battleCurrentStateViewerHelper.InitStateObject(uIBattle.StateObject_Text, uIBattle.DamagePercentObject_Text);
            battleCurrentStateViewerHelper.InitStatObject(uIBattle.StatName_Text[0], uIBattle.StatName_Text[1]);

            battleHpHelper.HpBarsInit(uIBattle.HpBars);
        }

        private void InitSkillObject()
        {
            battleHeroSkillHelper.InitSkill(uIBattle.Skills);
        }
        #endregion

        #region Battle Method

        public void BattleStart()
        {
            ResetValues();

            uIBattle.Open();

            battleDiceHelper.StartRolling();

            GameManager.Input.MouseAction = null;
            GameManager.Input.KeyAction -= StopRolling;
            GameManager.Input.KeyAction += StopRolling;

            battleHeroSkillHelper.InitSkillInfo(Hero);
            battleCurrentStateViewerHelper.ResetTexts();

            battleHpHelper.ResetImageAndText(Hero, Monster);
        }

        private void ResetValues()
        {
            currentBattlePhase = BattlePhase.Power_Resistance;

            attackCount = 1;
            attackSuccessCount = 0;
            heroGiveDamage = 0;
            monsterGiveDamage = 0;

            CanSkipPhase = true;

            currentCastingSkill = null;
        }

        public void StopRolling()
        {
            if (Input.GetKeyDown(KeyCode.Space) && canSkipPhase)
            {
                canSkipPhase = false;

                var tu = new Tuple<int, int, bool>(0, 0, false);

                switch (currentBattlePhase)
                {
                    case BattlePhase.Power_Resistance:
                        tu = battleCalculateHelper.CalculationDice(hero.HeroInfo.atk, monster.DefiniteAttackStat);
                        break;
                    case BattlePhase.Power_Defense:
                        tu = battleCalculateHelper.CalculationDice(hero.HeroInfo.atk, monster.Defense);
                        break;
                    case BattlePhase.SkillAttack:
                        tu = battleCalculateHelper.CalculationDice(currentCastingSkill.HitRate);
                        --attackCount;
                        break;
                }

                heroValue = tu.Item1;
                monsterValue = tu.Item2;
                isWin = tu.Item3;

                if ((currentBattlePhase != BattlePhase.SkillAttack) && (isWin))
                {
                    if(currentBattlePhase == BattlePhase.Power_Resistance)
                    {
                        SetDamage(100,true);
                    }
                    else
                    {
                        SetDamage((heroValue - monsterValue)*10);
                    }
                }
                else if(currentBattlePhase == BattlePhase.SkillAttack)
                {
                    DamageCalculation();
                }

                

                battleDiceHelper.SetDicesValue(heroValue, monsterValue);

                startWaitTime = Time.time;
                nowWating = true;
            }

        }

        public void RestartRolling()
        {
            battleDiceHelper.StartRolling();
        }

        public void NextPhase()
        {
            if(isWin && ((currentBattlePhase == BattlePhase.Power_Resistance) || (currentBattlePhase == BattlePhase.Power_Defense)))
            {
                battleCurrentStateViewerHelper.SetStateText(State.Start);
                currentBattlePhase = BattlePhase.SkillSelect;
            }
            else if(!isWin && (currentBattlePhase == BattlePhase.Power_Resistance))
            {
                currentBattlePhase = BattlePhase.Power_Defense;
            }
            else if(!isWin && (currentBattlePhase == BattlePhase.Power_Defense))
            {
                currentBattlePhase = BattlePhase.HpAdjustment;
                battleCurrentStateViewerHelper.SetStateText(State.End);
            }
            else if((currentBattlePhase == BattlePhase.SkillAttack) && attackCount == 0)
            {
                currentBattlePhase = BattlePhase.HpAdjustment;
            }
            else if(currentBattlePhase == BattlePhase.HpAdjustment)
            {
                currentBattlePhase = BattlePhase.End;
            }

            CheckBattlePhase();
        }

        private void SetDamage(int value, bool force = false)
        {
            skillDamagePercent = value * 0.01f;

            skillHitRatePercent[0] = value;

            if (force) skillHitRatePercent[1] = 100;
            else skillHitRatePercent[1] = ((value - 5) >= 0) ? value - 5 : 0;

            skillHitRatePercent[2] = ((value + 10) <= 100) ? value + 10 : 100;

            battleHeroSkillHelper.SetSkillsExplain(skillDamagePercent, skillHitRatePercent);
        }

        private void CheckBattlePhase()
        {
            switch (currentBattlePhase)
            {
                case BattlePhase.Power_Resistance:
                    battleCurrentStateViewerHelper.SetStatText(0);
                    break;
                case BattlePhase.Power_Defense:
                    battleCurrentStateViewerHelper.SetStatText(2);
                    break;
                case BattlePhase.SkillSelect:
                    battleCurrentStateViewerHelper.SetStatText(2);
                    uIBattle.SetActiveSkillImages(true);
                    break;
                case BattlePhase.HpAdjustment:
                    HpAdjusting();
                    break;
                case BattlePhase.End:
                    StartCloseBattleUI();
                    break;
                default:
                    break;
            }

            if((currentBattlePhase != BattlePhase.SkillSelect) && (currentBattlePhase != BattlePhase.HpAdjustment))
            {
                canSkipPhase = true;
                RestartRolling();
            }
        }

        public void SkillSelected(Skill skill)
        {
            currentBattlePhase = BattlePhase.SkillAttack;

            currentCastingSkill = skill;

            if (currentCastingSkill.SkillInfo.sdHeroSkillInfo.skillType == Define.Actor.SkillType.Consecutive)
            {
                attackCount = 4;
            }
            else attackCount = 1;

            uIBattle.SetActiveSkillImages(false);
            NextPhase();

            CanSkipPhase = true;
        }

        public void DamageCalculation()
        {
            if (isWin)
            {
                heroGiveDamage += currentCastingSkill.CurrentSkillDamage;
                attackSuccessCount++;
            }
            else
            {
                monsterGiveDamage += currentCastingSkill.CurrentSkillDamage;
            }
        }

        private void HpAdjusting()
        {
            if (attackSuccessCount != 0)
            {
                var monsterMaxHp = Monster.Monsterinfo.boMonsterStatInfo.MaxHp;

                battleHpHelper.SetMonsterHpBar(Monster.Hp, monsterMaxHp, heroGiveDamage);
            }
            else
            {
                battleHpHelper.SetHeroHpBar(Hero.HeroInfo.currentHp ,Hero.HeroInfo.Hp, monsterGiveDamage);
            }

        }

        public void StartCloseBattleUI()
        {
            uIBattle.CloseStart();

            hero.CanMove = false;

            var actorController = ControllerManager.Instance.GetController<ActorController>();

            GameManager.Input.KeyAction = null;
            GameManager.Input.MouseAction += actorController.HeroMove;

            actorController.CheckSkipOrEndTurn();
        }

        public void HpAdjustmentObject()
        {
            if (attackSuccessCount != 0)
            {
                monster.Hp -= heroGiveDamage;
            }
            else
            {
                hero.HeroInfo.currentHp -= (int)monsterGiveDamage;
            }
        }
        #endregion

        #region End Battle Method

        public void EndGame(List<Hero> liveHeroes, List<Hero> deadHeroes)
        {
            endBattleUI.AddExp = (int)currentGetExp;

            endBattleUI.LiveHeroList = liveHeroes;
            endBattleUI.DeadHeroList = deadHeroes;

            endBattleUI.Open();
        }

        #endregion
    }
}
