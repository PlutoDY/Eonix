using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.DB;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Eonix.Controller;

namespace Eonix.Battle
{
    public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private BoHeroSkillInfo skillInfo;
        public BoHeroSkillInfo SkillInfo { get { return skillInfo; } }

        private Image _image;

        private GameObject skillExplainObject;

        [SerializeField]
        private Text skillExplain_TextComponent;

        private int hitRate;
        public int HitRate { get { return hitRate; } }

        public void InitObject()
        {
            _image = GetComponent<Image>();

            skillExplainObject = transform.GetChild(0).gameObject;

            skillExplain_TextComponent = skillExplainObject.transform.GetChild(0).GetComponent<Text>();
        }

        public void SetSkillInfo(BoHeroSkillInfo boHeroSkillInfo)
        {
            skillInfo = boHeroSkillInfo;

            SetSkillImage();
        }

        private void SetSkillImage()
        {
            var path = skillInfo.sdHeroSkillInfo.skillImageResourcePath;

            _image.sprite = Define.SpriteArtPath.sprite(Define.SpriteArtPath.ArtType.Skill, path);
        }

        public void SetSkillExplainText(float skillDamage, int hitRate)
        {
            var damage = skillInfo.sdHeroSkillInfo.skillDamage * skillDamage;

            this.hitRate = hitRate;

            var type = string.Empty;

            if (skillInfo.sdHeroSkillInfo.skillType == Define.Actor.SkillType.Nomal) type = "�Ϲ�";
            else if (skillInfo.sdHeroSkillInfo.skillType == Define.Actor.SkillType.Upgrade) type = "��ȭ";
            else type = "��Ÿ";

            var explainText = $"��ų �̸� : {skillInfo.sdHeroSkillInfo.skillName}\n���� ��� : {type}\n������ : {damage}\n����Ȯ�� : {hitRate}%";

            skillExplain_TextComponent.text = explainText;
        }

        public void Selected()
        {
            ControllerManager.Instance.GetController<BattleController>().SkillSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            skillExplainObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            skillExplainObject.SetActive(false);
        }

        private void OnDisable()
        {
            skillExplainObject.SetActive(false);
        }
    }
}