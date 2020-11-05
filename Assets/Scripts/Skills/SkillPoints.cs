using RPG.Combat;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RPG.Skills
{
    public class SkillPoints : MonoBehaviour
    {
        [SerializeField] int skillPoints = 1;
        public float myDmg;
        BaseStats baseStats;        


        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();            
        }
        private void Start()
        {
            baseStats.onLevelUp += AddSkillPoint;
            
        }
        private void Update()
        {
            
        }
        

        private void AddSkillPoint()
        {
            skillPoints++;
        }

        public int SubtractSkillPoint(int amountToSubtract)
        {
            skillPoints -= amountToSubtract;
            return skillPoints;
        }
        public int PlayerSkillPoints()
        {
            return skillPoints;
        }
    }

}