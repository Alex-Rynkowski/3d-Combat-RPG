using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class Dissolve : MonoBehaviour
    {
        [SerializeField] Transform getDissolvePosition;
        [SerializeField] GameObject[] objectsToDissolve;        
        [SerializeField] float dissolveSpeed = 1f;


        private void Update()
        {
            for (int i = 0; i < objectsToDissolve.Length; i++)
            {
                Dissolving(i);                
            }
        }

        private void Dissolving(int i) ///dissolve (Gameobject[] objectsToDissolve), the objects are assigned in the inspector
        {
            float xBounds = getDissolvePosition.GetComponent<Renderer>().bounds.size.x;
            float zBounds = getDissolvePosition.GetComponent<Renderer>().bounds.size.z;
            float area = Mathf.Max(xBounds, zBounds) / 100;
            
            //setting the alpha to 1, making the object not visible                       
            Renderer theRenderer = objectsToDissolve[i].GetComponent<Renderer>();
                        
            float amountToReduce = 10f * (Vector3.Distance(PlayerPosition(), getDissolvePosition.position) / 100) - area;            
            theRenderer.material.SetFloat("Vector1_ECD573FF", Mathf.MoveTowards(1f - amountToReduce, 1f, dissolveSpeed * Time.deltaTime));
        }

        private Vector3 PlayerPosition()
        {
            return FindObjectOfType<PlayerController>().transform.position;
            
        }

        private void OnDrawGizmos()
        {            
            Gizmos.DrawWireSphere(getDissolvePosition.transform.position, 10f);
        }

    }

}
