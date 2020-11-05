using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A, B, C, E, F
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTimer = 2f;
        [SerializeField] float fadeInTimer = 1f;
        [SerializeField] float waitBeforeFadeIn = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }

        }

        private IEnumerator Transition()
        {

            if (sceneToLoad < 0)
            {
                Debug.LogError("The scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            Fader theFader = FindObjectOfType<Fader>();

            yield return theFader.FadeOut(fadeOutTimer);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);            
            wrapper.Load();            
            Portal otherPortal = GetOtherPortal();            
            UpdatePlayer(otherPortal);
            //yield return new WaitForEndOfFrame();
            wrapper.Save();            
            yield return new WaitForSeconds(waitBeforeFadeIn);
            yield return theFader.FadeIn(fadeInTimer);            
            Destroy(gameObject);

        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;
        }


    }
}
