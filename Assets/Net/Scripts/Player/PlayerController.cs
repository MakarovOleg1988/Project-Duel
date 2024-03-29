using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NetGame
{
    public class PlayerController : MonoBehaviour, IPunObservable
    {
        public static PlayerController instance { get; private set; }
        private NewControls _controls;

        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private PhotonView _photonView;
        private Transform _pool;

        [Space, SerializeField, Range(1f, 20f)] private float _speedPlayer;
        [SerializeField, Range(1f, 5f)] public float _healthPlayer;
        
        public float HealthPlayer
        {
            get { return _healthPlayer; }
            set { _healthPlayer = value; }
        }

        [Space, SerializeField, Range(0.1f, 5f)] private float _attackDelay;
        [SerializeField, Range(0f, 5f)] private float _rotateDelay;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            _controls = new NewControls();
            _pool = FindObjectOfType<Pool>().transform;
            FindObjectOfType<GameManager>().AddPLayer(this);  
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            StartCoroutine(Fire());
            StartCoroutine(Focus());

            if (!_photonView.IsMine) return;

            _controls.Player1.Enable();
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

        private IEnumerator Focus()
        {
            while (true)
            {
                transform.LookAt(_target);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                yield return new WaitForSeconds(_rotateDelay);
            }
        }

        void SetParentBullet(GameObject bullet)
        {
            bullet.transform.parent = _pool.transform;
            bullet.name = ("Bullet");
        }

        private void MovePlayer()
        {
            if (!_photonView.IsMine) return;
            var direction = _controls.Player1.Movement.ReadValue<Vector2>();

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
                _controls.Player1.Disable();
                _controls.Player2.Disable();
                Destroy(this.gameObject);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PlayerData.Create(this));
            }
            else
            {
                ((PlayerData)stream.ReceiveNext()).Set(this);
            }
        }

        private void OnDestroy()
        {
            _controls.Player1.Disable();
            _controls.Player2.Disable();
        }
    }
}
