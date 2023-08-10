using Enums;
using UnityEngine;

public interface IObjectFactory
{
    public GameObject Instantiate(EResource resource);
}