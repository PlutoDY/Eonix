using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Network;
using System;
using Eonix.RM;
using Eonix.Define;

namespace Eonix.UI
{
    using Util = Util.Util;

    public class UILogin : UIWindow
    {
        public GameObject TitleUI;

        public static GameObject LoginPanel;

        public List<GameObject> loginUIList = new List<GameObject>();

        public List<InputField> loginUIInputFieldList = new List<InputField>();

        public bool login = false;

        GameObject SignUpUI;

        public override void Start()
        {
            base.Start();
            
            Open();
        }

        private void OnEnable()
        {
            TitleUI = FindObjectOfType<UITitle>().gameObject;

            LoginPanel = Util.FindChild(gameObject, "Panel", true);

            loginUIInputFieldList = Util.FindChild<InputField>(gameObject, true);

            loginUIList = Util.FindChild(LoginPanel, true);

            SignUpUI = ResourceManager.Instance.LoadObject(Resource.UIPath + "/SignupCanvas");
        }

        private void OnDisable()
        {

        }

        public void CheckInputField()
        {
            List<string> idString = new List<string>();

            foreach (GameObject ele in loginUIList)
            {
                var inputField = ele.GetComponent<InputField>();

                if(inputField == null)
                {
                    continue;
                }

                var holder = Util.FindChild<Text>(ele, "Placeholder", false);

                if (inputField.text.Length < 5 && inputField.text != string.Empty)
                {
                    CheckInputFieldTextTooShorts(holder, inputField);
                    continue;
                }
                
                if(inputField.text == string.Empty)

                {
                    CheckInputFieldisEmpty(holder, inputField);
                    continue;
                }

                idString.Add(inputField.text);
            }

            if (idString.Count == 2)
            {
                ServerManager.Instance.Login(idString[0], idString[1]);
            }
            
        }

        private bool CheckInputFieldisEmpty(Text placeHolder, InputField inputField)
        {
            if (inputField.text == string.Empty)
            {
                if (placeHolder == null)
                {
                    Debug.LogError("Error");
                }

                placeHolder.text = $"필수 입력란입니다.";

                return true;
            }

            return false;
        }

        private bool CheckInputFieldTextTooShorts(Text placeHolder, InputField inputField)
        {

            if(inputField.text.Length < 5)
            {
                inputField.text = string.Empty;

                if(placeHolder == null)
                {
                    Debug.LogError("Error");
                }

                placeHolder.text = $"5자 이상 입력해주세요.";

                return true;
            }

            return false;
        }

        public void LoginCaseCheck(int statue)
        {
            switch (statue)
            {
                case 200:
                    Close();
                    TitleUI.SetActive(true);
                    break;
                case 409:

                    Debug.Log("Login Faild Same Id");   

                    break;
                case 403:

                    Debug.Log("Login Faild Banned Id");

                    break;

                default:
                    break;
            }
        }


        // Sign Up Button을 클릭할 때...
        public void TrySignUp()
        {
            if (UIWindowManager.Instance.GetWindow<UISignup>() == null)
            {
                Instantiate(SignUpUI); // = UIWindowManager에 등록을 한 것입니다.
            }
            else
            {
                UIWindowManager.Instance.GetWindow<UISignup>().Open();
            }

            Close();
        }
    }
}
