using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDisposalSystem : MonoBehaviour
{
    private EnemyHealthSystem _healthSystem;
    private WaitForSeconds _disposalDelay = new WaitForSeconds(2.5f);

    public Action<GameObject> onDisposal;

    public void Init(EnemyHealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie += () => StartCoroutine(OnDieHandler());
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= () => StartCoroutine(OnDieHandler());
    }

    private IEnumerator OnDieHandler()
    {
        yield return _disposalDelay;
        onDisposal.Invoke(gameObject);
    }
}
