using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyDisposalManager
{
    private IExperienceTaker _experienceTaker;
    private ObjectPool _objectPool;
    private int _disposalDelayMs = 2500;

    public Action onEnemyKilled;

    public void Init(IExperienceTaker experienceTaker, ObjectPool objectPool)
    {
        _experienceTaker = experienceTaker;
        _objectPool = objectPool;
    }

    public void SubscribeEnemy(EnemyHealthSystem healthSystem)
    {
        healthSystem.onDisposal += DisposalHandler;
    }

    private void DisposalHandler(GameObject enemy)
    {
        onEnemyKilled.Invoke();
        UnsubscribeEnemy(enemy);
        TransferExperience(enemy);
        DelayedDisposal(enemy).Forget();
    }

    private async UniTask DelayedDisposal(GameObject enemy)
    {
        await UniTask.Delay(_disposalDelayMs);
        if (enemy != null)
        {
            _objectPool.Return(enemy);
        }
    }

    private void UnsubscribeEnemy(GameObject enemy)
    {
        var healthSystem = enemy.GetComponent<EnemyHealthSystem>();
        healthSystem.onDisposal -= DisposalHandler;
    }

    private void TransferExperience(GameObject enemy)
    {
        var killExperience = enemy.GetComponent<IExperienceMaker>().MakeExperience();
        _experienceTaker.TakeExperience(killExperience);
    }
}
