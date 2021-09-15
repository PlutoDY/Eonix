using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Stage;
using Eonix.DB;
using Eonix.SD;
using Eonix.Battle;
using System.Linq;
using System;

namespace Eonix.Actor
{
    using Controller = Eonix.Controller.Controller;
    using ControllerManager = Eonix.Controller.ControllerManager;
    using MouseEvent = Define.Define.MouseEvent;
    using Layer = Define.Layer;

    public class ActorController : Controller
    {
        [SerializeField]
        public BaseStage baseStage;

        [SerializeField]
        public BoUser User;

        [SerializeField]
        public StaticDataModule SD;

        public List<Hero> heroList = new List<Hero>();
        public List<Monster> monsterList = new List<Monster>();

        public List<Hero> deadHeroList = new List<Hero>();
        public List<Monster> deadMonsterList = new List<Monster>();

        public ActorGenerator actorGenerator;
        public ActorTranslator actorTranslator;


        #region Value of HeroMove
        private Camera stageCam;

        private Ray ray;
        private RaycastHit hit;

        int _mask = (1 << (int)Layer.Player) | (1 << (int)Layer.Checker);

        [SerializeField]
        private Hero clickedHero;

        [SerializeField]
        private Hero previousClickedHero;

        [SerializeField]
        private GameObject clickedChecker;

        [SerializeField]
        private Cell clickedCell;

        [SerializeField]
        private Monster clickedMonster;

        private bool isActive = false;
        #endregion

        #region Value of Monster

        private bool endMonsterMove;
        public bool EndMonsterMove
        {
            get { return endMonsterMove; }
            set 
            {
                endMonsterMove = value;

                if (EndMonsterMove && (monsterMoveCounter < monsterList.Count))
                {
                    StartMonsterMove();
                }
                else if (monsterMoveCounter >= monsterList.Count)
                {
                    monsterMoveCounter = 0;
                    StartHeroMove();
                }
            }
        }

        private int monsterMoveCounter = 0;
        #endregion

        private void Awake()
        {
            baseStage = BaseStage.Instance;

            stageCam = baseStage.StageCam;

            User = GameManager.User;

            SD = GameManager.SD;
        }

        public override void Start()
        {
            base.Start();

            GenerationActros();
            TanslatorActros();

            InitMonsterListForHelper();

            GameManager.Input.MouseAction -= HeroMove;
            GameManager.Input.MouseAction += HeroMove;
        }

        private void GenerationActros()
        {
            actorGenerator = GetComponent<ActorGenerator>();

            var boHeroes = User.boHeroes.boHeroes.Where(_ => _.inParty == true).ToList();

            var monsterIds = User.boStage.sdStage.genMonsters;

            var boMonsters = new List<BoMonster>();

            foreach(int monsterId in monsterIds)
            {
                boMonsters.Add(new BoMonster(monsterId));
            }

            actorGenerator.GenerationActors(baseStage,boHeroes, boMonsters);

        }

        private void TanslatorActros()
        {
            actorTranslator = GetComponent<ActorTranslator>();

            var listTuple = actorTranslator.TranslatorActors();

            heroList = listTuple.Item1;
            monsterList = listTuple.Item2;
        }

        private void InitMonsterListForHelper()
        {
            foreach(Monster monster in monsterList)
            {
                monster.MonsterValueInit();
            }
        }

