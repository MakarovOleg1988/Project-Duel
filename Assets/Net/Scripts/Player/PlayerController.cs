using System.Collections;
using UnityEngine;

namespace NetGame
{
    public class PlayerController : MonoBehaviour
    {
        private NewControls _controls;

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _firePoint;
        private Transform _pool;

        [Space, SerializeField, Range(1f, 20f)] private float _speedPlayer;
        [SerializeField, Range(1f, 5f)] public int _healthPlayer;

        [Space, SerializeField, Range(0.1f, 5f)] private float _attackDelay;
        [SerializeField, Range(0f, 5f)] private float _rotateDelay;

        public bool _turnPlayer1;

        void Start()
        {
            _controls = new NewControls();
            if (_turnPlayer1 == true) _controls.Player1.Enable();
            else _controls.Player2.Enable();

            _rb = GetComponent<Rigidbody>();
            _pool = FindObjectOfType<Pool>().transform;

            StartCoroutine(Fire());
            StartCoroutine(Focus());
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                var bullet = Instantiate(_bulletPrefab, _firePoint.transform.position, transform.rotation);
                SetParentBullet(bullet);
                yield return new WaitForSeconds(_attackDelay);
            }
        }

        void SetParentBullet(GameObject bullet)
        {
            bullet.transform.parent = _pool.transform;
            bullet.name = ("Bullet");
        }

        private IEnumerator Focus()
        {
            while (true)
            {
                transform.LookAt(_target);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                yield return new WaitForSeconds(_rotateDelay);
            }
        }

        void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            var direction = _turnPlayer1
                  ? _controls.Player1.Movement.ReadValue<Vector2>()
                  : _controls.Player2.Movement2.ReadValue<Vector2>();

            if (direction.x == 0 && direction.y == 0) return;

            var _velocity = new Vector3(direction.x, 0f, direction.y);
            transform.position += _velocity * _speedPlayer * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<ProjectileController>();
           
            if (bullet == null) return;
            _healthPlayer -= bullet.GetDamage;

            if (_healthPlayer <= 0)
            {
                Destroy(gameObject);
                Debug.Log("LOSE");
            }
        }

        private void OnDestroy()
        {
            _controls.Player1.Disable();
            _controls.Player2.Disable();
        }
    }
}
