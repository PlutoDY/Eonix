using Eonix.Manager;
using Eonix.DB;
using Eonix.Define;
using Eonix.SD;
using Eonix.UI;
using Eonix.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eonix
{
    public class GameManager : SingleTon<GameManager>
    {
        public float loadProgress;

        [SerializeField]
        private BoUser boUser;
        public static BoUser User => Instance?.boUser;

        [SerializeField]
        private StaticDataModule sd = new StaticDataModule();
        public static StaticDataModule SD => Instance.sd;

        [SerializeField]
        private InputManager input = new InputManager();
        public static InputManager Input => Instance.input;

        public TitleController titleController;

        protected override void Awake()
        {
            base.Awake();

            if (gameObject != null)
            {
                DontDestroyOnLoad(this);
                titleController?.Initialize();
            }
        }

        public void OnAplicationSetting()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public static void Log(string log)
        {
            Debug.Log(log);
        }

        private void Update()
        {
            if(input != null)
                input.OnUpdate();
        }

        public void LoadScene(SceneType sceneName, IEnumerator loadCoroutine = null, Action loadComplete = null)
        {
            StartCoroutine(WaitForLoad());

            IEnumerator WaitForLoad()
            {
                loadProgress = 0;

                yield return SceneManager.LoadSceneAsync(SceneType.Loading.ToString());

                var asyncOper = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);

                asyncOper.allowSceneActivation = false;

                if (loadCoroutine != null) { yield return StartCoroutine(loadCoroutine); }

                while (!asyncOper.isDone)
                {
                    if (loadProgress >= .9f)
                    {
                        loadProgress = 1;   

                        asyncOper.allowSceneActivation = true;

                        if(UIWindowManager.Instance.GetWindow<UILoading>().gameObject != null) { UIWindowManager.Instance.Initialize(); }
                    }
                    else
                    {
                        loadProgress = asyncOper.progress;
                    }
                    yield return null;
                }

                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());
                loadComplete?.Invoke();
            }
        }
    }
}