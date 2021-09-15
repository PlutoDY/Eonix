    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Actor;

namespace Eonix.Battle {
    public class BattleHpHelper : MonoBehaviour
    {
        List<HpBar> hpBars = new List<HpBar>();

        public void HpBarsInit(List<GameObject> gObjList)
        {
            foreach(GameObject gObj in gObjList)
            {
                HpBar hpBar = new HpBar();

                gObj.TryGetComponent<HpBar>(out hpBar);

                hpBar.InitCompoenet();

                hpBars.Add(hpBar);
            }
        }

        public void ResetImageAndText(Hero hero, Monster monster)
        {
            hpBars[0].InitImageAndText(hero.CurrentHp, hero.MaxHp);
            hpBars[1].InitImageAndText(monster.Hp, monster.Monsterinfo.boMonsterStatInfo.MaxHp);
        }

        public void SetHeroHpBar(float currentHp, float maxHp, float reductionHp)
        {
            hpBars[0].SetHpBarAndAmount(currentHp, maxHp, reductionHp);
        }

        public void SetMonsterHpBar(float currentHp, float maxHp, float reductionHp)
        {
            hpBars[1].SetHpBarAndAmount(currentHp, maxHp, reductionHp);
        }

    }
}
