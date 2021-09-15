using Eonix.RM;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Eonix.Util;
using UnityEngine.UI;

namespace Eonix.UI
{
    public class UIParty : UIWindow
    {
        #region GameObject
        public List<GameObject> partyCardObjects = new List<GameObject>();
        public List<GameObject> listCardObject = new List<GameObject>();
        #endregion

        #region Value
        public int partyCount = -1;
        public int listCount = -1;

        public static int page = 0;
        #endregion

        #region Outter Script
        public List<DB.BoHero> allHeroesList = new List<DB.BoHero>();

        public List<DB.BoHero> inPartyHeroList = new List<DB.BoHero>();
        public List<DB.BoHero> inListHeroList = new List<DB.BoHero>();

        public List<Card> party_CardCompo = new List<Card>();
        public List<Card> list_CardCompo = new List<Card>();

        public Card clickedCard;
        #endregion

        #region override Method
        public override void InitWindow()
        {
            isOpen = true;

            base.InitWindow();

            OutterScriptInit();
            ObjectInit();
            ValueInit();
            HeroCurrentStateInit();
            ComponentsInit();

            canCloseESC = true;
        }

        public override void Open(bool force = false)
        {
            base.Open(force);
        }

        public override void Close(bool force = false)
        {
            base.Close(force);

            DB.DataBaseManager.Instance.UpdateMyData<DB.DtoRetainedHero>(SerializationUtil.DtoToParam(new DB.DtoRetainedHero()));
        }
        #endregion

        #region Init Methods
        private void OutterScriptInit()
        {
            allHeroesList = GameManager.User.boHeroes.boHeroes;
        }

        private void ObjectInit()
        {
            CardObjectInit();
        }

        private void CardObjectInit()
        {
            var images_Object = Util.Util.FindChild(gameObject, "Images", true);

            var party_Object = images_Object.transform.GetChild(0).gameObject;
            var list_Object = images_Object.transform.GetChild(1).gameObject;

            for(int i = 0; i < party_Object.transform.childCount; i++)
            {
                var addedObject = party_Object.transform.GetChild(i).gameObject;

                partyCardObjects.Add(addedObject);
                party_CardCompo.Add(addedObject.GetComponent<Card>());
            }

            for(int i = 0; i < list_Object.transform.childCount; i++)
            {
                var addedObject = list_Object.transform.GetChild(i).gameObject;

                listCardObject.Add(addedObject);
                list_CardCompo.Add(addedObject.GetComponent<Card>());
            }

        }

        private void ValueInit()
        {
            partyCount = allHeroesList.Select(_ => _.inParty == true).Count();
            listCount = allHeroesList.Select(_ => _.inParty == false).Count();
        }

        private void ComponentsInit()
        {
            CardCompoInit();
            CardCompoInit_Hero();
        }

        private void CardCompoInit()
        {
            foreach(Card partyCard in party_CardCompo) 
            {
                partyCard.SetCardType(Define.CardType.Party);
            }

            foreach(Card listCard in list_CardCompo)
            {
                listCard.SetCardType(Define.CardType.List);
            }
        }

        private void CardCompoInit_Hero()
        {   
            foreach(Card partyCard in party_CardCompo)
            {
                var inPartyHero = inPartyHeroList.Where(_ => _.heroPartyIndex == partyCard.CardNumber_Party).SingleOrDefault();

                if(inPartyHero == null)
                {
                    partyCard.SetCardInfo(null);
                }
                else
                {
                    partyCard.SetCardInfo(inPartyHero);
                }
            }

            for(int i = (page * 5); i < (page*5) + 5; i++)
            {
                var inListHero = inListHeroList.Where(_ => _.heroListIndex == i).SingleOrDefault();

                if(inListHero == null)
                {
                    list_CardCompo[i % 5].SetCardInfo(null);
                }
                else
                {
                    list_CardCompo[i % 5].SetCardInfo(inListHero);
                }
            }
        }

