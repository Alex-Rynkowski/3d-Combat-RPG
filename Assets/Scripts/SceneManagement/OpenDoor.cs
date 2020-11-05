using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class OpenDoor : MonoBehaviour
    {
        [SerializeField] Animator theAnimator;

        [SerializeField] GameObject theHouse;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {

                theAnimator.SetTrigger("Open_Door");

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                theAnimator.SetTrigger("Close_Door");
            }
        }
    }

}