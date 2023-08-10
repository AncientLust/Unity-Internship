using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private ObjectPool _objectPool;
    private ParticleSystem _particleSystem;

    public void Init(ObjectPool objectPool)
    {
        _objectPool = objectPool;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        _particleSystem.Play();
        StartCoroutine(ReturnToPoolOnceFinished());
    }

    private IEnumerator ReturnToPoolOnceFinished()
    {
        while (_particleSystem.isPlaying)
        {
            yield return null;
        }

        _objectPool.Return(gameObject);
    }
}
