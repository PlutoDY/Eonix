using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.Util
{
    public class Util
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();

            if (component == null)
                component = go.AddComponent<T>();

            return component;
        }

        public static GameObject FindChild(GameObject go, string name = null, bool child = false)
        {
            Transform transform = FindChild<Transform>(go, name, child);
            if (transform == null)
            {
                return null;
            }

            return transform.gameObject;
        }

        public static T FindChild<T>(GameObject go, string name = null, bool child = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            if (!child)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);

                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();

                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>(true))
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }

            return null;
        }

        public static List<T> FindChild<T>(GameObject go, bool child = false) where T : UnityEngine.Object
        {
            List<T> list = new List<T>();

            if (go == null)
                return null;

            foreach (T component in go.GetComponentsInChildren<T>(child))
            {
                list.Add(component);
            }

            return list;
        }

        public static List<GameObject> FindChild(GameObject go, bool child = false)
        {
            List<Transform> list = FindChild<Transform>(go, child);

            List<GameObject> objList = new List<GameObject>();

            foreach(Transform ele in list)
            {
                objList.Add(ele.gameObject);
            }


            return objList;
        }
    }
}