        public void HeroMove(MouseEvent evt)
        {
            ray = stageCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, int.MaxValue, _mask))
            {
                var hitObject = hit.transform.gameObject;

                if (hitObject == null) return;

                if (hitObject.layer == (int)Layer.Player)
                {
                    previousClickedHero = clickedHero;
                    clickedHero = heroList.Where(_ => _ == hitObject.GetComponent<Hero>()).SingleOrDefault();

                    ClickHero();
                    return;
                } 
                else if(hit.transform.gameObject.layer == (int)Layer.Checker)
                {
                    previousClickedHero = clickedHero;
                    clickedChecker = hitObject;
                    ClickChecker();
                    return;
                }
            }
        }

        #region ClickHero
        private void ClickHero()
        {
            if(clickedHero == null)
            {
                Debug.LogError("Hit Object is exist but Hero Component is not exist");
                return;
            }

            if (!clickedHero.CanMove) return;

            CheckClickedObject();
        }

        private void CheckClickedObject()
        {
            if(clickedHero == previousClickedHero)
            {
                ShowChecker(clickedHero.CurrentIndex, isActive);
                isActive = !isActive;
            }
            else if(previousClickedHero == null)
            {
                ShowChecker(clickedHero.CurrentIndex, true);
            }
            else if(clickedHero != previousClickedHero)
            {
                ShowChecker(previousClickedHero.CurrentIndex, false);
                ShowChecker(clickedHero.CurrentIndex, true);
            }
        }

        private void ShowChecker(int index, bool active)
        {
            var middleCell = baseStage.Cells[index];

            var middleCell_NeighborCells = middleCell.neighborCell;

            foreach(Tuple<Cell, int> tuple in middleCell_NeighborCells)
            {
                var cellObject = tuple.Item1;

                cellObject.Checker.SetActive(active);

                CheckMonsterForChecker(cellObject);
            }
        }

        private void CheckMonsterForChecker(Cell cell)
        {
            var rendererCompo = cell.Checker.GetComponentInChildren<Renderer>();

            foreach(Monster monster in monsterList)
            {
                cell.Checker.transform.position = new Vector3(cell.Checker.transform.position.x, 0, cell.Checker.transform.position.z);

                if (monster.CurrentIndex == cell.Index)
                {
                    rendererCompo.material.color = Color.blue;
                    cell.Checker.transform.position = new Vector3(cell.Checker.transform.position.x, 1, cell.Checker.transform.position.z);
                    break;
                }

                rendererCompo.material.color = Color.red;
            }
        }
        #endregion

        #region ClickChekcer
        private void ClickChecker()
        {
            if(clickedChecker == null)
            {
                Debug.LogError("Hit Object is exist but Checker is not exist");
                return;
            }

            clickedCell = clickedChecker.GetComponentInParent<Cell>();

            CheckMonsterForBattle(clickedCell);

            ShowChecker(previousClickedHero.CurrentIndex, false);
        }

        private void CheckMonsterForBattle(Cell cell)
        {
            var checker = monsterList.FindIndex(_ => _.CurrentIndex == cell.Index);

            if (checker == -1)
            {
                HeroMoveCell();
            }
            else 
            {
                clickedMonster = monsterList.Where(_ => _.CurrentIndex == cell.Index).SingleOrDefault();

                StartAttack();
            }
        }

        private void HeroMoveCell()
        {
            clickedHero.MoveEndVector = clickedCell.transform.position;
            clickedHero.MoveIndex = clickedCell.Index;
        }

        private void StartAttack()
        {
            clickedHero.CanMove = false;

            var battleController = ControllerManager.Instance.GetController<BattleController>();

            battleController.Hero = clickedHero;

            battleController.Monster = clickedMonster;

            battleController.BattleStart();
        }

        #region Check Skip or End Turn
        public void CheckSkipOrEndTurn(bool force = false)
        {
            var canMoveHeroCount = heroList.Where(_ => _.CanMove == true).Count();

            if(canMoveHeroCount == 0 || force)
            {
                monsterList.ForEach(_ => _.CanMove = true);
                EndMonsterMove = true;
            }
        }
        #endregion

        #region Check End Move Monster

        private void StartMonsterMove()
        {
            EndMonsterMove = false;

            monsterList[monsterMoveCounter].InitMonsterMove();

            monsterMoveCounter++;
        }

        private void StartHeroMove()
        {
            heroList.ForEach(_ => _.CanMove = true);
        }

        #endregion

        #endregion

    }
}
