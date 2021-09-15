using Eonix.Define;
using Eonix.RM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.UI
{
    public class UISignupComplete : UIWindow
    {
        UnityEngine.Object setNickname;

        public void Awake()
        {
            setNickname = ResourceManager.Instance.LoadObject(Resource.UIPath + "/SetNickNameCanvas");
        }

        public void PressCompleteButton()
        {
            Instantiate(setNickname);
            Close();
        }
    }

}