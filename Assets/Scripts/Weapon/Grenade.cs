using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;

    private float _damage = 50;
    private float _explosionRadius = 3;
    private float _explosionDelay = 0.25f;
    private float _disappearDelay = 2;

    private ObjectPool _objectPool;
    private Rigidbody _rigidbody;
    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;

    public void Init(ObjectPool objectPool, Vector3 position)
    {
        _objectPool = objectPool;
        _rigidbody.isKinematic = false;
        _meshRenderer.enabled = true;
        _meshCollider.enabled = true;
        transform.position = position;
    }

    public void Throw(Vector3 throwDirection, float throwForce)
    {
        _rigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        StartCoroutine(DelayedExplosion());
    }

    public void Awake()
    {
        CacheComponents();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    private IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explosion();
        ApplyDamage();
        StartCoroutine(DelayedReturnToPool());
    }

    private IEnumerator DelayedReturnToPool()
    {
        yield return new WaitForSeconds(_disappearDelay);
        _objectPool.Return(gameObject);
    }

    private void Explosion()
    {
        _explosionEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        _explosionEffect.Play();
        _rigidbody.isKinematic = true;
        _meshRenderer.enabled = false;
        _meshCollider.enabled = false;
    }

    private void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            var damagable = hitCollider.GetComponent<IDamageable>();
            var playerFacade = hitCollider.GetComponent<IPlayerFacade>();
            if (damagable != null && playerFacade == null)
            {
                damagable.TakeDamage(_damage);
            }
        }
    }
}
