using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] GameObject wayPointPrefab;
        [SerializeField] int amountOfWaypoints = 4;
        [SerializeField] float minRange = 5f;
        [SerializeField] float maxRange = 5f;

        private void Start()
        {

            SpawnWaypoints(amountOfWaypoints);

        }
        private void Update()
        {
            TheTerrainHeightPos();
        }
        private void OnDrawGizmos()
        {
            const float wayPointGizmoRadius = .3f;

            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GetWaypoint(i), wayPointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        //Function to instantiate waypoints at random positions
        int SpawnWaypoints(int waypointsToSpawn)
        {
            Vector3 theYPos = TheTerrainHeightPos();
            gameObject.transform.position = new Vector3(transform.position.x, theYPos.y, transform.position.z);
            if (wayPointPrefab == null) return 0;
            while (waypointsToSpawn > 0)
            {
                if (waypointsToSpawn == amountOfWaypoints)
                {
                    //Spawn the first waypoint at the position of this gameobject
                    var firstChild = Instantiate(wayPointPrefab, gameObject.transform.position, gameObject.transform.rotation);
                    firstChild.transform.parent = gameObject.transform;
                }
                else
                {
                    //Spawn waypoints until "waypointsToSpawn" reaches 0 (value is set in the inspector)
                    var spawnTheChild = Instantiate(wayPointPrefab, gameObject.transform.position, gameObject.transform.rotation);
                    spawnTheChild.transform.parent = gameObject.transform;
                    spawnTheChild.transform.position = new Vector3(transform.position.x + Random.Range(minRange, maxRange), theYPos.y, transform.position.z + Random.Range(minRange, maxRange));
                }
                waypointsToSpawn--;
            }
            return waypointsToSpawn;
        }

        private Vector3 TheTerrainHeightPos()
        {
            Vector3 theYPos = transform.position;
            theYPos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            return theYPos;
        }
    }
}
