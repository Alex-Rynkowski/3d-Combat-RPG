using UnityEngine;
using System.Collections;
using System;

namespace RPG.Core
{
    public class PersistentObjects : MonoBehaviour
    {
        [SerializeField] GameObject persistenObjectPrefab;

        static bool hasSpawned = false;

        private void Start()
        {
            if (hasSpawned) return;

            SpawnPersistenObjects();

            hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
            GameObject PersistenObjects = Instantiate(persistenObjectPrefab);
            DontDestroyOnLoad(PersistenObjects);
        }
    }

}
