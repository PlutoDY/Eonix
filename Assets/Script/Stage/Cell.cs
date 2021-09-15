using Eonix.RM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Util;

namespace Eonix.Stage
{
    using PrefabsPanels = Define.PrefabsPath.PrefabsPanels;

    public class Cell : MonoBehaviour, IPoolableObject
    {
        [SerializeField]
        private PrefabsPanels panelName;
        [SerializeField]
        private int index;

        public int Index { get { return index; } private set { index = value; } }
        public PrefabsPanels PanelName { get { return panelName; } }

        [SerializeField]
        public List<Tuple<Cell, int>> neighborCell = new List<Tuple<Cell, int>>();

        public GameObject Checker;

        public bool CanRecyle { get; set; } = true;
        public Action OnRecyleStart { get; set; }

        public int distanceHeroWeights = 0;
        public int distanceMonsterWeights = 0;

        public void SetInfo(int index, PrefabsPanels panelName)
        {
            Checker = transform.GetChild(0).gameObject;
            CanRecyle = false;

            this.index = index;

            this.panelName = panelName;

            InstantiateCell();
        }

        public void InstantiateCell()
        {
            Instantiate(ResourceManager.Instance.LoadObject(Define.PrefabsPath.PanelPath(panelName)),gameObject.transform);
        }

        public void SetNeighborCells(Cell gameObject, int weights)
        {
            neighborCell.Add(new Tuple<Cell,  int>(gameObject, weights));
        }
    }
}