using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
    public class SkillPointsDisplay : MonoBehaviour
    {
        [SerializeField] Text skillPointsText;

        SkillPoints skillPoints;
        private void Awake()
        {
            skillPoints = GameObject.FindWithTag("Player").GetComponent<SkillPoints>();
        }
        private void Update()
        {
            if (GetComponent<SkillTree>().SkillTreeActive())
            {
                skillPointsText.text = string.Format("Skill Points: {0:0}", skillPoints.PlayerSkillPoints());
            }
            
        }
    }
}
