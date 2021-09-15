using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Actor;
using UnityEngine.UI;
using System.Linq;
using Eonix.DB;
using System.Globalization;

namespace Eonix.Battle
{
    using SpriteArtPath = Define.SpriteArtPath;
    using ArtType = Define.SpriteArtPath.ArtType;
    using RM = RM.ResourceManager;

    public class BattleHeroSkillHelper : MonoBehaviour
    {
        [SerializeField]
        List<Skill> skills = new List<Skill>();

        public void InitSkill(List<GameObject> gObjList)
        {
            foreach(GameObject gObj in gObjList)
            {
                Skill newSkill = new Skill();

                gObj.TryGetComponent(out newSkill);

                newSkill.InitObject();

                skills.Add(newSkill);
            }
        }

        public void InitSkillInfo(Hero hero)
        {
            for(int i = 0; i < 3; i++)
            {
                skills[i].SetSkillInfo(hero.HeroInfo.boHeroSkillInfos[i]);
            }
        }

        public void SetSkillsExplain(float damagePercent, int[] rateHitList)
        {
            for(int i = 0; i < 3; i++)
            {
                skills[i].SetSkillExplainText(damagePercent, rateHitList[i]);
            }
        }
    }
}