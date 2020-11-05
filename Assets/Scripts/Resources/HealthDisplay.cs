using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.Resources
{

    public class HealthDisplay : MonoBehaviour
    {        
        [SerializeField]Text healthText;
        [SerializeField] Image healthBar;

        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();            
        }
        private void Update()
        {
            UpdateHealthText();
            UpdateHealthBar();
        }

        private void UpdateHealthText()
        {
            healthText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }

        private void UpdateHealthBar()
        {
            healthBar.fillAmount = health.GetHealthPoints() / health.GetMaxHealthPoints();
        }
    }

}