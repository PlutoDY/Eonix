using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.Battle {
    public class BattleHpHelper : MonoBehaviour
    {
        private Image hpObject_ImageCompo;
        public Image HpObject_ImageCompo
        {
            get { return hpObject_ImageCompo; }
            set { hpObject_ImageCompo = value; }
        }

        private Text hpAmount_TextCompo;
        public Text HpAmount_TextCompo
        {
            get { return hpAmount_TextCompo; }
            set { hpAmount_TextCompo = value; }
        }

        private float maxHpAmount;
        public float MaxHpAmount
        {
            get { return maxHpAmount; }
            set { maxHpAmount = value; }
        }

        private float currentHpAmount;
        public float CurrentHpAmount
        {
            get { return currentHpAmount; }
            set { currentHpAmount = value; }
        }

        private float goalFillAmount = 1f;
        private float frameReduceValue = 0f;

        public void InitComponent()
        {
            HpObject_ImageCompo = GetComponent<Image>();
            HpAmount_TextCompo = transform.GetChild(1).GetComponent<Text>();
        }

        public void InitValues(float MaxHp, float currentHp)
        {
            MaxHpAmount = MaxHp;
            CurrentHpAmount = currentHp;

            HpAmount_TextCompo.text = $"{CurrentHpAmount}/{MaxHpAmount}";
            HpObject_ImageCompo.fillAmount = CurrentHpAmount / MaxHpAmount;
        }

        public void UpdateHp(float ReducedHp)
        {
            CurrentHpAmount -= ReducedHp;

            if (CurrentHpAmount <= 0)
            {
                CurrentHpAmount = 0;
            }

            HpAmount_TextCompo.text = $"{CurrentHpAmount}/{MaxHpAmount}";

            StartUpdateImage();
        }

        private void StartUpdateImage()
        {
            goalFillAmount = CurrentHpAmount / MaxHpAmount;
            frameReduceValue = (HpObject_ImageCompo.fillAmount - goalFillAmount) / 60;

            Debug.Log($"goalFillAmount = {goalFillAmount} | frameReduceValue = {frameReduceValue}");

            StartCoroutine(ReduceFillAmount());
        }

        private IEnumerator ReduceFillAmount()
        {
            int counter = 0;

            while (HpObject_ImageCompo.fillAmount >= goalFillAmount && counter <= 60)
            {
                HpObject_ImageCompo.fillAmount -= frameReduceValue;

                yield return null;

                counter++;
            }

            yield return new WaitForSeconds(1f);

            /*Controller.ControllerManager.Instance.GetController<BattleController>().EndPhase();*/
        }
    }
}
