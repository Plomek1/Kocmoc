using UnityEngine;
using UnityEngine.Audio;

namespace Kocmoc.Gameplay
{
    public class Weapon : Module
    {
        public Transform attackTarget {  get; private set; }

        [SerializeField] private Transform barrelBase;

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
        }

        private void Update()
        {
            float angleToTarget;
            if (attackTarget)
            {
                Vector2 directionToTarget = (attackTarget.position - barrelBase.position).normalized;
                angleToTarget = Vector2.SignedAngle(barrelBase.transform.up, directionToTarget);
            }
            else
                angleToTarget = Vector2.SignedAngle(barrelBase.transform.up, transform.up);
            
            if (angleToTarget != 0)
            {
                Vector3 currentRotation = barrelBase.localRotation.eulerAngles;
                if (currentRotation.z > 180f) currentRotation.z -= 360f;
                
                currentRotation.z += Mathf.Sign(angleToTarget) * Time.deltaTime * 50;
                currentRotation.z =  Mathf.Clamp(currentRotation.z, -data.rotationRange, data.rotationRange);
                
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
    }
}
