using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Resources
{
    public class PlayerResources : MonoBehaviour, ISaveable
    {
        [SerializeField] float resourcePoints = 0;
        [SerializeField] GameObject notEnoughResourcesText;

        bool restoreState = false;
        private void Start()
        {            
            if (!restoreState)
            {
                resourcePoints = MaxResources();
            }

        }
        private void Update()
        {            
            if (PlayerResourcePoints() < MaxResources())
            {
                resourcePoints += ResourceRegeneration();
            }
            
            if (Input.GetKey(KeyCode.G))
            {                
                NotEnoughResourcesText();
            }
        }

        private float ResourceRegeneration()
        {
            float regeneration;
            BaseStats baseStats = GetComponent<BaseStats>();
            regeneration = baseStats.GetStat(Stat.ResourceRegeneration);

            return regeneration * Time.deltaTime;
        }


        public void NotEnoughResourcesText()
        {
            GameObject notEnoughRP = Instantiate(notEnoughResourcesText, transform.position, transform.rotation);
            Destroy(notEnoughRP, 3);
        }

        public float MaxResources()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Resources);
        }

        public float PlayerResourcePoints()
        {            
            return resourcePoints;            
        }

        public float ResourceCost(float resourceCost)
        {
            resourcePoints -= resourceCost;
            return resourcePoints;
        }

        public object CaptureState()
        {
            return resourcePoints;
        }

        public void RestoreState(object state)
        {
            restoreState = true;
            resourcePoints = (float)state;
        }
    }

}