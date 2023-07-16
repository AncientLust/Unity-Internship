using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed = 30;
    private Renderer _renderer;
    private ObjectPool _objectPool;

    private float _damage = 0;
    private float _pushPower = 0;

    public void Init(ObjectPool objectPool, float damage, float pushPower)
    {
        _objectPool = objectPool;
        _damage = damage;
        _pushPower = pushPower;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        MoveForward();
        ReturnToPoolOnceOutOfSight();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void ReturnToPoolOnceOutOfSight()
    {
        if (!_renderer.isVisible)
        {
            _objectPool.Return(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        ApplyDamage(collider);
        ApplyPush(collider);
    }

    private void ApplyDamage(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(_damage);
        }
    }

    private void ApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * _pushPower);
        }
    }
}
