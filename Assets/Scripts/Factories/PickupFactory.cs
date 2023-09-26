using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PickupFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict = new();
    private ObjectPool _objectPool;
    private IAudioPlayer _iAudioPlayer;

    public PickupFactory()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Pickups");
        foreach (var prefab in prefabs)
        {
            if (Enum.TryParse(prefab.name, out EResource resource))
            {
                _prefabDict[resource] = prefab;
                continue;
            }

            Debug.LogError($"EResource doesn't have value {prefab.name}.");
        }
    }

    public void Init(ObjectPool objectPool, IAudioPlayer iAudioPlayer)
    {
        _objectPool = objectPool;
        _iAudioPlayer = iAudioPlayer;
    }

    public GameObject Instantiate(EResource resource)
    {
        var pickupObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var pickup = pickupObj.GetComponent<Pickup>();
        pickup.Init(_objectPool, _iAudioPlayer);
        return pickupObj;
    }
}
