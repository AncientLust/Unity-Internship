using Enums;
using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float _damage = 50;
    private float _explosionRadius = 3;
    private WaitForSeconds _detonationDelay = new WaitForSeconds(0.25f);

    private ObjectPool _objectPool;
    private AudioPlayer _audioPlayer;
    private Rigidbody _rigidbody;
    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;

    public void Init(ObjectPool objectPool, AudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
        _objectPool = objectPool;
    }

    public void Throw(Vector3 position, Vector3 throwDirection, float throwForce)
    {
        _rigidbody.isKinematic = false;
        _meshRenderer.enabled = true;
        _meshCollider.enabled = true;

        _rigidbody.position = position;
        _rigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        StartCoroutine(DelayedDetonation());
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

    private IEnumerator DelayedDetonation()
    {
        yield return _detonationDelay;
        Explosion();
        ApplyDamage();
        _objectPool.Return(gameObject);
    }

    private void Explosion()
    {
        _rigidbody.isKinematic = true;
        _meshRenderer.enabled = false;
        _meshCollider.enabled = false;

        _audioPlayer.PlaySound(ESound.Explosion);
        var explosion = _objectPool.Get(EResource.Explosion).GetComponent<Effect>();
        explosion.transform.position = transform.position;
        explosion.Play();
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
