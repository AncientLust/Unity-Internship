using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed = 20;
    private Renderer _renderer;
    
    public float Damage { get; set; } = 0;

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
        DestroyOnceOutOfSight();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void DestroyOnceOutOfSight()
    {
        if (!_renderer.isVisible)
        {
            ObjectPool.Instance.Add(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();

        if (damagable != null)
        {
            damagable.TakeDamage(Damage);
        }
    }
}
