using UnityEngine;
using Enums;

public class PlayerThrowSystem : MonoBehaviour
{
    private ObjectPool _objectPool;
    private ThrowGrenadeSkill _throwGrenadeSkill;
    private float _throwForce = 10f;
    private Transform _throwPoint;

    public void Init(ObjectPool objectPool, ThrowGrenadeSkill throwGrenadeSkill)
    {
        _objectPool = objectPool;
        _throwGrenadeSkill = throwGrenadeSkill;
        _throwPoint = transform.Find("ThrowPoint").GetComponent<Transform>();
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _throwGrenadeSkill.onActivation += ThrowGrenade;
    }

    private void Unsubscribe()
    {
        _throwGrenadeSkill.onActivation -= ThrowGrenade;
    }

    private void ThrowGrenade()
    {
        var grenade = _objectPool.Get(EResource.Grenade).GetComponent<Grenade>();
        grenade.Throw(_throwPoint.position, _throwPoint.forward, _throwForce);
    }
}
