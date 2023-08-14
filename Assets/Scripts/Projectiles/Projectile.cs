using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Renderer _renderer;
    protected ObjectPool _objectPool;
    protected IAudioPlayer _iAudioPlayer;

    protected float _speed;
    protected float _damage;
    protected float _pushPower;
    protected bool _isPenetratable;
    protected bool _canBeReturned;
    protected WaitForSeconds _minimumLifetime = new(2f);

    public void Init(ObjectPool objectPool, IAudioPlayer iAudioPlayer)
    {
        _objectPool = objectPool;
        _iAudioPlayer = iAudioPlayer;
    }

    public void Launch(float speed, float damage, float pushPower, bool isPenetratable)
    {
        _speed = speed;
        _damage = damage;
        _pushPower = pushPower;
        _isPenetratable = isPenetratable;

        _canBeReturned = false;
        StartCoroutine(MinimumLifetimeCountdown());
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
        if (!_renderer.isVisible && _canBeReturned)
        {
            _objectPool.Return(gameObject);
            _canBeReturned = false;
        }
    }

    virtual protected void OnTriggerEnter(Collider collider)
    {
        TryApplyPush(collider);
        TryApplyDamage(collider);
        PenetrationCheck();
    }

    protected void TryApplyDamage(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(_damage);
        }
    }

    protected void TryApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * _pushPower);
        }
    }

    private protected void PenetrationCheck()
    {
        if (!_isPenetratable)
        {
            _objectPool.Return(gameObject);
            _canBeReturned = false;
        }
    }

    private IEnumerator MinimumLifetimeCountdown()
    {
        yield return _minimumLifetime;
        _canBeReturned = true;
    }
}
