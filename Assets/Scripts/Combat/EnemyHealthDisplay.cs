using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPG.Resources;
using RPG.Core;

namespace RPG.Combat
{

    public class EnemyHealthDisplay : MonoBehaviour
    {        
        Health health;

        [SerializeField] Image healthBar;        
        [SerializeField] float xRot;
        [SerializeField] float yRot;
        [SerializeField] float zRot;               
        
        private void Awake()
        {            
            health = GetComponent<Health>();
            var canvas = GetComponentInChildren<Canvas>();           

        }
        private void Update()
        {
            UpdateHealthBar();
            CanvasRotation();
            
        }

        private void UpdateHealthBar()
        {
            healthBar.fillAmount = health.GetHealthPoints() / health.GetMaxHealthPoints();            
        }

        private void CanvasRotation()
        {            
            healthBar.GetComponentInParent<Canvas>().transform.rotation = Quaternion.Euler(xRot,yRot,zRot);        
        }
    }

}