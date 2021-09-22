using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Controller;

namespace Eonix.Battle {
    public class HpBar : MonoBehaviour
    {
        public Image fillObject_ImageComponent;

        public Text hpAmountObject_TextComponenet;

        decimal currentHp;
        decimal maxHp;
        decimal damage;
        decimal frameDecreaseHp;

        public void InitCompoenet() 
        {
            fillObject_ImageComponent = GetComponent<Image>();

            hpAmountObject_TextComponenet = transform.GetChild(1).GetComponent<Text>();
        }

        public void InitImageAndText(float currentHp, float maxHp)
        {
            fillObject_ImageComponent.fillAmount = currentHp/maxHp;
            hpAmountObject_TextComponenet.text = $"{(int)currentHp}/{(int)maxHp}";
        }

        public void SetHpBarAndAmount(float currentHp, float maxHp, float reductionHp)
        {
            this.currentHp = (decimal)currentHp;

            this.maxHp = (decimal)maxHp;

            damage = (decimal)reductionHp;

            frameDecreaseHp = damage / 100m;


            StartCoroutine(ReductingHpBar());
        }

        private IEnumerator ReductingHpBar()
        {
            var percent = (currentHp - frameDecreaseHp) / maxHp;

            fillObject_ImageComponent.fillAmount = Mathf.Round((float)percent * 100) * 0.01f;

            for(int i = 0;i < 100; i++)
            {
                currentHp -= frameDecreaseHp;

                percent = currentHp / maxHp;

                fillObject_ImageComponent.fillAmount = Mathf.Round((float)percent * 100) * 0.01f;

                if ((int)currentHp <= 0) { hpAmountObject_TextComponenet.text = $"ZERO"; }
                else hpAmountObject_TextComponenet.text = $"{(int)currentHp} / {(int)maxHp}";

                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);

            var battleController = ControllerManager.Instance.GetController<BattleController>();

            battleController.HpAdjustmentObject();

            battleController.NextPhase();
        }
    } 
}
