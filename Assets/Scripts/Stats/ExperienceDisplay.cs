using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.Stats
{

    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] Text EXPBarText;
        [SerializeField] Image EXPBarImg;

        Experience experience;
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            EXPBarLabel();
            EXPBar();

        }

        private void EXPBar()
        {
            EXPBarImg.fillAmount = experience.EXPPercentage();
        }

        private void EXPBarLabel()
        {
            EXPBarText.text = string.Format("Exp: {0:0}/{1:0}", experience.PlayerExperiencePoints(), experience.EXPToNextLevel());
        }
    }

}