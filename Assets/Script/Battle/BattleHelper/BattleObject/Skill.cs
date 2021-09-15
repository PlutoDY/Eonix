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

        private float currentSkillDamage;
        public float CurrentSkillDamage { get { return currentSkillDamage; } }

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

        public void SetSkillExplainText(float skillDamagePercent, int hitRate)
        {
            currentSkillDamage = skillInfo.sdHeroSkillInfo.skillDamage * skillDamagePercent;

            this.hitRate = hitRate;

            var type = string.Empty;

            if (skillInfo.sdHeroSkillInfo.skillType == Define.Actor.SkillType.Nomal) type = "일반";
            else if (skillInfo.sdHeroSkillInfo.skillType == Define.Actor.SkillType.Upgrade) type = "강화";
            else type = "연타";

            var explainText = $"스킬 이름 : {skillInfo.sdHeroSkillInfo.skillName}\n공격 방식 : {type}\n데미지 : {Mathf.Ceil(CurrentSkillDamage)}\n적중확률 : {hitRate}%";

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