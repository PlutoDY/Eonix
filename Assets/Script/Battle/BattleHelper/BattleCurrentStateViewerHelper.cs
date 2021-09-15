using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.Battle {
    public class BattleCurrentStateViewerHelper : MonoBehaviour
    {
        Text stateViewerObject_TextComponent;

        Text damagePercent_TextComponent;

        Text heroStatObject_TextComponent;
        Text monsterStatObject_TextComponent;

        public void InitStateObject(Text stateViewerTextComponent, Text damagePercentTextComponent)
        {
            stateViewerObject_TextComponent = stateViewerTextComponent;

            damagePercent_TextComponent = damagePercentTextComponent;

            ResetStateText();
        }

        public void InitStatObject(Text heroStatObjectTextComponent, Text monsterStatObjectTextComponent)
        {
            heroStatObject_TextComponent = heroStatObjectTextComponent;
            monsterStatObject_TextComponent = monsterStatObjectTextComponent;
            SetStatText(0);
        }

        public void ResetStateText()
        {
            stateViewerObject_TextComponent.text = StateText.stateText[0];
            damagePercent_TextComponent.text = string.Empty;
        }

        public void SetStatText(int counter)
        {
            heroStatObject_TextComponent.text = Define.Battle.attackHeroToMonsterText[counter];
            monsterStatObject_TextComponent.text = Define.Battle.attackHeroToMonsterText[counter + 1];
        }



        public void SetStateText(StateText.State state)
        {
            stateViewerObject_TextComponent.text = StateText.stateText[(int)state];
        }

        public void SetDamagePercentText(float percent)
        {
            damagePercent_TextComponent.text = $"{(int)percent}%";
        }

        public static class StateText
        {
            public static string[] stateText
                = new string[] { "전투 시작", "제압 성공", "제압 실패","공격 성공", "적중!", "빗나감", "전투 종료" };

            public enum State { Start, OverPowering, NotOverPowering,AttackSuccess, Win, Faild, End }
        }

    }
}