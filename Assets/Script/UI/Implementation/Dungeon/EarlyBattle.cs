using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    public class EarlyBattle : UIWindow
    {
        public int turn = 0;
        
        public Text TurnText;

        public Image[] Images;

        bool _isLerp = true;

        bool _isMyTurn = false;

        float _timeStartedLerping;

        public override void Start()
        {
            isOpen = true;

            base.Start();

            turn = 1;

            Open();

            _isMyTurn = true;

            _timeStartedLerping = Time.time;

            TurnText.text = $"{turn}";
        }


        private void Update()
        {
            if (_isMyTurn)
            {
                Open();
                TurnStart();

                if (!_isMyTurn)
                {
                    ++turn;
                    Close();
                }
            }
        }

        public void TurnStart()
        {
            if (_isLerp)
            {
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / 2;

                gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, percentageComplete);

                if (percentageComplete >= 1.0f) { _isLerp = !_isLerp; _isMyTurn = false; }
            }
        }

        public void TurnReStart()
        {
            _timeStartedLerping = Time.time;
            _isMyTurn = true;
            _isLerp = true;

            Open();
        }
    }
}
