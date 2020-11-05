using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using RPG.Skills;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover theMover;
        Fighter theFighter;

        private void Awake()
        {
            theMover = GetComponent<Mover>();
            theFighter = GetComponent<Fighter>();
        }
        
            
        private void Update()
        {            
            if (GetComponent<Health>().IsDead()) return;            
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 10f);            
        }


        private bool InteractWithCombat()
        {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    theFighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
                        
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {                    
                    theMover.StartMoveAction(hit.point);
                }
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
