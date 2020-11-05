using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Resources
{

    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] Text resourceText;        
        [SerializeField] Image resourceImage;

        PlayerResources playerResources;

        private void Awake()
        {
            playerResources = GameObject.FindWithTag("Player").GetComponent<PlayerResources>();
        }
        
        void Update()
        {            
            ResourceText();
            ResourceBar();            
        }

        private void ResourceBar()
        {
            resourceImage.fillAmount = playerResources.PlayerResourcePoints() / playerResources.MaxResources();
        }

        private void ResourceText()
        {
            resourceText.text = string.Format("{0:0}/{1:0}", playerResources.PlayerResourcePoints(), playerResources.MaxResources());
        }
    }

}