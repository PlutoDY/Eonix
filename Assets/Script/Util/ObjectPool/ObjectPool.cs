 using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace Eonix.Util
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {

        private List<T> pool = new List<T>();

        public List<T> Pool => pool;

        public Transform holder;

        public bool canRecyle => pool.Find(_ => _.CanRecyle) != null;

        public void RegistPoolableObject(T obj)
        {
            pool.Add(obj);
        }

        public T GetPoolableObject(Func<T, bool> pred)
        {

            if (!canRecyle)
            {
                var protoObj = pool.Where(_ => _.name == typeof(T).Name).SingleOrDefault();

                for(int i = 0; i < pool.Count; i++) { Debug.Log("Pool Name i : " + pool[i].name); }

                if(protoObj != null)
                {
                    var newObject = GameObject.Instantiate(protoObj.gameObject, holder);
                    newObject.name = protoObj.name;
                    newObject.SetActive(false);
                    RegistPoolableObject(newObject.GetComponent<T>());
                }
                else
                {
                    return null;
                }
            }

            T recyleObject = pool.Find(_ => pred(_) && _.CanRecyle);

            if (recyleObject == null)
                return null;

            recyleObject.OnRecyleStart?.Invoke();

            recyleObject.CanRecyle = false;

            return recyleObject;
        }
    }
}
