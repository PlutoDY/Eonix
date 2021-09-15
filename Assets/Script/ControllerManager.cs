using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Util;
using System;

namespace Eonix.Controller {
    public class ControllerManager : SingleTon<ControllerManager>
    {
        public List<Controller> totalControllers = new List<Controller>();

        public Dictionary<string, Controller> cachedTotalControllerDic = new Dictionary<string, Controller>();

        public Dictionary<string, object> cachedInstanceDic = new Dictionary<string, object>();

        public void Initialize()
        {
            totalControllers.Clear();

            InitAllControllers();
        }

        public void AddTotalController(Controller controller)
        {
            var key = controller.GetType().Name;

            if(totalControllers.Contains(controller) || cachedTotalControllerDic.ContainsKey(key))
            {
                if(cachedTotalControllerDic[key] != null)
                {
                    return;
                }
                else
                {
                    for(int i = 0; i < totalControllers.Count; i++)
                    {
                        if (totalControllers[i].Equals(null))
                            totalControllers.RemoveAt(i);
                    }

                    totalControllers.Add(controller);
                    cachedTotalControllerDic[key] = controller;

                    return;
                }
            }

            totalControllers.Add(controller);
            cachedTotalControllerDic.Add(key, controller);
        }

        public T GetController<T>() where T : Controller
        {
            string type = typeof(T).Name;

            if (!cachedTotalControllerDic.ContainsKey(type))
                return null;

            if (!cachedInstanceDic.ContainsKey(type))
                cachedInstanceDic.Add(type, (T)Convert.ChangeType(cachedTotalControllerDic[type], typeof(T)));
            else if (cachedInstanceDic[type].Equals(null))
                cachedInstanceDic[type] = (T)Convert.ChangeType(cachedTotalControllerDic[type],typeof(T));

            return (T)cachedInstanceDic[type];
        }

        public void InitAllControllers()
        {
            for(int i = 0; i < totalControllers.Count; i++)
            {
                var targetController = totalControllers[i];

                if (targetController != null)
                    targetController.InitController();
            }
        }

        public void InitController() { }

        public void RemoveController(Controller controller)
        {
            if (totalControllers.Contains(controller)) totalControllers.Remove(controller);
        }
    }
}