        private void HeroCurrentStateInit()
        {
            inPartyHeroList = allHeroesList.Where(_ => _.inParty == true).ToList();
            inListHeroList = allHeroesList.Where(_ => _.inParty == false).ToList();
        }
        #endregion

        #region ClickCard
        public void ClickCard(GameObject clickedObject)
        {
            if (clickedCard != null)
            {
                if (clickedCard.WaitChange) { ClickJoinButton();}

                if (clickedCard.Flip) clickedCard.Flip = false;



                if (clickedCard == clickedObject.GetComponent<Card>())
                {
                    clickedCard = null;
                    return; 
                }

                clickedCard = null;
            }

            clickedCard = clickedObject.GetComponent<Card>();
            clickedCard.Flip = !clickedCard.Flip;
        }

        public void ClickJoinButton()
        {
            clickedCard.WaitChange = !clickedCard.WaitChange;

            foreach(Card partyCard in party_CardCompo)
            {
                partyCard.Marker.SetActive(!partyCard.Marker.activeSelf);
            }
        }

        public void ClickMarker()
        {
            ClickJoinButton();

            // 클릭한 오브젝트 가져온 뒤
            var clickedMarkObject = EventSystem.current.currentSelectedGameObject.gameObject;

            // 그 상위 객체에 있는 카드 컴포넌트 가져오고
            var Card_Compo = clickedMarkObject.GetComponentInParent<Card>();

            var newPartyHero = SetPartyIndex(clickedCard.BoHero, Card_Compo.CardNumber_Party);
            var newListHero = SetListIndex(Card_Compo.BoHero);

            clickedCard.BoHero = null;
            Card_Compo.BoHero = null;

            clickedCard.Flip = false;

            HeroCurrentStateInit();

            if (newPartyHero != null)
            {
                var changeHero_inParty = inPartyHeroList.Where(_ => (_.inParty == true) && (_.heroListIndex != -1)).SingleOrDefault();
                var changeHero_inParty_Listindex = changeHero_inParty.heroListIndex;
                var changeIndexListHeroes = inListHeroList.Where(_ => _.heroListIndex > changeHero_inParty_Listindex).ToList();

                foreach (DB.BoHero bohero in changeIndexListHeroes)
                {
                    bohero.heroListIndex -= 1;
                }

                changeHero_inParty.heroListIndex = -1;

                Define.IndexSetter.listIndex = inListHeroList.Count;
            }

            Card_Compo.SetCardInfo(newPartyHero);

            if (newListHero != null)
            {
                var changeHero_inList = inListHeroList.Where(_ => (_.inParty == false) && (_.heroPartyIndex != -1)).SingleOrDefault();
                changeHero_inList.heroPartyIndex = -1;
                newListHero.heroListIndex = Define.IndexSetter.listIndex++;
            }

            clickedCard.SetCardInfo(newListHero);

            CardCompoInit_Hero();

            clickedCard = null;
        }

        private DB.BoHero SetPartyIndex(DB.BoHero bohero, int partyIndex)
        {
            bohero.inParty = true;
            bohero.heroPartyIndex = partyIndex;

            return bohero;
        }

        private DB.BoHero SetListIndex(DB.BoHero bohero)
        {
            if (bohero != null)
            {
                bohero.inParty = false;
            }

            return bohero;
        }

        public void ClickExcludeButton()
        {
            Debug.Log(Define.IndexSetter.listIndex);

            var clickedHero = clickedCard.BoHero;

            clickedCard.BoHero = null;

            clickedCard.Flip = false;

            clickedHero.inParty = false;
            clickedHero.heroPartyIndex = -1;

            clickedHero.heroListIndex = Define.IndexSetter.listIndex;

            HeroCurrentStateInit();
            CardCompoInit_Hero();

            clickedCard = null;
        }
        #endregion
    }
}