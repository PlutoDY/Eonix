using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.Battle
{
    using SkillType = Define.Actor.SkillType;

    public class SkillExplainHelper : MonoBehaviour
    {
        [SerializeField]
        private string skillName;
        public string SkillName
        {
            get { return skillName; }
            set { skillName = value; }
        }

        [SerializeField]
        private float skillDamage;
        public float SkillDamage
        {
            get { return skillDamage; }
            set { skillDamage = value; }
        }

        [SerializeField]
        private SkillType skillType;
        public SkillType SkillType
        {
            get { return skillType; }
            set { skillType = value; }
        }

        [SerializeField]
        private float rateHit;
        public float RateHit
        {
            get { return rateHit; }
            set { rateHit = value; }
        }

        [SerializeField]
        private Text explainText;
        public Text ExplainText
        {
            get { return explainText; }
            set { explainText = value; }
        }

        public void MakeSkillExplain()
        {
            ExplainText = transform.GetChild(0).GetComponent<Text>();

            var exText = $"{SkillName} ";

            switch (skillType)
            {
                case SkillType.Nomal:
                    exText += "(일반 공격)";
                    break;
                case SkillType.Upgrade:
                    exText += "(강화 공격)";
                    break;
                case SkillType.Consecutive:
                    exText += "(연속 공격)";
                    break;
            }

            exText += $"\n{skillDamage}만큼의 데미지를 입힙니다.\n적중확률 : {RateHit}%";

            ExplainText.text = exText;

        }
    }
}
