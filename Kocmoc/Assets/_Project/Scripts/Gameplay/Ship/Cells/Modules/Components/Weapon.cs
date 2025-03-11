using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Weapon : Module
    {
        public Transform attackTarget {  get; private set; }

        [SerializeField] private Transform barrelBase;
        [SerializeField] private Barrel[] barrels;

        private float timeToShoot;

        private int currentFirePoint;
        private bool lookingAtTarget;

        public new WeaponData data
        {
            get => base.data as WeaponData;
            protected set => base.data = value;
        }

        protected override void SetController(ShipController controller)
        {
            base.SetController(controller);
            controller.AttackTargetChanged += SetAttackTarget;
        }

        public void SetAttackTarget(Transform target)
        {
            attackTarget = target;
            lookingAtTarget = false;
        }

        private void Update()
        {
            RotateTowardsTarget();

            if (timeToShoot > 0)
            {
                timeToShoot -= Time.deltaTime;
                return;
            }

            if (attackTarget && lookingAtTarget)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            Projectile projectile = Instantiate(data.projectilePrefab, barrels[currentFirePoint].firePoint.position, barrelBase.rotation);
            projectile.Init(new ProjectileData()
            {
                source = ship.gameObject,
                damage = data.damage,
                speed = data.projectileSpeed,
                lifetime = data.projectileLifetime,
                ignores = new List<Transform>() { transform },
            });

            barrels[currentFirePoint].animator?.SetTrigger("Shoot");

            timeToShoot = data.shootCooldown;

            currentFirePoint++;
            if (currentFirePoint == barrels.Length) currentFirePoint = 0;
        }

        private void RotateTowardsTarget()
        {
            float angleToTarget;
            if (attackTarget)
            {
                Vector2 directionToTarget = (attackTarget.position - barrelBase.position).normalized;
                angleToTarget = Vector2.SignedAngle(barrelBase.transform.up, directionToTarget);
                lookingAtTarget = Vector2.Dot(barrelBase.up, directionToTarget) >= data.shootOffset;
            }
            else
                angleToTarget = Vector2.SignedAngle(barrelBase.transform.up, transform.up);

            if (angleToTarget != 0)
            {
                Vector3 currentRotation = barrelBase.localRotation.eulerAngles;
                if (currentRotation.z > 180f) currentRotation.z -= 360f;

                currentRotation.z += Mathf.Sign(angleToTarget) * data.rotationSpeed * Time.deltaTime;
                currentRotation.z = Mathf.Clamp(currentRotation.z, -data.rotationRange, data.rotationRange);

                barrelBase.localRotation = Quaternion.Euler(currentRotation);
            }
        }

        public override void Init()
        {
            cell = GetComponent<ShipCell>();

            foreach (ModuleData data in cell.data.modules)
            {
                this.data = data as WeaponData;
                if (this.data != null) break;
            }
        }

        [System.Serializable]
        public struct Barrel
        {
            public Transform firePoint;
            public Animator animator;
            
        }
    }
}
