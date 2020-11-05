using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{    
    public class PlayerSkills : MonoBehaviour
    {

        [SerializeField] Button skillButton;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                skillButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        public void UseSkill(KeyCodes key)
        {            
            Input.GetKeyUp((key.ToString()));
        }
        public void UseSkills()
        {
            print("Button pressed!");
        }
    }
}
