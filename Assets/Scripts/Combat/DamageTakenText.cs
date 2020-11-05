using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{

    public class DamageTakenText : MonoBehaviour
    {

        [SerializeField] GameObject damageTakenText;

        public float DamageText(Vector3 targetPosition, float damage)
        {
            Instantiate(damageTakenText, targetPosition, Quaternion.identity);
                        
            return damage;
        }

    }

}