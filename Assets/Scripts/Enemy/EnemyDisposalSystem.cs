using System;
using UnityEngine;

public class EnemyDisposalSystem : MonoBehaviour
{
    private EnemyHealthSystem _healthSystem;
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

        if (_healthSystem != null) _healthSystem.onDie += OnDieHandler;
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= OnDieHandler;
    }

    private void OnDieHandler()
    {
        onDisposal.Invoke(gameObject);
    }
}
