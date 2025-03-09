using System.Collections;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        private ProjectileData data;

        public void Init(ProjectileData data)
        {
            this.data = data;
            StartCoroutine(DeleteAfterTime());
        }

        private IEnumerator DeleteAfterTime()
        {
            yield return new WaitForSeconds(data.lifetime);
            Delete();
        }

        public void Update()
        {
            transform.position += transform.up * data.speed * Time.deltaTime;
        }

        private void Delete()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
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
    }
}
