using Eonix.Util;
using Eonix.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

namespace Eonix.RM
{
    public class ResourceManager : SingleTon<ResourceManager>
    {
        public void Initalize()
        {
            LoadAllSpriteAtlas();
            LoadAllUI();
            LoadAllPrefabs();
        }

        public GameObject LoadObject(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public void LoadPoolableObject<T>(PoolType poolType, string path, int poolCount = 1, Action loadComplete = null)
            where T : MonoBehaviour, IPoolableObject
        {
            var obj = LoadObject(path);
            var tComponent = obj.GetComponent<T>();

            ObjectPoolManager.Instance.RegistPool(poolType, tComponent, poolCount);

            loadComplete?.Invoke();
        }

        private void LoadAllSpriteAtlas()
        {
            var atlases = Resources.LoadAll<SpriteAtlas>(Define.Resource.AtlasPath);
            SpriteLoader.SetAtlas(atlases);
        }

        private void LoadAllPrefabs()
        {

        }

        private void LoadAllUI()
        {

        }
    }

}