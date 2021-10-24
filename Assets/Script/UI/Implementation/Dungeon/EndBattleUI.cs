using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Battle;
using Eonix.Actor;
using System.Linq;

namespace Eonix.UI
{
    using SceneType = Define.SceneType;

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

        [SerializeField]
        private List<Hero> liveHeroList = new List<Hero>();
        public List<Hero> LiveHeroList
        {
            get { return liveHeroList; }
            set { liveHeroList = value; }
        }

        [SerializeField]
        private List<Hero> deadHeroList = new List<Hero>();
        public List<Hero> DeadHeroList
        {
            get { return deadHeroList; }
            set { deadHeroList = value; }
        }

        [SerializeField]
        private int addExp;
        public int AddExp
        {
            get { return addExp; }
            set { addExp = value; }
        }

        #endregion

        public override void Start()
        {
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

            SetExpGague();
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

            var currentExp = hero.HeroInfo.currentExp;
            var maxExp = hero.HeroInfo.maxExp;

            levelText_TextCompo[count].text = $"Lv.{heroLevel} {currentExp} / {maxExp}";
            expGagueObject_ImageCompoList[count].fillAmount = currentExp / maxExp;
        }

        private void InitNoneHeroCard(int count)
        {
            heroCardObject_ImageCompoList[count].sprite = Define.SpriteArtPath.sprite(Define.SpriteArtPath.ArtType.Card, "Card_None");
        }

        #endregion

        #region Set Exp Bar
        float[] targetFillAmount = new float[3];
        
        [SerializeField]
        int[] startLevel = new int[3];
        [SerializeField]
        float[] startExp = new float[3];
        [SerializeField]
        int[] endLevel = new int[3];
        [SerializeField]
        float[] endExp = new float[3];

        public void SetExpGague()
        {
            for(int i = 0; i < heroList.Count; i++)
            {
                var beforeLevel = heroList[i].HeroInfo.heroLevel;

                startLevel[i] = beforeLevel;
                
                var beforeCurrentExp = heroList[i].HeroInfo.currentExp;

                startExp[i] = beforeCurrentExp;

                var beforeMaxExp = heroList[i].HeroInfo.maxExp;

                var remainFillAmount = 1 - (beforeCurrentExp / beforeMaxExp);

                if (liveHeroList.Contains(heroList[i]))
                {
                    heroList[i].SetExp(AddExp);
                }
                else
                {
                    heroList[i].SetExp(AddExp / 2);
                }

                var afterLevel = heroList[i].HeroInfo.heroLevel;

                endLevel[i] = afterLevel;

                var currentExp = heroList[i].HeroInfo.currentExp;

                endExp[i] = currentExp;

                var maxExp = heroList[i].HeroInfo.maxExp;

                if (afterLevel - beforeLevel > 1)
                {
                    targetFillAmount[i] = (currentExp / maxExp) + (remainFillAmount) + ((afterLevel - beforeLevel - 1));
                }
                else if(afterLevel - beforeLevel == 1)
                {
                    targetFillAmount[i] = (currentExp / maxExp) + remainFillAmount;
                }
                else
                {
                    targetFillAmount[i] = (currentExp / maxExp) - (beforeCurrentExp / beforeMaxExp);
                }

                targetFillAmount[i] /= 100f;
            }

            StartCoroutine(UpdateExpGague());
        }

        private IEnumerator UpdateExpGague()
        {
            int counter = 0;

            while (counter < 100)
            {

                for (int i = 0; i < heroList.Count; i++)
                {
                    if (expGagueObject_ImageCompoList[i].fillAmount >= 0.99f) { expGagueObject_ImageCompoList[i].fillAmount = 0f; }

                    expGagueObject_ImageCompoList[i].fillAmount += targetFillAmount[i];

                    if (liveHeroList.Contains(heroList[i]))
                    {
                        SetExpText(i, AddExp / 100f);
                    }
                    else
                    {
                        SetExpText(i, AddExp / 200f);
                    }
                }

                yield return new WaitForSeconds(0.01f);

                counter++;
            }
        }



        private void SetExpText(int count, float addExp)
        {
            if (startLevel[count] >= endLevel[count] && startExp[count] >= endExp[count])
            {
                var te = $"Lv. {heroList[count].HeroInfo.heroLevel} " +
                    $"{heroList[count].HeroInfo.currentExp} / {heroList[count].HeroInfo.maxExp}";

                levelText_TextCompo[count].text = te;

                return;
            }

            var maxExp = GameManager.SD.sdMaxExpInfos[startLevel[count]].maxExp;

            if(startExp[count] >= maxExp && startLevel[count] < endLevel[count])
            {
                startLevel[count]++;

                startExp[count] = 0;

                SetExpText(count, addExp);

                return;
            }

            startExp[count] += addExp;

            var text = $"Lv. {startLevel[count]} {(int)startExp[count]} / {(int)maxExp}";

            levelText_TextCompo[count].text = text;

        }

        #endregion

        #region Click Button (Push Button)

        public void PushExitButton()
        {
            var stageManager = StageManager.Instance;
            var user = GameManager.User;

            user.boStage.sdStage = GameManager.SD.sdStages.Where(_ => _.num == 1).FirstOrDefault();

            GameManager.Instance.LoadScene(SceneType.InGame, stageManager.ChangeStage(), stageManager.OnChangeStageComplete);
        }
        
        public void PushRetryButton()
        {

        }

        #endregion
    }
}