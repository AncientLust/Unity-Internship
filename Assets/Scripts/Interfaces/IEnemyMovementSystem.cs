using UnityEngine;

public interface IEnemyMovementSystem
{
    public void SetTarget(Transform target);
    public void Push(Vector3 force);
    public void SetPosition(Vector3 position);
}
