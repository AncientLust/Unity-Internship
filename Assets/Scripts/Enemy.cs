using UnityEngine;

public class Enemy : Human
{
    float _stopDistance = 0.25f;
    float _damage = 10;
    GameObject _target;
    Rigidbody _rigidbody;

    void Start()
    {
        InitHealth();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            ResetVelosity();
            return;
        }

        CheckIfKilled();
        Regenerate();
        HideHealthIfHealthy();
        MoveIfPlayerAlive();
    }

    private void CheckIfKilled()
    {
        if (_health <= 0)
        {
            IsDead = true;
            gameObject.SetActive(false);
        }
    }

    private void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void MoveIfPlayerAlive()
    {
        if (_target.activeInHierarchy)
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
        float distanceToPlayer = Vector3.Distance(transform.position, _target.transform.position);
        if (distanceToPlayer > _stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);
        }

        transform.LookAt(_target.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Player>().TakeDamage(_damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            float damage = other.gameObject.GetComponent<Projectile>().Damage;
            TakeDamage(damage);
        }
    }

    private void HideHealthIfHealthy()
    {
        _healthBar.gameObject.SetActive(_health != _maxHealth);
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
