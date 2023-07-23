using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float _speed = 30;
    protected Renderer _renderer;
    protected ObjectPool _objectPool;

    protected float _damage = 0;
    protected float _pushPower = 0;

    public bool IsPenetratiable { get; set; } = false;

    public void Init(ObjectPool objectPool, float damage, float pushPower)
    {
        _objectPool = objectPool;
        _damage = damage;
        _pushPower = pushPower;
    }

    protected void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    protected void Update()
    {
        MoveForward();
        ReturnToPoolOnceOutOfSight();
    }

    protected void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    protected void ReturnToPoolOnceOutOfSight()
    {
        if (!_renderer.isVisible)
        {
            _objectPool.Return(gameObject);
        }
    }

    protected void OnTriggerEnter(Collider collider)
    {
        var player = collider.gameObject.GetComponent<IPlayerFacade>();
        if (player == null)
        {
            ApplyDamage(collider);
            ApplyPush(collider);
            PenetrationCheck();
        }
    }

    protected void ApplyDamage(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(_damage);
        }
    }

    protected void ApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * _pushPower);
        }
    }

    protected void PenetrationCheck()
    {
        if (!IsPenetratiable)
        {
            _objectPool.Return(gameObject);
        }
    }
}
