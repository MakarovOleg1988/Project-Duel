using System.Collections;
using UnityEngine;

namespace NetGame
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField, Range(1f, 20f)] private float _speedBullet;
        [SerializeField, Range(1f, 5f)] private int _damageBullet;
        [SerializeField, Range(1f, 10f)] private float _lifetime;

        public int GetDamage => _damageBullet;

        void Start()
        {
            StartCoroutine(Ondie());
        }

        void Update()
        {
            transform.position += transform.forward * _speedBullet * Time.deltaTime;   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent < PlayerController>() == null) return;
                Destroy(gameObject);
        }


        private IEnumerator Ondie()
        {
            yield return new WaitForSeconds(_lifetime);
            Destroy(gameObject);
        }
    }
}
