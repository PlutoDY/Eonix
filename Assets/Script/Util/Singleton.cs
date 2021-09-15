using UnityEngine;

namespace Eonix.Util
{
    public abstract class SingleTon<T> : MonoBehaviour where T : SingleTon<T>
    {
        private static object syncObject = new object();

        private static T instance;

        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (syncObject)
                    {
                        instance = FindObjectOfType<T>();
                        if(instance == null)
                        {
                            GameObject obj = new GameObject();
                            obj.name = typeof(T).Name;
                            instance = obj.AddComponent<T>();
                        }
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            lock (syncObject)
            {
                if(instance == null)
                {
                    instance = this as T;
                }
                else
                {
                    Destroy(Instance);
                }
            }
        }

        private void OnDestroy()
        {
            lock (syncObject)
            {
                if(instance != this) { return; }

                instance = null;
            }
        }
    }
}
