using BackEnd;
using Eonix;
using Eonix.DB;
using Eonix.UI;
using Eonix.Util;
using Eonix.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

namespace Eonix.Network
{
    public class ServerManager : SingleTon<ServerManager>
    {
        public bool isFirstLogin;

        public DtoUserInfo userInfo;

        protected override void Awake()
        {
            base.Awake();

            if(gameObject != null)
            {
                DontDestroyOnLoad(this);
                SendQueue.StartSendQueue(true);
            }
        }

        public void Update()
        {
            if (Backend.IsInitialized)
            {
                Backend.AsyncPoll();

                if (SendQueue.IsInitialize)
                {
                    SendQueue.Poll();
                }
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) SendQueue.PauseSendQueue();
            else SendQueue.ResumeSendQueue();
        }

        private void OnApplicationQuit()
        {
#if UNITY_EDITOR
            SendQueue.StopSendQueue();
#endif
        }

        public void Initialize()
        {
            Backend.InitializeAsync(true, true, callback =>
            {
                if (callback.IsSuccess())
                {
                    GameManager.Instance.titleController.LoadComplete = true;
                }
                else
                {
                    Debug.Log($"Server Init Error\n{callback}");
                }
            });

        }

        public void CheckAppVersion()
        {
#if UNITY_EDITOR
            GameManager.Instance.titleController.LoadComplete = true;
#endif
            Backend.Utils.GetLatestVersion(callback:c =>
            {
                if (c.IsSuccess())
                {
                    var com = c.GetReturnValuetoJSON()["version"].ToString();

                    if(com == Application.version) { GameManager.Instance.titleController.LoadComplete = true; }
                }
            });
        }

        public void Login(string id, string password)
        {
            UILogin uILogin = FindObjectOfType<UILogin>();

            Backend.BMember.CustomLogin(id, password, callback =>
            {
                uILogin.LoginCaseCheck(int.Parse(callback.GetStatusCode()));

                if (callback.IsSuccess())
                {
                    var bro = Backend.GameData.GetMyData("Account", new Where(), 10);

                    var jData = bro.GetReturnValuetoJSON();

                    uILogin.loginUIInputFieldList[0].text = "SUCCESS";

                    if (jData["rows"].Count == 0)
                    {
                        isFirstLogin = true;

                        InsertUserInfo();

                        bro = Backend.GameData.GetMyData("Account", new Where(), 10);
                        jData = bro.GetReturnValuetoJSON();
                    }
                    else
                    {
                        isFirstLogin = false;
                    }

                    userInfo = SerializationUtil.JsonToObject<DtoUserInfo>(jData["rows"][0], DeserializeType.DTO);

                    GameManager.Instance.titleController.LoadComplete = true;
                }
                else
                {
                    uILogin.loginUIInputFieldList[0].text = callback.GetMessage();

                    Debug.Log($"Login Faild\n{callback}");
                }
            });
        }

        public void SignUp(string id, string password)
        {
            Backend.BMember.CustomSignUp(id, password, callback =>
            {
                UISignup uISignup = FindObjectOfType<UISignup>();
                if (callback.IsSuccess())
                {
                    Debug.Log("Sign Up Success");
                }
                else
                {
                    Debug.Log("Sign Up Faild");
                }

                uISignup.SignUpCode(int.Parse(callback.GetStatusCode()));
            });
        }

        public void SetNickname(string nickname)
        {
            UISetNickName uISetNickName = FindObjectOfType<UISetNickName>();

            if(uISetNickName.gameObject == null) { Debug.Log("UISetNickName is Null"); }

            Backend.BMember.CreateNickname(nickname, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("Set nickname Success");
                }
                else
                {
                    Debug.Log("Set nickname Faild");
                }

                uISetNickName.CaseChecekr(int.Parse(callback.GetStatusCode()));
            });
        }

        private void InsertUserInfo() {

            var bro = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"];

            Param p = new Param
            {
                {"nickname", bro["nickname"]},
                {"subscriptionType", bro["subscriptionType"]}
            };

            Backend.GameData.Insert("Account", p, callback =>
            {
                if(callback.IsSuccess())
                {
                    Debug.Log($"Insert Success");
                }
                else
                {
                    Debug.Log($"Insert Fail");
                }
            });
        }
    }
}
