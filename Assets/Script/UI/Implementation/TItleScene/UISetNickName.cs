using UnityEngine;
using UnityEngine.UI;
using Eonix.Util;
using System.Collections.Generic;
using Eonix.Network;
using Eonix.RM;
using Eonix.Define;

namespace Eonix.UI
{
    public class UISetNickName : UIWindow
    {
        // gameObject -> SetNickNameCanvas

        public InputField nicknameInputField;

        public List<Text> nicknameInputText;

        public override void Start()
        {
            base.Start();

            nicknameInputField = Util.Util.FindChild<InputField>(gameObject, "InputField", true);

            nicknameInputText = Util.Util.FindChild<Text>(nicknameInputField.gameObject, false);

            if (nicknameInputText == null)
            {
                Debug.LogError("nicknameINputField is NULL");
            }
        }

        public void PressSubmitButton()
        {

            if(nicknameInputField.text.Substring(0,1) == " " || nicknameInputField.text.Substring(nicknameInputField.text.Length-2, 1) == " ")
            {
                nicknameInputField.text = string.Empty;
                nicknameInputText[0].text = "�� �� ������ �Ұ��մϴ�. �ٽ� �Է����ּ���.";
            }
            else
            {
                nicknameInputText[0].text = string.Empty;
            }

            ServerManager.Instance.SetNickname(nicknameInputText[1].text);
        }

        public void CaseChecekr(int code)
        {
            switch (code)
            {
                case 204:
                    UIWindowManager.Instance.GetWindow<UILogin>().Open();
                    Close();
                    break;
                case 409:
                    nicknameInputText[0].text = "�ߺ��Ǵ� �г����Դϴ�. �ٽ��Է����ּ���...";
                    break;
                default:
                    break;
            }
        }
    }
}
