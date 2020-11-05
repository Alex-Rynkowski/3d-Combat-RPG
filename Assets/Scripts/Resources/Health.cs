using UnityEngine;
using System.Collections;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 0;
        bool restoreState = false;

        bool targetIsDead = false;


        private void Start()
        {            
            
            GetComponent<BaseStats>().onLevelUp += UpdateHealthOnLevelUp;
            if (!restoreState)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return targetIsDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            //print(gameObject.name + " took damage: " + damage);
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0 && !targetIsDead)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public float GetPercentage()
        {            
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPoints()
        {            
            return healthPoints;
        }

        void UpdateHealthOnLevelUp()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {            
            if (targetIsDead) return;

            targetIsDead = true;
            GetComponent<Animator>().SetTrigger("dieAnimation");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if (exp == null) return;

            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            restoreState = true;
            healthPoints = (float)state;

            if (healthPoints == 0 && !targetIsDead)
            {
                Die();
            }
        }
    }
}
