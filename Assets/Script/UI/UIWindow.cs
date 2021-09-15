using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        private CanvasGroup cachedCanvasGroup;

        public CanvasGroup CachedCanvasGroup
        {
            get
            {
                if (cachedCanvasGroup == null)
                {
                    cachedCanvasGroup = GetComponent<CanvasGroup>();
                }

                return cachedCanvasGroup;
            }
        }

        public bool canCloseESC;

        public bool isOpen;

        public virtual void Start()
        {
            InitWindow();
        }

        public virtual void InitWindow()
        {
            UIWindowManager.Instance.AddTotalWindow(this);

            if (!isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public virtual void Open(bool force = false)
        {
            if(!isOpen || force)
            {
                isOpen = true;

                UIWindowManager.Instance.AddOpendWindow(this);

                SetCanvasGroup(isOpen);

                transform.SetAsLastSibling();
            }
        }

        public virtual void Close(bool force = false)
        {
            if(isOpen || force)
            {
                isOpen = false;

                UIWindowManager.Instance.RemoveOpendWindow(this);
                SetCanvasGroup(false);
            }
        }

        private void SetCanvasGroup(bool isActive)
        {
            CachedCanvasGroup.alpha = Convert.ToInt32(isActive);
            CachedCanvasGroup.interactable = isActive;
            CachedCanvasGroup.blocksRaycasts = isActive;
        }

        private void OnDestroy()
        {
            UIWindowManager.Instance.RemoveOpendWindow(this);
        }


    }
}
