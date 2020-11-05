using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPacksToSpawn;
    [SerializeField] int PacksToSpawnMin = 1;
    [SerializeField] int PacksToSpawnMax = 5;

    int AmountOfPacksToSpawn;
    int SpawnPack;
    BoxCollider spawnArea;
    private void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
        TotalPacksToSpawn();

        //SpawnPack = Random.Range(0, enemyPacksToSpawn.Length);      

        while (AmountOfPacksToSpawn > 0)
        {
            Instantiate(enemyPacksToSpawn[0], SpawnArea(spawnArea.bounds), Quaternion.identity);            
            AmountOfPacksToSpawn--;
        }

    }

    private void TotalPacksToSpawn()
    {
        AmountOfPacksToSpawn = Random.Range(PacksToSpawnMin, PacksToSpawnMax);
    }

    private Vector3 SpawnArea(Bounds bounds)
    {
        //print("Bouds: " + bounds);
        float x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
        return new Vector3(x, 1.5f, z);
    }
}
