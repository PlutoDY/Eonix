using Eonix.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.UI
{
    public class UIWindowManager : SingleTon<UIWindowManager>
    {
        public List<UIWindow> totalOpenWindows = new List<UIWindow>();

        public List<UIWindow> totalUIWindows = new List<UIWindow>();

        public Dictionary<string, UIWindow> cachedTotalUIWindowDic = new Dictionary<string, UIWindow>();

        public Dictionary<string, object> cachedInstanceDic = new Dictionary<string, object>();

        public void Initialize()
        {
            totalOpenWindows.Clear();
            totalUIWindows.Clear();

            InitAllWindows();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                var targetWindow = GetTopUIWindow();

                if(targetWindow != null && targetWindow.canCloseESC)
                {
                    targetWindow.Close();
                }
            }
        }

        public void AddTotalWindow(UIWindow uiWindow)
        {
            var key = uiWindow.GetType().Name;

            if(totalUIWindows.Contains(uiWindow) || cachedTotalUIWindowDic.ContainsKey(key))
            {
                if(cachedTotalUIWindowDic[key] != null)
                {
                    return;
                }
                else
                {
                    for(int i = 0; i < totalUIWindows.Count; ++i)
                    {
                        if (totalUIWindows[i].Equals(null))
                            totalUIWindows.RemoveAt(i);
                    }

                    totalUIWindows.Add(uiWindow);
                    cachedTotalUIWindowDic[key] = uiWindow;

                    return;
                }
            }

            totalUIWindows.Add(uiWindow);
            cachedTotalUIWindowDic.Add(key, uiWindow);

        }

        public void AddOpendWindow(UIWindow uiWindow)
        {
            if (!totalOpenWindows.Contains(uiWindow)) { totalOpenWindows.Add(uiWindow);}
        }

        public void RemoveOpendWindow(UIWindow uIWindow)
        {
            if (totalOpenWindows.Contains(uIWindow)) totalOpenWindows.Remove(uIWindow);
        }

        public T GetWindow<T>() where T : UIWindow
        {
            string type = typeof(T).Name;

            if (!cachedTotalUIWindowDic.ContainsKey(type))
                return null;

            if (!cachedInstanceDic.ContainsKey(type))
                cachedInstanceDic.Add(type, (T)Convert.ChangeType(cachedTotalUIWindowDic[type], typeof(T)));
            else if (cachedInstanceDic[type].Equals(null))
                cachedInstanceDic[type] = (T)Convert.ChangeType(cachedTotalUIWindowDic[type],typeof(T));


            return (T)cachedInstanceDic[type];
        }

        public void CloseAll()
        {
            for(int i = 0; i < totalUIWindows.Count; ++i)
            {
                var targetWindow = totalUIWindows[i];
                if (targetWindow != null)
                    targetWindow.Close();
            }
        }

        public void Close() {  }

        public void InitAllWindows()
        {
            for(int i = 0;i<totalUIWindows.Count;i++)
            {
                var targetWindow = totalUIWindows[i];

                if (targetWindow != null)
                    targetWindow.InitWindow();
            }
        }

        public void InitWindow() { }

        public UIWindow GetTopUIWindow()
        {
            UIWindow targetWindow = null;

            var openWindowCount = totalOpenWindows.Count;

            if (openWindowCount > 0)
                targetWindow = totalOpenWindows[openWindowCount - 1];

            return targetWindow;
        }
    }
}
