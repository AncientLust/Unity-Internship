using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _stopDistance = 0.25f;
    private float _damage = 10;
    private GameObject _target;
    private Rigidbody _rigidbody;
    private HealthSystem _healthSystem;
    private StatsSystem _statsSystem;

    private void Start()
    {
        CacheComponents();
    }

    private void Update()
    {
        ActIfGameRunning();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void ActIfGameRunning()
    {
        if (!GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            ResetVelosity();
            return;
        }

        _healthSystem.Regenerate();
        _healthSystem.HideHealthIfHealthy();
        MoveIfPlayerAlive();
    }

    private void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void MoveIfPlayerAlive()
    {
        if (_target.gameObject.activeInHierarchy)
        {
            MoveToPlayer();
        }
        else
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.transform.position);
        if (distanceToPlayer > _stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _statsSystem.MoveSpeed * Time.deltaTime);
        }

        transform.LookAt(_target.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playersHealthSystem = collision.gameObject.GetComponent<HealthSystem>();
            bool playerKilled = playersHealthSystem.TakeDamageTrueIfFatal(_damage * _statsSystem.PowerMultiplier);

            if (playerKilled)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            var projectileDamage = other.gameObject.GetComponent<Projectile>().Damage;
            if (_healthSystem.TakeDamageTrueIfFatal(projectileDamage))
            {
                _target.gameObject.GetComponent<ExperienceSystem>().AddExperience(10);
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
