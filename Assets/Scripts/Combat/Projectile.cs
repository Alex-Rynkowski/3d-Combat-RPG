using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{

    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLiveTime = 20f;
        [SerializeField] GameObject[] destroyOnImpact = null;
        [SerializeField] float destroAfterImpact = 2f;
        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

            if (isHoming && target.IsDead()) return;
            transform.LookAt(GetAimLocation());
        }

        public void SetTarget(Health theTarget, GameObject theInstigator, float theDamage)
        {
            target = theTarget;
            damage = theDamage;
            instigator = theInstigator;

            Destroy(gameObject, maxLiveTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            projectileSpeed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnImpact)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, destroAfterImpact);
        }

    }


}