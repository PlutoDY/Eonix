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
                    exText += "(�Ϲ� ����)";
                    break;
                case SkillType.Upgrade:
                    exText += "(��ȭ ����)";
                    break;
                case SkillType.Consecutive:
                    exText += "(���� ����)";
                    break;
            }

            exText += $"\n{skillDamage}��ŭ�� �������� �����ϴ�.\n����Ȯ�� : {RateHit}%";

            ExplainText.text = exText;

        }
    }
}
