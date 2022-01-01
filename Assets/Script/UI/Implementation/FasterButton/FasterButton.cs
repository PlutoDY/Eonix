using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Controller;

namespace Eonix.UI {

    public class FasterButton : MonoBehaviour
    {
        private bool isClick = false;

        private Button button;
        private Image buttonSprite;

        private string buttonEnablePath = "Art/Sprite/Battle/Faster 1";
        private string buttonDisablePath = "Art/Sprite/Battle/Faster";

        public void Start()
        {
            button = GetComponent<Button>();

            buttonSprite = button.GetComponent<Image>();

        }

        public void ClickButton()
        {
            isClick = !isClick;

            var actorController = ControllerManager.Instance.GetController<Actor.ActorController>();

            actorController.ChangeSpeedValue(isClick);

            var sp = Resources.Load<Sprite>(buttonEnablePath);

            Debug.Log(sp);

            if (isClick)
            {
                Debug.Log("is Click");
                buttonSprite.sprite = Resources.Load<Sprite>(buttonEnablePath);
            }
            else
            {
                Debug.Log("is not Click");
                buttonSprite.sprite = Resources.Load<Sprite>(buttonDisablePath);
            }
        }
    }
}