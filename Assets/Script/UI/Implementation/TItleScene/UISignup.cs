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
        /// 애니메이션이 종료 될 때 보여줄 GameObject
        /// </summary>
        public List<GameObject> ShowUIEndAnimation = new List<GameObject>();

        /// <summary>
        /// 회원가입 입력 필드 리스트
        /// </summary>
        public static List<InputField> SignUpInputField = new List<InputField>();

        /// <summary>
        /// 회원가입 정상적 완료 시에 보여줄 SignUpCompleteUI
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
        /// 애니메이션이 끝날 시 보여줄 GameObject들을 활성화 시켜줍니다.
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
        /// 회원 가입 취소 버튼을 눌렀을 경우 실행되는 함수 -> ButtonEvent
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
        /// 회원가입 완료 버튼을 눌렀을 때 실행되는 함수
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
        /// 입력란에 정확한 포멧이 들어갔는지 확인하는 
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
                ErrorTextSet(inputField.gameObject, "필수 항목입니다!");
            }

            return checker;
        }

        private bool InputTextisTooShort(InputField inputField)
        {
            checker = false;

            if (inputField.text.Length < 5)
            {
                checker = true;
                ErrorTextSet(inputField.gameObject, "5자 이상 입력해주세요!");
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
                ErrorTextSet(inputField.gameObject, "영문 대소문자 또는 숫자만 입력가능합니다.");
            }

            return checkInput;
        }

        private bool CheckPasswordisMatch()
        {
            checker = true;

            if (SignUpInputField[1].text != SignUpInputField[2].text)
            {
                ErrorTextSet(SignUpInputField[1].gameObject, "비밀번호가 다릅니다.");
                ErrorTextSet(SignUpInputField[2].gameObject, "비밀번호가 다릅니다.");
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
                    ErrorTextSet(SignUpInputField[0].gameObject, "계정 한계를 초과했습니다.");
                    break;
                case 409:
                    ErrorTextSet(SignUpInputField[0].gameObject, "중복되는 ID가 있습니다.");
                    break;
            }
        }
    }
}
