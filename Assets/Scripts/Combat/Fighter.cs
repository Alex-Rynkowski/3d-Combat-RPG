using UnityEngine;
using System.Collections;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Skills;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] float wpnDmg;
        Health target;

        Mover theMover;
        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Weapon currentWeapon = null;

        private void Awake()
        {
            theMover = GetComponent<Mover>();
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Start()
        {            

        }
        private void Update()
        {
            //wpnDmg = currentWeapon.WeaponDamage(20);

            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (target != null && !GetIsInRange())
            {
                theMover.MoveTo(target.transform.position);
            }
            else
            {
                theMover.Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeBetweenAttacks <= timeSinceLastAttack)
            {
                //This will trigger the Hit() event                
                if (gameObject.tag == "Player" && EnoughResources())
                {
                    TriggerAttack();
                }
                else if (gameObject.tag == "Player")
                {
                    GetComponent<PlayerResources>().NotEnoughResourcesText();
                }

                if (gameObject.tag != "Player")
                {
                    TriggerAttack();
                }

                timeSinceLastAttack = 0;

            }
        }

        private bool EnoughResources()
        {
            return GetComponent<PlayerResources>().PlayerResourcePoints() > currentWeapon.ResourceCost();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("cancelAttack");
            GetComponent<Animator>().SetTrigger("attackAnmiation");
        }

        // Animation event
        void Hit()
        {
            BaseStats basestats = GetComponent<BaseStats>();
            float damageCalc = basestats.GetStat(Stat.Damage) * 0.1f;
            float damage = Random.Range(basestats.GetStat(Stat.Damage) - damageCalc, basestats.GetStat(Stat.Damage) + damageCalc);

            if (target == null) return;
            if (currentWeapon.HasProjectile())
            {
                print(damage);
                GetComponent<PlayerResources>().ResourceCost(currentWeapon.ResourceCost());
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {                
                target.TakeDamage(gameObject, damage);
                GetComponent<DamageTakenText>().DamageText(new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z),damage);
            }

        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;

        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attackAnmiation");
            GetComponent<Animator>().SetTrigger("cancelAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {                
                yield return currentWeapon.WeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {                
                yield return currentWeapon.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;

            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

