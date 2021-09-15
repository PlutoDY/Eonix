using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.Battle
{

    public class BattleDiceHelper : MonoBehaviour
    {
        [SerializeField]
        List<Dice> dices = new List<Dice>();

        public void InitDices(List<GameObject> gObjList)
        {
            foreach(GameObject gObj in gObjList)
            {
                Dice newDice = new Dice();

                gObj.TryGetComponent<Dice>(out newDice);

                newDice.InitDice();

                dices.Add(newDice);
            }
        }

        public void SetDicesValue(int heroValue, int monsterValue)
        {
            dices[0].SetDiceValue(heroValue);

            dices[1].SetDiceValue(monsterValue);
        }

        public void StartRolling()
        {
            dices.ForEach(_ => _.ResetDice());
        }

        public void SetDicesImage(bool isWin)
        {
            dices[0].ChangeDiceImage(isWin);

            dices[1].ChangeDiceImage(!isWin);
        }
    }
}
