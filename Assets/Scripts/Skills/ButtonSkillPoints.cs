using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSkillPoints : MonoBehaviour
{
    [SerializeField] float maxSkillPoints;
    [SerializeField] Text btnText;

    public int currenSkillPoints;
    void Start()
    {
        MaxSkillPoints();
    }
    private void Update()
    {        
        
    }

    public float MaxSkillPoints()
    {
        return maxSkillPoints;
    }
    public int CurrentSkillPoints()
    {
        print(this.gameObject + "  " + currenSkillPoints);
        return currenSkillPoints;
    }

    public int AddCurPoint(int pointToAdd)
    {
        currenSkillPoints += pointToAdd;
        return currenSkillPoints;
    }

}
