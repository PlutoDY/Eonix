using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eonix.Util;
using Eonix.Define;
using Eonix.DB;
using Eonix.RM;
using System.Linq;
using System;
using Eonix.Manager;

namespace Eonix.Stage
{
    using PanelName = Define.PrefabsPath.PrefabsPanels;
    using MoveDistance = Define.MoveDistance;


    public abstract class BaseStage : SingleTon<BaseStage>
    {
        /// <summary>
        /// ���� ���� �� ����
        /// </summary>
        [SerializeField]
        private int width;

        /// <summary>
        /// ���� ���� �� ����
        /// </summary>
        [SerializeField]
        private int height;

        /// <summary>
        /// width(BaseStage-12) ������Ƽ
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// height(BaseStage-17) ������Ƽ
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// ���� ���� Cell Ŭ���� ��ũ��Ʈ List
        /// </summary>
        [SerializeField]
        private Cell[] cells;

        public Cell[] Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        /// <summary>
        /// �ʿ��� ������ �� ���� Cell�� ������ �ִ� List
        /// </summary>
        [SerializeField]
        private List<int> floorIndex = new List<int>();

        public List<int> FloorIndex
        {
            get { return floorIndex; }
            set { floorIndex = value; }
        }

        /// <summary>
        /// Cell�� ó�� ���� ��ġ(= 0�� Cell{cells[0]} ��ġ)
        /// </summary>
        [SerializeField]
        private Vector3 startPosition = Vector3.zero;
        
        /// <summary>
        /// Cell�� Cell���� ���� ũ��
        /// </summary>
        private const float _moveDelta = 3.3f;

        /// <summary>
        /// ī�޶�
        /// </summary>
        private Camera stageCam;
        public Camera StageCam
        {
            get { return stageCam; }
        }

        public virtual void Start()
        {
            InitializationStage();
            InstantiateCells();

            SetCellsInfo();
        }

        private void InitializationStage()
        {
            cells = new Cell[width * height];

            startPosition = new Vector3((width / 2) * -_moveDelta, 0, (height / 2) * -_moveDelta);

            var camYPos = (height * 3.3f + 2) / 1.73f;
            stageCam = FindObjectOfType<Camera>();
            stageCam.transform.position =
                new Vector3(
                    0,
                    camYPos + 1,
                    startPosition.z - 2
                );


            Define.Battle.InitText();
        }

        public virtual void InstantiateCells()
        {
            var emptyCellResourcePath = PrefabsPath.PanelPath(PanelName.EmptyCell);

            ResourceManager.Instance.LoadPoolableObject<Cell>(PoolType.EmptyCell, emptyCellResourcePath, cells.Length);

            var CellPool = ObjectPoolManager.Instance.GetPool<Cell>(PoolType.EmptyCell);

            for (int i = 0; i < cells.Length; i++)
            {
                var cell = CellPool.GetPoolableObject(_ => _);

                cell.transform.position =
                    new Vector3(
                        startPosition.x + ((i % Width) * _moveDelta),
                        0f,
                        startPosition.z + ((i / Width) * _moveDelta)
                        );

                cell.transform.parent = gameObject.transform;

                cell.gameObject.SetActive(true);

                Cells[i] = cell;
            }
        }

        /// <summary>
        /// �ʿ� Cell���� ���� �Ŀ� Cell���� ���� �Է�
        /// </summary>
        public abstract void SetCellsInfo();

        public virtual void SetNeighborCells()
        {
            MoveDistance.SetMoveDistance(Width);

            for(int cellIndex = 0; cellIndex < Cells.Length; cellIndex++)
            {
                for(int j = 0; j < MoveDistance.moveDistance.Length; j++)
                {
                    var movedis = MoveDistance.moveDistance[j];
                    var checkIndex = movedis + cellIndex;

                    if(FloorIndex.Contains(cellIndex) || FloorIndex.Contains(checkIndex)) continue;
                    if (checkIndex < 0 || checkIndex >= Cells.Length) continue;
                    if ((cellIndex % Width == 0) && ((movedis == Width - 1) || (movedis == -1) || (movedis == -Width - 1))) continue;
                    if ((cellIndex % Width == Width - 1) &&( (movedis == Width + 1) || (movedis == 1) || (movedis == -Width + 1))) continue;

                    var weights = ((movedis == -1) || (movedis == 1) || (movedis == Width) || (movedis == -Width)) ? 10 : 14;

                    Cells[cellIndex].SetNeighborCells(Cells[checkIndex], weights);
                }
            }
        }
    }

}
