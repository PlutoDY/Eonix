using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Define;
using System.Linq;

namespace Eonix.Stage
{
    using PrefabsPanels = PrefabsPath.PrefabsPanels;

    public class VoidFristStage : BaseStage
    {
        private List<int> floorIndexList = new List<int>() { 12 };

        public override void Start()
        {
            Width = 5;
            Height = 5;

            FloorIndex = floorIndexList;

            base.Start();
        }


        public override void SetCellsInfo()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                var cell = Cells[i];

                if (floorIndexList.Contains(i))
                {
                    cell.SetInfo(i, PrefabsPanels.Floor3X3);
                }
                else
                {
                    cell.SetInfo(i, PrefabsPanels.VoidPanel);
                }
            }

            SetNeighborCells();
        }

        public override void SetNeighborCells()
        {
            base.SetNeighborCells();
        }
    }
}
