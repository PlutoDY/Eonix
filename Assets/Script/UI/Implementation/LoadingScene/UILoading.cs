using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    public class UILoading : UIWindow
    {
        private string dot = string.Empty;
        private const string loadStateDescription = "Load Next Scene";

        public Text loadStateDesc;
        public Image loadGague;

        public Camera cam;

        private void Update()
        {
            loadGague.fillAmount = GameManager.Instance.loadProgress;

            if(Time.frameCount %20 == 0)
            {
                if (dot.Length >= 3)
                    dot = string.Empty;
                else
                    dot = string.Concat(dot, ".");

                loadStateDesc.text = $"{loadStateDescription}{dot}";
            }
        }
    }
}
