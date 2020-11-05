using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float minTimeToWaitAtWaypoint = 1f;
        [SerializeField] float maxTimeToWaitAtWaypoint = 5f;
        [SerializeField] float suspicionsTime = 3f;
        [SerializeField] PatrolPath thePatrolPath;
        [SerializeField] float waypointTolerance = 1;
        [SerializeField] float timeToWaitAtWaypoint = 2f;

        Fighter theFigher;
        Mover theMover;
        GameObject player;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float waitAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex;

        private void Awake()
        {
            theFigher = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            theMover = GetComponent<Mover>();
        }
        private void Start()
        {
            guardPosition = transform.position;
            transform.position = thePatrolPath.transform.position;
        }

        private void Update()
        {

            if (GetComponent<Health>().IsDead()) return;
            if (InAttackRangeOfPlayer() && theFigher.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionsTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTime();

        }

        private void UpdateTime()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            waitAtWaypoint += Time.deltaTime;                                                    
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (thePatrolPath != null)
            {
                if (AtWaypoint())
                {
                    waitAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurretWaypoint();
            }

            if (waitAtWaypoint > timeToWaitAtWaypoint)
            {
                //WaitAtWaypointRandomizer(minTimeToWaitAtWaypoint, maxTimeToWaitAtWaypoint);
                theMover.StartMoveAction(nextPosition);
            }
        }

        private float WaitAtWaypointRandomizer(float minTime, float maxTime)
        {
            timeToWaitAtWaypoint = Random.Range(minTime, maxTime);
            return timeToWaitAtWaypoint;
        }

        bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurretWaypoint());
            return distanceToWaypoint < waypointTolerance;


        }
        private Vector3 GetCurretWaypoint()
        {
            return thePatrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = thePatrolPath.GetNextIndex(currentWaypointIndex);
        }

        private void AttackBehaviour()
        {
            theFigher.Attack(player);
        }

        private void SuspicionBehaviour()
        {

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, this.transform.position) < chaseDistance;

        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);

        }
    }
}
