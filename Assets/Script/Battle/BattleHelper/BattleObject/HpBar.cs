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

            var redu = Mathf.Ceil(reductionHp);

            reductionFillAmount = redu / maxHp;
            decreasePerFrame = reductionFillAmount / 100f;
            targetForDecreasing = (currentHp - redu) / maxHp;

            reductionTextAmount = redu;
            decreaseAmountPerFrame = reductionTextAmount / 100f;

            Debug.Log($"currentHp = {currentHp} | maxHp = {maxHp} | reductionHp = {redu}");

            StartCoroutine(ReductingHpBar());
        }

        private IEnumerator ReductingHpBar()
        {
            for(float f = fillObject_ImageComponent.fillAmount; f >= targetForDecreasing; f -= decreasePerFrame)
            {
                Debug.Log($"Current FillObject ImageComponent FillAmount = {fillObject_ImageComponent.fillAmount}");

                fillObject_ImageComponent.fillAmount = f;

                hpAmountObject_TextComponenet.text = $"{(int)(currentHpAmount)} / {(int)(maxHpAmount)}";

                currentHpAmount -= decreaseAmountPerFrame;

                if((int)currentHpAmount == 0)
                {
                    targetForDecreasing = 0.0f;

                    hpAmountObject_TextComponenet.text = "KILL!";

                    break;
                }

                yield return new WaitForSeconds(0.01f);
            }

            fillObject_ImageComponent.fillAmount = targetForDecreasing;
        }
    } 
}
