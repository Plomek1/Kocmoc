using UnityEngine;

namespace Kocmoc.Gameplay
{
    public interface IDamageable
    {
        int health { get; }

        void Damage(DamageData damage);
        void Die();
    }

    [System.Serializable]
    public struct DamageData
    {
        public int damage;
        public float armorPenetration;
        public float shieldPenetration;
    }
}
