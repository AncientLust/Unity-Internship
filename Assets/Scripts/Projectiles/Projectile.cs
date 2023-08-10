using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Renderer _renderer;
    protected ObjectPool _objectPool;
    protected AudioPlayer _audioPlayer;

    protected float _speed;
    protected float _damage;
    protected float _pushPower;

    public bool IsPenetratiable { get; set; } = false;

    public void Init(ObjectPool objectPool, AudioPlayer audioPlayer)
    {
        _objectPool = objectPool;
        _audioPlayer = audioPlayer;
    }

    public void Launch(float speed, float damage, float pushPower)
    {
        _speed = speed;
        _damage = damage;
        _pushPower = pushPower;
    }

    protected void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
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

    virtual protected void OnTriggerEnter(Collider collider)
    {
        TryApplyPush(collider);
        TryApplyDamage(collider);
        PenetrationCheck();
    }

    virtual protected void TryApplyDamage(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(_damage);
        }
    }

    virtual protected void TryApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * _pushPower);
        }
    }

    virtual protected void PenetrationCheck()
    {
        if (!IsPenetratiable)
        {
            _objectPool.Return(gameObject);
        }
    }
}
