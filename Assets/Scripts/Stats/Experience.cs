using UnityEngine;
using System.Collections;
using System;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {            
            experiencePoints += experience;
            onExperienceGained();
        }
        public float PlayerExperiencePoints()
        {
            return experiencePoints;
        }

        public float ResetEXPPoints()
        {
            experiencePoints = 0;
            return experiencePoints;
        }

        public float EXPPercentage()
        {
            float percentage = PlayerExperiencePoints() / EXPToNextLevel();
            return percentage;
        }

        public float EXPToNextLevel()
        {
            float expToNextLevel = 0;
            if (gameObject.tag == "Player")
            {
                expToNextLevel = GetComponent<BaseStats>().GetStat(Stat.ExpereinceToLevelUp);
            }

            return expToNextLevel;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
