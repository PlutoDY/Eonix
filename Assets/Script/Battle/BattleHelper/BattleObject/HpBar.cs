using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.Battle {
    public class HpBar : MonoBehaviour
    {
        public Image fillObject_ImageComponent;

        public Text hpAmountObject_TextComponenet;

        private float reductionFillAmount;
        private float decreasePerFrame;
        private float targetForDecreasing;

        private float reductionTextAmount;
        private float decreaseAmountPerFrame;

        private float currentHpAmount;
        private float maxHpAmount;

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
            currentHpAmount = currentHp;
            maxHpAmount = maxHp;

            reductionFillAmount = reductionHp/maxHp;
            decreasePerFrame = reductionFillAmount / 100f;
            targetForDecreasing = (currentHp - reductionHp) / maxHp;

            reductionTextAmount = reductionHp;
            decreaseAmountPerFrame = reductionTextAmount / 100f;

            StartCoroutine(ReductingHpBar());
        }

        private IEnumerator ReductingHpBar()
        {
            for(float f = fillObject_ImageComponent.fillAmount; f >= targetForDecreasing; f -= decreasePerFrame)
            {
                Debug.Log($"Current FillObject ImageComponent FillAmount = {fillObject_ImageComponent.fillAmount}");

                fillObject_ImageComponent.fillAmount = f;

                hpAmountObject_TextComponenet.text = $"{(int)(currentHpAmount) / (int)(maxHpAmount)}";

                currentHpAmount -= decreaseAmountPerFrame;

                yield return new WaitForSeconds(1f);
            }

            fillObject_ImageComponent.fillAmount = targetForDecreasing;
        }
    } 
}
