using Eonix.Define;
using Eonix.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Eonix.Manager
{
    public class InputManager : SingleTon<InputManager>
    {
        public Action KeyAction = null;
        public Action<Define.Define.MouseEvent> MouseAction = null;

        public bool _press = false;

        protected override void Awake()
        {
            base.Awake();

            if(gameObject != null)
            {
                DontDestroyOnLoad(this);
            }
        }

        public void OnUpdate()
        {
            if (Input.anyKey == false && MouseAction == null)
                return;
            if (Input.anyKey && KeyAction != null && MouseAction == null)
            {
                KeyAction.Invoke();
            }
            
            if(MouseAction != null)
            {
                if(EventSystem.current == null) return;

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Input.GetMouseButton(0) && !_press)
                {
                    MouseAction.Invoke(Define.Define.MouseEvent.Press);
                    _press = true;

                }

                if(Input.GetMouseButtonUp(0) && _press)
                {
                    _press = false;
                }
            }
        }
    }
}
