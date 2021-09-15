using Eonix.Define;
using Eonix.Network;
using Eonix.RM;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    using Util = Util.Util;

    public class UISignup : UIWindow
    {
        /// <summary>
        /// �ִϸ��̼��� ���� �� �� ������ GameObject
        /// </summary>
        public List<GameObject> ShowUIEndAnimation = new List<GameObject>();

        /// <summary>
        /// ȸ������ �Է� �ʵ� ����Ʈ
        /// </summary>
        public static List<InputField> SignUpInputField = new List<InputField>();

        /// <summary>
        /// ȸ������ ������ �Ϸ� �ÿ� ������ SignUpCompleteUI
        /// </summary>
        public static UnityEngine.Object SignUpComplete;

        public static GameObject mine;

        private void Awake()
        {
            ShowUIEndAnimation.Clear();
            SignUpInputField.Clear();
            SignUpComplete = null;

            ShowUIEndAnimation = Util.FindChild(gameObject, true);
        }

        /// <summary>
        /// �ִϸ��̼��� ���� �� ������ GameObject���� Ȱ��ȭ �����ݴϴ�.
        /// </summary>
        public void EndEnableUIAnimation()
        {
            foreach(GameObject ele in ShowUIEndAnimation)
            {
                if(ele.activeSelf == false)
                {
                    ele.SetActive(true);
                }

                if(ele.GetComponent<InputField>() != null)
                {
                    SignUpInputField.Add(ele.GetComponent<InputField>());
                }
            }
        }

        /// <summary>
        /// ȸ�� ���� ��� ��ư�� ������ ��� ����Ǵ� �Լ� -> ButtonEvent
        /// </summary>
        public void PressCancleSignupButton()
        {
            UIWindowManager.Instance.GetWindow<UILogin>().Open();
            Close();
        }

        private bool temp;

        public bool Temp
        {
            get { return temp; }
            set
            {
                if (!temp) { temp = false; }
                else { temp = value; }
            }
        }

        /// <summary>
        /// ȸ������ �Ϸ� ��ư�� ������ �� ����Ǵ� �Լ�
        /// </summary>
        public void PressSubmitSignupButton()
        {
            temp = true;

            foreach(InputField inputField in SignUpInputField)
            {
                Temp = CheckInputField(inputField);
            }

            Debug.Log($"Temp : {temp}");

            if (Temp)
            {
                if (CheckPasswordisMatch())
                {
                    ServerManager.Instance.SignUp(SignUpInputField[0].text, SignUpInputField[1].text);
                }
            }

        }

        /// <summary>
        /// �Է¶��� ��Ȯ�� ������ ������ Ȯ���ϴ� 
        /// </summary>
        private bool checker = false;


        private bool CheckInputField(InputField inputField)
        {

            if (InputTextisEmpty(inputField) == false)
            {
                if (CheckInputFormat(inputField))
                {
                    if (InputTextisTooShort(inputField) == false)
                    {
                        ErrorTextSet(inputField.gameObject, string.Empty);
                        return true;
                    }
                }
            }

            return false;

        }



        private bool InputTextisEmpty(InputField inputField)
        {
            checker = false;

            if(inputField.text == string.Empty)
            {
                checker = true;
                ErrorTextSet(inputField.gameObject, "�ʼ� �׸��Դϴ�!");
            }

            return checker;
        }

        private bool InputTextisTooShort(InputField inputField)
        {
            checker = false;

            if (inputField.text.Length < 5)
            {
                checker = true;
                ErrorTextSet(inputField.gameObject, "5�� �̻� �Է����ּ���!");
            }

            return checker;
        }

        private bool CheckInputFormat(InputField inputField)
        {
            bool checkInput = true;
            if (inputField.gameObject.name.CompareTo("IdInputField") == 0)
            {
                checkInput = Regex.IsMatch(inputField.text, @"^[a-zA-Z0-9]+$");
            }

            if (!checkInput)
            {
                ErrorTextSet(inputField.gameObject, "���� ��ҹ��� �Ǵ� ���ڸ� �Է°����մϴ�.");
            }

            return checkInput;
        }

        private bool CheckPasswordisMatch()
        {
            checker = true;

            if (SignUpInputField[1].text != SignUpInputField[2].text)
            {
                ErrorTextSet(SignUpInputField[1].gameObject, "��й�ȣ�� �ٸ��ϴ�.");
                ErrorTextSet(SignUpInputField[2].gameObject, "��й�ȣ�� �ٸ��ϴ�.");
                checker = false;
            }
            else
            {
                ErrorTextSet(SignUpInputField[1].gameObject, string.Empty);
                ErrorTextSet(SignUpInputField[2].gameObject, string.Empty);
            }

            return checker;
        }

        private void ErrorTextSet(GameObject ele, string text)
        {
            var errorText = ele.GetComponentsInChildren<Text>();

            errorText[errorText.Length - 1].text = text;
        }

        public void SignUpCode(int code)
        {
            Debug.Log("code : " + code);

            switch (code)
            {
                case 201:
                    SignUpComplete = ResourceManager.Instance.LoadObject(Resource.UIPath + "/SignUpCompleteCanvas");
                    Instantiate(SignUpComplete);
                    Close();
                    break;
                case 403:
                    ErrorTextSet(SignUpInputField[0].gameObject, "���� �Ѱ踦 �ʰ��߽��ϴ�.");
                    break;
                case 409:
                    ErrorTextSet(SignUpInputField[0].gameObject, "�ߺ��Ǵ� ID�� �ֽ��ϴ�.");
                    break;
            }
        }
    }
}
