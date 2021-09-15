using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    public class UIPvE : UIWindow
    {
        public Text noInPartyHeroText;

        private int inPartyHeroCount;
        public override void Start()
        {
            isOpen = true;

            base.Start();

            canCloseESC = true;
        }

        public override void Open(bool force = false)
        {
            inPartyHeroCount = GameManager.User.boHeroes.boHeroes.Where(_ => _.inParty == true).Count();

            var selectedWorldButtons = GetComponentsInChildren<Button>();
            if (inPartyHeroCount <= 0) 
            { 
                noInPartyHeroText.text = "파티에는 영웅이 1명 이상은 배치되어야합니다.";
                

                foreach (Button button in selectedWorldButtons)
                {
                    button.interactable = false;
                }
            }
            else
            {
                noInPartyHeroText .text= string.Empty;

                foreach (Button button in selectedWorldButtons)
                {
                    button.interactable = true;
                }
            }

            base.Open(force);


        }

        public void PressWorldZeroButton()
        {
            UIWindowManager.Instance.GetWindow<FristDungeonUI>().Open();
        }

        public void PressWorldOneButton()
        {
            UIWindowManager.Instance.GetWindow<SecondDungeonUI>().Open();
        }
    }
}