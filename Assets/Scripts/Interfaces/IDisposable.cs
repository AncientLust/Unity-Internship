using System;
using UnityEngine;

public interface IDisposable
{
    public event Action<GameObject> OnDispose;
}