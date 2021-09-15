using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Define;


namespace Eonix.UI
{
    public class Card : MonoBehaviour
    {
        #region Value
        private const string nomalPath = "Card_None";
        private const string backCardPath = "Card_Back";

        private Sprite nomalCardSprite = null;
        private Sprite backCardSprite = null;

        [SerializeField]
        private CardType cardType;

        public CardType CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        [SerializeField]
        private bool flip = false;
        public bool Flip
        {
            get { return flip; }
            set
            {
                flip = value;
                FlipCard();
            }
        }

        [SerializeField]
        private int cardNumber_Party = -1;
        public int CardNumber_Party
        {
            get { return cardNumber_Party; }
            set { cardNumber_Party = value; }
        }

        private bool waitChange = false;
        public bool WaitChange
        {
            get { return waitChange; }
            set 
            { 
                waitChange = value;
                ChangeJoinButtonText();
            }
        }
        #endregion

        #region OutterScript

        [SerializeField]
        private DB.BoHero boHero;

        public DB.BoHero BoHero
        {
            get { return boHero; }
            set { boHero = value; }
        }
        #endregion

        #region InnerCompo

        private Image cardImage = null;

        public Image CardImage
        {
            get { return cardImage; }
            set { cardImage = value; }                              
        }

        private Button flipButton;
        public Button FlipButton
        {
            get { return flipButton; }
            set { flipButton = value; }
        }

        #endregion

        #region OutterCompo
        private Button excludeButton;
        public Button ExcludeButton
        {
            get { return excludeButton; }
            private set { excludeButton = value; }
        }

        private Button joinButton;
        public Button JoinButton
        {
            get { return joinButton; }
            private set { joinButton = value; }
        }

        private Text joinButtonText;
        public Text JoinButtonText
        {
            get { return joinButtonText; }
            private set { joinButtonText = value;}
        }

        #endregion

        #region GameObjects
        [SerializeField]
        private GameObject marker;

        public GameObject Marker
        {
            get { return marker; }
            private set { marker = value; }
        }
        #endregion

        public void InitCard()
        {
            CardImage = GetComponent<Image>();
            FlipButton = GetComponent<Button>();

            ExcludeButton = gameObject.transform.GetChild(0).GetComponent<Button>();
            JoinButton = gameObject.transform.GetChild(1).GetComponent<Button>();

            nomalCardSprite = SpriteArtPath.sprite(SpriteArtPath.ArtType.Card, nomalPath);
            backCardSprite = SpriteArtPath.sprite(SpriteArtPath.ArtType.Card, backCardPath);

            JoinButtonText = JoinButton.GetComponentInChildren<Text>();
        }

        public void SetCardType(CardType cardType)
        {
            InitCard();

            CardType = cardType;

            if(CardType == CardType.Party)
            {
                CardNumber_Party = IndexSetter.cardIndex++;

                Marker = gameObject.transform.GetChild(2).gameObject;

                ExcludeButton.interactable = true;
                JoinButton.interactable = false;
            }
            else
            {
                ExcludeButton.interactable = false;
                JoinButton.interactable = true;
            }
        }

        public void SetCardInfo(DB.BoHero boHero)
        {

            if (boHero != null)
            {
                this.boHero = boHero;
            }
            else { this.boHero = null; }

            if(this.boHero == null)
            {
                cardImage.sprite = nomalCardSprite;
                FlipButton.interactable = false;
            }
            else
            {
                cardImage.sprite = SpriteArtPath.sprite(SpriteArtPath.ArtType.Card, boHero.boHeroInfo.sdHeroinfo.heroCardResourcePath);
                FlipButton.interactable = true;
            }
        }

        public void FlipCard()
        {
            if (Flip)
            {
                CardImage.sprite = backCardSprite;
            }
            else
            {
                if (boHero != null)
                {
                    CardImage.sprite = SpriteArtPath.sprite(SpriteArtPath.ArtType.Card, boHero.boHeroInfo.sdHeroinfo.heroCardResourcePath);
                }
                else
                {
                    CardImage.sprite = nomalCardSprite;
                }
            }
            

            ExcludeButton.gameObject.SetActive(Flip);
            JoinButton.gameObject.SetActive(Flip);
        }

        public void ChangeJoinButtonText()
        {

        }
    }
}
