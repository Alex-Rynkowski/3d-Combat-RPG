using System;
using UnityEngine;
using RPG.Resources;
using RPG.Skills;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverrride = null;
        [SerializeField] GameObject equippedWeaponPrefab = null;
        public float weaponDamage = 5f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float resourceCost = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] SkillName skillName;

        public float skillBonusDamage;
        const string weaponName = "Weapon";
        PlayerResources playerResources;
                

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroOldWeapon(rightHand, leftHand);
            if (equippedWeaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);

                GameObject weapon = Instantiate(equippedWeaponPrefab, handTransform);
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrride != null)
            {
                animator.runtimeAnimatorController = animatorOverrride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public float ResourceCost()
        {
            return resourceCost;
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject theInstigator, float calculatedDamage)
        {            
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, theInstigator, calculatedDamage);
        }

        public float WeaponDamage()
        {          
            
            return weaponDamage + skillBonusDamage;
        }



        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float WeaponRange()
        {
            return weaponRange;
        }
    }
}
