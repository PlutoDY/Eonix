using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BattleCalculateHelper : MonoBehaviour
{
    public Tuple<int, int, bool> CalculationDice(float powerValue, int defenseValue)
    {
        int power_intager = (int)powerValue;

        var power = Random.Range(power_intager / 2, power_intager+1);
        var defense = Random.Range((defenseValue / 2), defenseValue+1);

        var battleResult = power >= defense ? true : false;

        return new Tuple<int, int,bool>(power, defense, battleResult);
    }

    public Tuple<int, int, bool> CalculationDice(float rateHit)
    {
        var power = Random.Range(1, 101);
        var defense = 100-(int)rateHit;

        var battleResult = power >= defense ? true : false;

        return new Tuple<int, int, bool>(power, defense, battleResult);
    }

}
