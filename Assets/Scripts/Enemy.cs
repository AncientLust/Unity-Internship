using UnityEngine;

public class Enemy : Human
{
    float _stopDistance = 0.7f;
    float _damage = 10;
    GameObject _target;
    Rigidbody _rigidbody;

    void Start()
    {
        InitHealth();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
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
            _target.GetComponent<Player>().TakeDamage(_damage);
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
            float damage = collision.gameObject.GetComponent<Projectile>().Damage;
            TakeDamage(damage);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
