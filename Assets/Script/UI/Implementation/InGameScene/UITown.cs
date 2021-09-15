using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eonix.UI
{
    public class UITown : UIWindow
    {
        public GameObject pVECanvas;
        public GameObject shopCanvas;
        public GameObject inventoryCanvas;
        public GameObject partyCanvas;
        private void Awake()
        {
            pVECanvas = Util.Util.FindChild(gameObject, "PvEUI", false);
            shopCanvas = Util.Util.FindChild(gameObject, "ShopUI", false);
            inventoryCanvas = Util.Util.FindChild(gameObject, "InventoryUI", false);
            partyCanvas = Util.Util.FindChild(gameObject, "PartyUI", false);
        }

        public override void Start()
        {
            base.Start();

            canCloseESC = false;

            Open();
        }

        public void PressPvEButton()
        {
            UIWindowManager.Instance.GetWindow<UIPvE>().Open();  
        }

        public void PressShopCanvas()
        {
            UIWindowManager.Instance.GetWindow<UIShop>().Open();
        }   

        public void PressInventoryCanvas()
        {
            UIWindowManager.Instance.GetWindow<UIInventory>().Open();
        }

        public void PreessPartyButton()
        {
            UIWindowManager.Instance.GetWindow<UIParty>().Open();
        }

    }
}
