using BackEnd;
using Eonix.Define;
using Eonix.Network;
using Eonix.RM;
using System;
using System.Collections;
using Eonix.UI;
using System.Collections.Generic;
using UnityEngine;
using Eonix.DB;

namespace Eonix
{
    using PrefabsPath = Define.PrefabsPath;
    public class TitleController : MonoBehaviour
    {
        public UITitle uiTitle;
        // Delete
        public static GameObject SignUpUI = null;

        private void Awake()
        {
            uiTitle = FindObjectOfType<UITitle>();
        }

        private bool loadComplete;

        public bool LoadComplete
        {
            get => loadComplete;
            set
            {
                loadComplete = true;
                if (loadComplete && !allLoaded)
                {
                    NextPhase();
                }
            }
        }

        private bool allLoaded;

        private IntroPhase introPhase = IntroPhase.Start;

        public void Initialize()
        {
            OnPhase(introPhase);
        }

        private UnityEngine.Object obj;

        private void LoginPhase()
        {
            var obj = ResourceManager.Instance.LoadObject(PrefabsPath.UIPath(PrefabsPath.PrefabsUIs.LoginCanvas));

            Instantiate(obj.gameObject);
            
            uiTitle.gameObject.SetActive(false);
        }

        private Coroutine loadGagueUpdateCourutine;

        public void SetLoadStateGague(IntroPhase phase)
        {
            if (phase == IntroPhase.Login) StopAllCoroutines();

            uiTitle.SetLoadStateDescription(phase.ToString());

            if(loadGagueUpdateCourutine != null)
            {
                StopCoroutine(loadGagueUpdateCourutine);
                loadGagueUpdateCourutine = null;
            }

            if(phase < IntroPhase.VersionCheck)
            {
                var loadPer = (float)phase / (float)IntroPhase.VersionCheck;
                loadGagueUpdateCourutine = StartCoroutine(uiTitle.LoadGagueUpdate(loadPer));
            }
            else if (phase == IntroPhase.VersionCheck)
            {
                uiTitle.LoadFillGague.fillAmount = 1f;
            }
            if(phase >= IntroPhase.Login)
            {
                var loadPer = (float)phase / (float)IntroPhase.Complete;
                loadGagueUpdateCourutine = StartCoroutine(uiTitle.LoadGagueUpdate(loadPer));
            }
        }

        private void OnPhase(IntroPhase phase)
        {

            SetLoadStateGague(phase);

            switch (phase)
            {
                case IntroPhase.Start:
                    LoadComplete = true;
                    break;
                case IntroPhase.ApplicationSetting:
                    GameManager.Instance.OnAplicationSetting();
                    LoadComplete = true;
                    break;
                case IntroPhase.ServerInit:
                    ServerManager.Instance.Initialize();
                    break;
                case IntroPhase.VersionCheck:
                    ServerManager.Instance.CheckAppVersion();
                    break;
                case IntroPhase.Login:
                    LoginPhase();
                    break;
                case IntroPhase.StaticData:
                    GameManager.SD.Initialize();
                    break;
                case IntroPhase.UserData:
                    DataBaseManager.Instance.LoaduserData(() => LoadComplete = true);
                    break;
                case IntroPhase.Resource:
                    ResourceManager.Instance.Initalize();
                    LoadComplete = true;
                    break;
                case IntroPhase.UI:
                    UIWindowManager.Instance.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.Complete:
                    var stageManager = StageManager.Instance;
                    GameManager.Instance.LoadScene(SceneType.InGame, stageManager.ChangeStage(), stageManager.OnChangeStageComplete);
                    allLoaded = true;
                    LoadComplete = true;
                    break;
                default:
                    break;
            }
        }


        private void NextPhase()
        {

            StartCoroutine(WaitForSeconds());

            IEnumerator WaitForSeconds()
            {
                yield return new WaitForSeconds(1f);   

                loadComplete = false;
                OnPhase(++introPhase);
            }
        }
    }
}
