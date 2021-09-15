using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.Controller
{
    public class Controller : MonoBehaviour
    {
        public virtual void Start()
        {
            InitController();
        }

        public virtual void InitController()
        {
            ControllerManager.Instance.AddTotalController(this);
        }

        public void OnDestroy()
        {
            ControllerManager.Instance.RemoveController(this);
        }
    }
}