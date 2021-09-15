using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.UI
{
    public class UIInventory : UIWindow
    {
        public override void Start()
        {
            isOpen = true;

            base.Start();

            canCloseESC = true;
        }
    }
}