using System;
using System.Collections;
using UnityEngine;

public class EnemyDisposalSystem : MonoBehaviour
{
    private EnemyHealthSystem _healthSystem;
    private WaitForSeconds _disposalDelay = new (2.5f);
    private bool _isInitialized;

    public Action<GameObject> onDisposal;

    public void Init(EnemyHealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _healthSystem.onDie += () => StartCoroutine(OnDieHandler());
    }

    private void Unsubscribe()
    {
        _healthSystem.onDie -= () => StartCoroutine(OnDieHandler());
    }

    private IEnumerator OnDieHandler()
    {
        yield return _disposalDelay;
        onDisposal.Invoke(gameObject);
    }
}
