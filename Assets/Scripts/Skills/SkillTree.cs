using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Control;
using System;
using RPG.Combat;
using UnityEngine.UI;
using Microsoft.SqlServer.Server;
using UnityEngine.EventSystems;
using UnityEditorInternal;

namespace RPG.Skills
{

    public class SkillTree : MonoBehaviour, IAction
    {
        [SerializeField] GameObject canvas;
        [SerializeField] GameObject[] loadSkillTreeButtons;
        [SerializeField] Button[] skillButtons;

        float moreDmg = 0;

        float timer = 0;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.T) && !SkillTreeActive())
            {

                canvas.gameObject.SetActive(true);
                if (timer != Time.time)
                {
                    timer = Time.time;
                    UpdateSkillPoints();
                }

                Cancel();
            }
            else if (Input.GetKeyUp(KeyCode.T) && SkillTreeActive())
            {
                canvas.SetActive(false);
                StartAction();
            }
        }
        private float DamageModifier()
        {
            return moreDmg;
        }
        private void UpdateSkillPoints()
        {
            foreach (ButtonSkillPoints btn in FindObjectsOfType<ButtonSkillPoints>())
            {
                btn.GetComponentInChildren<Text>().text = string.Format("{0:0}/{1:0}", btn.CurrentSkillPoints(), btn.MaxSkillPoints());
            }
        }



        private void IncrementSkillPoint()
        {
            foreach (ButtonSkillPoints btn in FindObjectsOfType<ButtonSkillPoints>())
            {
                if (EventSystem.current.currentSelectedGameObject == btn.gameObject)
                {
                    btn.AddCurPoint(1);
                    EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text = string.Format("{0:0}/{1:0}", btn.CurrentSkillPoints(), btn.MaxSkillPoints());
                }
            }
        }
        private bool ShouldIncrement()
        {            
            foreach (ButtonSkillPoints btn in FindObjectsOfType<ButtonSkillPoints>())
            {
                if (EventSystem.current.currentSelectedGameObject == btn.gameObject)
                {
                    if (btn.CurrentSkillPoints() == btn.MaxSkillPoints()) return false;
                }
            }

            return true;
        }
        public void UseSkillPoint(int pointsToSubtract)
        {
            SkillPoints skillPoints = GameObject.FindWithTag("Player").GetComponent<SkillPoints>();

            if (skillPoints.PlayerSkillPoints() == 0) return;
            if (!ShouldIncrement()) return;

            IncrementSkillPoint();

            skillPoints.SubtractSkillPoint(pointsToSubtract);
        }

        public bool SkillTreeActive()
        {
            if (canvas.activeInHierarchy) return true;

            return false;
        }

        public void LoadTree(GameObject skillTreeToLoad)
        {
            foreach (GameObject trees in loadSkillTreeButtons)
            {
                trees.SetActive(false);
            }

            skillTreeToLoad.SetActive(true);
        }

        public void ExtraDamage(float damageModifier)
        {
            if (!ShouldIncrement()) return;
            moreDmg = damageModifier;

        }

        public void SkillName(string skillName)
        {
            if (!ShouldIncrement()) return;
            SkillPoints skillPoints = GameObject.FindWithTag("Player").GetComponent<SkillPoints>();

            foreach (Weapon resource in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Weapon)))
            {
                if (resource.name == skillName && skillPoints.PlayerSkillPoints() > 0)
                {
                    resource.skillBonusDamage = resource.WeaponDamage() + (DamageModifier() - resource.weaponDamage);
                }
            }
        }

        public void Cancel()
        {
            PlayerController playerCon = FindObjectOfType<PlayerController>();
            playerCon.enabled = false;
        }

        private void StartAction()
        {
            PlayerController playerCon = FindObjectOfType<PlayerController>();
            playerCon.enabled = true;
        }
    }
}