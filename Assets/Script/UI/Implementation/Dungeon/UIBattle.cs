using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Eonix.Define;
using Eonix.Actor;
using Eonix.Battle;

namespace Eonix.UI
{
    using Battle = Define.Battle;

    public class UIBattle : UIWindow
    {
        #region Controller

        BattleController battleController;

        #endregion

        #region In Object Component

        Animator _animtorComponent;

        #endregion

        #region UI Object Compoenent

        #region Not Use Object

        Transform panelsParents;

        Transform panel;

        Transform skillImageParents;

        #endregion

        #region Use Object

        List<GameObject> dices = new List<GameObject>();

        public List<GameObject> Dices { get { return dices; } }

        Text stateObject_Text;

        public Text StateObject_Text { get { return stateObject_Text; } }

        Text damagePercentObject_Text;

        public Text DamagePercentObject_Text { get { return damagePercentObject_Text; } }

        List<GameObject> skills = new List<GameObject>();

        public List<GameObject> Skills { get { return skills; } }

        List<Text> statName_Text = new List<Text>();

        public List<Text> StatName_Text { get { return statName_Text; } }

        [SerializeField]
        List<GameObject> hpBars = new List<GameObject>();

        public List<GameObject> HpBars { get { return hpBars; } }

        #endregion

        #endregion

        public override void InitWindow()
        {
            isOpen = true;
            canCloseESC = true;

            panelsParents = transform.GetChild(1);

            for(int i = 0; i < panelsParents.childCount-1; i++)
            {
                panel = panelsParents.GetChild(i);

                dices.Add(panel.GetChild(0).gameObject);

                statName_Text.Add(panel.GetChild(1).GetComponent<Text>());

                if (i == 0)
                    hpBars.Add(panel.GetChild(3).gameObject);
                else
                    hpBars.Add(panel.GetChild(2).gameObject);
            }

            panel = panelsParents.GetChild(2);

            stateObject_Text = panel.GetChild(1).GetComponent<Text>();

            damagePercentObject_Text = panel.GetChild(2).GetComponent<Text>();

            skillImageParents = panelsParents.GetChild(0).GetChild(2);

            for(int i = 0; i < skillImageParents.childCount; i++)
            {
                skills.Add(skillImageParents.GetChild(i).gameObject);
            }

            base.InitWindow();
        }

        public override void Start()
        {
            _animtorComponent = GetComponent<Animator>();

            _animtorComponent.enabled = false;
        }

        public override void Open(bool force = false)
        {
            base.Open(force);

            _animtorComponent.enabled = true;
        }

        public void SetActiveSkillImages(bool isActive)
        {
            skillImageParents.gameObject.SetActive(isActive);
        }

        public void CloseStart()
        {
            _animtorComponent.SetBool("isClose", true);
        }

        public void EndCloseAnimation()
        {
            _animtorComponent.SetBool("isClose", false);
            Close();

            _animtorComponent.enabled = false;
        }

        public void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}