using UnityEngine;
using Enums;

public class PlayerThrowSystem : MonoBehaviour
{
    protected ObjectPool _objectPool;
    protected ThrowGrenadeSkill _throwGrenadeSkill;
    protected float _throwForce = 5f;
    protected Transform _throwPoint;

    public void Init(ObjectPool objectPool, ThrowGrenadeSkill throwGrenadeSkill)
    {
        _objectPool = objectPool;
        _throwGrenadeSkill = throwGrenadeSkill;
        _throwPoint = transform.Find("ThrowPoint").GetComponent<Transform>();
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void Subscribe()
    {
        _throwGrenadeSkill.onActivation += ThrowGrenade;
    }

    protected void Unsubscribe()
    {
        _throwGrenadeSkill.onActivation -= ThrowGrenade;
    }

    protected void ThrowGrenade()
    {
        var grenade = _objectPool.Get(EResource.Grenade).GetComponent<Grenade>();
        grenade.Init(_objectPool, _throwPoint.position);
        grenade.Throw(_throwPoint.forward, _throwForce);
    }
}
