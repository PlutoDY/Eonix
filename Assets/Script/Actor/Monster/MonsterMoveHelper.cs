using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Stage;
using System;
using System.Linq;

namespace Eonix.Actor
{
    using Distance = Define.Distance;

    public class MonsterMoveHelper : MonoBehaviour
    {
        [SerializeField]
        private int[] checkIndexArray;

        private BaseStage baseStage;
        public BaseStage BaseStage
        {
            get { return baseStage; }
            set 
            {
                baseStage = value; 
                InitCheckIndexArray(); 
            }
        }

        public List<Monster> monsters = new List<Monster>();

        private Monster currnetMoveMonster;
        public Monster CurrentMoveMonster
        {
            get { return currnetMoveMonster; }
            set { currnetMoveMonster = value; }
        }

        #region A* Value
        [SerializeField]
        private List<int> fScore = new List<int>();

        [SerializeField]
        private List<int> gScore = new List<int>();

        [SerializeField]
        private List<int> openIndex = new List<int>();

        [SerializeField]
        private List<int> closeIndex = new List<int>();

        [SerializeField]
        private List<int> path = new List<int>();

        [SerializeField]
        private List<int> parentIndex = new List<int>();
        #endregion

        private void InitCheckIndexArray()
        {
            var stageWidth = BaseStage.Width;

            Array.Resize(ref checkIndexArray, 8);

            checkIndexArray = new int[8] { -stageWidth - 1,-stageWidth,-stageWidth+1,-1,
                1,stageWidth-1, stageWidth,stageWidth+1 };
        }

        public Hero SetTargetHero(List<Hero> heroes)
        {
            Hero targetHero = null;

            int distanceToHeroValue = -1;

            Distance distanceToHero;

            foreach(Hero hero in heroes)
            {
                distanceToHero = new Distance(CurrentMoveMonster.CurrentIndex, hero.CurrentIndex, baseStage.Width);
                if (targetHero == null)
                {
                    targetHero = hero;
                    distanceToHeroValue = Distance.CalculationDistance(distanceToHero);
                }
                else
                {
                    var tempDistanceToHeroValue = Distance.CalculationDistance(distanceToHero);

                    if(distanceToHeroValue > tempDistanceToHeroValue)
                    {
                        distanceToHeroValue = tempDistanceToHeroValue;
                        targetHero = hero;
                    }
                }
            }

            return targetHero;
        }

        public void InitListValues()
        {
            var stageSize = baseStage.Width * baseStage.Height;

            openIndex.Clear();
            closeIndex.Clear();
            path.Clear();

            fScore = Enumerable.Repeat(int.MaxValue, stageSize).ToList();
            gScore = Enumerable.Repeat(int.MaxValue, stageSize).ToList();

            parentIndex = Enumerable.Repeat(-1, stageSize).ToList();

            var index = CurrentMoveMonster.CurrentIndex;
            var targetIndex = CurrentMoveMonster.TargetHero.CurrentIndex;
            openIndex.Add(index);

            gScore[index] = 0;

            fScore[index] = Distance.CalculationDistance(new Distance(index, targetIndex, baseStage.Width));
        }

        public List<int> CalculationMove()
        {
            int counter = 0;

            int tempValue;
            var currentIndex = -1;

            while(openIndex.Count != 0 && counter < 100)
            {
                tempValue = int.MaxValue;
                foreach(int index in openIndex)
                {
                    if(tempValue > fScore[index])
                    {
                        tempValue = fScore[index];
                        currentIndex = index;
                    }
                }

                if (currentIndex == CurrentMoveMonster.TargetHero.CurrentIndex)
                    return Reconstruct_path(currentIndex);

                openIndex.Remove(currentIndex);
                closeIndex.Add(currentIndex);

                foreach(Tuple<Cell ,int> neighborCellInfo in baseStage.Cells[currentIndex].neighborCell)
                {
                    var neighborCellIndex = neighborCellInfo.Item1.Index;

                    if (CheckMonster(currentIndex)) continue;

                    if (closeIndex.Exists(_ => _ == neighborCellIndex) == true) continue;

                    if (openIndex.Exists(_ => _ == neighborCellIndex) == false)
                        openIndex.Add(neighborCellIndex);

                    var tentative_gScore = gScore[currentIndex] + neighborCellInfo.Item2;

                    if (tentative_gScore >= gScore[neighborCellIndex])
                        continue;

                    parentIndex[neighborCellIndex] = currentIndex;
                    gScore[neighborCellIndex] = tentative_gScore;
                    fScore[neighborCellIndex] = gScore[neighborCellIndex]
                        + Distance.CalculationDistance(new Distance(neighborCellIndex,
                        CurrentMoveMonster.TargetHero.CurrentIndex, baseStage.Width));
                }

                counter++;
            }

            return new List<int>() { -1 };
        }

        private bool CheckMonster(int currentIndex)
        {
            foreach(Monster monster in monsters)
            {
                if (monster.Equals(CurrentMoveMonster))
                {
                    continue;
                }
                else if (currentIndex == monster.CurrentIndex) return true;
            }

            return false;
        }

        private List<int> Reconstruct_path(int currentIndex)
        {
            path.Add(currentIndex);
            while(currentIndex != -1)
            {
                currentIndex = parentIndex[currentIndex];
                path.Add(currentIndex);
            }

            path.Remove(-1);
            path.Remove(CurrentMoveMonster.CurrentIndex);
            path.Remove(CurrentMoveMonster.TargetHero.CurrentIndex);
            return path;
        }


    }
}