using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    using Util = Util.Util;

    public class UITitle : MonoBehaviour
    {
        [SerializeField]
        private Text loadStateDescription;

        [SerializeField]
        private Image loadFillGague;

        public Image LoadFillGague
        {
            get => loadFillGague;

            set { loadFillGague = value; }
        }

        private void Awake()
        {
            loadStateDescription = Util.FindChild<Text>(gameObject, "LodingText", true);

            if(loadStateDescription == null)
            {
                Debug.Log("loadStateDescription is null");
            }


            loadFillGague = Util.FindChild<Image>(gameObject, "LogdingGague", true);

            if(loadFillGague == null)
            {
                Debug.Log("loadFillGague is null");
            }
        }

        public void SetLoadStateDescription(string loadState)
        {

            if (loadStateDescription == null)
                loadStateDescription = Util.FindChild<Text>(gameObject, "LodingText", true);

            loadStateDescription.text = $"Load {loadState}";

        }

        public IEnumerator LoadGagueUpdate(float loadPer)
        {
            if(loadFillGague == null)
            {
                loadFillGague = Util.FindChild<Image>(gameObject, "LogdingGague", true);
            }

            while(!Mathf.Approximately(loadFillGague.fillAmount, loadPer))
            {
                loadFillGague.fillAmount = Mathf.Lerp(loadFillGague.fillAmount,loadPer, Time.deltaTime * 2f);

                yield return null;
            }
        }
    }
}
