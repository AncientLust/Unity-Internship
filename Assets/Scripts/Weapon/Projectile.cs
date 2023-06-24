using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed = 20;
    private Renderer _renderer;
    
    public float Damage { get; set; } = 0;
    public float PushPower { get; set; } = 0;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            return;
        }

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
            ObjectPool.Instance.Return(gameObject);
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
            damagable.TakeDamage(Damage);
        }
    }

    private void ApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * PushPower);
        }
    }
}
