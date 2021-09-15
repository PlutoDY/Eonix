using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Battle;
using Eonix.Actor;
using System.Linq;

namespace Eonix.UI
{
    public class EndBattleUI : UIWindow
    {
        private const int maxHeroCount = 3;

        #region Script of UIWindow

        public override void InitWindow()
        {
            isOpen = true;
            canCloseESC = false;

            base.InitWindow();

            InitObjects();
        }

        public override void Open(bool force = false)
        {
            SetHero();

            base.Open(force);
        }

        #endregion

        #region UI Object
        private Transform rootPanel_TransformCompo;

        [SerializeField]
        private GameObject heroCardImageParentObject;

        [SerializeField]
        private List<GameObject> levelUpObjectList;

        #region Text Compo

        [SerializeField]
        private Text battleResultObject_TextCompo;

        [SerializeField]
        private List<Text> heroName_TextCompo;

        [SerializeField]
        private List<Text> levelText_TextCompo;

        #endregion

        #region Image Compo

        [SerializeField]
        private List<Image> heroCardObject_ImageCompoList;

        [SerializeField]
        private List<Image> expGagueObject_ImageCompoList;
        #endregion

        #endregion

        #region Buttons

        [SerializeField]
        private GameObject buttonObjectsParent;

        [SerializeField]
        private Button backTownButton;

        [SerializeField]
        private Button retryButton;

        #endregion

        #region Value

        private List<Hero> heroList = new List<Hero>();

        private List<Hero> liveHeroList = new List<Hero>();
        public List<Hero> LiveHeroList
        {
            get { return liveHeroList; }
            set { liveHeroList = value; }
        }

        private List<Hero> deadHeroList = new List<Hero>();
        public List<Hero> DeadHeroList
        {
            get { return deadHeroList; }
            set { deadHeroList = value; }
        }

        #endregion

        public override void Start()
        {
            InitObjects();

            isOpen = true;
            canCloseESC = true;


            base.Start();
        }

        #region Init Method

        public void InitObjects()
        {
            rootPanel_TransformCompo = transform.GetChild(0);

            battleResultObject_TextCompo = rootPanel_TransformCompo.GetChild(1).GetComponent<Text>();

            heroCardImageParentObject = rootPanel_TransformCompo.GetChild(2).gameObject;

            for(int i = 0; i < maxHeroCount; i++)
            {
                heroCardObject_ImageCompoList.Add(heroCardImageParentObject.transform.GetChild(i).GetComponent<Image>());

                heroName_TextCompo.Add(heroCardObject_ImageCompoList[i].transform.GetChild(1).GetComponent<Text>());
                expGagueObject_ImageCompoList.Add(heroCardObject_ImageCompoList[i].transform.GetChild(0).GetChild(0).GetComponent<Image>());
                levelText_TextCompo.Add(heroCardObject_ImageCompoList[i].transform.GetChild(0).GetChild(1).GetComponent<Text>());
                levelUpObjectList.Add(levelText_TextCompo[i].transform.GetChild(0).gameObject);
            }

            InitButton();
        }

        private void InitButton()
        {
            buttonObjectsParent = rootPanel_TransformCompo.GetChild(3).gameObject;

            backTownButton = buttonObjectsParent.GetComponentsInChildren<Button>()[0];
            retryButton = buttonObjectsParent.GetComponentsInChildren<Button>()[1];
        }
        #endregion

        #region Set Method

        private void SetHero()
        {
            SetBattleResultText();

            heroList.AddRange(liveHeroList);
            heroList.AddRange(deadHeroList);

            heroList = heroList.OrderBy(_ => _.HeroInfo.heroPartyIndex).ToList();

            for(int i = 0; i < maxHeroCount; i++)
            {
                if (heroList.Count <= i)
                {
                    InitNoneHeroCard(i);
                    continue;
                }

                InitHeroCard(heroList[i], i);
            }
        }

        private void SetBattleResultText()
        {
            if (liveHeroList.Count == 0)
            {
                battleResultObject_TextCompo.text = "DETEAT..";
                battleResultObject_TextCompo.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                battleResultObject_TextCompo.text = "VICTORY!";
                battleResultObject_TextCompo.color = new Color(255, 215, 0, 255);
            }
        }

        private void InitHeroCard(Hero hero, int count)
        {
            var heroCardResourcePath = hero.HeroInfo.boHeroInfo.sdHeroinfo.heroCardResourcePath;

            heroCardObject_ImageCompoList[count].sprite = Define.SpriteArtPath.sprite(Define.SpriteArtPath.ArtType.Card, heroCardResourcePath);
            heroName_TextCompo[count].text = hero.HeroInfo.boHeroInfo.sdHeroinfo.heroName;

            var heroLevel = hero.HeroInfo.heroLevel;

            var currentExp = hero.HeroInfo.heroCurrentExp;
            var maxExp = hero.HeroInfo.maxExp;

            levelText_TextCompo[count].text = $"Lv.{heroLevel} {currentExp}/{maxExp}";
            expGagueObject_ImageCompoList[count].fillAmount = currentExp / maxExp;
        }

        private void InitNoneHeroCard(int count)
        {
            heroCardObject_ImageCompoList[count].sprite = Define.SpriteArtPath.sprite(Define.SpriteArtPath.ArtType.Card, "Card_None");
        }

        private float currentFillAmount;
        private int loopCount = 0;

        public void SetGauge(int count, int loop, float leftFillAmount)
        {

            StartCoroutine(ExpBarFilling(count, loop, leftFillAmount));
        }

        private IEnumerator ExpBarFilling(int count, int loop, float leftAmount)
        {
            loopCount = loop;

            for(float f = currentFillAmount; ;)
            {
                if(loopCount != 0)
                {
                    f += (1 - currentFillAmount) / 100f;
                }
                else
                {
                    f += (leftAmount - currentFillAmount) / 100;
                }

                if(f >= .99f && loopCount == 0)
                {
                    break;
                }
                else if(f >= .99f && loopCount != -1)
                {
                    loopCount--;
                    f = 0;
                }

                expGagueObject_ImageCompoList[count].fillAmount = f;

                yield return new WaitForSeconds(.01f);
            }
        }

        #endregion
    }
}