using System;
using UnityEngine;

public class EnemyDisposalManager
{
    private IExperienceTaker _experienceTaker;
    private ObjectPool _objectPool;

    public Action onDisposed;

    public void Init(IExperienceTaker experienceTaker, ObjectPool objectPool)
    {
        _experienceTaker = experienceTaker;
        _objectPool = objectPool;
    }

    public void SubscribeEnemy(EnemyDisposalSystem disposalSystem)
    {
        disposalSystem.onDisposal += DisposalHandler;
    }

    private void DisposalHandler(GameObject enemy)
    {
        UnsubscribeEnemy(enemy);
        TransferExperience(enemy);
        _objectPool.Return(enemy);
        onDisposed.Invoke();
    }

    private void UnsubscribeEnemy(GameObject enemy)
    {
        var disposalSystem = enemy.GetComponent<EnemyDisposalSystem>();
        disposalSystem.onDisposal -= DisposalHandler;
    }

    private void TransferExperience(GameObject enemy)
    {
        var killExperience = enemy.GetComponent<IExperienceMaker>().MakeExperience();
        _experienceTaker.TakeExperience(killExperience);
    }
}
