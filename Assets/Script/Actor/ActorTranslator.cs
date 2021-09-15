using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.DB;

namespace Eonix.Actor
{
    public class ActorTranslator : MonoBehaviour
    {
        Transform heroGroupObject = null;
        Transform monsterGroupObject = null;

        public Tuple<List<Hero>, List<Monster>> TranslatorActors()
        {
            var actorController = GetComponent<ActorController>();

            heroGroupObject = actorController.actorGenerator.HeroGroupObject_TransformCompo;
            monsterGroupObject = actorController.actorGenerator.MonsterGroupObject_TransformCompo;

            List<Hero> heroList = TranslatorActorToHero();
            List<Monster> monsterList = TranslatorActorToMonster();

            return new Tuple<List<Hero>, List<Monster>>(heroList, monsterList);
        }

        private List<Hero> TranslatorActorToHero()
        {
            var heroList = new List<Hero>();

            for (int i = 0; i < heroGroupObject.childCount; i++)
            {
                var heroObject_HeroCompo = heroGroupObject.GetChild(i).GetComponent<Hero>();

                if (heroObject_HeroCompo == null)
                {
                    Debug.LogError($"heroObject_HeroCompo is Null\n" +
                    $"Object Name = {heroGroupObject.GetChild(i).name}");

                    continue;
                }

                heroObject_HeroCompo.InitHero();

                heroList.Add(heroObject_HeroCompo);
            }

            return heroList;
        }

        private List<Monster> TranslatorActorToMonster()
        {
            var monsterList = new List<Monster>();

            for(int i = 0; i < monsterGroupObject.childCount; i++)
            {
                var monsterObject_MonsterCompo = monsterGroupObject.GetChild(i).GetComponent<Monster>();

                if (monsterObject_MonsterCompo == null)
                {
                    Debug.LogError($"monsterObject_MonsterCompo is Null\n" +
                        $"Object Name = {monsterObject_MonsterCompo}");
                }

                monsterList.Add(monsterObject_MonsterCompo);

            }

            return monsterList;
        }
    }
}
