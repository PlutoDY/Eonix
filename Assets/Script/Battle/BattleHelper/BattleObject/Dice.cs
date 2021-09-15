using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Define;

using ArtType = Eonix.Define.SpriteArtPath.ArtType;

public class Dice : MonoBehaviour
{
    #region Sprite

    private Sprite nomalDice;
    private Sprite breakDice;

    #endregion

    public Animation _animation;

    public Image _image;

    public GameObject diceValueObject;
    public Text diceValue_TextComponent;

    public GameObject rollingNumberObject;

    public void InitDice()
    {
        _animation = GetComponent<Animation>();
        _image = GetComponent<Image>();

        var diceValue = transform.GetChild(0);

        diceValueObject = diceValue.gameObject;
        diceValue_TextComponent = diceValue.GetComponent<Text>();

        rollingNumberObject = transform.GetChild(1).gameObject;

        nomalDice = SpriteArtPath.sprite(ArtType.Battle, DicePath.nomalPath);
        breakDice = SpriteArtPath.sprite(ArtType.Battle, DicePath.breakPath);

        ResetDice();
    }

    public void ResetDice()
    {
        _animation.enabled = true;

        _image.sprite = nomalDice;

        diceValueObject.SetActive(false);
        diceValue_TextComponent.text = string.Empty;
        
        rollingNumberObject.SetActive(true);
    }

    public void SetDiceValue(int value)
    {
        _animation.enabled = false;

        diceValueObject.SetActive(true);
        diceValue_TextComponent.text = $"{value}";

        rollingNumberObject.SetActive(false);
    }

    public void ChangeDiceImage(bool isWin)
    {
        if (isWin)
        {
            _image.sprite = nomalDice;
        }
        else
        {
            _image.sprite = breakDice;
        }
    }
}
