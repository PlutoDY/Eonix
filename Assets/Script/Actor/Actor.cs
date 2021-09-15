using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.Actor
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private int currentIndex;
        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        [SerializeField]
        private int moveIndex = -1;
        public int MoveIndex
        {
            get { return moveIndex; }
            set { moveIndex = value; }
        }

        [SerializeField]
        private bool canMove;
        public bool CanMove
        {
            get { return canMove; }
            set { canMove = value; }
        }

        [SerializeField]
        private bool isLive = true;
        public bool IsLive
        {
            get { return isLive; }
            set { isLive = true; }
        }
    }
}
