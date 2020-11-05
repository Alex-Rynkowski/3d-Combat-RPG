using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] int currentLevel = 0;
        [SerializeField] GameObject levelUp = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;        
        Experience experience;
        private void Awake()
        {
            experience = GetComponent<Experience>();
        }
        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }

        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                experience.ResetEXPPoints();
                currentLevel = newLevel;
                onLevelUp();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUp, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            float total = 0;

            foreach (IModifierProvider provider in modifierProviders)
            {

                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }





            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float percentageTotal = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    percentageTotal += modifier;
                }
            }

            return percentageTotal;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = experience.PlayerExperiencePoints();
            int penultimateLevel = progression.GetLevels(Stat.ExpereinceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExpereinceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}