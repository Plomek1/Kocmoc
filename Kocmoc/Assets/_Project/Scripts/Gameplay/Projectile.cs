using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private ProjectileData data;
        private Rigidbody2D rb;

        public void Init(ProjectileData data)
        {
            this.data = data;

            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.up * data.speed;
            
            StartCoroutine(DeleteAfterTime());
        }

        private IEnumerator DeleteAfterTime()
        {
            yield return new WaitForSeconds(data.lifetime);
            Delete();
        }

        public void Update()
        {
        }

        private void Delete()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (data.ignores.Contains(collision.transform)) return;
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(data.damage);
                Delete();
            }
        }
    }

    [System.Serializable]
    public struct ProjectileData
    {
        public GameObject source;
        public DamageData damage;
        public float speed;
        public float lifetime;
        public List<Transform> ignores;
    }
}
